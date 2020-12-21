using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class UserDetail
    {
        public int UserId { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string UserApprovalStatus { get; set; }
        public string UserStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ReferralCode { get; set; }
        public string UserImage { get; set; }
        // public Address Address { get; set; }
        public AddressDetails AddressDetails { get; set; }
      //  public BusinessDetails Businessdetails { get; set; }
#pragma warning disable CA2227 // Collection properties should be read only
        public List<UserDocumentMapping> UserDocumentMapping { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
    
}
