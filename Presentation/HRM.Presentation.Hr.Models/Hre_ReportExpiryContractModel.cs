using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportExpiryContractModel : BaseViewModel
    {
        public Nullable<System.DateTime> DateExtend { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureCode)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureCode)]
        public string CodeOSN { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public string PositionName { get; set; }
        public string StatusEvaluation { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        public string ContractTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        public Guid? ContractTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateSigned)]
        public DateTime? DateSigned { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        [DataType(DataType.Date)]
        public DateTime? DateHire { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractNextID)]
        public string NextContractName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_AppendixCT)]
        public bool AppendixCT { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_HasMonthOrther)]
        public bool HasMonthOrther { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_ExcludeQuitEmp)]
        public bool ExcludeQuitEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_ExcludeHavingNextContract)]
        public bool ExcludeHavingNextContract { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public DateTime? NextContractDateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        public DateTime? NextContractDateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_IsEvaluation)]
        public bool IsEvaluation { get; set; }

        public string ContractResult { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_RankRateID_NextContract)]
        public Guid? RankDetailForNextContract { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_RankRateID_NextContract)]
        public string RankDetailForNextContractIds { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ClassRateID)]
        public Guid? SalaryClassID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ClassRateID)]
        public string ClassRateName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_Salary)]
        public Nullable<double> Salary { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ClassRateID)]

        public string WorkPlaceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNo)]
        [MaxLength(50)]
        public string ContractNo { get; set; }
        public Guid ExportID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_EvaType)]

        public string EvaType { get; set; }

        public string StatusView { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_MonthSenior)]
        public double? MonthSenior { get; set; }

        public string AllowanceID1 { get; set; }
        public string AllowanceID1Name { get; set; }
        public string Allowance1 { get; set; }

        public string AllowanceID2 { get; set; }
        public string AllowanceID2Name { get; set; }
        public string Allowance2 { get; set; }

        public string AllowanceID3 { get; set; }
        public string AllowanceID3Name { get; set; }
        public string Allowance3 { get; set; }

        public string AllowanceID4 { get; set; }
        public string AllowanceID4Name { get; set; }
        public string Allowance4 { get; set; }

        public string ValueFields { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateSigned)]
        public DateTime? DateSignedFrom { get; set; }
        public DateTime? DateSignedTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEndNextContract)]
        public DateTime? DateEndNextContract { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_TypeOfPass)]
        public string TypeOfPass { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Remark)]
        public string Remark { get; set; }
        public string AnnexCode { get; set; }
        public string contrctextend1 { get; set; }
        public string contrctextend2 { get; set; }
        public string contrctextend3 { get; set; }
        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureID = "OrgStructureID";
            public const string Code = "Code";
            public const string CodeOSN = "CodeOSN";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string ContractTypeName = "ContractTypeName";
            public const string DateSigned = "DateSigned";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string NextContractName = "NextContractName";
            public const string NextContractDateStart = "NextContractDateStart";
            public const string NextContractDateEnd = "NextContractDateEnd";
            public const string RankDetailForNextContract = "RankDetailForNextContract";
            public const string RankDetailForNextContractIds = "RankDetailForNextContractIds";
            public const string SalaryClassID = "SalaryClassID";
            public const string ClassRateName = "ClassRateName";
            public const string DateHire = "DateHire";
            public const string MonthSenior = "MonthSenior";
            public const string Salary = "Salary";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string ContractNo = "ContractNo";
            public const string Status = "Status";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string StatusView = "StatusView";
            public const string AllowanceID1 = "AllowanceID1";
            public const string AllowanceID1Name = "AllowanceID1Name";
            public const string Allowance1 = "Allowance1";
            public const string AllowanceID2 = "AllowanceID2";
            public const string AllowanceID2Name = "AllowanceID2Name";
            public const string Allowance2 = "Allowance2";
            public const string AllowanceID3 = "AllowanceID3";
            public const string AllowanceID3Name = "AllowanceID3Name";
            public const string Allowance3 = "Allowance3";
            public const string AllowanceID4 = "AllowanceID4";
            public const string AllowanceID4Name = "AllowanceID4Name";
            public const string Allowance4 = "Allowance4";
            public const string DateEndNextContract = "DateEndNextContract";
            public const string TypeOfPass = "TypeOfPass";
            public const string Remark = "Remark";
            public const string DateExtend = "DateExtend";

        }
    }
}
