using Outreach_FRMS_Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Outreach_FRMS_BL
{
    public interface IMailService
    {
        Task SendEmailToUserAsync(Users model);
        Task SendEmailToAdminAsync(Users model);
        Task SendEmailForResetPassword(Users model, string message);
        Task SendEmailToInvitee(ApplicationInvite model, string message);
    }
}
