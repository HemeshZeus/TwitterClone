namespace com.tweetapp.service
{
    using FluentValidation;

    public class UpdateTweetValidation : AbstractValidator<UpdateTweetModel>
    {
        public UpdateTweetValidation()
        {
            RuleFor(x => x.TweetId).NotEmpty().NotNull().WithMessage("Tweet id Must not be null or empty.  ");
            RuleFor(x => x.Message).NotEmpty().NotNull().Must(ValidateMessage).WithMessage("Tweet message Length should be between 1 -144.  ");
            RuleFor(x => x.Tag).Must(ValidateTag).WithMessage("Tag length must be between 1- 50). ");
        }
        private bool ValidateMessage(string msg)
        {
            if (msg != null && msg.Length < 145)
            {
                return true;
            }
            return false;
        }

        private bool ValidateTag(string msg)
        {
            if (msg.Length < 51)
            {
                return true;
            }
            return false;
        }
    }
}
