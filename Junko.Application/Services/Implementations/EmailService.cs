using Junko.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Junko.Domain.InterFaces;

namespace Junko.Application.Services.Implementations
{
    public class EmailService : IEmailService
    {
        #region consractor

        private readonly ISiteSettingRepository _siteSettingRepository;

        public EmailService(ISiteSettingRepository siteSettingRepository)
        {
            _siteSettingRepository = siteSettingRepository;
        }

        #endregion

        public async void SendEmail(string to, string subject, string body)
        {
            var defaultSiteEmail = await _siteSettingRepository.GetDefaultEmail();

            MailMessage mail = new MailMessage();

            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(defaultSiteEmail.From, defaultSiteEmail.DisplayName);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            if (defaultSiteEmail.Port != 0)
            {
                SmtpServer.Port = defaultSiteEmail.Port;
                SmtpServer.EnableSsl = defaultSiteEmail.EnableSSL;
            }

            SmtpServer.Credentials = new System.Net.NetworkCredential(defaultSiteEmail.From, defaultSiteEmail.Password);
            SmtpServer.Send(mail);
        }
    }
}
