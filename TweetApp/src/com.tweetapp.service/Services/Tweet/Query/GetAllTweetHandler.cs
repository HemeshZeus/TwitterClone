namespace com.tweetapp.service
{
    using AutoMapper;
    using com.tweetapp.DAO;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetAllTweetHandler : IRequestHandler<GetAllTweetModel, ValidatableResponse<List<TweetView>>>
    {
        private IConfiguration _configuration;

        public GetAllTweetHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [Obsolete]
        public async Task<ValidatableResponse<List<TweetView>>> Handle(GetAllTweetModel request, CancellationToken cancellationToken)
        {
            ValidatableResponse<List<TweetView>> validatableResponse;

            try
            {
                MongoDbTweetHelper mongoDbTweetHelper = new(_configuration);
                List<Tweet> dbTweets;
                
                dbTweets = mongoDbTweetHelper.LoadAllDocuments<Tweet>("Tweets");

                if (dbTweets.Count > 0)
                {
                    MapperConfiguration config = new(cfg =>
                    {
                        cfg.CreateMap<Tweet, TweetView>()
                           .ForMember(destination => destination.Id, map => map.MapFrom(source => source.Id))
                           .ForMember(destination => destination.Message, map => map.MapFrom(source => source.Message))
                           .ForMember(destination => destination.Tag, map => map.MapFrom(source => source.Tag))
                           .ForMember(destination => destination.CreateDate, map => map.MapFrom(source => source.CreateDate))
                           .ForMember(destination => destination.CreatedById, map => map.MapFrom(source => source.CreatedById))
                           .ForMember(destination => destination.CreatedByName, map => map.MapFrom(source => source.CreatedByName));
                    });

                    var unSortedlistTweets = config.CreateMapper().Map<List<Tweet>, List<TweetView>>(dbTweets);

                    var listTweets = unSortedlistTweets.OrderByDescending(o => o.CreateDate).ToList();

                    foreach (TweetView tweet in listTweets)
                    {
                        tweet.LikedByLoggedUser = Common.GetLikeStatus(request.EmailId, tweet.Id.ToString(), _configuration);
                    }
                    validatableResponse = new ValidatableResponse<List<TweetView>>("Tweets", null, listTweets);
                    validatableResponse.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    validatableResponse = new ValidatableResponse<List<TweetView>>("No Tweet Found", null, new List<TweetView>());
                    validatableResponse.StatusCode = (int)HttpStatusCode.OK;
                }
            }
            catch (Exception)
            {
                validatableResponse = new ValidatableResponse<List<TweetView>>("We are experiencing an internal server error. Contact your site administrator.", (int)HttpStatusCode.InternalServerError);
                validatableResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return await Task.FromResult(validatableResponse);

        }
    }
}
