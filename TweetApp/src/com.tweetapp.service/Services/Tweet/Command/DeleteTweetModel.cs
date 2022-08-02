namespace com.tweetapp.service
{
    using MediatR;

    /// <summary>
    /// User Model View cLass
    /// </summary>
    public class DeleteTweetModel : IRequest<ValidatableResponse<object>>
    {
        public string TweetId { get; set; }
        public string UserId { get; set; }
    }
}
