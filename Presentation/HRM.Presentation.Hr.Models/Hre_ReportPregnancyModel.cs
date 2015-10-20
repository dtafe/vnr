using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportPregnancyModel : BaseViewModel
    {
        

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_AllowLimitOvertime_DateStart)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_AllowLimitOvertime_DateEnd)]
        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_Type)]
        public string Type { get; set; }

        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string ValueFields { get; set; }

        public string NameFamily { get; set; }
        public string FirstName { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string Notes { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Type = "Type";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string Channel = "Channel";
            public const string Region = "Region";
            public const string Area = "Area";

        }
      
    }
}
