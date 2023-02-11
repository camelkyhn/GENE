using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Gene.Business.IServices.System;
using Gene.Middleware.Dtos.System;
using Gene.Middleware.System;
using Microsoft.Extensions.Options;

namespace Gene.Business.Services.System
{
    public class MailService : IMailService
    {
        private readonly AppConfiguration _configuration;
        private SmtpClient SmtpClient { get; }

        public MailService(IOptions<AppConfiguration> configurationOptions)
        {
            _configuration = configurationOptions.Value;
            SmtpClient = new SmtpClient
            {
                Host = _configuration.SmtpHost,
                Port = _configuration.SmtpPort,
                EnableSsl = false,
                Credentials = new NetworkCredential(_configuration.SmtpUsername, _configuration.SmtpPassword)
            };
        }

        public async Task<Result<bool?>> SendMailAsync(MailDto dto)
        {
            var result = new Result<bool?>();
            try
            {
                var message = new MailMessage(_configuration.SmtpUsername, dto.ToEmailAddress)
                {
                    Body = dto.Message,
                    Subject = dto.Subject,
                    IsBodyHtml = !dto.IsBodyPlainText,
                    From = new MailAddress(_configuration.SmtpUsername, "Gene Software")
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
                        Host = _configuration.SmtpHost,
                        Port = _configuration.SmtpPort,
                        EnableSsl = false,
                        Credentials = new NetworkCredential(_configuration.SmtpUsername, _configuration.SmtpPassword)
                    }.SendMailAsync(new MailMessage(_configuration.SmtpUsername, recipient)
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