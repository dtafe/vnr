using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_RelativesEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        [MaxLength(50)]
        public string RelativesCode { get; set; }
        public Guid ProfileID { get; set; }
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        public Guid? DependantID { get; set; }
        [MaxLength(150)]
        public string RelativeName { get; set; }
        [MaxLength(50)]
        public string YearOfBirth { get; set; }
        [MaxLength(50)]
        public string YearOfLose { get; set; }
        [MaxLength(50)]
        public string Gender { get; set; }
        [MaxLength(150)]
        public string PAStreet { get; set; }
        public Guid? NationalityID { get; set; }
        public Guid? RelativeTypeID { get; set; }
        [MaxLength(100)]
        public string OrgStructureName { get; set; }

        [MaxLength(100)]
        public string PositionName { get; set; }
        [MaxLength(150)]
        public string RelativeTypeName { get; set; }
        public bool? IsColleaggue { get; set; }
        public Guid? JobTitleID { get; set; }
        [MaxLength(150)]
        public string OldHusbandWife { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }
        [MaxLength(150)]
        public string CodeEmpEx { get; set; }
        [MaxLength(150)]
        public string Career { get; set; }
        [MaxLength(150)]
        public string AgencyWork { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(50)]
        public string IDNo { get; set; }
        [MaxLength(50)]
        public string CodeTax { get; set; }
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        [MaxLength(150)]
        public string DependantName { get; set; }
        [MaxLength(150)]
        public string CountryName { get; set; }
        public List<Guid?> OrgStructureID { get; set; }
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
        public bool? CheckAddDependant { get; set; }

        public DateTime? MonthOfEffect { get; set; }
        public DateTime? MonthOfExpiry { get; set; }
        public string EmpCodeRelativesName { get; set; }
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

        public Nullable<bool> DeclareFile { get; set; }
        public Nullable<bool> BirthCertificate { get; set; }
        public Nullable<bool> HouseHold { get; set; }
        public Nullable<bool> MarriageLicenses { get; set; }
        public Nullable<bool> StudyingSchools { get; set; }
        public Nullable<bool> LaborDisabled { get; set; }
        public Nullable<bool> NurturingObligations { get; set; }
        public Nullable<bool> IDCardNo { get; set; }
        public Nullable<bool> IsRegisterAtCompany { get; set; }
    }
    public class Hre_RelativesMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string RelativeName { get; set; }
    }
}
