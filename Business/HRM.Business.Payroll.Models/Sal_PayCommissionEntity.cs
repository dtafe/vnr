using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_PayCommissionEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.Guid> CutoffDurationID { get; set; }
        public Nullable<System.DateTime> MonthYear { get; set; }
        public string Status { get; set; }
    }
}
