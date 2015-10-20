using System;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;

namespace HRM.Presentation.Insurance.Models
{
    public class Ins_ReportInsCostCenterMonthlyModel : BaseViewModel
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
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_MoneySocialInsurance)]
        public double? MoneySocialInsurance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_MoneyUnEmpInsurance)]
        public double? MoneyUnEmpInsurance { get; set; }
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
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_CountProfile)]
        public int? ProfileAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
        public List<Guid> WorkPlaceIDs { get; set; }
        public bool? IsExport { get; set; }
        public Guid ExportId { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
        public Guid? SocialInsPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
        public List<Guid> SocialInsPlaceIDs { get; set; }
         [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_IsPayback)]
        public bool? IsPayback { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string CostCentreID = "CostCentreID";
            public const string CostCentreName = "CostCentreName ";
            public const string MonthYear = "MonthYear";
            public const string MoneySocialInsurance = "MoneySocialInsurance";
            public const string MoneyUnEmpInsurance = "MoneyUnEmpInsurance";
            public const string SalaryInsurance = "SalaryInsurance";
            public const string MoneyHealthInsurance = "MoneyHealthInsurance";
            public const string ProfileName = "ProfileName";
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
            public const string ProfileAmount = "ProfileAmount";
            public const string WorkPlaceID = "WorkPlaceID";
            public const string SocialInsPlaceIDs = "SocialInsPlaceIDs";
            public const string IsPayback = "IsPayback";
        }
    }
    public class Ins_ReportInsCostCenterMonthlySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_MonthSearch)]
        public DateTime? MonthYear { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public string CostCentreName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
