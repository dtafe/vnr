using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Fin_PurchaseRequestModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_FunctionID)]
        public Guid? FunctionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_FunctionID)]
        public string FunctionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_BudgetOwnerID)]
        public Guid? BudgetOwnerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_BudgetOwnerID)]
        public string BudgetOwnerName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_ChannelID)]
        public Guid? ChannelID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_ChannelID)]
        public string ChannelName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_BudgetChargedIn)]
        public DateTime? BudgetChargedIn { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_From)]
        public DateTime? From { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_To)]
        public DateTime? To { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_SupplierID)]
        public Guid? SupplierID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_SupplierID)]
        public string SupplierName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UserApprovedID)]
        public Guid? UserApprovedID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UserApprovedID)]
        public string UserApprovedName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_UserCreate)]
        public Nullable<System.Guid> UserCreateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_UserCreate)]
        public string UserCreateName { get; set; }
        public string CurrencyName1 { get; set; }
        public string CurrencyName2 { get; set; }

        private Guid _id = Guid.Empty;
        public double? Total { get; set; }
        public Guid PruchaseRequest_ID
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
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_CurrencyName1)]
        public Nullable<System.Guid> CurrencyID1 { get; set; }
        public Nullable<System.Guid> CurrencyID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_CurrencyRate1)]
        public int? CurrencyRate1 { get; set; }
        public int? CurencryRate2 { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string FunctionName = "FunctionName";
            public const string BudgetOwnerName = "BudgetOwnerName";
            public const string ChannelName = "ChannelName";
            public const string BudgetChargedIn = "BudgetChargedIn";
            public const string From = "From";
            public const string To = "To";
            public const string SupplierName = "SupplierName";
            public const string DistrictName = "DistrictName";
            public const string Description = "Description";
            public const string Status = "Status";
            public const string UserCreateName = "UserCreateName";
            public const string UserApprovedName = "UserApprovedName";
            public const string Total = "Total";
        }
    }
  
}
