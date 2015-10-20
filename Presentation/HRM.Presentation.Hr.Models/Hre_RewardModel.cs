using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_RewardModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Reward_ProfileID)]
        public Guid ProfileID { get; set; }

        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_DateOfIssuance)]
        public DateTime DateOfIssuance { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_Reason)]
        [MaxLength(1000)]
        public string Reason { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_RewardedDecidingOrgID)]
        public int? RewardedDecidingOrgID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_DateOfEffective)]
        public DateTime? DateOfEffective { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_Description)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_UserApproveID)]
        public Guid? UserApproveID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_UserApproveID)]
        [MaxLength(150)]
        public string UserApproveName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_Requester)]
        [MaxLength(150)]
        public string RequesterName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_RequesterID)]
        public Guid? RequesterID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_RewardValue)]
        public double? RewardValue { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_NoOfReward)]
        [MaxLength(150)]
        public string NoOfReward { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
       
        [DisplayName(ConstantDisplay.HRM_HR_Reward_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public List<Guid?> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_RewardedTypeID)]
        public Nullable<System.Guid> RewardedTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_RewardedTypeID)]
        public string RewardedTypeName { get; set; }

        private Guid _id = Guid.Empty;
        public Guid Reward_ID
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

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string Reason = "Reason";
            public const string DateOfIssuance = "DateOfIssuance";
            public const string RewardedDecidingOrgID = "RewardedDecidingOrgID";
            public const string DateOfEffective = "DateOfEffective";
            public const string Description = "Description";
            public const string UserApproveID = "UserApproveID";
            public const string UserApproveName = "UserApproveName";
            public const string RequesterID = "RequesterID";
            public const string ProfileName = "ProfileName";
            public const string PositionID = "PositionID";
            public const string PositionName = "PositionName";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureID = "OrgStructureID";
            public const string RequesterName = "RequesterName";
            public const string RewardValue = "RewardValue";
            public const string NoOfReward = "NoOfReward";
            public const string JobTitleName = "JobTitleName";
            public const string JobTitleID = "JobTitleID";
            public const string UserName = "UserName";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
            public const string Reward_ID = "Reward_ID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }

    public class Hre_RewardSearchModel
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

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        public DateTime? DateFrom_DateOfIssuance { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public DateTime? DateTo_DateOfIssuance { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string JobTitleID = "JobTitleID";
            public const string PositionID = "PositionID";
            public const string OrgStructureID = "OrgStructureID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string DateFrom_DateOfIssuance = "DateFrom_DateOfIssuance";
            public const string DateTo_DateOfIssuance = "DateTo_DateOfIssuance";
        }
    }
}
