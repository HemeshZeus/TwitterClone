namespace com.tweetapp.service
{
    using System;

    /// <summary>
    /// Tweet model view class
    /// </summary>
    public class TweetView
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Tag { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedById { get; set; }
        public bool UiRDisplay { get; set; } = false;
        public bool UiEDisplay { get; set; } = false;
        public bool LikedByLoggedUser { get; set; }
    }
}
