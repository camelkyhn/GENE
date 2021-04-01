using System;
using Gene.Middleware.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Gene.Middleware.System
{
    public class EnvironmentVariable
    {
        public static IConfiguration Configuration { get; set; }

        private static void ConfigurationSetupIfNull()
        {
            if (Configuration == null)
            {
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                Configuration = builder.Build();
            }
        }

        public static string DbConnectionString
        {
            get
            {
                ConfigurationSetupIfNull();
                var dbConnectionString = Configuration["Configuration:DB_CONNECTION_STRING"];
                if (string.IsNullOrEmpty(dbConnectionString))
                {
                    throw new EnvironmentVariableException("DB_CONNECTION_STRING");
                }

                return dbConnectionString;
            }
        }

        public static string AdminEmail
        {
            get
            {
                ConfigurationSetupIfNull();
                var adminEmail = Configuration["Configuration:ADMIN_EMAIL"];
                if (string.IsNullOrEmpty(adminEmail))
                {
                    throw new EnvironmentVariableException("ADMIN_EMAIL");
                }

                return adminEmail;
            }
        }

        public static string AdminPassword
        {
            get
            {
                ConfigurationSetupIfNull();
                var adminPassword = Configuration["Configuration:ADMIN_PASSWORD"];
                if (string.IsNullOrEmpty(adminPassword))
                {
                    throw new EnvironmentVariableException("ADMIN_PASSWORD");
                }

                return adminPassword;
            }
        }

        public static string SmtpHost
        {
            get
            {
                ConfigurationSetupIfNull();
                var smtpHost = Configuration["Configuration:SMTP_HOST"];
                if (string.IsNullOrEmpty(smtpHost))
                {
                    throw new EnvironmentVariableException("SMTP_HOST");
                }

                return smtpHost;
            }
        }

        public static int SmtpPort
        {
            get
            {
                ConfigurationSetupIfNull();
                var smtpPort = Convert.ToInt32(Configuration["Configuration:SMTP_PORT"]);
                if (smtpPort <= 0)
                {
                    throw new EnvironmentVariableException("SMTP_PORT");
                }

                return smtpPort;
            }
        }

        public static string SmtpUsername
        {
            get
            {
                ConfigurationSetupIfNull();
                var smtpUsername = Configuration["Configuration:SMTP_USERNAME"];
                if (string.IsNullOrEmpty(smtpUsername))
                {
                    throw new EnvironmentVariableException("SMTP_USERNAME");
                }

                return smtpUsername;
            }
        }

        public static string SmtpPassword
        {
            get
            {
                ConfigurationSetupIfNull();
                var smtpPassword = Configuration["Configuration:SMTP_PASSWORD"];
                if (string.IsNullOrEmpty(smtpPassword))
                {
                    throw new EnvironmentVariableException("SMTP_PASSWORD");
                }

                return smtpPassword;
            }
        }
    }
}