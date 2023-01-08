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



            //var smtp =new SmtpClient("localhost")
            //{

            //    Port = 25,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    EnableSsl = false

            //    //EnableSsl = false,// true;
            //    //DeliveryMethod = SmtpDeliveryMethod.Network,
            //    //Port = 25,
            //    // UseDefaultCredentials
            //    // DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
            //    // PickupDirectoryLocation = @"C:\Demos"

            //};



            Email.DefaultSender = new SmtpSender(() =>  smtp);
            Email.DefaultRenderer = new RazorRenderer();



            //await Email
            //    .From(from, fromAlias)
            //    .To(to, toAlias)
            //    .Subject(subject)
            //    .UsingTemplate(body, new { })
            //    .Body(body)
            //    // .Body("Thanks for buying our products.")
            //    .SendAsync();


            try
            {
                var email = await Email
              .From(from, fromAlias)
              .To(to, toAlias)
              .Subject(subject)
              .UsingTemplate(body.ToString(), new { })
              //.Body(body)
              // .Body("Thanks for buying our products.")
              .SendAsync();

                return true;

            }
            catch(Exception ex)
            {
                return false;

            }

          


           

           // await smtp.SendMailAsync(email);


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
