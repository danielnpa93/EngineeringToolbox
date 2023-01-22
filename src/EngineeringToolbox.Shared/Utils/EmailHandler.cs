using System.Net.Mail;
using System.Net;
using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;

namespace EngineeringToolbox.Shared.Utils
{
    public class EmailHandler
    {
        public static async Task<bool> SendEmail(string from, string to, string subject, string body, string password, string fromAlias = null, string toAlias = null)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, password),
                Timeout = 20000
            };

            Email.DefaultSender = new SmtpSender(() => smtp);
            Email.DefaultRenderer = new RazorRenderer();

            try
            {
                var email = await Email
              .From(from, fromAlias)
              .To(to, toAlias)
              .Subject(subject)
              .UsingTemplate(body.ToString(), new { })
              .SendAsync();

                return true;

            }
            catch (Exception ex)
            {
                return false;

            }
        }
    }
}
