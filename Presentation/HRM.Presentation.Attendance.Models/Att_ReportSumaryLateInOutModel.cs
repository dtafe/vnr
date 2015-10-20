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
    public class Att_ReportSumaryLateInOutModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        public string GroupCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string SectionCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public string PositionCode { get; set; }
        

        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        public string Division { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_NumTimeLate)]
        public int? NumTimeLate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_NumTimeEarly)]
        public int? NumTimeEarly { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_LateMinutes)]
        public double? LateMinutes { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_EarlyMinutes)]
        public double? EarlyMinutes { get; set; }
        public double? TotalLateEarlyMinutes
        {
            get
            {
                return LateMinutes + EarlyMinutes;
            }
        }

        [DisplayName(ConstantDisplay.HRM_Attendance_Remark)]
        public string Remark { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }
   
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string DepartmentCode { get; set; }
    
        public Guid? ShiftID { get; set; }
        //public string ShiftID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        public string ShiftIDs { get; set; }
       
        [DisplayName(ConstantDisplay.HRM_Att_Report_NoDisplay0Data)]
        public bool NoDisplay0Data { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }




        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }
       
        public Guid ExportId { get; set; }
      
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
            public const string NumTimeLate = "NumTimeLate";
            public const string NumTimeEarly = "NumTimeEarly";
            public const string LateMinutes = "LateMinutes";
            public const string EarlyMinutes = "EarlyMinutes";
            public const string TotalLateEarlyMinutes = "TotalLateEarlyMinutes";
            public const string Remark = "Remark";
            public const string OrgStructureID = "OrgStructureID";
            public const string DepartmentCode = "DepartmentCode";
            public const string PositionCode = "PositionCode";
            public const string NoDisplay0Data = "NoDisplay0Data";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
        }
    }
}
