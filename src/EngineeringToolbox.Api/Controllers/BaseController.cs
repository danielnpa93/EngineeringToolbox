using EngineeringToolbox.Domain.Nofication;
using EngineeringToolbox.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace EngineeringToolbox.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly NotificationContext _notificationContext;

        public BaseController(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }
        protected IActionResult CustomResponse(object? result)
        {
            if (_notificationContext.HasNotifications)
            {
                Log.Error($"[{Request.Path}] Error: {JsonSerializer.Serialize(_notificationContext.Notifications)}");
                return StatusCode((int)_notificationContext.Status, new ResultModel()
                {
                    Errors = _notificationContext.Notifications.Select(n => n.Message),
                    Success = false,
                    DisplayMessage = "Bad Request Error"
                });
            }

            if (result == null)
                return NoContent();

            return Ok(new ResultModel<object>() { Data = result, Success = true });
        }
    }
}
