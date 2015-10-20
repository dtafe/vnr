using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Category.Models
{
    public class Pur_MCAMEntity : HRMBaseModel
    {
     
        public Guid? ProfileID { get; set; }
        public DateTime? DateRequest { get; set; }
        public Guid? ModelID { get; set; }
        public Guid? PaymentMethodID { get; set; }
        public DateTime? StartDate { get; set; }
        public string ReceivePlace { get; set; }
        public string Status { get; set; }
        public DateTime? EndDate { get; set; }
        public string VehicleIdNo { get; set; }
        public string EngineNo { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public Guid? ColorID { get; set; }
        public Guid? RecevivePlaceID { get; set; }
        public DateTime? LiquidationDate { get; set; }
        public string Note { get; set; }
        public string NotMeetConditionReason { get; set; }
        public double? InterestRate { get; set; }
        public double? AmountPayment { get; set; }
        public double? AmountDeposit { get; set; }
        public double? AmountRemain { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string IDNo { get; set; }
        public string ModelType { get; set; }
        public string ModelCode { get; set; }
        public string ModelName { get; set; }
        public string PaymentmethodCode { get; set; }
        public DateTime? DateHire { get; set; }
        public double? DeadlinePayment { get; set; }
        public double? PaymentPerMonth { get; set; }
        public Guid? ColorModelID { get; set; }
        public string OrgStructureName { get; set; }
        public Guid? OrgStructureID { get; set; }
        public double? GrossAmount { get; set; }
        public string Cat_contractType { get; set; }
        public string ColorCode { get; set; }
        public string WorkPlaceID { get; set; }
        public string CurrencyCode { get; set; }
        public string StatusCheck { get; set; }
        public string Color { get; set; }
        public string CurrencyAmountRemain { get; set; }
        public string CurrencyAmountDeposit { get; set; }
    }
}
