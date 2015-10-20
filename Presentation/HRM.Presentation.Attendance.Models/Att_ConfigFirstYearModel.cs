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
  
    public class Att_ConfigFirstYearModel : BaseViewModel
    {
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
        }
    }
    public class Att_ConfigFirstYearSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_ConfigFirstYear_NumerDayChange)]
        public double? NumerDayChange { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_ConfigFirstYear_YearChange)]
        public int YearChange { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_ConfigFirstYear_TypeAnnual)]
        public string TypeAnnual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_ConfigFirstYear_YearChange)]
        public int Year { get; set; }
        [DisplayName(ConstantDisplay.HRM_ConvertRemainAnnualLeaveDayTo)]
        public bool ConvertRemainAnnualLeaveDayTo { get; set; }
        public Guid? AllowanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_PaidMonthOfSalary)]
        public DateTime PaidMonthOfSalary { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
