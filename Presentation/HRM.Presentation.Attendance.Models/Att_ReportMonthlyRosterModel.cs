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
    public class Att_ReportMonthlyRosterModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeBranch)]
 
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateExport)]
        public DateTime DateExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_OvertimeTypeID)]

        public Guid? PayrollID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_PayrollID)]
        public string PayrollIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_ReportMonthlyRoster_OnlyHolydayNotHaveRoster)]
        public bool OnlyHolydayNotHaveRoster { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_PositionName)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_DepartmentName)]
        public string DepartmentName { get; set; }
           [DisplayName(ConstantDisplay.HRM_Att_Report_SectionName)]
        public string SectionName { get; set; }
        public Guid ExportId { get; set; }

        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string PositionName = "PositionName";
            public const string DepartmentName = "DepartmentName";
            public const string SectionName = "SectionName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
        }
    }                           
}                               
                                