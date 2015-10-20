using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRM.Business.Finance.Models
{
    public class FIN_ClaimCostPaymentApproveEntity : HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string ClaimName { get; set; }
        public string AccountCode { get; set; }
        public string ClaimCode { get; set; }
        public Nullable<System.Guid> TravelRequestID { get; set; }
        public string TravelRequestName { get; set; }
        public string Status { get; set; }
        public string UserApprovedName { get; set; }
        public double? Total { get; set; }
        public Nullable<System.Guid> UserCreateID { get; set; }
        public string UserCreateName { get; set; }
        public Nullable<System.Guid> UserApproveID { get; set; }
        public string UserApproveName { get; set; }

        public string Subject { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateFromEstimate { get; set; }
        public Nullable<System.DateTime> DateToEstimate { get; set; }
        public string Attachment { get; set; }
        public Nullable<double> CostEstimate { get; set; }
        public Nullable<double> CashAdvance { get; set; }
        public Nullable<double> CostActual { get; set; }
        public Nullable<double> Balance { get; set; }
        public Nullable<double> PaidAmount { get; set; }
        public Nullable<System.DateTime> DateRequest { get; set; }
        public Nullable<System.DateTime> DateFromActual { get; set; }
        public Nullable<System.DateTime> DateToActual { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string ExpensesType { get; set; }
        public Nullable<System.Guid> CashAdvanceID { get; set; }
        public string CashAdvanceName { get; set; }
        public Nullable<bool> IsCashAdvance { get; set; }
        public string Other { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string Channel { get; set; }
        public string WorkPlaceName { get; set; }
        public string JobTitle { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureCode { get; set; }
        public string SupervisorName { get; set; }
        public string JobTitleName { get; set; }

    }
}
