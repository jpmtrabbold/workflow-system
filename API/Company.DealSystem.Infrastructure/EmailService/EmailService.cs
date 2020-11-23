using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Domain.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace Company.DealSystem.Infrastructure.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public EmailService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task<(bool success, string message)> SendEmail(string from, string to, string subject, string content, bool contentIsHtml = true, string cc = "", string bcc = "")
        {
            return (true, "E-mail sent successfully");
            try
            {
                var fromAddress = new MailAddress(from, "DealSystem");
                var toAddress = new MailAddress(to, "Recipient");
                const string fromPassword = "";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = content
                })
                {
                    smtp.Send(message);
                }
                return (true, "E-mail sent successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error trying to send e-mail: {ex}" );
            }
        }
    }
}