using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class FIN_ClaimModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_FIN_Claim_ProfileID)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_AccountCode)]
        public string AccountCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_ClaimName)]
        public string ClaimName { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_ClaimCode)]
        public string ClaimCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_TravelRequestID)]
        public Nullable<System.Guid> TravelRequestID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_TravelRequestID)]
        public string TravelRequestName { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_Status)]
        public string StatusView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UserApprovedID)]
        public string UserApprovedName { get; set; }
        private Guid _id = Guid.Empty;


        public Nullable<System.Guid> UserCreateID { get; set; }

        public string UserCreateName { get; set; }

        public Nullable<System.Guid> UserApproveID { get; set; }

        public string UserApproveName { get; set; }

        [DisplayName(ConstantDisplay.HRM_FIN_Claim_PayTo)]

        public string PayTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UserApprovedID)]
        public Guid? UserApprovedID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Subject)]
        public string Subject { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_DateFromEstimate)]
        public Nullable<System.DateTime> DateFromEstimate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_DateToEstimate)]
        public Nullable<System.DateTime> DateToEstimate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_FileAttachment)]
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
        [DisplayName(ConstantDisplay.HRM_Category_Grade_CurrencyID)]
        public Nullable<System.Guid> CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Grade_CurrencyID)]
        public string CurrencyName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Position)]
        public string Position { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Dept)]
        public string Dept { get; set; }

        [DisplayName(ConstantDisplay.HRM_FIN_Claim_IsPaymentType)]
        public bool? IsPaymentType { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_IsOther)]
        public bool? IsOther { get; set; }

        [DisplayName(ConstantDisplay.HRM_FIN_Claim_IsManufactureName)]
        public bool? IsManufactureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_IsProfile)]

        public bool? IsProfile { get; set; }


        [DisplayName(ConstantDisplay.HRM_FIN_Claim_TotalAmount)]
        public int? TotalAmount { get; set; }

        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_TotalAmount)]
        public int? TotalAmountCashAdvance { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_Total)]

        public double? Total { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_ToTalAll)]

        public double? TotalAll { get; set; }


        public Guid Claim_ID
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
        //public Guid TravelRequest_ID
        //{
        //    get
        //    {
        //        _id = TravelRequestID.Value;
        //        return _id;
        //    }
        //    set
        //    {
        //        _id = value;
        //        TravelRequestID = value;
        //    }
        //}
        public bool isPayment { get; set; }

        [DisplayName(ConstantDisplay.HRM_FIN_Claim_ExpensesType)]
        public string ExpensesType { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_CashAdvanceName)]
        public Nullable<System.Guid> CashAdvanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_IsCashAdvance)]
        public Nullable<bool> IsCashAdvance { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_IsNoneCashAdvance)]
        public Nullable<bool> IsNoneCashAdvance { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_Other)]
        public string Other { get; set; }
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
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_CashAdvanceName)]
        public string CashAdvanceName { get; set; }
        public string PositionName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Total = "Total";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string AccountCode = "AccountCode";
            public const string ClaimName = "ClaimName";
            public const string ClaimCode = "ClaimCode";
            public const string TravelRequestID = "TravelRequestID";
            public const string TravelRequestName = "TravelRequestName";
            public const string Status = "Status";
            public const string StatusView = "StatusView";

            public const string UserApprovedName = "UserApprovedName";
            public const string CostActual = "CostActual";
            public const string Balance = "Balance";
            public const string PaidAmount = "PaidAmount";
            public const string DateFromActual = "DateFromActual";
            public const string DateToActual = "DateToActual";
            public const string CashAdvanceName = "CashAdvanceName";
            public const string TotalAll = "TotalAll";
        }
    }
    public class FIN_ClaimSearchByTravelRequestIDModel
    {
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_TravelRequestID)]
        public Nullable<System.Guid> TravelRequestID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class FIN_ApprovedClaimSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_TravelRequestID)]
        public Nullable<System.Guid> UserApproveID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class FIN_ClaimSearchModel:BaseViewSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_DateRequest)]
        public Nullable<System.DateTime> DateRequest { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_ClaimName)]
        public string ClaimName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UserCreateID)]
        public Nullable<System.Guid> UserCreateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UserApprovedID)]
        public Nullable<System.Guid> UserApproveID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequest_TravelRequestCode)]
        public string TravelRequestCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequest_TravelRequestName)]
        public string TravelRequestName { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }



}
