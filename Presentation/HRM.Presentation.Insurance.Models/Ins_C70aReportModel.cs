using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Insurance.Models
{
    public class Ins_C70aReportModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_STT)]
        public string Stt { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Pregnancy_DateOfBirth)]
        public string Birthday { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsNo)]
        public string SocialInsNo { get; set; }
        public Double SalaryInsurance { get; set; }
        public string MonthJoinInsurance { get; set; }
        public int LeaveInMonth { get; set; }
        public int LeaveInYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Amount)]
        public Double Money { get; set; }
        public string StatusGroup { get; set; }
        [DisplayName(ConstantDisplay.HRM_Common_Search_Duration)]
        public DateTime DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Common_DateEnd)]
        public DateTime DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Notes)]
        public string Notes { get; set; }
        public string Notes1 { get; set; }
        public string FemaleBirthYear { get; set; }
        public string MaleBirthYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_Group_GroupName)]
        public string GroupName { get; set; }
        public string BeginSocialInsIssueDate { get; set; }
        public string CodeParentOrgLevel { get; set; }

        public int TotalRow { get; set; }

        public DateTime SDateStart { get; set; }
        public DateTime SDateEnd { get; set; }
        public DateTime SMonth { get; set; }
        public string CutOffDurationName { get; set; }

        public partial class FieldNames
        {
            public const string Stt = "Stt";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Birthday = "Birthday";
            public const string SocialInsNo = "SocialInsNo";
            public const string SalaryInsurance = "SalaryInsurance";
            public const string MonthJoinInsurance = "MonthJoinInsurance";
            public const string LeaveInMonth = "LeaveInMonth";
            public const string LeaveInYear = "LeaveInYear";
            public const string Money = "Money";
            public const string StatusGroup = "StatusGroup";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Notes = "Notes";
            public const string Notes1 = "Notes1";
            public const string FemaleBirthYear = "FemaleBirthYear";
            public const string MaleBirthYear = "MaleBirthYear";
            public const string Status = "Status";
            public const string GroupName = "GroupName";
            public const string BeginSocialInsIssueDate = "BeginSocialInsIssueDate";
            public const string CodeParentOrgLevel = "CodeParentOrgLevel";
        }
    }

    public class Ins_SearchC70AReportModel
    {
        public Guid? CutOffDurationID { get; set; }
        public DateTime DateMonth { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string OrgStructureID { get; set; }
        public string CutOffDurationName { get; set; }

        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
    }
    
}
