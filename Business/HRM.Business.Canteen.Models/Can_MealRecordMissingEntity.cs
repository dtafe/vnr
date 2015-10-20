using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Canteen.Models
{
    public class Can_MealRecordMissingEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public List<Guid?> OrgStructureID { get; set; }
        public List<Guid> ProfileIDs { get; set; }
        public Guid? ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string EmpCode { get; set; }
        public string OrgStructureName { get; set; }
        public DateTime? WorkDate { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public Guid? TamScanReasonMissID { get; set; }
        public string TamScanReasonMissName { get; set; }
        public bool? IsFullPay { get; set; }
        public decimal? Amount { get; set; }
        public string ActionStatus { get; set; }
        public Guid? MealAllowanceTypeSettingID { get; set; }
        public string MealAllowanceTypeSettingName { get; set; }
        public int TotalRow { get; set; }
    }
    public class Can_MealRecordMissingForDeleteEntity
    {
        public string strIDs { get; set; }
    }
}
