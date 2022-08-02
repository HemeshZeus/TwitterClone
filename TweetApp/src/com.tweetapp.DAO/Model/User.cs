namespace com.tweetapp.DAO
{
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// User Model cLass
    /// </summary>
    public class User
    {
        [BsonId]
        [BsonElement("_id")]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime DOB { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string MobilePhone { get; set; }

        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
