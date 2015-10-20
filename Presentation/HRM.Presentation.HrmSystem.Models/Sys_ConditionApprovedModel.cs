using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_ConditionApprovedSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_ConditionName)]
        public string ConditionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_ApprovedType)]
        public string ApprovedType { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_ExpensesType)]
        public string ExpensesType { get; set; }
    }
    public class Sys_ConditionApprovedModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_ConditionName)]
        public string ConditionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_ApprovedType)]
        public string ApprovedType { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_ExpensesType)]
        public string ExpensesType { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_JobTitle)]
        public Nullable<System.Guid> JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Position)]
        public Nullable<System.Guid> PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_WorkPlace)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_ProcessApproved)]
        public Nullable<System.Guid> ProcessApprovedID { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org1)]
        public Nullable<System.Guid> OrgID1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org2)]
        public Nullable<System.Guid> OrgID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org3)]
        public Nullable<System.Guid> OrgID3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org4)]
        public Nullable<System.Guid> OrgID4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org5)]
        public Nullable<System.Guid> OrgID5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org6)]
        public Nullable<System.Guid> OrgID6 { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_JobTitle)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Position)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_WorkPlace)]
        public string WorkPlaceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_ProcessApproved)]
        public string ProcessName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org1)]
        public string OrgCode1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org2)]
        public string OrgCode2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org3)]
        public string OrgCode3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org4)]
        public string OrgCode4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org5)]
        public string OrgCode5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConditionApproved_Org6)]
        public string OrgCode6 { get; set; }

        public partial class FieldNames
        {
            public const string ConditionName = "ConditionName";
            public const string Description = "Description";
            public const string ApprovedType = "ApprovedType";
            public const string ExpensesType = "ExpensesType";

            public const string JobTitleID = "JobTitleID";
            public const string PositionID = "PositionID";
            public const string WorkPlaceID = "WorkPlaceID";
            public const string ProcessApprovedID = "ProcessApprovedID";

            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string ProcessName = "ProcessName";

            public const string OrgID1 = "OrgID1";
            public const string OrgID2 = "OrgID2";
            public const string OrgID3 = "OrgID3";
            public const string OrgID4 = "OrgID4";
            public const string OrgID5 = "OrgID5";
            public const string OrgID6 = "OrgID6";

            public const string OrgCode1 = "OrgCode1";
            public const string OrgCode2 = "OrgCode2";
            public const string OrgCode3 = "OrgCode3";
            public const string OrgCode4 = "OrgCode4";
            public const string OrgCode5 = "OrgCode5";
            public const string OrgCode6 = "OrgCode6";
        }
    }
}
