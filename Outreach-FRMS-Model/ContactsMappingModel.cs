using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class ContactsMappingModel
    {
       // public long ContactMappingId { get; set; }
        public long UserId { get; set; }
        public long ConnectedFriendsId { get; set; }
      //  public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}
