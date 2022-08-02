namespace com.tweetapp.service
{
    using FluentValidation;

    public class AddReplyTweetValidation : AbstractValidator<AddReplyTweetModel>
    {
        public AddReplyTweetValidation()
        {
            RuleFor(x => x.Replymsg).NotEmpty().NotNull().WithMessage("Reply Must not be null or empty.  ");
        }
    }
}
