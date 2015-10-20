using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_RelativesModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Relatives_IsRegisterAtCompany)]
        public Nullable<bool> IsRegisterAtCompany { get; set; }
        [MaxLength(50)]
        public string RelativesCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_DependantID)]
        public Guid? DependantID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_RelativeName)]
        [MaxLength(150)]
        public string RelativeName { get; set; }

       // [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_HR_Relatives_YearOfBirth)]
        public string YearOfBirth { get; set; }
        public DateTime? YearOfBirths { get; set; }

       
        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_HR_Relatives_Gender)]
        public string Gender { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_JobTitleID)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_Career)]
        [MaxLength(150)]
        public string Career { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_IDNo)]
        [MaxLength(50)]
        public string IDNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_Notes)]
        public string Notes { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_DependantName)]
        [MaxLength(150)]
        public string DependantName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_NationalityID)]
        public Guid? NationalityID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_CountryName)]
        [MaxLength(150)]
        public string CountryName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_RelativeTypeName)]
        [MaxLength(150)]
        public string RelativeTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relatives_RelativeTypeID)]
        public Guid? RelativeTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public List<Guid?> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        [MaxLength(100)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        [MaxLength(100)]
        public string PositionName { get; set; }
        private Guid _id = Guid.Empty;
        public Guid Relative_ID
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
        [DisplayName(ConstantDisplay.HRM_HR_Relatives_CheckAddDependant)]
        public bool? CheckAddDependant { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_MonthOfEffect)]
        public DateTime? MonthOfEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_MonthOfExpiry)]
        public DateTime? MonthOfExpiry { get; set; }
        public string EmpCodeRelativesName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Relatives_Address)]
        public string Address { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

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
            public const string Address = "Address";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string ID = "ID";
            public const string RelativesCode = "RelativesCode";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string DependantID = "DependantID";
            public const string DependantName = "DependantName";
            public const string RelativeName = "RelativeName";
            public const string YearOfBirth = "YearOfBirth";
            public const string YearOfBirths = "YearOfBirths";
            public const string Gender = "Gender";
            public const string CountryID = "CountryID";
            public const string CountryName = "CountryName";
            public const string JobTitleID = "JobTitleID";
            public const string JobTitleName = "JobTitleName";
            public const string Career = "Career";
            public const string Notes = "Notes";
            public const string RelativeTypeName = "RelativeTypeName";
            public const string RelativeTypeID = "RelativeTypeID";
            public const string PositionID = "PositionID";
            public const string PositionName = "PositionName";
            public const string OrgStructureName = "OrgStructureName";
            public const string Relative_ID = "Relative_ID";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }

    public class Hre_RelativesSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_SoftSkill_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string PositionID = "PositionID";
            public const string JobTitleID = "JobTitleID";
            public const string OrgStructureID = "OrgStructureID";
        }
    }

    public class Hre_RelativesMultiModel
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string RelativeName { get; set; }
    }
}
