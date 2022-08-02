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
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Injecting Mediator
        /// </summary>
        /// <param name="mediator"></param>
        public UserController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        /// <summary>
        /// get endpoint for getting all users
        /// </summary>
        [HttpGet("/api/v{version:apiVersion}/tweets/users/all")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> GetAllUsers([FromQuery] bool IsActiveOnly = false)
        {
            var response = await this._mediator.Send(new GetAllUsersModel() { ActiveOnly = IsActiveOnly });
            return response.HttpResponseQueryResult;
        }

        /// <summary>
        /// get endpoint for searching user
        /// </summary>
        [HttpGet("/api/v{version:apiVersion}/tweets/search")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> GetUser([FromQuery]string name)
        {
            var response = await this._mediator.Send(new GetUserModel() { Name = name });
            return response.HttpResponseQueryResult;
        }


    }
}
