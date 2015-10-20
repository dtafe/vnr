using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_ProductiveEntity : HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.Guid> ProductID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<System.Guid> CutOffDurationID { get; set; }
        public string Note { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public Nullable<System.Guid> ProductItemID { get; set; }
        public Guid? OrgStructureID { get; set; }

        public string ProfileName { get; set; }
        public string ProductName { get; set; }
        public string ProductItemName { get; set; }
        public string CurrencyName { get; set; }
    }
}
