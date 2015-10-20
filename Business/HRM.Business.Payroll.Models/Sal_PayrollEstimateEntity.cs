using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_PayrollEstimateEntity : HRMBaseModel
    {
        public Nullable<System.Guid> CutOffDurationID { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureIDs { get; set; }
        public Nullable<System.Guid> PayrollGroupID { get; set; }
        public string OrgStructureType { get; set; }
        public string StatusEmp { get; set; }
        public Nullable<double> RateAdjust { get; set; }
        public Nullable<double> BonusBudget { get; set; }
        public string OrgStructureName { get; set; }
        public DateTime? MonthStart { get; set; }
        public DateTime? MonthEnd { get; set; }

    }
}
