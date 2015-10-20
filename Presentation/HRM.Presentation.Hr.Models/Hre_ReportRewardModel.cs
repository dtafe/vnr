using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportRewardModel : BaseViewModel
    { 
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
         [DisplayName(ConstantDisplay.HRM_HR_Discipline_DateOfEffective)]
        public DateTime? DateOfEffective { get; set; }

         [DisplayName(ConstantDisplay.HRM_HR_Reward_Reason)]
         [MaxLength(1000)]
         public string Reason { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_DecisionNo)]
         public string NoOfReward { get; set; }

         [DisplayName(ConstantDisplay.HRM_HR_ReportReward_RewardedTitleName)]
         public string RewardedTitleName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_ReportReward_RewardedTimeName)]
         public string RewardedTimeName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Reward_UserApproveID)]
         [MaxLength(150)]
         public string UserApproveName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Reward_Requester)]
         [MaxLength(150)]
         public string RequesterName { get; set; }

        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsChecked { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";

            public const string DateOfEffective = "DateOfEffective";
            public const string Reason = "Reason";
            public const string RewardedTitleName = "RewardedTitleName";
            public const string RewardedTimeName = "RewardedTimeName";
            public const string UserApproveName = "UserApproveName";
            public const string NoOfReward = "NoOfReward";
        }
      
    }
}
