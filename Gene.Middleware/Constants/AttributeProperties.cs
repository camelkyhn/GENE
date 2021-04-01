namespace Gene.Middleware.Constants
{
    public class MinLengths
    {
        public const int Text = 1;
        public const int TinyText = 1;
        public const int ShortText = 1;
        public const int LongText = 1;
        public const int DescriptiveText = 1;
        public const int PasswordText = 8;
    }

    public class MaxLengths
    {
        public const int Text = 128;
        public const int TinyText = 32;
        public const int ShortText = 64;
        public const int LongText = 256;
        public const int DescriptiveText = 512;
        public const int PasswordText = 128;
    }

    public class Positions
    {
        public const string Top = "Top";
        public const string Right = "Right";
        public const string Bottom = "Bottom";
        public const string Left = "Left";
    }

    public class Tooltips
    {
        public const string Password = "Password must contain a lower character and an upper character and a digit and a symbol!";
        public const string PhoneNumber = "Optional, Ex: +90 555 555 55 55";
        public const string IsAllData = "Cancels the pagination when checked.";
    }

    public class PlaceHolders
    {
        public const string ConfirmNewPassword = "Confirm the new password.";
        public const string ConfirmPassword = "Confirm your password.";
        public const string CurrentPassword = "Enter your current password.";
        public const string Email = "Enter your email address.";
        public const string NewPassword = "Enter the new password.";
        public const string Password = "Enter your password.";
        public const string PhoneNumber = "Optional, Ex: +90 555 555 55 55";
    }
}