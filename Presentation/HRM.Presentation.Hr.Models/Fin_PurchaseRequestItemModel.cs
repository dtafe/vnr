using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Fin_PurchaseRequestItemModel : BaseViewModel
    {
        public Guid? PurchaseRequestID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_CostCenterID)]
        public Guid? CostCenterID { get; set; }
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_CateCodeType)]
        public string CateCodeType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Name)]
        public string Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_ProjectID)]
        public Guid? ProjectID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_ProjectID)]
        public string ProjectName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_ItemID)]
        public Guid? ItemID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_PurchaseItemName)]
        public string PurchaseItemName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_PurchaseItemCost)]
        public string PurchaseItemCost { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Quantity)]
        public int? Quantity { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_UnitPrice)]
        public double? UnitPrice { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Amount)]
        public double? Amount { get; set; }
        public string Status { get; set; }

        private Guid _id = Guid.Empty;
        public Guid PruchaseRequestItem_ID
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
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_AmountConvert)]
        public double? AmountConvert { get; set; } 
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string CateCodeType = "CateCodeType";
            public const string Name = "Name";
            public const string ProjectName = "ProjectName";
            public const string PurchaseItemName = "PurchaseItemName";
            public const string Quantity = "Quantity";
            public const string UnitPrice = "UnitPrice";
            public const string Amount = "Amount";
            public const string PurchaseItemCost = "PurchaseItemCost";
            public const string AmountConvert = "AmountConvert";
            
        }
    }
    public class Fin_PurchaseRequestSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_FunctionID)]
        public Guid? FunctionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_From)]
        public DateTime? MonthFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_To)]
        public DateTime? MonthTo { get; set; }

        public Guid? UserCreateID { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_UserApprove)]
        public Guid? UserApproveID { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
    }

    public class Fin_ApprovedPurchaseRequestSearchModel
    {
        public Guid? UserApproveID { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }

        // public Guid ExportId { get; set; }
    }
}
