using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_DependantMultiEntity
    {
        public Guid Id { get; set; }
        [MaxLength(150)]
        public string DependantName { get; set; }
    }
    public class Hre_DependantEntity : HRMBaseModel
    {
        public Nullable<bool> IsRegisterAtCompany { get; set; }
        public DateTime? DateQuit { get; set; }
        public int TotalRow { get; set; }
        public Guid ProfileID { get; set; }
        [MaxLength(150)]
        public string DependantName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public Guid? RelationID { get; set; }
        public double? ReductionAmount { get; set; }
        public DateTime? MonthOfEffect { get; set; }
        public DateTime? MonthOfExpiry { get; set; }
        public DateTime? MonthOriginate { get; set; }
        [MaxLength(50)]
        public string CodeTax { get; set; }
        [MaxLength(50)]
        public string IDNo { get; set; }
        public Guid? NationlatyID { get; set; }
        [MaxLength(50)]
        public string No { get; set; }
        [MaxLength(50)]
        public string BookNo { get; set; }
        public Guid? ProvinceID { get; set; }
        public Guid? DistrictID { get; set; }
        public Guid? VillageID { get; set; }
        public Guid? CountryID { get; set; }
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [MaxLength(150)]
        public string RelativeTypeName { get; set; }
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        [MaxLength(150)]
        public string PositionName { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }

        private Guid _id = Guid.Empty;
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
        public string Taxcode { get; set; }

        public Nullable<bool> DeclareFile { get; set; }
        public Nullable<bool> BirthCertificate { get; set; }
        public Nullable<bool> HouseHold { get; set; }
        public Nullable<bool> MarriageLicenses { get; set; }
        public Nullable<bool> StudyingSchools { get; set; }
        public Nullable<bool> LaborDisabled { get; set; }
        public Nullable<bool> NurturingObligations { get; set; }
        public Nullable<bool> IDCardNo { get; set; }

    }
}
