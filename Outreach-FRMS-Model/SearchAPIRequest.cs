using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class SearchAPIRequest
    {
        public string Key { get; set; }
        public string QuerySearch { get; set; }
        public string PageToken { get; set; }
        public int BusinessCategory { get; set; }
    }
}
