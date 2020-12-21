using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class ApplicationInvite
    {
        public long InvitorUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RefrenceToken { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string IsReferenceUsed { get; set; }
        public long ReferenceUserId { get; set; }
    }
}
