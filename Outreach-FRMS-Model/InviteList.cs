using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class InviteList
    {
        public long InviteeUserId { get; set; }
        public long InvitorUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
