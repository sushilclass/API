using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class Address
    {
        public string Street { get; set; }
        public long? City { get; set; }
        public long? State { get; set; }
        public long? Country { get; set; }
        public string ZipCode { get; set; }
    }
}
