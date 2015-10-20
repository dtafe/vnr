using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportProfileDisciplineEntity : HRMBaseModel
    {
        public string StatusSynView { get; set; }
        public int? count { get; set; }
        public string StatusView { get; set; }
        public string DisciplineLevelView { get; set; }
        public string BranchName { get; set; }
        public string GroupName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string DivisionName { get; set; }
        public Nullable<System.Guid> OrgStructureID1 { get; set; }
        public int? DisciplineCount { get; set; }
        public Guid ProfileID { get; set; }
        public DateTime? DateOfEffective { get; set; }
        public string DateOfBirth { get; set; }
        public DateTime? DateReview { get; set; }
        public DateTime? DateHire { get; set; }
        public string NameEntityName { get; set; }
        public string Status { get; set; }
        public string DisciplineLevel { get; set; }
        public string ViolationExplain { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public DateTime? DateOfExpiry { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DateOfIssuance { get; set; }
        public string DecisionNo { get; set; }
        public DateTime? DateOfEffect { get; set; }
        public string DescriptionOfViolation { get; set; }
        public Nullable<System.Guid> DisciplineTypeID { get; set; }
        public string DisciplineTypeName { get; set; }
        public int? DisciplinedDecidingOrgID { get; set; }
        public string DisciplinedDecidingOrgName { get; set; }
        public string ContentOfDiscipline { get; set; }
        public DateTime? DateEndOfViolation { get; set; }
        public DateTime? DateOfViolation { get; set; }
        public string DisciplinedTypesSuggestName { get; set; }
        public string TermsViolation { get; set; }
        public Nullable<double> AmountOfFine { get; set; }
        public string PersonApproved { get; set; }
        public string Notes { get; set; }
        public string DisciplinedTypesName { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

    }
}
