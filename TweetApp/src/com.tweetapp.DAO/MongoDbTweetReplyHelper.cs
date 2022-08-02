namespace com.tweetapp.DAO
{
    using Microsoft.Extensions.Configuration;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// mongoDb helper class for Database operation
    /// </summary>
    public class MongoDbTweetReplyHelper
    {
        public IConfiguration Configuration { get; }
        private IMongoDatabase db;
        public MongoDbTweetReplyHelper(IConfiguration configuration)
        {
            this.Configuration = configuration;
            var client = new MongoClient(Configuration.GetSection("DbSettings")["dbConnection"]);
            db = client.GetDatabase(Configuration.GetSection("DbSettings")["database"]);
        }

        /// <summary>
        /// Insert new document into collection
        /// </summary>
        /// <typeparam name="T">Document data type</typeparam>
        /// <param name="collectionName">Collection name</param>
        /// <param name="document">Document</param>
        public void InsertDocument<T>(string collectionName, T document)
        {
            var collection = db.GetCollection<T>(collectionName);
            collection.InsertOne(document);
        }

        /// <summary>
        /// Load document List by value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<T> LoadDocumentByFilterTweetId<T>(string collectionName, string value)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("TweetId", value);

            return collection.Find(filter).ToList();
        }
    }
}
