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
    public class TweetController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Injecting Mediator
        /// </summary>
        /// <param name="mediator"></param>
        public TweetController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        /// <summary>
        /// get endpoint for getting all Tweets
        /// </summary>
        [HttpGet("/api/v{version:apiVersion}/tweets/all")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> GetAllTweets()
        {
            var email = this.User.FindFirst(s => s.Type == "userid")?.Value;
            var response = await this._mediator.Send(new GetAllTweetModel() { EmailId = email});
            return response.HttpResponseQueryResult;
        }
        /// <summary>
        /// Get All tweet of specific user
        /// </summary>
        /// <param name="userrid"></param>
        /// <returns></returns>
        [HttpGet("/api/v{version:apiVersion}/tweets")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> GetUserTweets([FromQuery] string? userrid)
        {
            userrid ??= this.User.FindFirst(s => s.Type == "userid")?.Value;
            var response = await this._mediator.Send(new GetUserTweetModel() { EmailId = userrid });
            return response.HttpResponseQueryResult;
        }

        /// <summary>
        /// post endpoint for adding new tweet
        /// </summary>
        [HttpPost("/api/v{version:apiVersion}/tweets/add")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> PostTweet([FromBody] CreateTweetModel createTweetModel)
        {
            createTweetModel.CreatedById = this.User.FindFirst(s => s.Type == "userid")?.Value;
            createTweetModel.CreatedByName = this.User.FindFirst(s => s.Type == "name")?.Value;
            var response = await this._mediator.Send(createTweetModel);
            return response.HttpResponseCommandResult;
        }

        /// <summary>
        /// Update tweet with tweetId by valid user
        /// </summary>
        /// <param name="updateTweetModel"></param>
        /// <returns></returns>
        [HttpPut("/api/v{version:apiVersion}/tweets/update")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> UpdateTweet(UpdateTweetModel updateTweetModel)
        {
            updateTweetModel.UserId = this.User.FindFirst(s => s.Type == "userid")?.Value;
            var response = await this._mediator.Send(updateTweetModel);
            return response.HttpResponseCommandResult;
        }

        /// <summary>
        /// Delete Tweet with tweetId by valid user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/v{version:apiVersion}/tweets/delete/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> DeleteTweet(string id)
        {
            string user = this.User.FindFirst(s => s.Type == "userid")?.Value;
            var response = await this._mediator.Send(new DeleteTweetModel() { TweetId = id, UserId= user});
            return response.HttpResponseCommandResult;
        }

        /// <summary>
        /// Like a specific tweet with tweetId by valid user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isLike"></param>
        /// <returns></returns>
        [HttpPost("/api/v{version:apiVersion}/tweets/like")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> AddLikeTweet([FromBody] AddLikeTweetModel data)
        {
            data.Email = this.User.FindFirst(s => s.Type == "userid")?.Value;
            var response = await this._mediator.Send(data);
            return response.HttpResponseCommandResult;
        }

        /// <summary>
        /// Add reply to a specific tweet with tweetId by valid user.
        /// </summary>
        /// <param name="addReplyTweetModel"></param>
        /// <returns></returns>
        [HttpPost("/api/v{version:apiVersion}/tweets/reply")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> AddReplyTweet(AddReplyTweetModel addReplyTweetModel)
        {
            addReplyTweetModel.Email = this.User.FindFirst(s => s.Type == "userid")?.Value;
            var response = await this._mediator.Send(addReplyTweetModel);
            return response.HttpResponseCommandResult;
        }

        /// <summary>
        /// Get tweetBoarddetails (likes/reply) for specific tweet by valid user.
        /// </summary>
        /// <param name="tweetId"></param>
        /// <returns></returns>
        [HttpGet("/api/v{version:apiVersion}/tweetboard")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> GetTweetBoard([FromQuery] string tweetId)
        {
            var response = await this._mediator.Send(new GetTweetBoardModel() { TweetId = tweetId});
            return response.HttpResponseQueryResult;
        }
    }
}
