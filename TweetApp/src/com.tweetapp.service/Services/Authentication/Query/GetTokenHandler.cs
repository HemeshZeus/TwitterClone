namespace com.tweetapp.service
{
    using com.tweetapp.DAO;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetTokenHandler : IRequestHandler<GetTokenModel, ValidatableResponse<LogInInfoView>>
    {
        private IConfiguration _configuration;

        public GetTokenHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [Obsolete]
        public async Task<ValidatableResponse<LogInInfoView>> Handle(GetTokenModel request, CancellationToken cancellationToken)
        {
            ValidatableResponse<LogInInfoView> validatableResponse;
            GetTokenValidation validator = new();

            var result = validator.Validate(request);
            if (result.IsValid)
            {
                try
                {
                    MongoDbUserHelper mongoDbUserHelper = new MongoDbUserHelper(_configuration);
                    var dbUser = mongoDbUserHelper.LoadDocumentById<User>("Users", request.UserId.ToLower());
                    string dbPass = "asdfghjkl" + request.Password + "zxcvbnm";
                    if (dbUser != null && dbUser.Password == dbPass)
                    {
                        dbUser.IsActive = true;

                        mongoDbUserHelper.UpdateDocument("Users", dbUser.Email, dbUser);

                        GenerateTokenHandler generateTokenHandler = new(_configuration);

                        string username = dbUser.FirstName +" "+ dbUser.LastName;

                        LogInInfoView userInfo = new();
                        userInfo.JwtToken = generateTokenHandler.GenerateToken(request.UserId, username);
                        userInfo.Name = username;
                        userInfo.UserNameId = request.UserId;
                        userInfo.Gender = dbUser.Gender;

                        validatableResponse = new ValidatableResponse<LogInInfoView>("Token generated", null, userInfo);
                        validatableResponse.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        validatableResponse = new ValidatableResponse<LogInInfoView>("Incorrect Credential", (int)HttpStatusCode.BadRequest);
                        validatableResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
                catch (Exception)
                {
                    validatableResponse = new ValidatableResponse<LogInInfoView>("We are experiencing an internal server error. Contact your site administrator.", (int)HttpStatusCode.InternalServerError);
                    validatableResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                validatableResponse = new ValidatableResponse<LogInInfoView>((result.ToString().Replace("\n", "")).Replace("\r", ""), (int)HttpStatusCode.BadRequest);
                validatableResponse.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return await Task.FromResult(validatableResponse);

        }
    }
}
