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
    public class Att_AnnualLeaveModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public string ProfileIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_Year)]
        public int Year { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_MonthStart)]
        public int MonthStart { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_InitAnlValue)]
        public double InitAnlValue { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_InitSickValue)]
        public double InitSickValue { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_InitSaveSickValue)]
        public double InitSaveSickValue { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_AnlValueLastYear)]
        public double? AnlValueLastYear { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_ExpireAnlValueLastYear)]
        public DateTime? ExpireAnlValueLastYear { get; set; }

       
        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_AnlMonthReset)]
        [MaxLength(150)]
        public string AnlMonthReset { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_MonthResetAnlOfBeforeYear)]
        public int? MonthResetAnlOfBeforeYear { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualDetailView_Year)]
        public Nullable<int> YearAnnual { get; set; }

        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string ProfileID = "ProfileID";
            public const string Year = "Year";
            public const string MonthStart = "MonthStart";
            public const string InitAnlValue = "InitAnlValue";
            public const string InitSickValue = "InitSickValue";
            public const string InitSaveSickValue = "InitSaveSickValue";
            public const string AnlValueLastYear = "AnlValueLastYear";
            public const string ExpireAnlValueLastYear = "ExpireAnlValueLastYear";
            public const string AnlMonthReset = "AnlMonthReset";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string MonthResetAnlOfBeforeYear = "MonthResetAnlOfBeforeYear";
        }
    }

    public class Att_AnnualLeaveSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Uniform_ProfileID)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_Year)]
        public int? Year { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string OrgStructureID = "OrgStructureID";
            public const string Year = "Year";
        }
    }
}
