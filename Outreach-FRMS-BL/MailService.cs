using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Outreach_FRMS_Model;
using Outreach_FRMS_Utility;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Outreach_FRMS_BL
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailToUserAsync(Users model)
        {
            //var email = new MimeMessage();
            //email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            //email.To.Add(MailboxAddress.Parse(model.EmailId));
            //email.Subject = model.Businessdetails.BusinessName + " " + "Registration Update";
            //var builder = new BodyBuilder();
            //builder.HtmlBody = "Hi "+model.FirstName+" "+model.LastName + ","+ "<br><br>" + "We have received the request for adding "+ model.Businessdetails.BusinessName + " on the Favs application.We are currently reviewing the documents you have shared with us for verification and once the review process is done, we will update you with the response."+"<br><br>"+" Thank you for joining the Favs application." ;
            //email.Body = builder.ToMessageBody();
            //using var smtp = new SmtpClient();
            //smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            //smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            //await smtp.SendAsync(email);
            //smtp.Disconnect(true);

            MailMessage mail = new MailMessage();
            mail.To.Add(model.EmailId);
            mail.From = new MailAddress(_mailSettings.Mail);
            mail.Subject = model.BusinessResearch.BusinessName + " " + "Registration Update"; 
            string Body = "Hi " + model.FirstName + " " + model.LastName + "," + "<br><br>" + "We have received the request for adding " + model.BusinessResearch.BusinessName + " on the Favs application.We are currently reviewing the documents you have shared with us for verification and once the review process is done, we will update you with the response." + "<br><br>" + " Thank you for joining the Favs application.";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        public async Task SendEmailToAdminAsync(Users model)
        {
            //var email = new MimeMessage();
            //email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            //email.To.Add(MailboxAddress.Parse(Resources.AdminEmail));
            //email.Subject = model.Businessdetails.BusinessName + " " + "Sent Request for Approval";
            //var builder = new BodyBuilder();
            //builder.HtmlBody = "Hi Team," + "<br><br>" + model.Businessdetails.BusinessName + "has just signed up to the Favs applications and have submitted their business verification documents. Please click here to review the same and take decision on their request.";
            //email.Body = builder.ToMessageBody();
            //using var smtp = new SmtpClient();
            //smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            //smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            //await smtp.SendAsync(email);
            //smtp.Disconnect(true);

            MailMessage mail = new MailMessage();
            mail.To.Add(Resources.AdminEmail);
            mail.From = new MailAddress(_mailSettings.Mail);
            //mail.Subject = model.Businessdetails.BusinessName + " " + "Sent Request for Approval";
            mail.Subject = model.BusinessResearch.BusinessName + " " + "Sent Request for Approval";
            string Body = "Hi Team," + "<br><br>" + model.BusinessResearch.BusinessName + "has just signed up to the Favs applications and have submitted their business verification documents. Please click here to review the same and take decision on their request.";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        public async Task SendEmailForResetPassword(Users model, string message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(model.EmailId);
                mail.From = new MailAddress(_mailSettings.Mail);
                mail.Subject = "Reset Password";
                // string Body = "Hi Team," + "<br><br>" + model.Businessdetails.BusinessName + "has just signed up to the Favs applications and have submitted their business verification documents. Please click here to review the same and take decision on their request.";
                string Body = message;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password); // Enter seders User name and password  
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch(SmtpFailedRecipientsException ex)
            {
            #pragma warning disable CA2200 // Rethrow to preserve stack details
                throw ex;
            #pragma warning restore CA2200 // Rethrow to preserve stack details
            }
           
        }

        public async Task SendEmailToInvitee(ApplicationInvite model, string message)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(model.EmailId);
            mail.From = new MailAddress(_mailSettings.Mail);
            mail.Subject = "Application Invitation";
            string Body = "Hi " + model.FirstName + " " + model.LastName + "," + "<br><br>" + "We have following link to download app and register your self." + "<br><br>" + "https://www.google.com/" + "<br><br>" + "<br><br>" + " "+ message + ""+ "<br><br>" + " Thank you for joining the application.";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
