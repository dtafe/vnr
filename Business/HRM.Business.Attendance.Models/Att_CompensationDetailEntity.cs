using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_CompensationDetailEntity : HRMBaseModel
    {
        public Nullable<int> MonthInYear { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<System.DateTime> MonthYear { get; set; }
        public Nullable<int> MonthBeginInYear { get; set; }
        public Nullable<int> MonthResetInitAvailable { get; set; }
        public Nullable<int> MonthStartProfile { get; set; }
        public Nullable<double> Available { get; set; }
        public Nullable<double> LeaveInMonth { get; set; }
        public Nullable<double> OvertimeInMonth { get; set; }
        public Nullable<double> TotalLeaveBef { get; set; }
        public Nullable<double> TotalOvertimeBef { get; set; }
        public Nullable<double> Remain { get; set; }
        public Nullable<double> InitAvailable { get; set; }
        public string CompBankCode { get; set; }
    }
}
