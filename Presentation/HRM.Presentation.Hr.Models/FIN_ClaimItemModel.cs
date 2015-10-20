using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class FIN_ClaimItemModel : BaseViewModel
    {

        public string ProfileName { get; set; }
        public string JobTitleName { get; set; }
        public string WorkPlaceName { get; set; }
        public string OrgStructureName { get; set; }
        public string CashAdvanceName { get; set; }

        public string SupervisorName { get; set; }
        public string Branch { get; set; }

        public string Area { get; set; }
        public string Region { get; set; }
        public string Channel { get; set; }

        public string StatusView { get; set; }

        public string AccountCode { get; set; }

        public string ClaimCode { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }

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
        public string ExpensesType { get; set; }

        public Nullable<bool> IsCashAdvance { get; set; }
        public string Other { get; set; }




        [DisplayName(ConstantDisplay.FIN_ClaimItem_ClaimID)]
        public Nullable<System.Guid> ClaimID { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_ClaimID)]
        public string ClaimName { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_Date)]
        public Nullable<System.DateTime> Date { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_Description2)]
        public string Description2 { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_Description3)]
        public string Description3 { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_Description4)]
        public string Description4 { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_Amount)]
        public Nullable<double> Amount { get; set; }
        private Guid _id = Guid.Empty;
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Total)]
        public double? Total { get; set; }
        public Guid ClaimItem_ID
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
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_TravelRequestName)]
        public string TravelRequestName { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_ClaimItemName)]
        public string ClaimItemName { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_DocumentNumber)]
        public string DocumentNumber { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_CurrencyName)]
        public string CurrencyName { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_CurrencyName)]
        public Guid? CurrencyID { get; set; }
         [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_TotalAmount)]
        public double? TotalAmount { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Amount = "Amount";
            public const string ClaimID = "ClaimID";
            public const string ClaimName = "ClaimName";
            public const string Date = "Date";
            public const string Description = "Description";
            public const string Description2 = "Description2";
            public const string Description3 = "Description3";
            public const string Description4 = "Description4";
            public const string DocumentNumber = "DocumentNumber";
            public const string ClaimItemName = "ClaimItemName";
            public const string CurrencyName = "CurrencyName";
            public const string StatusView = "StatusView";

        }
       
    }
    public class FIN_ClaimItemSearchByClaimIDModel
    {
        [DisplayName(ConstantDisplay.FIN_ClaimItem_ClaimID)]
        public Nullable<System.Guid> ClaimID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
  
}
