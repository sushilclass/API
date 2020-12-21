using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class InvitationRequest
    {
#pragma warning disable CA2227 // Collection properties should be read only
        public List<ContactsMappingModel> MyContact { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning disable CA2227 // Collection properties should be read only
        public List<RequestInvites> Invitor { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning disable CA2227 // Collection properties should be read only
        public List<RequestInvites> Invitee { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        public long UserId { get; set; }
    }
}
