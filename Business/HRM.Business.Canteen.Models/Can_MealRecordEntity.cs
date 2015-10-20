using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Canteen.Models
{
    public class Can_MealRecordEntity : HRMBaseModel
    {
        public string CardCode { get; set; }
        public DateTime? WorkDay { get; set; }
        public DateTime? TimeLog { get; set; }
        public string MachineCode { get; set; }
        public Guid? LineID { get; set; }
        public Guid? CateringID { get; set; }
        public Guid? CanteenID { get; set; }
        public Guid? MealAllowanceTypeID { get; set; }
        public string MealAllowanceTypeName { get; set; }
        public decimal? Amount { get; set; }
        public bool? NoWorkDay { get; set; }
        public string LineName { get; set; }
        public string CateringName { get; set; }
        public string CanteenName { get; set; }
        public string ProfileName { get; set; }
        public string CodeAttendance { get; set; }
        public string OrgStructureName { get; set; }
        public string CodeEmp { get; set; }
        public Guid? ProfileID { get; set; }

        public int TotalRow { get; set; }
    }

    /// <summary>
    /// [Hieu.Van] xử dụng khi đỗ dữ liệu tìm kiếm từ ngày đến ngày
    /// </summary>
    public class Can_MealRecordCheckEntity
    {
        public Guid ID { get; set; }
        public DateTime? TimeLog { get; set; }
        public Guid? ProfileID { get; set; }
    }
}
