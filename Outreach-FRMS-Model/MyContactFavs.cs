using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class MyContactFavs
    {
        public long UserId { get; set; }
        public long MyContactId { get; set; }
        public long RestaurantId { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
