using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Category.Models
{
    public class Cat_KPIBonusItemModel : BaseViewModel
    {
        public Nullable<System.Guid> ShopID { get; set; }
        public Nullable<System.Guid> KPIBonusID { get; set; }
        public Nullable<double> Value { get; set; }
        public string ShopName { get; set; }
          public string KPIBonusName { get; set; }
          public partial class FieldNames
          {
              public const string ShopID = "ShopID";
              public const string KPIBonusID = "KPIBonusID";
              public const string Value = "Value";
              public const string ShopName = "ShopName";
              public const string KPIBonusName = "KPIBonusName";
          }
    }

    public class Cat_KPIBonusItemSearchMoel
    {
        public Nullable<System.Guid> ShopID { get; set; }
        public Nullable<System.Guid> KPIBonusID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    
}
