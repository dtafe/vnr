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
    public class Att_ReportMonthlyOvertimeModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_HR_Dependant_MonthOfEffect)]
         public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        [MaxLength(50)]
        public string GroupCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        [MaxLength(50)]
        public string SectionCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(150)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(150)]
        public string CodeOrg { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        [MaxLength(50)]
        public string Division { get; set; }


        /// <summary>
        /// ///////////////////////////////////////////////////////////
        /// </summary>

        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportMonthlyOvertime_StatusEmployee)]
        public string StatusEmployee { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportMonthlyOvertime_PayrollGroup)]
        public string PayrollGroup { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportMonthlyOvertime_OvertimeStatus)]
        public string OvertimeStatus { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportMonthlyOvertime_CumulativeType)]
        public string CumulativeType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportMonthlyOvertime_TypeHour)]
        public string TypeHour { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportMonthlyOvertime_OvertimeDetail)]
        public bool OvertimeDetail { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportMonthlyOvertime_RemoveNotHasWorkday)]
        public bool RemoveNotHasWorkday { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportMonthlyOvertime_IncludeAllEmployee)]
        public bool IncludeAllEmployee { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportMonthlyOvertime_RemoveCompensation100)]
        public bool RemoveCompensation100 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportMonthlyOvertime_RecordIns)]
        public bool RecordIns { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportMonthlyOvertime_Cumulative)]
        public bool Cumulative { get; set; }


        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }
        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string GroupCode = "GroupCode";
            public const string SectionCode = "SectionCode";
            public const string CodeOrg = "CodeOrg";
            public const string Division = "Division";
            public const string OrgStructureID = "OrgStructureID";
        }
    }
}
