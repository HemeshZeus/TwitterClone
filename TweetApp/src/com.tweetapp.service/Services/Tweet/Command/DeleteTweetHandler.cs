namespace com.tweetapp.service
{
    using com.tweetapp.DAO;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteTweetHandler : IRequestHandler<DeleteTweetModel, ValidatableResponse<object>>
    {
        private IConfiguration _configuration;

        public DeleteTweetHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [Obsolete]
        public async Task<ValidatableResponse<object>> Handle(DeleteTweetModel request, CancellationToken cancellationToken)
        {
            ValidatableResponse<object> validatableResponse;
            DeleteTweetValidation validator = new();

            var result = validator.Validate(request);
            if (result.IsValid)
            {
                try
                {
                    MongoDbTweetHelper mongoDbTweetHelper = new(_configuration);

                    var dbtweet = mongoDbTweetHelper.LoadDocumentById<Tweet>("Tweets", request.TweetId);

                    if (dbtweet.CreatedById == request.UserId)
                    {
                        mongoDbTweetHelper.DeleteDocument<Tweet>("Tweets", request.TweetId);

                        validatableResponse = new ValidatableResponse<object>("Tweet Successfully Deleted", null, null);
                        validatableResponse.StatusCode = (int)HttpStatusCode.OK;
                    }

                    else
                    {
                        validatableResponse = new ValidatableResponse<object>("UnAuthorised to Delete", (int)HttpStatusCode.Unauthorized);
                        validatableResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
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
