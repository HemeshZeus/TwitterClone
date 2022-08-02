namespace com.tweetapp.service
{
    using MediatR;
    using System.Collections.Generic;

    public class GetTweetBoardModel : IRequest<ValidatableResponse<TweetBoardView>>
    {
        public string TweetId { get; set; }
    }
}
