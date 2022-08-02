namespace com.tweetapp.service
{
    using com.tweetapp.DAO;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class RagisterUserHandler : IRequestHandler<RegisterUserModel, ValidatableResponse<object>>
    {
        private IConfiguration _configuration;

        public RagisterUserHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [Obsolete]
        public async Task<ValidatableResponse<object>> Handle(RegisterUserModel request, CancellationToken cancellationToken)
        {
            ValidatableResponse<object> validatableResponse;
            RegisterUserValidation validator = new();

            var result = validator.Validate(request);
            if (result.IsValid)
            {
                try
                {
                    MongoDbUserHelper mongoDbUserHelper = new MongoDbUserHelper(_configuration);
                    var dbUser = mongoDbUserHelper.LoadDocumentById<User>("Users", request.Email);
                    if (dbUser != null)
                    {
                        validatableResponse = new ValidatableResponse<object>("Oops Error Occurred Try again.. Email Id already existing ..!!!", (int)HttpStatusCode.BadRequest);
                        validatableResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else
                    {
                        User user = new();
                        user.Email = request.Email.ToLower();
                        user.DOB = request.DOB.ToUniversalTime();
                        user.FirstName = request.FirstName;
                        user.Gender = request.Gender;
                        user.LastName = request.LastName;
                        user.MobilePhone = request.MobilePhone;
                        user.Password = "asdfghjkl" + request.Password + "zxcvbnm";
                        user.IsActive = false;
                        user.LastSeen = DateTime.UtcNow;

                        mongoDbUserHelper.InsertDocument<User>("Users", user);

                        validatableResponse = new ValidatableResponse<object>("Sucessfully registered", null, null);
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
