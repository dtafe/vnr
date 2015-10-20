using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportSumarySeniorHDTProfileModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Dept)]
        //public string Dept { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_Code)]
        public Guid? HDTJobGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_Code)]
        public string HDTJobGroupCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_HDTJobGroupName)]
        public string HDTJobGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_MonthInsuranceType4)]
        public int? MonthInsuranceType4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_MonthInsuranceType5)]
        public int? MonthInsuranceType5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateFrom)]
        public DateTime? DateSearchFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateTo)]
        public DateTime? DateSearchTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_Code)]
        public string CodeSearch { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public Guid ExportId { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Dept)]
        public string Dept { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Unit)]
        public string Unit { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Part)]
        public string Part { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Line)]
        public string Line { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Session)]
        public string Session { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string Dept = "Dept";
            public const string Unit = "Unit";
            public const string Part = "Part";
            public const string Line = "Line";
            public const string Session = "Session";
            public const string ProfileName = "ProfileName";
            public const string HDTJobGroupCode = "HDTJobGroupCode";
            public const string HDTJobGroupName = "HDTJobGroupName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string MonthInsuranceType4 = "MonthInsuranceType4";
            public const string MonthInsuranceType5 = "MonthInsuranceType5";
            public const string DateSearchFrom = "DateSearchFrom";
            public const string DateSearchTo = "DateSearchTo";
            public const string CodeSearch = "CodeSearch";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
}
