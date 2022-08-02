namespace com.tweetapp.DAO
{
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson.Serialization.IdGenerators;
    using System;

    /// <summary>
    /// Tweet model class
    /// </summary>
    public class TweetReply
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }
        public string TweetId { get; set; }
        public string EmailId { get; set; }
        public string ReplyMsg { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}