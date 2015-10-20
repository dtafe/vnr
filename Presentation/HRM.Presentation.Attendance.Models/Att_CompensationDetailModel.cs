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
    public class Att_CompensationDetailModel : BaseViewModel
    {
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
        public Nullable<int> MonthInYear { get; set; }
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
            public const string OvertimeInMonth = "OvertimeInMonth";
            public const string TotalLeaveBef = "TotalLeaveBef";
            public const string TotalOvertimeBef = "TotalOvertimeBef";
            public const string Remain = "Remain";
            public const string InitAvailable = "InitAvailable";
            public const string CompBankCode = "CompBankCode";
            public const string MonthInYear = "MonthInYear";
        }
    }

    public class Att_CompensationDetailSearchModel : BaseViewModel
    {
        public int Year { get; set; }
        public string OrgStructureID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public System.Guid ExportId { get; set; }
    }
    public class Att_CompensationDetailSearchV2Model
    {
        public int Year { get; set; }
        public string OrgStructureID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }

    }
    public class Att_CompensationDetailViewSeachModel
    {
        public string ProfileID { get; set; }
        public int? Year { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
