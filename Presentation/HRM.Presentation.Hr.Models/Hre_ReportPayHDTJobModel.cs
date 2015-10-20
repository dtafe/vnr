using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportPayHDTJobModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateFrom)]
        public DateTime DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CountType4)]
        public int? CountType4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CountType5)]
        public int? CountType5 { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string ValueFields { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Dept)]
        public string Dept { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Unit)]
        public string Unit { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Part)]
        public string Part { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Line)]
        public string Line { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Session)]
        public string Session { get; set; }

        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string OrgStructureName = "OrgStructureName";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string CountType4 = "CountType4";
            public const string CountType5 = "CountType5";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string Dept = "Dept";
            public const string Unit = "Unit";
            public const string Part = "Part";
            public const string Line = "Line";
            public const string Session = "Session";
        }
      
    }
}
