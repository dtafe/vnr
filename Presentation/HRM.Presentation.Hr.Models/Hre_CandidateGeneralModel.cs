using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_CandidateGeneralModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_ProfileID)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_RankRateID)]
        public Nullable<System.Guid> RankRateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_RankRateID)]
        public string RankRateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_SalaryClassID)]
        public Nullable<System.Guid> SalaryClassID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_SalaryClassID)]
        public string SalaryClassName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_BasicSalary)]
        public Nullable<double> BasicSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_WorkPlaceID)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_WorkPlaceID)]
        public string WorkPlaceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_ContractTypeID)]
        public Nullable<System.Guid> ContractTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_ContractTypeID)]
        public string ContractTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_EnteringDate)]
        public Nullable<System.DateTime> EnteringDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_OrgStructureID)]
        public Nullable<System.Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_OrgStructureID)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_GradeAttendanceID)]
        public Nullable<System.Guid> GradeAttendanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_GradeAttendanceID)]
        public string GradeAttendanceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_GradePayrollID)]
        public Nullable<System.Guid> GradePayrollID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_GradePayrollID)]
        public string GradePayrollName { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public Nullable<System.Guid> JobTitleID { get; set; }
        public string JobTitleName { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public string PositionName { get; set; }
        public Nullable<System.Guid> AllowanceID1 { get; set; }
        public string AllowanceID1Name { get; set; }
        public Nullable<System.Guid> AllowanceID2 { get; set; }
        public string AllowanceID2Name { get; set; }
        public Nullable<System.Guid> AllowanceID3 { get; set; }
        public string AllowanceID3Name { get; set; }
        public Nullable<System.Guid> AllowanceID4 { get; set; }
        public string AllowanceID4Name { get; set; }
        public Nullable<System.Guid> AllowanceID5 { get; set; }
        public string AllowanceID5Name { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<double> Allowance4 { get; set; }
        public Nullable<double> Allowance5 { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string CurrencyName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileName = "ProfileName";
            public const string GradePayrollName = "GradePayrollName";
            public const string GradeAttendanceName = "GradeAttendanceName";
            public const string OrgStructureName = "OrgStructureName";
            public const string EnteringDate = "EnteringDate";
            public const string ContractTypeName = "ContractTypeName";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string BasicSalary = "BasicSalary";
            public const string SalaryClassName = "SalaryClassName";
            public const string RankRateName = "RankRateName";
            public const string IDNo = "IDNo";
            public const string Gender = "Gender";
            public const string CodeEmp = "CodeEmp";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
    public class Hre_CandidateGeneralSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_OrgStructureID)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_RankRateID)]
        public Nullable<System.Guid> RankRateID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_GradeAttendanceID)]
        public Nullable<System.Guid> GradeAttendanceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hre_CandidateGeneral_GradePayrollID)]
        public Nullable<System.Guid> GradePayrollID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Dependant_IDNo)]
        public string IDNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Dependant_Gender)]
        public string Gender { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
    }
}
