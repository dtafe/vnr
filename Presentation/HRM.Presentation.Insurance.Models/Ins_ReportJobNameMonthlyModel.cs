using System;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;

namespace HRM.Presentation.Insurance.Models
{
    public class Ins_ReportJobNameMonthlyModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CostCentreID)]
        public Guid? CostCentreID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CostCentreName)]
        public string CostCentreName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_MonthSearch)]
        public DateTime? MonthYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_IsSocialInsurance)]
        public bool? IsSocialInsurance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_IsHealthInsurance)]
        public bool? IsHealthInsurance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_IsUnEmpInsurance)]
        public bool? IsUnEmpInsurance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_MoneySocialInsurance)]
        public double? MoneySocialInsurance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_MoneyUnEmpInsurance)]
        public double? MoneyUnEmpInsurance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_SalaryInsurance)]
        public double? MoneySocialInsurancePreMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_SalaryInsurance)]
        public double? SalaryInsurance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_MoneyHealthInsurance)]
        public double? MoneyHealthInsurance { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileIds { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SalaryClassID)]
        public string Rank { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public DateTime? DateHire { get; set; }
         [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
        public Guid? WorkPlaceID { get; set; }

         [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
         public List<Guid> WorkPlaceIDs { get; set; }


         [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
         public Guid? SocialInsPlaceID { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
         public List<Guid> SocialInsPlaceIDs { get; set; }    

        //   [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_Allowance1)]
        public double? Allowance1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_Allowance2)]
        public double? Allowance2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_Allowance3)]
        public double? Allowance3 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_Allowance4)]
        public double? Allowance4 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_AllowanceAdditional)]
        public double? AllowanceAdditional { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_AmountChargeIns)]
        public double? AmountChargeIns { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_SocialInsEmpRate)]
        public double? SocialInsEmpRate { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_HealthInsEmpRate)]
        public double? HealthInsEmpRate { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_UnemployEmpRate)]
        public double? UnemployEmpRate { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_SocialInsComRate)]
        public double? SocialInsComRate { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_HealthInsComRate)]
        public double? HealthInsComRate { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_UnemployComRate)]
        public double? UnemployComRate { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_SocialInsEmpAmount)]
        public double? SocialInsEmpAmount { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_HealthInsEmpAmount)]
        public double? HealthInsEmpAmount { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_UnemployEmpAmount)]
        public double? UnemployEmpAmount { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_SocialInsComAmount)]
        public double? SocialInsComAmount { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_HealthInsComAmount)]
        public double? HealthInsComAmount { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_UnemployComAmount)]
        public double? UnemployComAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_JobName)]
        public string JobName { get; set; }
        public bool? IsDecreaseWorkingDays { get; set; }

        public string Location { get; set; }
        public bool? IsExport { get; set; }
        public Guid ExportId { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string CostCentreID = "CostCentreID";
            public const string CostCentreName = "CostCentreName ";
            public const string MoneySocialInsurancePreMonth = "MoneySocialInsurancePreMonth";
            public const string MonthYear = "MonthYear";
            public const string IsSocialInsurance = "IsSocialInsurance";
            public const string IsHealthInsurance = "IsHealthInsurance";
            public const string IsUnEmpInsurance = "IsUnEmpInsurance";
            public const string MoneySocialInsurance = "MoneySocialInsurance";
            public const string MoneyUnEmpInsurance = "MoneyUnEmpInsurance";
            public const string SalaryInsurance = "SalaryInsurance";
            public const string MoneyHealthInsurance = "MoneyHealthInsurance";
            public const string ProfileName = "ProfileName";
            public const string Allowance1 = "Allowance1";
            public const string Allowance2 = "Allowance2";
            public const string Allowance3 = "Allowance3";
            public const string Allowance4 = "Allowance4";
            public const string AllowanceAdditional = "AllowanceAdditional";
            public const string AmountChargeIns = "AmountChargeIns";
            public const string SocialInsEmpRate = "SocialInsEmpRate";
            public const string HealthInsEmpRate = "HealthInsEmpRate";
            public const string UnemployEmpRate = "UnemployEmpRate";
            public const string SocialInsComRate = "SocialInsComRate";
            public const string HealthInsComRate = "HealthInsComRate";
            public const string UnemployComRate = "UnemployComRate";
            public const string SocialInsEmpAmount = "SocialInsEmpAmount";
            public const string HealthInsEmpAmount = "HealthInsEmpAmount";
            public const string UnemployEmpAmount = "UnemployEmpAmount";
            public const string SocialInsComAmount = "SocialInsComAmount";
            public const string HealthInsComAmount = "HealthInsComAmount";
            public const string UnemployComAmount = "UnemployComAmount";
            public const string JobName = "JobName";
            public const string IsDecreaseWorkingDays = "IsDecreaseWorkingDays";
            public const string Location = "Location";
            public const string Rank = "Rank";
            public const string DateHire = "DateHire";
            public const string WorkPlaceID = "WorkPlaceID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
    public class Ins_ReportJobNameMonthlySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_MonthSearch)]
        public DateTime? MonthYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_WorkPlaceID)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
