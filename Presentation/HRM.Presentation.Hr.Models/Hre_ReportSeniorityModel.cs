using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportSeniorityModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public DateTime? DateHire { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportSeniority_YearSeniority)]
        public int? YearSeniority { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportSeniority_MonthSeniority)]
        public int? MonthSeniority { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportSeniority_GrossAmount)]
        public string GrossAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractTypeID)]
        public string ContractTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractTypeID)]
        public string ContractTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateOfEffect)]
        public DateTime? DateOfEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportSeniority_CurrencyList)]
        public string CurrencyList { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Notes)]
        public string Notes { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsChecked { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportSeniority_DateSeniority)]
        public DateTime? DateSeniority { get; set; }
        public double? AnnualYearRest { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string DateHire = "DateHire";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string YearSeniority = "YearSeniority";
            public const string MonthSeniority = "MonthSeniority";
            public const string GrossAmount = "GrossAmount";
            public const string ContractTypeID = "ContractTypeID";
            public const string ContractTypeName = "ContractTypeName";
            public const string DateOfEffect = "DateOfEffect";
            public const string CurrencyList = "CurrencyList";
            public const string Notes = "Notes";
            public const string AnnualYearRest = "AnnualYearRest";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";
        }
    }
}
