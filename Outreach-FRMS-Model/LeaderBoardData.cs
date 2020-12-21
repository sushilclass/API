using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class LeaderBoardData
    {
#pragma warning disable CA2227 // Collection properties should be read only
        public List<MyFavsList> myFavs { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning disable CA2227 // Collection properties should be read only
        public List<MyFavsList> myContactFavs { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning disable CA2227 // Collection properties should be read only
        public List<MyFavsList> others { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        public long UserId { get; set; }
    }
}
