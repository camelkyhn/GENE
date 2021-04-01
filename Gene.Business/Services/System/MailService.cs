using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Gene.Business.IServices.System;
using Gene.Middleware.Dtos.System;
using Gene.Middleware.System;

namespace Gene.Business.Services.System
{
    public class MailService : IMailService
    {
        private SmtpClient SmtpClient { get; }

        public MailService()
        {
            SmtpClient = new SmtpClient
            {
                Host = EnvironmentVariable.SmtpHost,
                Port = EnvironmentVariable.SmtpPort,
                EnableSsl = false,
                Credentials = new NetworkCredential(EnvironmentVariable.SmtpUsername, EnvironmentVariable.SmtpPassword)
            };
        }

        public async Task<Result<bool?>> SendMailAsync(MailDto dto)
        {
            var result = new Result<bool?>();
            try
            {
                var message = new MailMessage(EnvironmentVariable.SmtpUsername, dto.ToEmailAddress)
                {
                    Body = dto.Message,
                    Subject = dto.Subject,
                    IsBodyHtml = !dto.IsBodyPlainText,
                    From = new MailAddress(EnvironmentVariable.SmtpUsername, "Gene Software")
                };

                await SmtpClient.SendMailAsync(message);
                result.Success(true);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<bool?>> SendMultipleMailAsync(MailDto dto, IEnumerable<string> recipients)
        {
            var result = new Result<bool?>();
            try
            {
                var sendingTaskList = new List<Task>();
                foreach (var recipient in recipients)
                {
                    sendingTaskList.Add(new SmtpClient
                    {
                        Host = EnvironmentVariable.SmtpHost,
                        Port = EnvironmentVariable.SmtpPort,
                        EnableSsl = false,
                        Credentials = new NetworkCredential(EnvironmentVariable.SmtpUsername, EnvironmentVariable.SmtpPassword)
                    }.SendMailAsync(new MailMessage(EnvironmentVariable.SmtpUsername, recipient)
                    {
                        Body = dto.Message,
                        Subject = dto.Subject,
                        IsBodyHtml = !dto.IsBodyPlainText
                    }));
                }

                await Task.WhenAll(sendingTaskList);
                result.Success(true);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<bool?>> SendCustomMailAsync(MailMessage mailMessage)
        {
            var result = new Result<bool?>();
            try
            {
                await SmtpClient.SendMailAsync(mailMessage);
                result.Success(true);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }
    }
}