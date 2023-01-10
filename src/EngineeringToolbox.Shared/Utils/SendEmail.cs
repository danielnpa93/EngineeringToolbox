using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EngineeringToolbox.Shared.Utils
{
    public static class SendEmail
    {

        //public static async Task Main()
        //{

        //    var adress = "danielnpa93@gmail.com";
        //    var password = "kjntwsfhslsaemdh";

        //    var sender = new SmtpSender(() => new SmtpClient("localhost")
        //    {

        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        Credentials = new NetworkCredential(adress, password),
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        EnableSsl = true

        //        //EnableSsl = false,// true;
        //        //DeliveryMethod = SmtpDeliveryMethod.Network,
        //        //Port = 25,
        //        // UseDefaultCredentials
        //        // DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
        //        // PickupDirectoryLocation = @"C:\Demos"

        //    });


        //    StringBuilder template = new StringBuilder();

        //    template.AppendLine("Dear @Model.FirstName");
        //    template.AppendLine("<p>Thanks for purchase @Model.ProductName</p>");
        //    template.AppendLine(" - Daniel");

        //    Email.DefaultSender = sender;
        //    Email.DefaultRenderer = new RazorRenderer();

        //    try
        //    {
        //        var email = await Email
        //        .From("danielnpa93@gmail.com")
        //        .To("daniel_npa@hotmail.com", "Daniel")
        //        .Subject("Thnaks!")
        //        .UsingTemplate(template.ToString(), new { FirstName = "Sue", ProductName = "Toolbox" })
        //        // .Body("Thanks for buying our products.")
        //        .SendAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //    }



        //}

        //public static async Task SendEmailMethod(string email, )
        //{

        //    var fromAddress = new MailAddress("danielnpa93@gmail.com", "Daniel Nunes");
        //    var toAddress = new MailAddress("carolrochamb@gmail.com", "Dani");
        //    const string fromPassword = "kjntwsfhslsaemdh";
        //    const string subject = "Subject";
        //    const string body = "Body";

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
        //        Timeout = 20000
        //    };

        //    try
        //    {
        //        using (var message = new MailMessage(fromAddress, toAddress)
        //        {
        //            Subject = subject,
        //            Body = body
        //        })
        //        {
        //            await smtp.SendMailAsync(message);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}

    }
}
