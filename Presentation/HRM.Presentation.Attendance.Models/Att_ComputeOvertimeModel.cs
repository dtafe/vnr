using System;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_ComputeOvertimeModel
    {
        public string FilterCompute { get; set; }
        public bool IsExport { get; set; }
        public Guid ExportID { get; set; }
        public string ValueFields { get; set; }

        public int TotalRow { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_DateFrom)]
        public DateTime DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_DateTo)]
        public DateTime DateTo { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        //public Guid[] OrgStructureId { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_DefinitionOfShiftWork)]
        public bool DefinitionOfShiftWork { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_RoundingFormula)]
        [MaxLength(1000)]
        public string RoundingFormula { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_Type)]
        [MaxLength(50)]
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_ProfileID)]
        public string ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        /// <summary>
        /// [True]Lấy tăng ca Ngoài ca làm việc (Đầu ca, Cuối Ca)
        /// [False] lấy tăng ca nằm trong shiftitem
        /// </summary>
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_isAllowGetOTOutterShift)]
        public bool isAllowGetOTOutterShift { get; set; }
        /// <summary>
        /// lấy tăng ca đầu ca
        /// </summary>
        /// 
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_isAllowGetBeforeShift)]
        public bool isAllowGetBeforeShift { get; set; }
        /// <summary>
        /// lấy tăng ca cuối ca
        /// </summary>
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_isAllowGetAfterShift)]
        public bool isAllowGetAfterShift { get; set; }
        /// <summary>
        /// lấy tăng ca trong ca
        /// </summary>
        /// 
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_isAllowGetInShift)]
        public bool isAllowGetInShift { get; set; }
        /// <summary>
        /// tính loại tăng ca vào đúng ngày thực tế
        /// </summary>
        /// 
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_isAllowGetTypeBaseOnActualDate)]
        public bool isAllowGetTypeBaseOnActualDate { get; set; }
        /// <summary>
        /// tính loại tăng ca và ngày bắt đầu tăng ca
        /// </summary>
        /// 
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_isAllowGetTypeBaseOnBeginShift)]
        public bool isAllowGetTypeBaseOnBeginShift { get; set; }
        /// <summary>
        /// tính loai tăng ca vào ngày kết thúc tăng ca
        /// </summary>
        /// 
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_isAllowGetTypeBaseOnEndShift)]
        public bool isAllowGetTypeBaseOnEndShift { get; set; }
        /// <summary>
        /// có tính OT ca đêm không
        /// </summary>
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_isAllowGetNightShift)]
        public bool isAllowGetNightShift { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_isNotAllowGetNightShift)]
        public bool isNotAllowGetNightShift { get; set; }
        ///// <summary>
        ///// Có cắt vào lúc 12h Khuya hay không?
        ///// </summary>
        //public bool isBreakMiddleNight { get; set; }
        /// <summary>
        /// số giờ tối thiểu để tính OT
        /// </summary>
        
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_MininumOvertimeHour)]
        public double MininumOvertimeHour { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_MaximumOvertimeHour)]
        public double MaximumOvertimeHour { get; set; }

       
    }
}
