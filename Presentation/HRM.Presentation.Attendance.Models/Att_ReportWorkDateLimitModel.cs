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
    public class Att_ReportWorkDateLimitModel : BaseViewModel
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
        public string EmpCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public string EmployeeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        public string DepartmentName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDate)]
        public DateTime? WorkDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_OvertimeTypeName)]      
        public string OvertimeTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_RegisterHours)]
        public double? RegisterHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ApproveHours)]
        public double? ApproveHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_InTime)]
        public DateTime? TimeIn { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_OutTime)]
        public DateTime? TimeOut { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ConvertedHours)]
        public double? ConvertedHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ConfirmHours)]
        public double? ConfirmHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public List<Guid?> OrgStructureId { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public List<Guid?> PositionId { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportWorkDateLimit_Template)]
        public Guid? TemplateId { get; set; }

        public partial class FieldNames
        {
            public const string EmpCode = "EmpCode";
            public const string EmployeeName = "EmployeeName";
            public const string JobTitleName = "JobTitleName";
            public const string DepartmentName = "DepartmentName";
            public const string WorkDate = "WorkDate";
            public const string OvertimeTypeName = "OvertimeTypeName";
            public const string RegisterHours = "RegisterHours";
            public const string ApproveHours = "ApproveHours";
            public const string TimeIn = "TimeIn";
            public const string TimeOut = "TimeOut";
            public const string ConvertedHours = "ConvertedHours";
            public const string ConfirmHours = "ConfirmHours";
            public const string Status = "Status";
        }
        
    }
}
