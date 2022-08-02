namespace com.tweetapp.service
{
    using com.tweetapp.DAO;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;

    public class Common
    {
        public static bool GetLikeStatus(string userId, string tweetId, IConfiguration con)
        {
            MongoDbTweetLikeHelper mongoDbTweetLikeHelper = new(con);
            List<TweetLikes> likes = mongoDbTweetLikeHelper.LoadDocumentByFilterTweetId<TweetLikes>("TweetLikes", tweetId);

            foreach (TweetLikes like in likes)
            {
                if (like.EmailId == userId)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetLikeID(string userId, string tweetId, IConfiguration con)
        {
            MongoDbTweetLikeHelper mongoDbTweetLikeHelper = new(con);
            List<TweetLikes> likes = mongoDbTweetLikeHelper.LoadDocumentByFilterTweetId<TweetLikes>("TweetLikes", tweetId);
            string id="";
            foreach (TweetLikes like in likes)
            {
                if (like.EmailId == userId)
                {
                    id = like.Id.ToString();
                }
            }
            return id;
        }
    }
}
