using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class FIN_ApproveCashAdvanceModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequest_ProfileID)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequest_ProfileID)]
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequest_AccountCode)]
        public string AccountCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequest_TravelRequestName)]
        public string TravelRequestName { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequest_TravelRequestCode)]
        public string TravelRequestCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UserApprovedID)]
        public Guid? UserApprovedID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequest_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_IsEntertaiment)]
        public Nullable<bool> IsEntertaiment { get; set; }
         [DisplayName(ConstantDisplay.HRM_FIN_Claim_IsOther)]
        public Nullable<bool> IsOther { get; set; }
         [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_IsTravelRequest)]
         public Nullable<bool> IsTravelRequest { get; set; }
        private Guid _id = Guid.Empty;
        public double? Total { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_CashAdvanceName)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UserApprovedID)]
        public Nullable<System.Guid> UserApproveID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UserApprovedID)]
        public string UserApprovedName { get; set; }
        public string PayTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UserCreateID)]
        public Nullable<System.Guid> UserCreateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UserCreateID)]
        public string UserCreateName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Subject)]
        public string Subject { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_DateFromEstimate)]
        public Nullable<System.DateTime> DateFromEstimate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_DateToEstimate)]
        public Nullable<System.DateTime> DateToEstimate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Attachment)]
        public string Attachment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_CostEstimate)]
        public Nullable<double> CostEstimate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_CashAdvance)]
        public Nullable<double> CashAdvance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_CostActual)]
        public Nullable<double> CostActual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Balance)]
        public Nullable<double> Balance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_PaidAmount)]
        public Nullable<double> PaidAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_DateRequest)]
        public Nullable<System.DateTime> DateRequest { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_DateFromActual)]
        public Nullable<System.DateTime> DateFromActual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_DateToActual)]
        public Nullable<System.DateTime> DateToActual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Position)]
        public string Position { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Dept)]
        public string Dept { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Grade_CurrencyID)]
        public Nullable<System.Guid> CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Grade_CurrencyID)]
        public string CurrencyName { get; set; }
        public bool isPayment {get;set; }

        public Guid CashAdvance_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_Area)]

        public string Area { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_Region)]
        public string Region { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_Channel)]
        public string Channel { get; set; }
        public string WorkPlaceName { get; set; }
        public string JobTitle { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureCode { get; set; }
        public string SupervisorName { get; set; }
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_CashAdvanceName)]
        public string CashAdvanceName { get; set; }
          [DisplayName(ConstantDisplay.HRM_FIN_TravelRequest_TravelRequestName)]
        public Guid? TravelRequestID { get; set; }
         [DisplayName(ConstantDisplay.HRM_FIN_Claim_Other)]
          public string Other { get; set; }

        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_TotalAmount)]

         public int? TotalAmount { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string ProfileName = "ProfileName";
            public const string AccountCode = "AccountCode";
            public const string TravelRequestName = "TravelRequestName";
            public const string TravelRequestCode = "TravelRequestCode";
            public const string Status = "Status";
            public const string IsClaim = "IsClaim";
            public const string UserApprovedName = "UserApprovedName";
            public const string DateRequest = "DateRequest";
            public const string UserCreateName = "UserCreateName";
            public const string Dept = "Dept";
            public const string CostEstimate = "CostEstimate";
            public const string CashAdvanceName = "CashAdvanceName";
            public const string CostActual = "CostActual";
            public const string PaidAmount = "PaidAmount";
            public const string CodeEmp = "CodeEmp";
            public const string Other = "Other";
        }
    }
    public class  FIN_ApproveCashAdvanceSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_DateRequest)]
        public Nullable<System.DateTime> DateRequest { get; set; }

        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_CashAdvanceName)]
        public string CashAdvanceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequest_TravelRequestCode)]
        public string TravelRequestCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequest_TravelRequestName)]
        public string TravelRequestName { get; set; }
        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
    }

   

}
