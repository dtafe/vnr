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
    public class Ins_InsuranceReportD02Model : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsNo)]
        public string SocialInsNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Pregnancy_DateOfBirth)]
        public string DateOfBirth { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public DateTime? HireDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_EndProbationDate)]
        public DateTime? EndProbationDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_EndDate)]
        public DateTime? EndDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_Position_JobTitle)]
        public string Position { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_JobTitle)]
        public string JobTitle { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_OldBasicSalary)]
        public double? OldBasicSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_NewBasicSalary)]
        public double? NewBasicSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_TitleMonthNow)]
        public string TitleMonthNow { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_MonthNow)]
        public DateTime? MonthNow { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_FromMonth)]
        public DateTime? FromMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_ToMonth)]
        public DateTime? ToMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_RateAdditional)]
        public string RateAdditional { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_NotCardHealth)]
        public string NotCardHealth { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_RateBHXH)]
        public double? RateBHXH { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_RateBHYT)]
        public double? RateBHYT { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_RateBHTN)]
        public double? RateBHTN { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_TotalSalalryCom)]
        public double? TotalSalalryCom { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_CountProfile)]
        public double? CountProfile { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_Status1)]
        public string Status1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_DateMonth)]
        public string DateMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_STT)]
        public string STT { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_STT)]
        public int ItemOrder { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_BOLD)]
        public string BOLD { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_CodeParentOrgLevel)]
        public string CodeParentOrgLevel { get; set; }
        public int TotalRow { get; set; }

        public string SearchNoteType { get; set; }
        public string SearchStatus { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public bool? IsExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_MonthYear)]
        public DateTime? MonthYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_DateTo)]
        public DateTime? DateTo { get; set; }
        public Guid ExportId { get; set; }
        public string JobName { get; set; }
        /// <summary> Số thứ tự theo nhóm tăng,giảm... (I,II,I.1,II.1) </summary>
        public string OrderGroup { get; set; }

        #region search tat ca,tang,giam
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_Type_All)]
        public bool? All { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_Type_Increase)]
        public bool? Increase { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_Type_Decrease)]
        public bool? Descrease { get; set; }
        #endregion

        #region phu cap
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_Allowance1)]
        public double? Allowance1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_Allowance2)]
        public double? Allowance2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_Allowance3)]
        public double? Allowance3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_Allowance4)]
        public double? Allowance4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_AllowanceAdditional)]
        public double? AllowanceAdditional { get; set; }
        #endregion

        [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
        public Guid? WorkPlaceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
        public List<Guid> WorkPlaceIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
        public Guid? SocialInsPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
        public List<Guid> SocialInsPlaceIDs { get; set; }    


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string SocialInsNo = "SocialInsNo";
            public const string Gender = "Gender";
            public const string DateOfBirth = "DateOfBirth";
            public const string HireDate = "HireDate";
            public const string EndProbationDate = "EndProbationDate";
            public const string EndDate = "EndDate";
            public const string Position = "Position";
            public const string JobTitle = "JobTitle";
            public const string JobName = "JobName";
            public const string OldBasicSalary = "OldBasicSalary";
            public const string NewBasicSalary = "NewBasicSalary";
            public const string TitleMonthNow = "TitleMonthNow";
            public const string MonthNow = "MonthNow";
            public const string FromMonth = "FromMonth";
            public const string ToMonth = "ToMonth";
            public const string RateAdditional = "RateAdditional";
            public const string NotCardHealth = "NotCardHealth";
            public const string RateBHXH = "RateBHXH";
            public const string RateBHYT = "RateBHYT";
            public const string RateBHTN = "RateBHTN";
            public const string TotalSalalryCom = "TotalSalalryCom";
            public const string CountProfile = "CountProfile";
            public const string Status = "Status";
            public const string Comment = "Comment";
            public const string Status1 = "Status1";
            public const string DateMonth = "DateMonth";
            public const string STT = "STT";
            public const string BOLD = "BOLD";
            public const string CodeParentOrgLevel = "CodeParentOrgLevel";
            public const string Allowance1 = "Allowance1";
            public const string Allowance2 = "Allowance2";
            public const string Allowance3 = "Allowance3";
            public const string Allowance4 = "Allowance4";
            public const string AllowanceAdditional = "AllowanceAdditional";
            public const string SocialInsPlaceID = "SocialInsPlaceID";
            public const string ItemOrder = "ItemOrder";
        }
    }
    public class Ins_InsuranceReportD02SearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_MonthYear)]
        public DateTime? MonthYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public Guid? WorkPlaceID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
    }

}
