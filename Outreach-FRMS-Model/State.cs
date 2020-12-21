using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class State
    {
        public int StateId { get; set; }

        public string StateName { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
        public List<City> City { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
