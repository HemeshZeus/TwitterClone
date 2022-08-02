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

    public class GetAllUsersHandler : IRequestHandler<GetAllUsersModel, ValidatableResponse<List<UserView>>>
    {
        private IConfiguration _configuration;

        public GetAllUsersHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [Obsolete]
        public async Task<ValidatableResponse<List<UserView>>> Handle(GetAllUsersModel request, CancellationToken cancellationToken)
        {
            ValidatableResponse<List<UserView>> validatableResponse;

            try
            {
                MongoDbUserHelper mongoDbUserHelper = new(_configuration);
                List<User> dbUsers;
                if (request.ActiveOnly == false)
                {
                    dbUsers = mongoDbUserHelper.LoadAllDocuments<User>("Users");
                }
                else
                {
                    dbUsers = mongoDbUserHelper.LoadDocumentByFilter<User>("Users", true);
                }

                if (dbUsers.Count > 0)
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

                    var listUsers = config.CreateMapper().Map<List<User>, List<UserView>>(dbUsers);

                    validatableResponse = new ValidatableResponse<List<UserView>>("Users", null, listUsers);
                    validatableResponse.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    validatableResponse = new ValidatableResponse<List<UserView>>("No User Found", null, new List<UserView>());
                    validatableResponse.StatusCode = (int)HttpStatusCode.OK;
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
