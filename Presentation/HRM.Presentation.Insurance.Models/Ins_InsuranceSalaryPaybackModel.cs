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
    /// <summary>Truy Lĩnh bảo hiểm</summary>
    public class Ins_InsuranceSalaryPaybackModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_ProfileID)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_MonthYear)]
        public DateTime? MonthYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_FromMonthEffect)]
        public DateTime? FromMonthEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_ToMonthEffect)]
        public DateTime? ToMonthEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_InsSalary)]
        public double? InsSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_InsSalaryPayBack)]
        public double? InsSalaryPayBack { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_InsSalaryAdjust)]
        public double? InsSalaryAdjust { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_AmoutHDTIns)]
        public double? AmoutHDTIns { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_AmoutHDTInsPayBack)]
        public double? AmoutHDTInsPayBack { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_JobtitleName)]
        public string JobtitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_IsSocialIns)]
        public bool? IsSocialIns { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_IsMedicalIns)]
        public bool? IsMedicalIns { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_IsUnemploymentIns)]
        public bool? IsUnemploymentIns { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_SocialInsEmpRate)]
        public double? SocialInsEmpRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_HealthInsEmpRate)]
        public double? HealthInsEmpRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_UnemployEmpRate)]
        public double? UnemployEmpRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_SocialInsComRate)]
        public double? SocialInsComRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_HealthInsComRate)]
        public double? HealthInsComRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_UnemployComRate)]
        public double? UnemployComRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_TypeID)]
        public Guid? TypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_IsCallPayBack)]
        public bool? IsCallPayBack { get; set; }
        public string CommentName { get; set; }
        public string TypeCode { get; set; }
        public string StatusCode { get; set; }
        public string TypeName { get; set; }
        public string StatusName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
        public Guid? SocialInsPlaceID { get; set; }
        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string MonthYear = "MonthYear";
            public const string FromMonthEffect = "FromMonthEffect";
            public const string ToMonthEffect = "ToMonthEffect";
            public const string InsSalary = "InsSalary";
            public const string InsSalaryPayBack = "InsSalaryPayBack";
            public const string InsSalaryAdjust = "InsSalaryAdjust";
            public const string AmoutHDTIns = "AmoutHDTIns";
            public const string AmoutHDTInsPayBack = "AmoutHDTInsPayBack";
            public const string JobtitleName = "JobtitleName";
            public const string IsSocialIns = "IsSocialIns";
            public const string IsMedicalIns = "IsMedicalIns";
            public const string IsUnemploymentIns = "IsUnemploymentIns";
            public const string SocialInsEmpRate = "SocialInsEmpRate";
            public const string HealthInsEmpRate = "HealthInsEmpRate";
            public const string UnemployEmpRate = "UnemployEmpRate";
            public const string SocialInsComRate = "SocialInsComRate";
            public const string HealthInsComRate = "HealthInsComRate";
            public const string UnemployComRate = "UnemployComRate";
            public const string Note = "Note";
            public const string TypeID = "TypeID";
            public const string Comment = "Comment";
            public const string IsCallPayBack = "IsCallPayBack";
            public const string SocialInsPlaceID = "SocialInsPlaceID";
        }
    }
    public class Ins_InsuranceSalaryPaybackSearchModel
    {

        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_MonthYear)]
        public DateTime? MonthYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }      
         [DisplayName(ConstantDisplay.HRM_Ins_InsuranceSalary_Payback_IsCallPayBack)]
        public bool? IsCallPayBack { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
