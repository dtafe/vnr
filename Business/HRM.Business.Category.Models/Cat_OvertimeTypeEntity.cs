using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Category.Models
{
    public class Cat_OvertimeTypeMultiEntity 
    {
        public Guid ID { get; set; }
        public string OvertimeTypeName { get; set; }
        public string Code { get; set; }
    }
    public class Cat_OvertimeTypeEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string OvertimeTypeName { get; set; }
        public string Code { get; set; }
        public double Rate { get; set; }
        public double TaxRate { get; set; }
        public double? TimeOffInLieuRate { get; set; }
        public bool? IsNightShift { get; set; }
        public Guid? OrgStructureID { get; set; }
        public string Comment { get; set; }
        public string CodeStatistic { get; set; }
        public bool? IsWorkDay { get; set; }
        public bool? IsHoliday { get; set; }
        public bool? IsWeeked { get; set; }
        public string Type { get; set; }
        public int? Order { get; set; }
        public string UserCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
    }
}
