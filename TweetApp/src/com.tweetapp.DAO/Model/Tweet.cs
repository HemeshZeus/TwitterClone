namespace com.tweetapp.DAO
{
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson.Serialization.IdGenerators;
    using System;

    /// <summary>
    /// Tweet model class
    /// </summary>
    public class Tweet
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Tag { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedById { get; set; }
    }
}
