namespace com.tweetapp.service
{
    using FluentValidation;

    public class AddLikeTweetValidation : AbstractValidator<AddLikeTweetModel>
    {
        public AddLikeTweetValidation()
        {
            RuleFor(x => x.TweetId).NotEmpty().NotNull().WithMessage("Tweet id Must not be null or empty.  ");
        }
    }
}
