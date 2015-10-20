using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_PayCommissionModel : BaseViewModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.Guid> CutoffDurationID { get; set; }
        public Nullable<System.DateTime> MonthYear { get; set; }
        public string Status { get; set; }
    }
}
