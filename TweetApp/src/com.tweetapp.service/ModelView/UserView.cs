namespace com.tweetapp.service
{
    using System;

    /// <summary>
    /// User Model View cLass
    /// </summary>
    public class UserView
    {
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Gender { get; set; }

        public bool IsActive { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
