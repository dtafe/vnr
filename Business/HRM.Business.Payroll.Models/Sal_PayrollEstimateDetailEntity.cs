using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_PayrollEstimateDetailEntity : HRMBaseModel
    {
        public Nullable<System.Guid> PayrollEstimateID { get; set; }


        public Nullable<System.Guid> OrgStructureID { get; set; }


        public Nullable<double> QuantityEmp { get; set; }
    
        public Nullable<double> SalaryAverage { get; set; }

        public Nullable<double> LeaveUnpaid { get; set; }
   
        public Nullable<double> OvertimeNormal { get; set; }

        public Nullable<double> OvertimeHoliday { get; set; }
    
        public Nullable<double> OvertimeWeekend { get; set; }
 
        public Nullable<double> OvertimeNightNormal { get; set; }
     
        public Nullable<double> OvertimeNightHoliday { get; set; }

        public Nullable<double> OvertimeNightWeekend { get; set; }

        public string OrgStructureName { get; set; }

        public string OrgStructureCode { get; set; }

        public Double? AmountHour { get; set; }
        public Double? AmountLeaveDay { get; set; }
        public Double? AmountOvertime { get; set; }
        public Double? AmountTotal { get; set; }
        public Double? RateAdjust { get; set; }
        public Double? BonusBudget { get; set; }
        public Nullable<float> LeaveUnpaidView { get; set; }

        public partial class FieldNames
        {

            public const string QuantityEmp = "QuantityEmp";
            public const string SalaryAverage = "SalaryAverage";
            public const string LeaveUnpaid = "LeaveUnpaid";
            public const string OvertimeNormal = "OvertimeNormal";
            public const string OvertimeHoliday = "OvertimeHoliday";
            public const string OvertimeWeekend = "OvertimeWeekend";
            public const string OvertimeNightNormal = "OvertimeNightNormal";
            public const string OvertimeNightHoliday = "OvertimeNightHoliday";
            public const string OvertimeNightWeekend = "OvertimeNightWeekend";

            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureCode = "OrgStructureCode";
            public const string AmountHour = "AmountHour";
            public const string AmountLeaveDay = "AmountLeaveDay";
            public const string AmountOvertime = "AmountOvertime";
            public const string AmountTotal = "AmountTotal";
            public const string RateAdjust = "RateAdjust";
            public const string BonusBudget = "BonusBudget";
        }
    }
}
