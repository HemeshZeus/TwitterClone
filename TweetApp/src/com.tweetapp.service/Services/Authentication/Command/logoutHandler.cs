namespace com.tweetapp.service
{
    using com.tweetapp.DAO;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class logoutHandler : IRequestHandler<logoutModel, ValidatableResponse<object>>
    {
        private IConfiguration _configuration;

        public logoutHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [Obsolete]
        public async Task<ValidatableResponse<object>> Handle(logoutModel request, CancellationToken cancellationToken)
        {
            ValidatableResponse<object> validatableResponse;
            
            try
            {
                MongoDbUserHelper mongoDbUserHelper = new MongoDbUserHelper(_configuration);


                var dbUser = mongoDbUserHelper.LoadDocumentById<User>("Users", request.UserId);

                dbUser.IsActive = false;
                dbUser.LastSeen = DateTime.UtcNow;

                mongoDbUserHelper.UpdateDocument("Users", dbUser.Email, dbUser);

                validatableResponse = new ValidatableResponse<object>("Logout successful ", null, null);
                validatableResponse.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception)
            {
                validatableResponse = new ValidatableResponse<object>("We are experiencing an internal server error. Contact your site administrator.", (int)HttpStatusCode.InternalServerError);
                validatableResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return await Task.FromResult(validatableResponse);

        }
    }
}
