namespace com.tweetapp.service
{
    using FluentValidation;

    public class DeleteTweetValidation : AbstractValidator<DeleteTweetModel>
    {
        public DeleteTweetValidation()
        {
            RuleFor(x => x.TweetId).NotEmpty().NotNull().WithMessage("Tweet id Must not be null or empty.  ");
        }
    }
}
