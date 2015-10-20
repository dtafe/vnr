using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_PlanHeadCountModel : BaseViewModel
    {
        public int TotalRow { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlanHeadCount_PlanName)]
        public string PlanName { get; set; }

        public Guid? OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public string JobTitleName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_PlanHeadCount_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlanHeadCount_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlanHeadCount_HrPlanHC)]
        public int? HrPlanHC { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlanHeadCount_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Nullable<System.Guid> PostionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public string PositionName { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string JobTitleName = "JobTitleName";
            public const string PlanName = "PlanName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string OrgStructureName = "OrgStructureName";
            public const string Note = "Note";
            public const string HrPlanHC = "HrPlanHC";
            public const string PositionName = "PositionName";
   
        }
    }

    public class Hre_PlanHeadCountSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlanHeadCount_PlanName)]
        public string PlanName { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        //public Guid? JobTitleID { get; set; }

 

        //[DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        //public string OrgStructureID { get; set; }

        //[DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        //public DateTime? DateFrom { get; set; }

        //[DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        //public DateTime? DateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string JobTitleID = "JobTitleID";
            public const string PositionID = "PositionID";
            public const string OrgStructureID = "OrgStructureID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
        }
    }
}
