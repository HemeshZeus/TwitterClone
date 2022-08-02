namespace com.tweetapp.service
{
    using MediatR;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    /// <summary>
    /// User Model View cLass
    /// </summary>
    public class UpdateTweetModel : IRequest<ValidatableResponse<object>>
    {
        public string TweetId { get; set; }
        public string Message { get; set; }
        public string Tag { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string UserId { get; set; }
    }
}
