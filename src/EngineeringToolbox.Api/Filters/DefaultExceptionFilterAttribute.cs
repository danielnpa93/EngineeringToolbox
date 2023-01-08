using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using EngineeringToolbox.Shared.Results;
using Serilog;

namespace EngineeringToolbox.Api.Filters
{
    public class DefaultExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            Log.Error($"[{context.HttpContext.Request.Path}] {context.Exception.InnerException?.Message ?? context.Exception.Message}");
            context.Result =
            new ObjectResult(new ResultModel
            {
                DisplayMessage = "Unknow Error Occurred",
                Errors = new string[] { context.Exception.InnerException?.Message ?? context.Exception.Message }
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
