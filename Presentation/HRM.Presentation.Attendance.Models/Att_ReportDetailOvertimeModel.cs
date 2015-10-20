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
    public class Att_ReportDetailOvertimeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeBranch)]
        public string CodeBranch { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeOrg)]
        public string CodeOrg { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeTeam)]
        public string CodeTeam { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeSection)]
        public string CodeSection { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeJobtitle)]
        public string CodeJobtitle { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodePosition)]
        public string CodePosition { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_BranchName)]
        public string BranchName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_OrgName)]
        public string OrgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_TeamName)]
        public string TeamName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_SectionName)]
        public string SectionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateOvertime)]
        public DateTime? DateOvertime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateExport)]
        public DateTime DateExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ReleaseNote_DateRelease)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_udInTime)]
        public DateTime? udInTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_udOutTime)]
        public DateTime? udOutTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_Cat_Shift)]
        public string ShiftName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_udSubmitApproveHour)]
        public string udSubmitApproveHour { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_OvertimeTypeID)]
        public List<Guid?> OverTimeTypeID { get; set; }
        public Guid ExportId { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_NoDisplay0Data)]
        public bool NoDisplay0Data { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool IsIncludeQuitEmp { get; set; }

        public string ValueFields { get; set; }


        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string CodeBranch = "CodeBranch";
            public const string CodeOrg = "CodeOrg";
            public const string CodeTeam = "CodeTeam";
            public const string CodeSection = "CodeSection";
            public const string CodeJobtitle = "CodeJobtitle";
            public const string CodePosition = "CodePosition";
            public const string UserExport = "UserExport";
            public const string BranchName = "BranchName";
            public const string OrgName = "OrgName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string DateOvertime = "DateOvertime";
            public const string DateExport = "DateExport";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string udInTime = "udInTime";
            public const string udOutTime = "udOutTime";
            public const string ShiftName = "ShiftName";
            public const string udSubmitApproveHour = "udSubmitApproveHour";
            public const string IsIncludeQuitEmp = "IsIncludeQuitEmp";
            public const string NoDisplay0Data = "NoDisplay0Data";
        }
    }
}
