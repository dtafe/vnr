using System;

namespace HRM.Business.Laundry.Models
{
    public class LMS_LaundryRecordEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }

        public Guid? ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string CodeAttendance { get; set; }
        public Guid? TamScanLogID { get; set; }
        public DateTime? TimeLog { get; set; }
        public string Hours { get; set; }
        public string MachineCode { get; set; }
        public Guid? ManchineOfLineID { get; set; }
        public Guid? LineID { get; set; }
        public string LineLMSName { get; set; }
        public Guid? MarkerID { get; set; }
        public string MarkerName { get; set; }
        public Guid? LockerID { get; set; }
        public string LockerLMSName { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string OrgStructureName { get; set; }
        public Guid? OrgStructureID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Price { get; set; }
        public int? Volume { get; set; }
        public int? TotalAmount { get; set; }

        public int? Total { get; set; }
        
    }

    /// <summary>
    /// [Hieu.Van] xử dụng khi đỗ dữ liệu tìm kiếm từ ngày đến ngày
    /// </summary>
    public class Lau_LaundryRecordCheckEntity
    {
        public Guid ID { get; set; }
        public Guid? ProfileID { get; set; }
        public DateTime? TimeLog { get; set; }
    }
}
