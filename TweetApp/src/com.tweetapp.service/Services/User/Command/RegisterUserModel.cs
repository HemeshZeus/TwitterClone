namespace com.tweetapp.service
{
    using MediatR;
    using System;

    /// <summary>
    /// User Model View cLass
    /// </summary>
    public class RegisterUserModel: IRequest<ValidatableResponse<object>>
    {
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime DOB { get; set; }

        public string MobilePhone { get; set; }

        public string Password { get; set; }
    }
}
