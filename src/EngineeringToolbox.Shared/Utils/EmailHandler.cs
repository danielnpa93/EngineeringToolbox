using System.Net.Mail;
using System.Net;
using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;

namespace EngineeringToolbox.Shared.Utils
{
    public class EmailHandler
    {
        public static async Task SendEmail(string from, string to, string subject, string body, string password, string fromAlias = null, string toAlias = null)
        {
            //var fromAddress = new MailAddress(from, fromAlias);
            //var toAddress = new MailAddress(to, toAlias);

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

            Email.DefaultSender = new SmtpSender(() =>  smtp);
            Email.DefaultRenderer = new RazorRenderer();

            await Email
                .From(from, fromAlias)
                .To(to,toAlias)
                .Subject(subject)
                .UsingTemplate(body, new {})
                .Body(body)
                // .Body("Thanks for buying our products.")
                .SendAsync();


            //using (var message = new MailMessage(fromAddress, toAddress)
            //{
            //    Subject = subject,
            //    Body = body,
                
            //})
            //{


            //    var email = await Email
            //    .From("danielnpa93@gmail.com")
            //    .To("daniel_npa@hotmail.com", "Daniel")
            //    .Subject("Thnaks!")
            //    .UsingTemplate(template.ToString(), new { FirstName = "Sue", ProductName = "Toolbox" })
            //    // .Body("Thanks for buying our products.")
            //    .SendAsync();

            //    await smtp.SendMailAsync(message);
            //}

        }
    }
}
