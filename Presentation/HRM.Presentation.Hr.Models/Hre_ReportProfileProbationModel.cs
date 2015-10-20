using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportProfileProbationModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeOrg)]
        public string CodeOrg { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_JobTitleName)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public DateTime? DateHire { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateEndProbation)]
        public DateTime? DateEndProbation { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProbExtendDate)]
        public DateTime? ProbExtendDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_IDNo)]
        public DateTime? IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDDateOfIssue)]
        public DateTime? IDDateOfIssue { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDPlaceOfIssue)]
        public string IDPlaceOfIssue { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_WorkPlace)]
        public string WorkPlace { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AllowLimitOvertime_DateStart)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AllowLimitOvertime_DateEnd)]
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public Guid ExportID { get; set; }

        public string Gender { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public partial class FieldNames
        {
            public const string Email = "Email";
            public const string Cellphone = "Cellphone";
            public const string Gender = "Gender";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string CodeOrg = "CodeOrg";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string DateHire = "DateHire";
            public const string DateEndProbation = "DateEndProbation";
            public const string ProbExtendDate = "ProbExtendDate";
            public const string IDNo = "IDNo";
            public const string IDDateOfIssue = "IDDateOfIssue";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string WorkPlace = "WorkPlace";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
        }
    }
}
