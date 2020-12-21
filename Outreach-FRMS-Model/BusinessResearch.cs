using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public  class BusinessResearch
    {
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public long? BusinessCity { get; set; }
        public long? BusinessState { get; set; }
        public string BusinessZipcode { get; set; }
        public long? BusinessCountry { get; set; }
        public decimal BusinessRating { get; set; }
        public string BusinessOpenNow { get; set; }
        public string BusinessPhotoReference { get; set; }
        public string BusinessKeywords { get; set; }
        public string BusinessTypes { get; set; }
        public string BusinessLink { get; set; }
        public long? BusinessCategoryId { get; set; }
        public long? UserId { get; set; }
        public long? RestaurantId { get; set; }

    }
}
