using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_KPIBonusItemEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> ShopID { get; set; }
        public Nullable<System.Guid> KPIBonusID { get; set; }
        public Nullable<double> Value { get; set; }
        public string ShopName { get; set; }
        public string KPIBonusName { get; set; }
    }
}
