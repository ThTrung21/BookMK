using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Service
{
    public class MailService
    {
        public static string Generate6Digits()
        {
            Random r = new Random();
            int code = r.Next(100000, 999999);
            return code.ToString();
        }
        public static void SendEmail(string recipientEmail, string subject, string body)
        {
            // sender email
            string senderEmail = "recovery.bookmk@gmail.com";
            string password = "acix gzej jsbo evyt\r\n";

            // structure smpt info
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, password);

            // create MailMessage để cấu hình email
            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail, subject, body);
            mailMessage.IsBodyHtml = true;

            try
            {
                // Gửi email
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
