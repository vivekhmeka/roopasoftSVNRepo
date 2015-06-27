using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Century21.Models
{
    class DBHelper
    {
        public static bool SendAgentNotificationEmail(string toEmail, string body)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("vivekh.meka@gmail.com", "vivekh530");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
            MailMessage msg = new MailMessage();
            msg.Subject = "New Customer Request";
            msg.From = new MailAddress("vivekh.meka@gmail.com");
            msg.Body = body;
            msg.IsBodyHtml = true;
            toEmail = "pradeep.das@roopasoft.com";
            msg.To.Add(new MailAddress(toEmail));
            client.Send(msg);
            return true;

        }
    }
}
