using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
   public class Country
    {
        public int CountryId { get; set; }

        public string CountryName { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
        public List<State> State { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }

    public class CountryStateCity
    {
      //  public List<ci> State { get; set; }
    }
}
