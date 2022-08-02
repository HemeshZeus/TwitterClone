using System;

namespace com.tweetapp.service
{
    public class ReplyTweetView
    {
        public string ReplyID { get; set; }
        public string ReplyMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public string EmailId { get; set; }
    }
}