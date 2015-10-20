using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_ReportWarringOvertimeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateFrom)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateEnd { get; set; }
        public string Type { get; set; }
        public bool OnWeek { get; set; }
        public bool OnMonth { get; set; }
        public bool OnYear { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DepartmentName)]
        public string OrgStructureName { get; set; }
        public string OrgStructureCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_BranchName)]
        public string BranchName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DepartmentName)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_TeamName)]
        public string TeamName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_SectionName)]
        public string SectionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_JobTitleName)]
        public string JobTitleName { get; set; }


        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }
      
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateExport)]
        public DateTime? DateExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureIDs { get; set; }
        public Guid ExportId { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OvertimeHours)]
        public string HourOverTime { get; set; }
        public string ValueFields { get; set; }
        public string Color { get; set; }
        
        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureCode = "OrgStructureCode";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string OrgStructureIDs = "OrgStructureIDs";
            public const string ExportId = "ExportId";
            public const string HourOverTime = "HourOverTime";
            public const string Color = "Color";
        }
    }                           
}                               
                                