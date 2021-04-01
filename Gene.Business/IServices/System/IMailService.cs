using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.System;
using Gene.Middleware.System;

namespace Gene.Business.IServices.System
{
    public interface IMailService
    {
        Task<Result<bool?>> SendMailAsync(MailDto dto);
        Task<Result<bool?>> SendMultipleMailAsync(MailDto dto, IEnumerable<string> recipients);
        Task<Result<bool?>> SendCustomMailAsync(MailMessage mailMessage);
    }
}