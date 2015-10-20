using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_RevenueRecordEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> ShopID { get; set; }
        public Nullable<System.Guid> KPIBonusID { get; set; }
        public Nullable<System.DateTime> Month { get; set; }
        public string Type { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Note { get; set; }
        public string ShopName { get; set; }
        public string KPIBonusName { get; set; }
        public double? Taget { get; set; }
        public double? Actual { get; set; }
        public double? Value { get; set; }
        public int NoShiftLeader { get; set; }
        public double? ShopActual { get; set; }
        public double? ShopTaget { get; set; }

    }
}
