using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PrototipoLogin.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return ConfigSendGridasync(message);

            //   return SendMail(message);
        }

        // Implementação do SendGrid
        private async Task ConfigSendGridasync(IdentityMessage message)
        {
            var msgApresentada = "Clique no LINK para confirmar o email: ";
            var api = Environment.GetEnvironmentVariable("FCMONEY_KEY");

            if (api == null)
            {
                Environment.SetEnvironmentVariable("FCMONEY_KEY", "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");

                api = Environment.GetEnvironmentVariable("FCMONEY_KEY");
            }

            if(!string.IsNullOrEmpty(message.Subject) && message.Subject.Equals("Confirme sua Conta"))
            {
                message.Destination = "igorvinicius1145@gmail.com";
                msgApresentada = "Solicitação de cadastro ao sistema por: " + message.Destination + "para autorizar o acesso clique no Link"; 
            }


            var client = new SendGridClient(api);
            var from = new EmailAddress("igor_vinicios100@hotmail.com", "Admin do FCMONEY");
            var to = new EmailAddress(message.Destination, message.Destination);
            var subject = message.Subject;
            var plainTextContent = "plainTextContent";
            var htmlContent = msgApresentada + message.Body;
            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                plainTextContent,
                htmlContent
                );

            var response = await client.SendEmailAsync(msg);

            /*
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new MailAddress("igorvinicius1145@gmail.com", "Admin do Portal");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["ContaDeEmail"], ConfigurationManager.AppSettings["SenhaEmail"]);

            // Criar um transport web para envio de e-mail
            var transportWeb = new Web(credentials);

            // Enviar o e-mail
            if (transportWeb != null)
            {
                var x = transportWeb.DeliverAsync(myMessage);
                return x;
            }
            else
            {
                return Task.FromResult(0);
            }
            */
        }

        // Implementação de e-mail manual
        /*
        private Task SendMail(IdentityMessage message)
        {
            if (ConfigurationManager.AppSettings["Internet"] == "true")
            {
                var text = HttpUtility.HtmlEncode(message.Body);

                var msg = new MailMessage();
                msg.From = new MailAddress("admin@portal.com.br", "Admin do Portal");
                msg.To.Add(new MailAddress(message.Destination));
                msg.Subject = message.Subject;
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Html));
                //igorvinicius1145@gmail.com

                var smtpClient = new SmtpClient("smtp.provedor.com", Convert.ToInt32(587));
                var credentials = new NetworkCredential(ConfigurationManager.AppSettings["ContaDeEmail"],
                    ConfigurationManager.AppSettings["SenhaEmail"]);
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = true;
                smtpClient.Send(msg);
            }

            return Task.FromResult(0);
        }
        */
    }
}