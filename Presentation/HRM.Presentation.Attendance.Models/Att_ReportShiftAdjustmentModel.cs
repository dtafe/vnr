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
    public class Att_ReportShiftAdjustmentModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        [MaxLength(150)]
        public string JobtitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DateLeave)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        [MaxLength(150)]
        public string ShiftName { get; set; }        
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public List<Guid?> OrgStructureID { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public double? SumLateEarlyMinute { get; set; }
        public Guid ExportId { get; set; }
        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string FromDate = "FromDate";
            public const string ToDate = "ToDate";
            public const string PositionName = "PositionName";
            public const string JobtitleName = "JobtitleName";          
            public const string Date = "Date";
            public const string ShiftName = "ShiftName";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string Paid = "Paid";
            public const string OrgStructureID = "OrgStructureID";
            public const string SumLateEarlyMinute = "SumLateEarlyMinute";       
        }        
    }
}
