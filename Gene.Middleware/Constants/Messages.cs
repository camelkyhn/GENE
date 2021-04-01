namespace Gene.Middleware.Constants
{
    public class Messages
    {
        public static string EmailConfirmation(string callbackUrl)
        {
            return "In order to confirm your email address, please <a href=\"" + callbackUrl + "\">click here.</a>";
        }

        public static string ResetPassword(string callbackUrl)
        {
            return "In order to reset your password, please <a href=\"" + callbackUrl + "\">click here.</a>";
        }
    }
}