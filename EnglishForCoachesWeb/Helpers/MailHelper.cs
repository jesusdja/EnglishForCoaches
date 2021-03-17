using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace EnglishForCoachesWeb.Helpers
{
    public static class MailHelper
    {
        private static readonly string credentialUserName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
        private static readonly string sentFrom = System.Configuration.ConfigurationManager.AppSettings["From"];
        private static readonly string pwd = System.Configuration.ConfigurationManager.AppSettings["Password"];
        private static readonly string smtp = System.Configuration.ConfigurationManager.AppSettings["SMTP"];

        public static bool EnviarMail(string mailDestino, string mailDestinoCc, string subject, string body)
        {
            try
            {
                var mail = GenerarMensaje(mailDestino, mailDestinoCc, subject, body);

                SmtpSend(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool EnviarMailConAdjunto(string mailDestino, string mailDestinoCc, string subject, string body, MemoryStream file, string filename)
        {
            try
            {
                var mail = GenerarMensaje(mailDestino, mailDestinoCc, subject, body);

                // Create  the file attachment for this e-mail message.
                Attachment data = new Attachment(file, MediaTypeNames.Text.Html);
                // Add time stamp information for the file.
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = DateTime.Now;
                disposition.ModificationDate = DateTime.Now;
                disposition.ReadDate = DateTime.Now;
                disposition.FileName = filename;
                mail.Attachments.Add(data);

                SmtpSend(mail);
                file.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static MailMessage GenerarMensaje(string mailDestino, string mailCc, string subject, string body)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(sentFrom);
            mail.To.Add(mailDestino);
            if (mailCc != "")
            {
                mail.CC.Add(mailCc);
            }
            mail.Subject = subject;
            mail.Body = body;
            return mail;
        }

        private static void SmtpSend(MailMessage mail)
        {
            SmtpClient SmtpServer = new SmtpClient(smtp);
            //SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(credentialUserName, pwd);
            //SmtpServer.EnableSsl = false;

            SmtpServer.Send(mail);
        }

        public static bool EnviarMailComentario(string codigo,string comentarios)
        {
            string mailDestino = "silvia.moreno@rfef.es";
            string asunto = "Comentarios para el juego #"+codigo;
            string body = "Código: #" + codigo + "\r\n";
             body += "Comentarios: " + comentarios + "\r\n";
            return EnviarMail(mailDestino, "", asunto, body);
        }

    }
}