using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class SearchData
    {
#pragma warning disable CA2227 // Collection properties should be read only
        public List<FavsSearchData> myfavslist { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning disable CA2227 // Collection properties should be read only
        public List<FavsSearchData> mycontactfavslist { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning disable CA2227 // Collection properties should be read only
        public List<FavsSearchData> othersfavslist { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
    public class FavsSearchData
    {
        public long RestaurantId { get; set; }
        public int BusinessCategoryId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public double Rating { get; set; }
        public string OpenNow { get; set; }
        public string PhotoReference { get; set; }
        public string Types { get; set; }
        public string Link { get; set; }
    }

   
}
