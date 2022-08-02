namespace com.tweetapp.service
{
    using System.Collections.Generic;

    /// <summary>
    /// Tweet model class
    /// </summary>
    public class TweetBoardView
    {
        public string TweetId { get; set; }
        public long TotalLike { get; set; }
        public List<LikedUserView> LikedUsers { get; set; }
        public List<ReplyTweetView> ReplyTweets { get; set; }

    }

}
