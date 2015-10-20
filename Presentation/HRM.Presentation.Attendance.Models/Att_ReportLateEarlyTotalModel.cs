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
    public class Att_ReportLateEarlyTotalModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]      
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]      
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Brand_Code)]
        public string BrandCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Brand_BrandName)]
        public string BrandName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        public string DepartmentCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DepartmentName)]
        public string DepartmentName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        public string TeamCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_TeamName)]
        public string TeamName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string SectionCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_SectionName)]
        public string SectionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? SDateFrom { get; set; }
        public DateTime? SDateTo { get; set; }
        public string OrgStructureID { get; set; }
        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }
        public double? Less2Hour { get; set; }
        public double? Over2Hour { get; set; }
        public double? TotalLateEarly { get; set; }
        public double? ForgetTamscan { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "Codemp";
            public const string ProfileName = "ProfileName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string BrandCode = "BrandCode";
            public const string BrandName = "BrandName";
            public const string DepartmentCode = "DepartmentCode";
            public const string DepartmentName = "DepartmentName";
            public const string TeamCode = "TeamCode";
            public const string TeamName = "TeamName";
            public const string SectionCode = "SectionCode";
            public const string SectionName = "SectionName";
            public const string OrgStructureName = "OrgStructureName";
            public const string Less2Hour = "Less2Hour";
            public const string Over2Hour = "Over2Hour";
            public const string TotalLateEarly = "TotalLateEarly";
            public const string ForgetTamscan = "ForgetTamscan";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
        }
    }
}
