namespace com.tweetapp.service
{
    using FluentValidation;
    using System;
    using System.Text.RegularExpressions;

    public class ForgetPasswordValidation : AbstractValidator<ForgetPasswordModel>
    {
        public ForgetPasswordValidation()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().Must(ValidateEmail).WithMessage("Invalid email-ID. ");
            RuleFor(x => x.NewPassword).NotEmpty().NotNull().Must(ValidatePassword).WithMessage("Invalid password (length must be greater than 5). ");
            RuleFor(x => x.DOB).NotEmpty().NotNull().Must(ValidateAge).WithMessage("Invalid DOB. ");
            //mobile number with country code may be approx 15 ++ digit lenght aprox -requirment not defined for this
            RuleFor(x => x.MobilePhone).NotEmpty().NotNull().WithMessage("Invalid Phone Number. ");
        }

        private bool ValidateEmail(string email)
        {
            bool isValidEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            return isValidEmail;
        }

        private bool ValidatePassword(string pass)
        {
            if (pass.Length > 5)
            {
                return true;
            }
            return false;
        }

        private bool ValidateAge(DateTime date)
        {
            int currentYear = DateTime.Now.Year;
            int dobYear = date.Year;

            if (date <= DateTime.UtcNow && dobYear > (currentYear - 120))
            {
                return true;
            }
            return false;
        }
    }
}
