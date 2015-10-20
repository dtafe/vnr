using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_ItemForShopEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> ShopID { get; set; }
        public string ShopName { get; set; }
        public Nullable<System.Guid> ItemID { get; set; }
        public string ItemName { get; set; }
        public Nullable<double> Target { get; set; }
        public Nullable<double> Actual { get; set; }
        public Nullable<System.DateTime> Month { get; set; }
        public string Note { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? IsPass { get; set; }
    }
}
