using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_WorkHistoryModel : BaseViewModel
    {
        public Nullable<System.Guid> AbilityTileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CostSourceID)]
        public Nullable<System.Guid> CostSourceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CostSourceID)]
        public string CostSourceName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkLocationOld)]
        public string WorkLocationOld { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_SalaryClassNameOld)]
        public string SalaryClassNameOld { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgOld)]
        public string OrgOld { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_OrgStructureOldID)]
        public Nullable<System.Guid> OrgStructureOldID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_OrgStructureOldID)]
        public string OrgStructureOldName { get; set; }
        public Guid WorkingPositionInfo_ID { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Code)]
        [MaxLength(50)]
        public string WorkHistoryCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_DateEffective)]
        public DateTime DateEffective { get; set; }
        public DateTime DateEffectiveOld { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_OrgStructureID)]
        public Guid? OrganizationStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_PositionID)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_CostCentreID)]
        public Guid? CostCentreID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_CostCentreID)]
        public string CostCentreName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_LaborType)]
        [MaxLength(150)]
        public string LaborType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_EmployeeTypeID)]
        public Guid? EmployeeTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_EmployeeTypeID)]
        public string EmployeeTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_PayrollGroupID)]
        public Guid? PayrollGroupID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_WorkLocation)]
        [MaxLength(1000)]
        public string WorkLocation { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_SupervisiorID)]
        public string Supervisor { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_JobTitleID)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Status)]
        [MaxLength(50)]
        public string StatusView { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Note)]
        [MaxLength(1000)]
        public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(150)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_JRType)]
        public string JRType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_UserApprove)]
        public string UserApprove { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Rromotion)]
        public string Rromotion { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_RequirCondit)]
        public string RequirCondit { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Description)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Role)]
        public string Role { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_FormType)]
        public string FormType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_SalaryClassID)]
        public Nullable<System.Guid> SalaryClassID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_SalaryClassID)]
        public string SalaryClassName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Duration)]
        public Nullable<int> Duration { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_DateNotice)]
        public Nullable<System.DateTime> DateNotice { get; set; }

        private Guid _id = Guid.Empty;
        public Guid WorkHistory_ID
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
         [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_JRType)]
        public Nullable<System.Guid> TypeOfTransferID { get; set; }
         public string TypeOfTransferName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_JRType)]
         public string NameEntityName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_DateComeBack)]
        public Nullable<System.DateTime> DateComeBack { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_HighSupervisiorID)]
         public string HighSupervisor { get; set; }
         public string HighSupervisorCode { get; set; }
         public Guid? WorkPlaceID { get; set; }
         public string WorkPlaceName { get; set; }

         public string UnitNameOld { get; set; }
         public string DivisionNameOld { get; set; }

         public string DepartmentNameOld { get; set; }
         public string SectionNameOld { get; set; }

         [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleOld)]
         public string JobTitleOld { get; set; }

         [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionOld)]
         public string PositionOld { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkPlaceOld)]
         public string WorkPlaceOld { get; set; }
        public partial class FieldNames
        {
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string ID = "ID";
            public const string WorkPlaceOld = "WorkPlaceOld";
            public const string PositionOld = "PositionOld";
            public const string OrgOld = "OrgOld";
            public const string JobTitleOld = "JobTitleOld";
            public const string WorkLocationOld = "WorkLocationOld";
            public const string UnitNameOld = "UnitNameOld";
            public const string DivisionNameOld = "DivisionNameOld";
            public const string DepartmentNameOld = "DepartmentNameOld";
            public const string SectionNameOld = "SectionNameOld";
            public const string TypeOfTransferName = "TypeOfTransferName";
            public const string JRType = "JRType";
            public const string SalaryClassID = "SalaryClassID";
            public const string SalaryClassName = "SalaryClassName";
            public const string SalaryClassNameOld = "SalaryClassNameOld";
            public const string WorkHistoryCode = "WorkHistoryCode";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileID = "ProfileID";
            public const string DateEffective = "DateEffective";
            public const string OrganizationStructureID = "OrganizationStructureID";
            public const string PositionID = "PositionID";
            public const string CostCentreID = "CostCentreID";
            public const string LaborType = "LaborType";
            public const string EmployeeTypeID = "EmployeeTypeID";
            public const string EmployeeTypeName = "EmployeeTypeName";
            public const string PayrollGroupID = "PayrollGroupID";
            public const string WorkLocation = "WorkLocation";
            public const string Supervisor = "Supervisor";
            public const string JobTitleID = "JobTitleID";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string Note = "Note";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string WorkHistory_ID = "WorkHistory_ID";
        }
    }

    public class Hre_WorkHistorySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrganizationStructureID { get; set; }

        public bool IsCreateTemplate { get; set; }

        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_JRType)]
        public Nullable<System.Guid> TypeOfTransferID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Status)]
        [MaxLength(50)]
        public string Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_SalaryClassID)]
        public Guid? SalaryClassID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_WorkLocation)]
        public string WorkLocation { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
    }

    public class Hre_WorkHistoryWaitingSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrganizationStructureID { get; set; }

        public bool IsCreateTemplate { get; set; }

        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_JRType)]
        public Nullable<System.Guid> TypeOfTransferID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Status)]
        [MaxLength(50)]
        public string Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_SalaryClassID)]
        public Guid? SalaryClassID { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
    }
}
