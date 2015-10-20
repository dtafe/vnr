using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_PayrollEstimateModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_CutOffDurationID)]

        public Nullable<System.Guid> CutOffDurationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_OrgStructureID)]

        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_PayrollGroupID)]

        public Nullable<System.Guid> PayrollGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Estimate_OrgStructureType)]

        public string OrgStructureType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_StatusSyn)]

        public string StatusEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Estimate_RateAdjust)]

        public Nullable<double> RateAdjust { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Estimate_BounusBudget)]

        public Nullable<double> BonusBudget { get; set; }
        public string OrgStructureName { get; set; }

        public DateTime? MonthStart { get; set; }
        public DateTime?  MonthEnd { get; set; }
        public Double? AmountHour { get; set; }
        public Double? AmountLeaveDay { get; set; }
        public Double? AmountOvertime { get; set; }
        public Double? AmountTotal { get; set; }
        public Guid ExportId { get; set; }


    }
}
