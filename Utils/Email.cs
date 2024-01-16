using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace Digimon_Project.Utils
{
    public static class Email
    {
        public static void SendMail(string toemail, string subject, string body)
        {
            // http://taoclosedbeta.tk/
            string from = "tamersadventure@gmail.com";
            using (MailMessage mailMessage =
            new MailMessage(new MailAddress(from),
            new MailAddress(toemail)))
            {
                mailMessage.Body = body;
                mailMessage.Subject = subject;
                try
                {
                    SmtpClient SmtpServer = new SmtpClient();
                    SmtpServer.Credentials =
                        new System.Net.NetworkCredential(from, "taok2019+");
                    SmtpServer.Port = 587;
                    SmtpServer.Host = "smtp.gmail.com";
                    SmtpServer.EnableSsl = true;
                    MailMessage mail = new MailMessage();
                    string[] addr = toemail.Split(','); // toemail is a string which contains many email address separated by comma
                    mail.From = new MailAddress(from);
                    byte i;
                    for (i = 0; i < addr.Length; i++)
                        mail.To.Add(addr[i]);
                    mail.Subject = mailMessage.Subject;
                    mail.Body = mailMessage.Body;
                    mail.IsBodyHtml = true;
                    mail.DeliveryNotificationOptions =
                        DeliveryNotificationOptions.OnFailure;
                    //   mail.ReplyTo = new MailAddress(toemail);
                    mail.ReplyToList.Add(toemail);
                    SmtpServer.Send(mail);
                    Debug.Print("Mail Sent to {0}", toemail);
                }
                catch (Exception ex)
                {
                    string exp = ex.ToString();
                    Debug.Print("Mail Not Sent ... and ther error is " + exp);
                }
            }
        }
    }
}
