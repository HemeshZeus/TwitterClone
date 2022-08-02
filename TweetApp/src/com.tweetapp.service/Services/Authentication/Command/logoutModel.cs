namespace com.tweetapp.service
{
    using MediatR;
    public class logoutModel : IRequest<ValidatableResponse<object>>
    {
        public string UserId { get; set; }
    }
}
