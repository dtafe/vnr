using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_DisciplineEntity : HRMBaseModel
    {
        public string DepartmentName { get; set; }
        public string DeptName { get; set; }
        public string BranchName { get; set; }
        public string GroupName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string DivisionName { get; set; }
        public Nullable<System.Guid> OrgStructureID1 { get; set; }
        public int? DisciplineCount { get; set; }
        public int TotalRow { get; set; }
        public Guid ProfileID { get; set; }
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        public DateTime? DateOfEffective { get; set; }
        public DateTime? DateOfInsurance { get; set; }
        [MaxLength(50)]
        public string DecisionNo { get; set; }
        //public DateTime? DateOfViolation { get; set; }
        [MaxLength(1000)]
        public string DescriptionOfViolation { get; set; }
        [MaxLength(1000)]
        public string ContentOfDiscipline { get; set; }
        public DateTime? DateOfExpiry { get; set; }
        public int? DisciplinedDecidingOrgID { get; set; }
        public Guid? DisciplineTypeID { get; set; }
        public string DisciplinedTypesName { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }
        [MaxLength(1000)]
        public string ViolationRule { get; set; }
        [MaxLength(1000)]
        public string Witnesses { get; set; }
        [MaxLength(1000)]
        public string ViolationRuleDescription { get; set; }
        [MaxLength(1000)]
        public string ViolationRuleDescriptionEN { get; set; }
        [MaxLength(1000)]
        public string ResultOfRecidivism { get; set; }
        [MaxLength(150)]
        public string TimeOfViolation { get; set; }
        public Guid? UserApprovedID { get; set; }
        public Guid? UserHeadID { get; set; }
        [MaxLength(1000)]
        public string ViolationExplain { get; set; }
        public Guid? UnionAgentID { get; set; }
        public Guid? HeadAgentID { get; set; }
        public Guid? ManagementAgentID { get; set; }
        [MaxLength(1000)]
        public string PlaceViolation { get; set; }
        public DateTime? DateEndOfViolation { get; set; }
        public string Attachment { get; set; }
        public double? PercentDeduction { get; set; }
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        [MaxLength(150)]
        public string PositionName { get; set; }
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        public List<int?> OrgStructureID { get; set; }
        [MaxLength(150)]
        public string UserApprovedName { get; set; }
        [MaxLength(150)]
        public string UserHeadName { get; set; }
        [MaxLength(150)]
        public string UnionAgentName { get; set; }
        [MaxLength(150)]
        public string HeadAgentName { get; set; }

        public string Code { get; set; }

        private Guid _id = Guid.Empty;
        public Guid Discipline_ID
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

        public Nullable<System.Guid> OrgID { get; set; }
        public string OrgName { get; set; }
        public Nullable<System.Guid> DisciplinedTypesSuggestID { get; set; }
        public string DisciplinedTypesSuggestName { get; set; }
        public Nullable<System.Guid> DisciplineResonID { get; set; }
        public string DisciplineResonName { get; set; }
        public string DisciplineLevel { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> DateReview { get; set; }
        public string TermsViolation { get; set; }
        public Nullable<double> AmountOfFine { get; set; }
        public string PersonApproved { get; set; }

        public DateTime? DateQuit { get; set; }
        public string DateQuitFormat { get; set; }
        public int? YearOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? DayOfBirth { get; set; }

        public double? MonthWorking { get; set; }
        public double? YearWorking { get; set; }

        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string BankNameCustomer { get; set; }
        public string BankBrandName { get; set; }
        public string Email { get; set; }
        public string DateNow { get; set; }
        public string DateNow_Day { get; set; }
        public string DateNow_Month { get; set; }
        public string DateNow_Year { get; set; }

        public DateTime? DateHire { get; set; }
        public string DateHireFormat { get; set; }
        public string StatusView { get; set; }
        public string StatusSynView { get; set; }
        public string DisciplineLevelView { get; set; }

        public string DateOfEffectiveFormat { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public string GenderView { get; set; }
        public Nullable<System.DateTime> DateOfIssuance { get; set; }
        public string DateOfIssuanceFormat { get; set; }
        public Nullable<System.DateTime> DateOfViolation { get; set; }
        public string DateOfViolationFormat { get; set; }
    }
}
