using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_AnnualDetailModel : BaseViewModel
    {
        //ngay phep tich luy Available + InitAvailable
        public Nullable<double> AvailableTotal{ get; set; }
        //da nghi den cuoi thang truoc TotalLeaveBefFromInitAvailable + TotalLeaveBef
        public Nullable<double> LeaveBefTotal { get; set; }
        //nghi phep trong thang LeaveInMonthFromInitAvailable+LeaveInMonth
        public Nullable<double> LeaveInMonthTotal { get; set; }
        public Nullable<int> MonthInYear { get; set; }
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


        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Year = "Year";
            public const string MonthYear = "MonthYear";
            public const string MonthBeginInYear = "MonthBeginInYear";
            public const string MonthResetInitAvailable = "MonthResetInitAvailable";
            public const string MonthStartProfile = "MonthStartProfile";
            public const string Available = "Available";
            public const string LeaveInMonth = "LeaveInMonth";
            public const string TotalLeaveBef = "TotalLeaveBef";
            public const string Remain = "Remain";
            public const string InitAvailable = "InitAvailable";
            public const string LeaveInMonthFromInitAvailable = "LeaveInMonthFromInitAvailable";
            public const string TotalLeaveBefFromInitAvailable = "TotalLeaveBefFromInitAvailable";
            public const string IsHaveResetInitAvailable = "IsHaveResetInitAvailable";
            public const string OrgStructureName = "OrgStructureName";
            public const string DateHire = "DateHire";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string MonthInYear = "MonthInYear";
            public const string AvailableTotal = "AvailableTotal";
            public const string LeaveBefTotal = "LeaveBefTotal";
            public const string LeaveInMonthTotal = "LeaveInMonthTotal";
        }
    }

    public class Att_AnnualDetailSearchModel : BaseViewModel
    {
        public int Year { get; set; }
        public string OrgStructureID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string PayrollGroup { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public System.Guid ExportId { get; set; }
    }

    public class Att_AnnualDetailSearchV2Model 
    {
        public int Year { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string OrgStructureID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
    }
    public class Att_AnnualDetailViewSearchMode
    {
        public string ProfileID { get; set; }
        public Nullable<int> YearAnnual { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
