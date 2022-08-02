namespace com.tweetapp.service
{
    using MediatR;
    using System.Collections.Generic;

    public class GetUserTweetModel : IRequest<ValidatableResponse<List<TweetView>>>
    {
        public string EmailId { get; set; }
    }
}
