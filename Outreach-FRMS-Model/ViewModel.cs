using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class ViewModel
    {
        public int UserId { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }       
        public string ConfirmPassword { get; set; }     
        public string MobileNo { get; set; }      
        public string EmailId { get; set; }
        public string UserApprovalStatus { get; set; }
        public string UserStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ReferralCode { get; set; }
        public string UserImage { get; set; }

        public string Street { get; set; }
        public int? City { get; set; }
        public int? State { get; set; }
        public int? Country { get; set; }
        public string ZipCode { get; set; }

        public string BusinessName { get; set; }
        public string BusinessCategory { get; set; }
        public string BusinessStreet { get; set; }
        public int? BusinessCity { get; set; }
        public int? BusinessState { get; set; }
        public int? BusinessCountry { get; set; }
        public string BusinessZipCode { get; set; }
        public string BusinessPhoneNo { get; set; }
        public string BusinessRegistrationNo { get; set; }

        public string DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentImage { get; set; }
       //public string ErrorMessage { get; set; }


    }
}
