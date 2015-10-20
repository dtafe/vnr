using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportHCSeniorityModel : BaseViewModel
    {
        public string Name { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_DateExport)]
        public DateTime DateExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public Guid OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_TypeID)]
        public Guid? OrgStructureTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_JobtitleID)]
        public List<Guid> JobtitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_DateSearch)]
        public DateTime MonthSearch { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public Guid ExportID { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }
        public partial class FieldNames
        {
            public const string Name = "Name";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
            public const string MonthSearch = "MonthSearch";
        }
    }
}
