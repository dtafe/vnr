using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportSummaryLateInOutEntity
    {
 
        /// <summary>
        /// mã nhân viên
        /// </summary>
        public string CodeEmp { get; set; }
        /// <summary>
        /// ProfileName
        /// </summary>
        public string ProfileName { get; set; }
        /// <summary>
        /// Mã Group (loại phòng ban)
        /// </summary>
        public string GroupCode { get; set; }
        /// <summary>
        /// Mã Section (Loại Phòng ban)
        /// </summary>
        public string SectionCode { get; set; }
        /// <summary>
        /// Tên Deparment (Loại Phòng Ban)
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// Mã Department (Loại Phòng Ban)
        /// </summary>
        public string Division { get; set; }
        /// <summary>
        /// Số Lần Đi Trễ
        /// </summary>
        public int? NumTimeLate { get; set; }
        /// <summary>
        /// Số Lần về sớm
        /// </summary>
        public int? NumTimeEarly { get; set; }
        /// <summary>
        /// số Phút đi trễ
        /// </summary>
        public int? LateMinutes { get; set; }
        /// <summary>
        /// số phút về sớm
        /// </summary>
        public int? EarlyMinutes { get; set; }
        /// <summary>
        /// ghi Chú
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// List phòng ban
        /// </summary>
        public List<int?> OrgStructureID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string DepartmentCode { get; set; }
       
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string PositionName { get; set; }
        public string JobtitleName { get; set; }
        public string PositionCode { get; set; }
        public string udTAMScanReasonMiss { get; set; }
        public string JobtitleCode { get; set; }

        public DateTime? udOutTime { get; set; }
        public DateTime? udInTime { get; set; }
        public DateTime? Date { get; set; }
        public string ShiftName { get; set; }
        public string EarlyDurationMore2 { get; set; }
        public string EarlyDurationLess2 { get; set; }

        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTime? DateFrom { get; set; }
        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? DateTo { get; set; }
        public string UserExport { get; set; }
        public DateTime DateExport { get; set; }

        public string LateForWork { get; set; }
        public string EarlyGoHome { get; set; }
    }
}
