namespace Gene.Middleware.Constants
{
    public class Errors
    {
        public const string AccountLockedOut = "This account locked out, will be released at: ";
        public const string AlreadyConfirmedEmail = "This email address is already confirmed!";
        public const string ConfirmNewPassword = "New Password and Confirmation Password do not match!";
        public const string ConfirmPassword = "Password and Confirmation Password do not match!";
        public const string CurrentPassword = "Entered current password is not correct!";
        public const string EmailTaken = "This email is already taken!";
        public const string EmptyModel = "Model can not be empty!";
        public const string ExistingData = "This data already exists in the database!";
        public const string ExpiredToken = "Token is expired or not correct!";
        public const string LoginConfirmEmail = "You have to confirm your email address in order to login!";
        public const string NotFoundEmail = "No user found with this email!";
        public const string NotFoundId = "No user found with this identifier!";
        public const string Password = "Password must contain a lower character and an upper character and a digit and a symbol!";
        public const string PhoneNumber = "Phone number is malformed! Ex: +90 555 555 55 55";
        public const string ResetPasswordConfirmEmail = "In order to reset your password, you have to confirm your email address!";
        public const string ResetPasswordNotFoundId = "No user found with this identifier! You need to do it from scratch.";
        public const string WrongLoginAttempt = "Wrong password or email entered!";
    }
}