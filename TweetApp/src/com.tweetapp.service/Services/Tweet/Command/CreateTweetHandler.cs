namespace com.tweetapp.service
{
    using com.tweetapp.DAO;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateTweetHandler : IRequestHandler<CreateTweetModel, ValidatableResponse<object>>
    {
        private IConfiguration _configuration;

        public CreateTweetHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [Obsolete]
        public async Task<ValidatableResponse<object>> Handle(CreateTweetModel request, CancellationToken cancellationToken)
        {
            ValidatableResponse<object> validatableResponse;
            CreateTweetValidation validator = new();

            var result = validator.Validate(request);
            if (result.IsValid)
            {
                try
                {
                    MongoDbTweetHelper mongoDbTweetHelper = new (_configuration);

                    Tweet Tweet = new();
                    Tweet.Message = request.Message;
                    Tweet.Tag = request.Tag;
                    Tweet.CreateDate = DateTime.UtcNow;
                    Tweet.CreatedById = request.CreatedById;
                    Tweet.CreatedByName = request.CreatedByName;
                    
                    mongoDbTweetHelper.InsertDocument<Tweet>("Tweets", Tweet);

                    validatableResponse = new ValidatableResponse<object>("Tweet Sucessfully Created", null, null);
                    validatableResponse.StatusCode = (int)HttpStatusCode.OK;

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
