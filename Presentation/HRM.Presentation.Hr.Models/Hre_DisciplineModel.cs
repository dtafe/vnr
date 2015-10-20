using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_DisciplineModel : BaseViewModel
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

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateOfEffective)]
        public DateTime? DateOfEffective { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateOfIssuance)]
        public DateTime? DateOfInsurance { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DecisionNo)]
        [MaxLength(50)]
        public string DecisionNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateOfViolation)]
        public DateTime? DateOfViolation { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DescriptionOfViolation)]
        [MaxLength(1000)]
        public string DescriptionOfViolation { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_ContentOfDiscipline)]
        [MaxLength(1000)]
        public string ContentOfDiscipline { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateOfExpiry)]
        public DateTime? DateOfExpiry { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DisciplinedDecidingOrgID)]
        public int? DisciplinedDecidingOrgID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_Notes)]
        [MaxLength(1000)]
        public string Notes { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_ViolationRule)]
        [MaxLength(1000)]
        public string ViolationRule { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_ViolationRuleDescription)]
        [MaxLength(1000)]
        public string ViolationRuleDescription { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_ViolationRuleDescriptionEN)]
        [MaxLength(1000)]
        public string ViolationRuleDescriptionEN { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_ResultOfRecidivism)]
        [MaxLength(1000)]
        public string ResultOfRecidivism { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_TimeOfViolation)]
        [MaxLength(150)]
        public string TimeOfViolation { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_UserApprovedID)]
        public Guid? UserApprovedID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_UserApprovedID)]
        [MaxLength(150)]
        public string UserApprovedName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_UserHeadID)]
        public Guid? UserHeadID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_UserHeadID)]
        [MaxLength(150)]
        public string UserHeadName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_ViolationExplain)]
        [MaxLength(1000)]
        public string ViolationExplain { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_UnionAgentID)]
        public Guid? UnionAgentID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_UnionAgentID)]
        [MaxLength(150)]
        public string UnionAgentName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_HeadAgentID)]
        public Guid? HeadAgentID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_HeadAgentID)]
        [MaxLength(150)]
        public string HeadAgentName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_PlaceViolation)]
        [MaxLength(1000)]
        public string PlaceViolation { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateEndOfViolation)]
        public DateTime? DateEndOfViolation { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public List<Guid?> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsChecked { get; set; }

        public Guid ExportID { get; set; }
  
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
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DisciplineTypeID)]
        public Guid? DisciplineTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DisciplineTypeID)]
        public string DisciplinedTypesName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Discipline_OrgID)]
        public Nullable<System.Guid> OrgID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_OrgID)]
        public string OrgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DisciplinedTypesSuggestID)]
        public Nullable<System.Guid> DisciplinedTypesSuggestID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DisciplinedTypesSuggestID)]
        public string DisciplinedTypesSuggestName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DisciplineResonID)]
        public Nullable<System.Guid> DisciplineResonID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DisciplineResonID)]
        public string DisciplineResonName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DisciplineLevel)]
        public string DisciplineLevel { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_Status)]
        public string Status { get; set; }
        public string StatusView { get; set; }
        public string StatusSynView { get; set; }
        public string DisciplineLevelView { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateReview)]
        public Nullable<System.DateTime> DateReview { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_TermsViolation)]
        public string TermsViolation { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_AmountOfFine)]
        public Nullable<double> AmountOfFine { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_PersonApproved)]
        public string PersonApproved { get; set; }

        public int CountDis { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public partial class FieldNames
        {
            public const string DateReview = "DateReview";
            public const string TermsViolation = "TermsViolation";
            public const string AmountOfFine = "AmountOfFine";
            public const string PersonApproved = "PersonApproved";
            public const string OrgName = "OrgName";
            public const string DisciplinedTypesSuggestName = "DisciplinedTypesSuggestName";
            public const string DisciplineResonName = "DisciplineResonName";
            public const string DisciplineLevel = "DisciplineLevel";
            public const string DisciplineLevelView = "DisciplineLevelView";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string DisciplineTypeID = "DisciplineTypeID";
            public const string DisciplinedTypesName = "DisciplinedTypesName";
            public const string ID = "ID";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            public const string DateOfEffective = "DateOfEffective";
            public const string ProfileName = "ProfileName";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string DateOfInsurance = "DateOfInsurance";
            public const string DecisionNo = "DecisionNo";
            public const string DateOfViolation = "DateOfViolation";
            public const string DescriptionOfViolation = "DescriptionOfViolation";
            public const string ContentOfDiscipline = "ContentOfDiscipline";
            public const string DateOfExpiry = "DateOfExpiry";
            public const string DisciplinedDecidingOrgID = "DisciplinedDecidingOrgID";
            public const string PositionName = "PositionName";
            public const string PositionID = "PositionID";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string Notes = "Notes";
            public const string ViolationRule = "ViolationRule";
            public const string ViolationRuleDescription = "ViolationRuleDescription";
            public const string ViolationRuleDescriptionEN = "ViolationRuleDescriptionEN";
            public const string ResultOfRecidivism = "ResultOfRecidivism";
            public const string TimeOfViolation = "TimeOfViolation";
            public const string UserApprovedID = "UserApprovedID";
            public const string ViolationExplain = "ViolationExplain";
            public const string UnionAgentID = "UnionAgentID";
            public const string HeadAgentID = "HeadAgentID";
            public const string PlaceViolation = "PlaceViolation";
            public const string DateEndOfViolation = "DateEndOfViolation";
            public const string JobTitleName = "JobTitleName";
            public const string JobTitleID = "JobTitleID";
            public const string UserName = "UserName";
            public const string userHeadID = "userHeadID";
            public const string UserApprovedName = "UserApprovedName";
            public const string UserHeadName = "UserHeadName";
            public const string UnionAgentName = "UnionAgentName";
            public const string HeadAgentName = "HeadAgentName";
            public const string Discipline_ID = "Discipline_ID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
        }
    }

    public class Hre_DisciplineSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Uniform_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Discipline_DisciplineTypeID)]
        public Guid? DisciplineTypeID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
