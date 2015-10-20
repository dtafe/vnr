using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Attendance.Models
{
    public class Att_CompensationConfigEntity : HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> MonthBeginInYear { get; set; }
        public Nullable<double> InitAvailable { get; set; }
        public Nullable<int> MonthResetInitAvailable { get; set; }
        public Nullable<int> MonthStartProfile { get; set; }
        public string CompBankCode { get; set; }
    }
}
