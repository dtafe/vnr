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
    public class Att_ReportDetailForgetCardModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
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

        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        [MaxLength(50)]
        public string Division { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Date)]
        public DateTime? Date { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        [MaxLength(150)]
        public string ShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_InTime)]
        public DateTime? InTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OutTime)]
        public DateTime? OutTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ScanType)]
        [MaxLength(50)]
        public string ScanType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public Guid ExportId { get; set; }

        public string ValueFields { get; set; }

        public Guid? ShiftID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        public List<Guid?> ShiftIDs { get; set; }

         [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
         public string UserExport { get; set; }
         [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
         public DateTime DateExport { get; set; }

         public bool IsCreateTemplate { get; set; }
         public bool IsCreateTemplateForDynamicGrid { get; set; }
     
        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string GroupCode = "GroupCode";
            public const string SectionCode = "SectionCode";
            public const string DepartmentName = "DepartmentName";
            public const string Division = "Division";
            public const string Date = "Date";
            public const string ShiftName = "ShiftName";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string ScanType = "ScanType";
            public const string OrgStructureID = "OrgStructureID";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
        }
    }
}
