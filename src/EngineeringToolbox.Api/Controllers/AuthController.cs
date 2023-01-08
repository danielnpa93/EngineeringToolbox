using EngineeringToolbox.Domain.Nofication;
using EngineeringToolbox.Shared.Results;
using EngineeringToolbox.Application.Interfaces;
using EngineeringToolbox.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EngineeringToolbox.Shared.Token;

namespace EngineeringToolbox.Api.Controllers
{
    [Authorize]
    [Route("api/v1/identity")]
    public class AuthController : BaseController
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices, 
                              NotificationContext notificationContext) : base(notificationContext)

        {
            _authServices = authServices;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(ResultModel<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserRegisterViewModel userRegister)
        {
            var result = await _authServices.RegisterUser(userRegister);

            return CustomResponse(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(ResultModel<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            var result = await _authServices.UserLogin(userLogin);

            return CustomResponse(result);
        }
    }
}
