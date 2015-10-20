using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class FIN_CashAdvanceItemModel : BaseViewModel
    {
        public Nullable<bool> IsEntertaiment { get; set; }
    
        public string Code { get; set; }
        public string Other { get; set; }
        public Nullable<System.DateTime> DateRequest { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public string PayTo { get; set; }

        public string ProfileName { get; set; }
        public string JobTitleName { get; set; }
        public string WorkPlaceName { get; set; }
        public string OrgStructureName { get; set; }
        public string SupervisorName { get; set; }
        public string TravelRequestName { get; set; }
        public string Branch { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string Channel { get; set; }


        public Nullable<System.Guid> CashAdvanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Amount)]
        public Nullable<double> Amount { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_Description)]
        public string Remark { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_CashAdvanceItemName)]
        public string CashAdvanceItemName { get; set; }
        public string CashAdvanceName { get; set; }

        private Guid _id = Guid.Empty;
        public Guid CashAdvanceItem_ID
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

        [DisplayName(ConstantDisplay.FIN_ClaimItem_CurrencyName)]
        public string CurrencyName { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_CurrencyName)]
        public Guid? CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_CashAdvanceItemName)]

        public double? TotalAmount { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Amount = "Amount";
            public const string CashAdvanceID = "CashAdvanceID";
            public const string Date = "Date";
            public const string Remark = "Remark";
            public const string Description2 = "Description2";
            public const string Description3 = "Description3";
            public const string Description4 = "Description4";
            public const string CashAdvanceName = "CashAdvanceName";
            public const string CashAdvanceItemName = "CashAdvanceItemName";
            public const string CurrencyName = "CurrencyName";
            public const string StatusView = "StatusView";

        }
       
    }
 
  
}
