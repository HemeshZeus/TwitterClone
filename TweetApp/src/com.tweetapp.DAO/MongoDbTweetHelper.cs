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
    public class MongoDbTweetHelper
    {
        public IConfiguration Configuration { get; }
        private IMongoDatabase db;
        public MongoDbTweetHelper(IConfiguration configuration)
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
        /// Load all documents in collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public List<T> LoadAllDocuments<T>(string collectionName)
        {
            var collection = db.GetCollection<T>(collectionName);

            return collection.Find(new BsonDocument()).ToList();
        }

        /// <summary>
        /// Load document List by value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<T> LoadDocumentByFilterCreatedById<T>(string collectionName, string value)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("CreatedById", value);

            return collection.Find(filter).ToList();
        }
        
        /// <summary>
        /// Load document by value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T LoadDocumentById<T>(string collectionName, string value)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", value);

            return collection.Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// Update document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="value"></param>
        /// <param name="document"></param>

        [Obsolete]
        public void UpdateDocument<T>(string collectionName, Guid id, T document)
        {
            var collection = db.GetCollection<T>(collectionName);

            var result = collection.ReplaceOne(
                new BsonDocument("_id", id),
                document,
                new UpdateOptions { IsUpsert = false });
        }

        /// <summary>
        /// Delete document by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="id"></param>
        public void DeleteDocument<T>(string collectionName, string id)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }
    }
}
