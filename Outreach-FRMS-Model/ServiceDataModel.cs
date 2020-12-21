using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class ServiceDataModel
    {
        public long RestaurantId { get; set; }
        public long BusinessCategoryId { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public double Rating { get; set; }
        public string OpenNow { get; set; }
        public string PhotoReference { get; set; }
        public string KeyWords { get; set; }
        public string Link { get; set; }
        public string BusinessType { get; set; }
    }
}
