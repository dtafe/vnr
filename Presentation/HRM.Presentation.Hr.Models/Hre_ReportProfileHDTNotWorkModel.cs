using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportProfileHDTNotWorkModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateFrom)]
        public DateTime DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_Code)]
        public string HDTJobGroupCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Code)]
        public string HDTJobTypeCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_Price)]
        public double? Price { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_FirstInTime)]
        public DateTime? FirstIntime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LastOutTime)]
        public DateTime? LastOutTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_Code)]
        public string LeaveDayTypeCode { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string ValueFields { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }




        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string OrgStructureIDs = "OrgStructureIDs";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string HDTJobGroupCode = "HDTJobGroupCode";
            public const string HDTJobTypeCode = "HDTJobTypeCode";
            public const string Price = "Price";
            public const string FirstIntime = "FirstIntime";
            public const string LastOutTime = "LastOutTime";
            public const string LeaveDayTypeCode = "LeaveDayTypeCode";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
        }
      
    }
}
