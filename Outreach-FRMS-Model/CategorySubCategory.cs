using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class CategorySubCategory
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
#pragma warning disable CA2227 // Collection properties should be read only
        public List<SubCategory> subCategory { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning disable CA2227 // Collection properties should be read only
        public List<Types> types { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
    public class SubCategory
    {
        public string SubCategoryName { get; set; }
        public int SubCategoryId { get; set; }
    }
    public class Types
    {
        public string type { get; set; }
        public int typeId { get; set; }
    }
}
