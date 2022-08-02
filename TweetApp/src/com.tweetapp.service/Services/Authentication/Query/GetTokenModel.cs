namespace com.tweetapp.service
{
    using MediatR;
    public class GetTokenModel : IRequest<ValidatableResponse<LogInInfoView>>
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
