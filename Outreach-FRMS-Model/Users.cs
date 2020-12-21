using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Outreach_FRMS_Model
{

    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string MobileNo { get; set; }
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid Email id.")]
        public string EmailId { get; set; }
        public string UserApprovalStatus { get; set; }
        public string UserStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ReferralCode { get; set; }
        public Address Address { get; set; }
        public BusinessDetails Businessdetails { get; set; }
        public UserDocumentMapping UserDocumentMapping { get; set; }

        public BusinessResearch BusinessResearch { get; set; }

    }
}
