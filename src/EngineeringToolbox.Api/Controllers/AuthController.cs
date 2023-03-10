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
        public async Task<IActionResult> Register(UserViewModel userRegister)
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

        [HttpPost("change_password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResultModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            await _authServices.ChangePassword(model);

            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpPost("reset_password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResultModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromQuery] string token,
                                                        [FromQuery] string userId,
                                                       ResetPasswordViewModel model)
        {
            await _authServices.ResetPassword(model, token, userId);

            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpPost("forgot_password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResultModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            await _authServices.ForgotPassword(email);

            return CustomResponse();
        }

        [HttpPost("change_email_request")]
        public async Task<IActionResult> ChangeEmailRequest(string newEmail)
        {
            await _authServices.ChangeEmailRequest(newEmail);

            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpGet("change_email")]
        public async Task<ContentResult> ChangeEmail([FromQuery] string token,
                                              [FromQuery] string email,
                                              [FromQuery] string userId)
        {
             var result = await _authServices.ChangeEmail(token, email, userId);

            var message = result ? $"Your email/username has been changed to {email}" :
                $"Error to change your email";

            return new ContentResult
            {
                ContentType = "text/html",
                Content = $"<div style='text-align: center;padding-top:50px'>{message}</div>"
            };
        }

        //[HttpPatch]
        //public async Task<IActionResult> ChangeUser([FromBody] JsonPatchDocument<User> userModel)
        //{
        //    ///id from token
        //    ///

        //}
    }
}
