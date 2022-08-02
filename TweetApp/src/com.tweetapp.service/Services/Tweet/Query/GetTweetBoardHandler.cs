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

    public class GetTweetBoardHandler : IRequestHandler<GetTweetBoardModel, ValidatableResponse<TweetBoardView>>
    {
        private IConfiguration _configuration;

        public GetTweetBoardHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [Obsolete]
        public async Task<ValidatableResponse<TweetBoardView>> Handle(GetTweetBoardModel request, CancellationToken cancellationToken)
        {
            ValidatableResponse<TweetBoardView> validatableResponse;

            try
            {
                TweetBoardView tweetBoardView = new();

                MongoDbTweetLikeHelper mongoDbTweetLikeHelper = new(_configuration);
                List<TweetLikes> likedUser = mongoDbTweetLikeHelper.LoadDocumentByFilterTweetId<TweetLikes>("TweetLikes", request.TweetId);
                tweetBoardView.TweetId = request.TweetId;
                tweetBoardView.TotalLike = likedUser.Count;

                MongoDbTweetReplyHelper mongoDbTweetReplyHelper = new(_configuration);
                List<TweetReply> replies = mongoDbTweetReplyHelper.LoadDocumentByFilterTweetId<TweetReply>("TweetReplies", request.TweetId);

                List<LikedUserView> likeUsers;

                MapperConfiguration config = new(cfg =>
                {
                    cfg.CreateMap<TweetLikes, LikedUserView>()
                       .ForMember(destination => destination.UserId, map => map.MapFrom(source => source.EmailId));
                });

                likeUsers = config.CreateMapper().Map<List<TweetLikes>, List<LikedUserView>>(likedUser);
                tweetBoardView.LikedUsers = likeUsers;

                List<ReplyTweetView> unSortedreplyTweets;

                MapperConfiguration configR = new(cfg =>
                {
                    cfg.CreateMap<TweetReply, ReplyTweetView>()
                       .ForMember(destination => destination.EmailId, map => map.MapFrom(source => source.EmailId))
                       .ForMember(destination => destination.ReplyMessage, map => map.MapFrom(source => source.ReplyMsg))
                       .ForMember(destination => destination.ReplyID, map => map.MapFrom(source => source.Id))
                       .ForMember(destination => destination.CreatedDate, map => map.MapFrom(source => source.CreatedDate));
                });

                unSortedreplyTweets = configR.CreateMapper().Map<List<TweetReply>, List<ReplyTweetView>>(replies);

                tweetBoardView.ReplyTweets = unSortedreplyTweets;


                validatableResponse = new ValidatableResponse<TweetBoardView>("TweetBoard View", null, tweetBoardView);
                validatableResponse.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception)
            {
                validatableResponse = new ValidatableResponse<TweetBoardView>("We are experiencing an internal server error. Contact your site administrator.", (int)HttpStatusCode.InternalServerError);
                validatableResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return await Task.FromResult(validatableResponse);

        }

    }
}
