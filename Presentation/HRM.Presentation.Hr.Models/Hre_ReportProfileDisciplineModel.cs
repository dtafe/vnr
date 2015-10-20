using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportProfileDisciplineModel : BaseViewModel
    {
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
        public string Status { get; set; }
        public string DisciplineLevel { get; set; }
        public string ViolationExplain { get; set; }
        public string NameEntityName { get; set; }
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
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateOfExpiry)]
        public DateTime? DateOfExpiry { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateOfIssuance)]
        public DateTime? DateOfIssuance { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DecisionNo)]
        public string DecisionNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateOfEffective)]
        public DateTime? DateOfEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DescriptionOfViolation)]
        public string DescriptionOfViolation { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportProfileDiscipline_DisciplineType)]
        public Nullable<System.Guid> DisciplineTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportProfileDiscipline_DisciplineType)]
        public string DisciplineTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DisciplinedDecidingOrgID)]
        public int? DisciplinedDecidingOrgID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DisciplinedDecidingOrgID)]
        public string DisciplinedDecidingOrgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_ContentOfDiscipline)]
        public string ContentOfDiscipline { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateEndOfViolation)]
        public DateTime? DateEndOfViolation { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateOfViolation)]
        public DateTime? DateOfViolation { get; set; }
        public string DisciplinedTypesSuggestName { get; set; }
        public string TermsViolation { get; set; }
        public Nullable<double> AmountOfFine { get; set; }
        public string PersonApproved { get; set; }
        public string Notes { get; set; }
        public string DisciplinedTypesName { get; set; }
        public string StatusSynView { get; set; }
        public int? count { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsChecked { get; set; }
        public Guid ExportID { get; set; }
        //public bool IsCreateTemplate { get; set; }
        //public bool IsCreateTemplateForDynamicGrid { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public partial class FieldNames
        {
            public const string StatusSynView = "StatusSynView";
            public const string count = "count";
            public const string DisciplinedTypesName = "DisciplinedTypesName";
            public const string Notes = "Notes";
            public const string DisciplineLevelView = "DisciplineLevelView";
            public const string StatusView = "StatusView";
            public const string PersonApproved = "PersonApproved";
            public const string DisciplinedTypesSuggestName = "DisciplinedTypesSuggestName";
            public const string AmountOfFine = "AmountOfFine";
            public const string TermsViolation = "TermsViolation";
            public const string NameEntityName = "NameEntityName";
            public const string ViolationExplain = "ViolationExplain";
            public const string DateOfEffective = "DateOfEffective";
            public const string DateReview = "DateReview";
            public const string DateHire = "DateHire";
            public const string DateOfBirth = "DateOfBirth";
            public const string Status = "Status";
            public const string DisciplineLevel = "DisciplineLevel";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string DateOfExpiry = "DateOfExpiry";
            public const string DateOfIssuance = "DateOfIssuance";
            public const string DecisionNo = "DecisionNo";
            public const string DateOfEffect = "DateOfEffect";
            public const string DescriptionOfViolation = "DescriptionOfViolation";
            public const string DisciplineTypeName = "DisciplineTypeName";
            public const string DisciplinedDecidingOrgName = "DisciplinedDecidingOrgName";
            public const string ContentOfDiscipline = "ContentOfDiscipline";
            public const string DateEndOfViolation = "DateEndOfViolation";
            public const string DateOfViolation = "DateOfViolation";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }

    }
}
