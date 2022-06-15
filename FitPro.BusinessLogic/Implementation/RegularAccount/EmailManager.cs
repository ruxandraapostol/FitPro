using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public static class EmailManager
    {
        public static void SendEmail(string lnkHref, string emailAddressTo)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(emailAddressTo);
            mail.From = new MailAddress("fitproapplicense@gmail.com");
            mail.Subject = "FitPro Forgot Password";
            mail.Body = "<b>Password Reset Link. </b><br/>" + lnkHref;
            mail.IsBodyHtml = true;

            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("fitproapplicense@gmail.com", "Parola_12345");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            };
        }
    }
}
