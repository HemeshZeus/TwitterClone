namespace com.tweetapp.service
{
    using FluentValidation;

    public class GetTokenValidation : AbstractValidator<GetTokenModel>
    {
        public GetTokenValidation()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull().WithMessage("UserId must not be empty or null. ");
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password must not be empty or null");
        }
    }
}
