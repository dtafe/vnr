using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportProfileNewEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string OrgStructureName { get; set; }
        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public string IDNo { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public string Gender { get; set; }
        public string SalaryClassName { get; set; }
        public DateTime? IDDateOfIssue { get; set; }
        public string PlaceOfBirth { get; set; }
        public string PAddress { get; set; }
        public string EducationLevelName { get; set; }
        public string GraduatedLevelName { get; set; }
        public string PAStreet { get; set; }
        public DateTime? DateHire { get; set; }
        public string BranchName { get; set; }
        public string SectionName { get; set; }
        public string TeamName { get; set; }
        public string DepartmentName { get; set; }
        public Guid? OrgStructureID { get; set; }
        public Guid? SalaryClassID { get; set; }
        public Guid? EmpTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
        public string StatusSyn { get; set; }
        public string DateOfBirth { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string CodeCandidate { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }

        public string UnitNameOrg { get; set; }
        public string DivisionNameOrg { get; set; }
        public string DepartmentNameOrg { get; set; }
        public string SectionNameOrg { get; set; }
        public string TeamNameOrg { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public DateTime? datequit { get; set; }
        public string EthnicGroupName { get; set; }
        public string ContractNo { get; set; }
        public DateTime? DateStart { get; set; }
        public string Code { get; set; }
        public string InsuranceAmount { get; set; }
        public DateTime? E_MaleBirth { get; set; }
        public DateTime? E_FeMaleBirth { get; set; }
        public int? E_ProfileCount { get; set; }
        public int? E_ProfileIsWorking { get; set; }
        public int? E_FEMALE { get; set; }
        public int? E_MALE { get; set; }
        public int? ProfileNew { get; set; }
        public int? E_Profile_FEMALE { get; set; }
        public int? E_Profile_MALE { get; set; }

        public string PProvinceName { get; set; }

        public string E_GrossAmount { get; set; }
        public string GrossAmount_Old { get; set; }
        public string GrossAmount_Now { get; set; }

        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string StatusSyn = "StatusSyn";
            public const string DateTo = "DateTo";
            public const string DateOfBirth = "DateOfBirth";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string DateHire = "DateHire";

            public const string IDNo = "IDNo";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string Gender = "Gender";
            public const string SalaryClassName = "SalaryClassName";
            public const string IDDateOfIssue = "IDDateOfIssue";

            public const string PlaceOfBirth = "PlaceOfBirth";
            public const string PAddress = "PAddress";
            public const string EducationLevelName = "EducationLevelName";
            public const string GraduatedLevelName = "GraduatedLevelName";
            public const string PAStreet = "PAStreet";

            //public const string BranchName = "BranchName";
            //public const string TeamName = "TeamName";
            //public const string SectionName = "SectionName";
            //public const string DepartmentName = "DepartmentName";
            public const string EmployeeTypeName = "EmployeeTypeName";


            public const string UnitNameOrg = "UnitNameOrg";
            public const string DivisionNameOrg = "DivisionNameOrg";
            public const string DepartmentNameOrg = "DepartmentNameOrg";
            public const string SectionNameOrg = "SectionNameOrg";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";



            public const string datequit = "datequit";
            public const string EthnicGroupName = "EthnicGroupName";
            public const string ContractNo = "ContractNo";
            public const string DateStart = "DateStart";
            public const string Code = "Code";
            public const string InsuranceAmount = "InsuranceAmount";
            public const string E_MaleBirth = "E_MaleBirth";
            public const string E_FeMaleBirth = "E_FeMaleBirth";
            public const string E_ProfileCount = "E_ProfileCount";
            public const string E_ProfileIsWorking = "E_ProfileIsWorking";
            public const string E_FEMALE = "E_FEMALE";
            public const string E_MALE = "E_MALE";
            public const string ProfileNew = "ProfileNew";
            public const string E_Profile_FEMALE = "E_Profile_FEMALE";
            public const string E_Profile_MALE = "E_Profile_MALE";
            public const string PProvinceName = "PProvinceName";
            public const string E_GrossAmount = "E_GrossAmount";
            public const string GrossAmount_Old = "GrossAmount_Old";
            public const string GrossAmount_Now = "GrossAmount_Now";



        }
    }
}
