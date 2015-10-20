using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HRM.Presentation.EmpPortal.Models
{
    public class Hre_WorkHistoryModel : BaseModelPortal
    {
        #region [Quoc.Do]
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Code)]
        public string WorkHistoryCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_DateEffective)]
        public DateTime DateEffective { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_OrgStructureID)]
        public Guid? OrganizationStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_PositionID)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_CostCentreID)]
        public Guid? CostCentreID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_LaborType)]
        public string LaborType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_EmployeeTypeID)]
        public Guid? EmployeeTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_PayrollGroupID)]
        public Guid? PayrollGroupID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_WorkLocation)]
        public string WorkLocation { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_SupervisiorID)]
        public Guid? SupervisiorID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_HighSupervisiorID)]
        public int? HighSupervisiorID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_JobTitleID)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Note)]
        public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        #endregion
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string WorkHistoryCode = "WorkHistoryCode";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileID = "ProfileID";
            public const string DateEffective = "DateEffective";
            public const string OrganizationStructureID = "OrganizationStructureID";
            public const string PositionID = "PositionID";
            public const string CostCentreID = "CostCentreID";
            public const string LaborType = "LaborType";
            public const string EmployeeTypeID = "EmployeeTypeID";
            public const string PayrollGroupID = "PayrollGroupID";
            public const string WorkLocation = "WorkLocation";
            public const string SupervisiorID = "SupervisiorID";
            public const string HighSupervisiorID = "HighSupervisiorID";
            public const string JobTitleID = "JobTitleID";
            public const string Status = "Status";
            public const string Note = "Note";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string WorkHistory_ID = "WorkHistory_ID";
        }
        
    }
}