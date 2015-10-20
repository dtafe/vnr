using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Category.Models
{
    public class Cat_LeaveDayTypeMultiEntity 
    {
        public Guid ID { get; set; }
        public string LeaveDayTypeName { get; set; }
        public string Code { get; set; }
        public string CodeStatistic { get; set; }
    }
    public class Cat_LeaveDayTypeEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string LeaveDayTypeName { get; set; }
        public string Code { get; set; }
        public double PaidRate { get; set; }
        public double SocialRate { get; set; }
        public string CodeStatistic { get; set; }
        public string TAMScanReasonMissName { get; set; }
        public Guid? MissInOutReasonID { get; set; }
        public bool IsWorkDay { get; set; }
        public bool IsAnnualLeave { get; set; }
        public bool? IsTimeOffInLieu { get; set; }
        public bool? IsTimeOffMakeUp { get; set; }
        public bool? IsLoadRelatives { get; set; }
        public double Penalty { get; set; }
        public Guid? OvertimeID { get; set; }
        public Guid? OrgStructureID { get; set; }
        public int? MaxPerTimes { get; set; }
        public int? MaxPerYear { get; set; }
        public int? MaxPerMonth { get; set; }
        public string Formula { get; set; }
        public string InsuranceType { get; set; }
        public string SalaryType { get; set; }
        public string OvertimeTypeName { get; set; }
        public string Notes { get; set; }
        public string OrgStructureName { get; set; }
    }
}
