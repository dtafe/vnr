﻿using HRM.Infrastructure.Utilities;
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
    public class Att_ReportProfileByShiftModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        [MaxLength(50)]
        public string BranchCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(50)]
        public string DepartmentCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        [MaxLength(50)]
        public string TeamCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay)]
        public DateTime WorkDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        [MaxLength(150)]
        public string ShiftName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_PayrollGroupName)]
        public string PayrollGroupIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_UserExport)]
        public string UserExport { get; set; }
        public Guid ExportID { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string BranchCode = "BranchCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string TeamCode = "TeamCode";
            public const string WorkDay = "WorkDay";
            public const string ShiftName = "ShiftName";
            public const string OrgStructureID = "OrgStructureID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
            public const string PayrollGroupIDs = "PayrollGroupIDs";
        }
    }                           
}                               
                                