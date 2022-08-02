namespace com.tweetapp.service
{
    using MediatR;
    using System;

    /// <summary>
    /// User Model View cLass
    /// </summary>
    public class ForgetPasswordModel : IRequest<ValidatableResponse<object>>
    {
        public string Email { get; set; }

        public DateTime DOB { get; set; }

        public string MobilePhone { get; set; }

        public string NewPassword { get; set; }
    }
}
