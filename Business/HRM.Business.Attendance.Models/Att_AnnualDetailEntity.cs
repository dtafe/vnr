using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_AnnualDetailEntity : HRMBaseModel
    {
        //ngay phep tich luy Available + InitAvailable
        public Nullable<double> AvailableTotal { get; set; }
        //da nghi den cuoi thang truoc TotalLeaveBefFromInitAvailable + TotalLeaveBef
        public Nullable<double> LeaveBefTotal { get; set; }
        //nghi phep trong thang LeaveInMonthFromInitAvailable+LeaveInMonth
        public Nullable<double> LeaveInMonthTotal { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<System.DateTime> MonthYear { get; set; }
        public Nullable<int> MonthBeginInYear { get; set; }
        public Nullable<int> MonthResetInitAvailable { get; set; }
        public Nullable<int> MonthStartProfile { get; set; }
        public Nullable<double> Available { get; set; }
        public Nullable<double> LeaveInMonth { get; set; }
        public Nullable<double> TotalLeaveBef { get; set; }
        public Nullable<double> Remain { get; set; }
        public Nullable<double> InitAvailable { get; set; }
        public Nullable<double> LeaveInMonthFromInitAvailable { get; set; }
        public Nullable<double> TotalLeaveBefFromInitAvailable { get; set; }
        public Nullable<bool> IsHaveResetInitAvailable { get; set; }

        public string OrgStructureName { get; set; }
        public Nullable<System.DateTime> DateHire { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string Type { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public Nullable<int> MonthInYear { get; set; }

    }

}
