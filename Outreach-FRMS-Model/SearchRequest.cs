using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class SearchRequest
    {
        public string searchString { get; set; }
        public int PageNumber { get; set; }
        public long? UserId { get; set; }
    }
}
