using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class Invite
    {
        public long InviteeUserId { get; set; }
        public long InvitorUserId { get; set; }
        public string RequestStatus { get; set; }
        public string IsRequestAccepted { get; set; }
        public string RequestToken { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ApprovalRequestDate { get; set; }
        public string ApprovedBy { get; set; }
    }
}
