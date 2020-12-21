using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class Suggestion
    {
        public long UserId { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Comments { get; set; }
    }
}
