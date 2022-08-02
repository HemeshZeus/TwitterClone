namespace com.tweetapp.service
{
    using com.tweetapp.DAO;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddLikeTweetHandler : IRequestHandler<AddLikeTweetModel, ValidatableResponse<object>>
    {
        private IConfiguration _configuration;

        public AddLikeTweetHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [Obsolete]
        public async Task<ValidatableResponse<object>> Handle(AddLikeTweetModel request, CancellationToken cancellationToken)
        {
            ValidatableResponse<object> validatableResponse;
            AddLikeTweetValidation validator = new();

            var result = validator.Validate(request);
            if (result.IsValid)
            {
                try
                {
                    MongoDbTweetLikeHelper mongoDbTweetLikeHelper = new(_configuration);

                    if (request.IsLike)
                    {
                        //to avoid multiple insertion (same user can not be inserted multiple times)
                        if (!Common.GetLikeStatus(request.Email, request.TweetId, _configuration))
                        {
                            TweetLikes like = new();
                            like.TweetId = request.TweetId;
                            like.EmailId = request.Email;
                            mongoDbTweetLikeHelper.InsertDocument<TweetLikes>("TweetLikes", like);
                        }

                        validatableResponse = new ValidatableResponse<object>("Tweet Successfully liked", null, null);
                        validatableResponse.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        var tId = Common.GetLikeID(request.Email, request.TweetId, _configuration);
                        mongoDbTweetLikeHelper.DeleteDocument<TweetLikes>("TweetLikes", tId);

                        validatableResponse = new ValidatableResponse<object>("Tweet Successfully Unliked", null, null);
                        validatableResponse.StatusCode = (int)HttpStatusCode.OK;
                    }
                    
                }
                catch (Exception)
                {
                    validatableResponse = new ValidatableResponse<object>("We are experiencing an internal server error. Contact your site administrator.", (int)HttpStatusCode.InternalServerError);
                    validatableResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                validatableResponse = new ValidatableResponse<object>((result.ToString().Replace("\n", "")).Replace("\r", ""), (int)HttpStatusCode.BadRequest);
                validatableResponse.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return await Task.FromResult(validatableResponse);

        }
    }
}
