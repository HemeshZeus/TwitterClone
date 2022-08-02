namespace com.tweetapp.webapi.Controllers
{
    using com.tweetapp.service;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    /// <summary>
    /// Authentication endpoints
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// Injecting Mediator
        /// </summary>
        /// <param name="mediator"></param>
        public AuthenticationController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        
        /// <summary>
        /// login endpoint returns token for valid users
        /// </summary>
        [HttpPost("/api/v{version:apiVersion}/tweets/login")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> Login([FromBody] GetTokenModel user)
        {
            var response = await this._mediator.Send(user);
            return response.HttpResponseQueryResult;
        }

        /// <summary>
        /// Forgot password endpoint to reset password
        /// </summary>
        [HttpPost("/api/v{version:apiVersion}/tweets/forgot")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgetPasswordModel user)
        {
            var response = await this._mediator.Send(user);
            return response.HttpResponseCommandResult;
        }

        /// <summary>
        /// register endpoint to register new user
        /// </summary>
        [HttpPost("/api/v{version:apiVersion}/tweets/register")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserModel user)
        {
            var response = await this._mediator.Send(user);
            return response.HttpResponseCommandResult;
        }

        /// <summary>
        /// logout endpoint to logout user
        /// </summary>
        /// <returns></returns>
        [HttpPost("/api/v{version:apiVersion}/tweets/logout")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> LogOut()
        {
            var email = this.User.FindFirst(s => s.Type == "userid")?.Value;
            var response = await this._mediator.Send(new logoutModel() { UserId = email});
            return response.HttpResponseCommandResult;
        }
    }
}
