using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class AuthenticateRequest
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string EmailId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
