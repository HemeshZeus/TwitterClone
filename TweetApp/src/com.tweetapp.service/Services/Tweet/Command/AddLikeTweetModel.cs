namespace com.tweetapp.service
{
    using MediatR;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    /// <summary>
    /// User Model View cLass
    /// </summary>
    public class AddLikeTweetModel : IRequest<ValidatableResponse<object>>
    {
        public string TweetId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public string Email { get; set; }
        public bool IsLike { get; set; }
    }
}
