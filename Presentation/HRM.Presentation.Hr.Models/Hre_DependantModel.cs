using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_DependantMultiModel 
    {
        public Guid ID { get; set; }
        public string DependantName { get; set; }
        public int TotalRow { get; set; }
    }
    public class Hre_DependantSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_DependantName)]
        [MaxLength(150)]
        public string DependantName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_RelationID)]
        public Guid? RelationID { get; set; }
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Hre_DependantModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Relatives_IsRegisterAtCompany)]
        public Nullable<bool> IsRegisterAtCompany { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Dependant_ProfileID)]
        public Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_DependantName)]
        [MaxLength(150)]
        public string DependantName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_DateOfBirth)]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_RelationID)]
        public Guid? RelationID { get; set; }
        [MaxLength(150)]
        public string RelativeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_ReductionAmount)]
        public double? ReductionAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_MonthOfEffect)]
        public DateTime? MonthOfEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_MonthOfExpiry)]
        public DateTime? MonthOfExpiry { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_MonthOriginate)]
        public DateTime? MonthOriginate { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_CodeTax)]
        [MaxLength(50)]
        public string CodeTax { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_IDNo)]
        [MaxLength(50)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_NationlatyID)]
        public Guid? NationlatyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_No)]
        [MaxLength(50)]
        public string No { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_BookNo)]
        [MaxLength(50)]
        public string BookNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_ProvinceID)]
        public Guid? ProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_DistrictID)]
        public Guid? DistrictID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_VilageID)]
        public Guid? VillageID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_CountryID)]
        public Guid? CountryID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [MaxLength(150)]
        public string RelativeTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        public List<Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobTitleID { get; set; }
        private Guid _id = Guid.Empty;
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_CompleteDate)]
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public Guid Dependant_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_CodeTax)]
        public string Taxcode { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_DeclareFile)]
        public Nullable<bool> DeclareFile { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_BirthCertificate)]
        public Nullable<bool> BirthCertificate { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_HouseHold)]
        public Nullable<bool> HouseHold { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_MarriageLicenses)]
        public Nullable<bool> MarriageLicenses { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_StudyingSchools)]
        public Nullable<bool> StudyingSchools { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_LaborDisabled)]
        public Nullable<bool> LaborDisabled { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_NurturingObligations)]
        public Nullable<bool> NurturingObligations { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_IDCardNo)]
        public Nullable<bool> IDCardNo { get; set; }
        public partial class FieldNames
        {
            public const string IDCardNo = "IDCardNo";
            public const string NurturingObligations = "NurturingObligations";
            public const string LaborDisabled = "LaborDisabled";
            public const string StudyingSchools = "StudyingSchools";
            public const string MarriageLicenses = "MarriageLicenses";
            public const string HouseHold = "HouseHold";
            public const string BirthCertificate = "BirthCertificate";
            public const string DeclareFile = "DeclareFile";
            public const string Taxcode = "Taxcode";
            public const string CompleteDate = "CompleteDate";
            public const string ProfileID = "ProfileID";
            public const string ID = "ID";
            public const string DependantName = "DependantName";
            public const string DateOfBirth = "DateOfBirth";
            public const string Gender = "Gender";
            public const string RelationID = "RelationID";
            public const string ReductionAmount = "ReductionAmount";
            public const string MonthOfEffect = "MonthOfEffect";
            public const string MonthOfExpiry = "MonthOfExpiry";
            public const string MonthOriginate = "MonthOriginate";
            public const string CodeTax = "CodeTax";
            public const string IDNo = "IDNo";
            public const string NationlatyID = "NationlatyID";
            public const string No = "No";
            public const string BookNo = "BookNo";
            public const string ProvinceID = "ProvinceID";
            public const string DistrictID = "DistrictID";
            public const string VillageID = "VillageID";
            public const string CountryID = "CountryID";
            public const string ProfileName = "ProfileName";
            public const string RelativeTypeName = "RelativeTypeName";

            public const string CodeEmp = "CodeEmp";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
}
