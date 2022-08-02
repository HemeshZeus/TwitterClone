namespace com.tweetapp.service
{
    using AutoMapper;
    using com.tweetapp.DAO;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetUserHandler : IRequestHandler<GetUserModel, ValidatableResponse<List<UserView>>>
    {
        private IConfiguration _configuration;

        public GetUserHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [Obsolete]
        public async Task<ValidatableResponse<List<UserView>>> Handle(GetUserModel request, CancellationToken cancellationToken)
        {
            ValidatableResponse<List<UserView>> validatableResponse;

            try
            {
                MongoDbUserHelper mongoDbUserHelper = new(_configuration);
                
                var dbUsers = mongoDbUserHelper.LoadAllDocuments<User>("Users");

                List<User> fliterDbUser = new();

                foreach(User u in dbUsers)
                {
                    string dbuser = u.Email.ToLower() + u.FirstName.ToLower() + u.LastName.ToLower();
                    var search = request.Name !=null ? request.Name.ToLower():"null";
                    if (dbuser.Contains(search))
                    {
                        fliterDbUser.Add(u);
                    }
                }

                if (fliterDbUser != null)
                {
                    MapperConfiguration config = new(cfg =>
                    {
                        cfg.CreateMap<User, UserView>()
                           .ForMember(destination => destination.Email, map => map.MapFrom(source => source.Email))
                           .ForMember(destination => destination.FirstName, map => map.MapFrom(source => source.FirstName))
                           .ForMember(destination => destination.LastName, map => map.MapFrom(source => source.LastName))
                           .ForMember(destination => destination.IsActive, map => map.MapFrom(source => source.IsActive))
                           .ForMember(destination => destination.LastSeen, map => map.MapFrom(source => source.LastSeen));
                    });

                    var user = config.CreateMapper().Map<List<User>, List<UserView>>(fliterDbUser);
                    validatableResponse = new ValidatableResponse<List<UserView>>("Users", null, user);
                    validatableResponse.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    validatableResponse = new ValidatableResponse<List<UserView>>("No User Found", (int)HttpStatusCode.BadRequest);
                    validatableResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            catch (Exception)
            {
                validatableResponse = new ValidatableResponse<List<UserView>>("We are experiencing an internal server error. Contact your site administrator.", (int)HttpStatusCode.InternalServerError);
                validatableResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return await Task.FromResult(validatableResponse);

        }
    }
}
