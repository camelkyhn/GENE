namespace Gene.Middleware.Dtos.System
{
    public class MailDto
    {
        public string ToEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsBodyPlainText { get; set; }
    }
}