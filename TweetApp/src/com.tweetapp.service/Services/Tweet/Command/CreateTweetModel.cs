namespace com.tweetapp.service
{
    using MediatR;
    using System;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    /// <summary>
    /// User Model View cLass
    /// </summary>
    public class CreateTweetModel : IRequest<ValidatableResponse<object>>
    {
        public string Message { get; set; }
        public string Tag { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public DateTime CreateDate { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string CreatedByName { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string CreatedById { get; set; }
    }
}
