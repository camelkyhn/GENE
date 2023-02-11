namespace Gene.Middleware.System
{
    public class AppConfiguration
    {
        public string DbConnectionString { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }
}