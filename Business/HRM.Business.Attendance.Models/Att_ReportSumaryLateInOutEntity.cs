using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportSumaryLateInOutEntity
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
        /// 
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        /// <summary>
        /// Mã Department (Loại Phòng Ban)
        /// </summary>
        public string Division { get; set; }
        /// <summary>
        /// Mã Position (Chức vụ)
        /// </summary>
        public string PositionCode { get; set; }
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
        public double? LateMinutes { get; set; }
        /// <summary>
        /// số phút về sớm
        /// </summary>
        public double? EarlyMinutes { get; set; }
        
        /// <summary>
        /// ghi Chú
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// List phòng ban
        /// </summary>
        public List<int?> OrgStructureID { get; set; }
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

    }
}
