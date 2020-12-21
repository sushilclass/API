using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class BusinessDetails
    {
        public string BusinessName { get; set; }
        public string BusinessCategory { get; set; }
        public Address Address { get; set; }
        public AddressDetails AddressDetails { get; set; }
        public string BusinessPhoneNo { get; set; }
        public string BusinessRegistrationNo { get; set; }
    }
}
