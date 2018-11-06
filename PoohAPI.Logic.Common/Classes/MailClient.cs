using PoohAPI.Logic.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace PoohAPI.Logic.Common.Classes
{
    public class MailClient : IMailClient
    {
        private readonly IConfiguration config;

        public MailClient(IConfiguration config)
        {
            this.config = config;
        }

        public void SendEmail(string userEmail, string subject, string body)
        {
            if (userEmail is null || subject is null || body is null)
                return;
            
            MailMessage mail = new MailMessage(this.config.GetValue<string>("MailSender"), userEmail);
            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            mail.Subject = subject;
            mail.Body = body;

            SmtpClient client = new SmtpClient();
            client.Host = this.config.GetValue<string>("MailHost");
            client.Port = this.config.GetValue<int>("MailPort");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(this.config.GetValue<string>("MailSender"), 
                this.config.GetValue<string>("MailPassword"));
            
            try {
                client.Send(mail);
            }
            catch (Exception e) {

            }
        }
    }
}
