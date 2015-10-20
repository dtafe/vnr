using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_SupplierModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_Category_Supplier_SupplierName)]
        public string SupplierName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Supplier_Code)]
        public string SupplierCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Supplier_Note)]
        public string Note { get; set; }
        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string SupplierName = "SupplierName";
            public const string SupplierCode = "SupplierCode";
            public const string Note = "Note";
        }
    }

    public class Cat_SupplierSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Supplier_SupplierName)]
        public string SupplierName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_SupplierMultiModel
    {
        public Guid ID { get; set; }
        public string SupplierName { get; set; }
        public int TotalRow { get; set; }
    }

}
