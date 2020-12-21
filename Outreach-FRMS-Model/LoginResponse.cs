using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }       
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string ProfileImage { get; set; }
        public string ReferralCode { get; set; }
        public Address Address { get; set; }
       // public AddressDetails AddressDetails { get; set; }
        public BusinessDetails Businessdetails { get; set; }
       // public UserDocumentMapping UserDocumentMapping { get; set; }
    }
}
