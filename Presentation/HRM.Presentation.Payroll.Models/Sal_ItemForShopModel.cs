using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_ItemForShopModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_ShopID)]
        public Nullable<System.Guid> ShopID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_ShopID)]
        public string ShopName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_ItemID)]
        public Nullable<System.Guid> ItemID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_ItemID)]
        public string ItemName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_Target)]
        public Nullable<double> Target { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_Actual)]
        public Nullable<double> Actual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_Month)]
        public Nullable<System.DateTime> Month { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_IsPass)]
        public bool? IsPass { get; set; }
      
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ShopName = "ShopName";
            public const string ShopID = "ShopID";
            public const string ItemID = "ItemID";
            public const string ItemName = "ItemName";
            public const string Target = "Target";
            public const string Actual = "Actual";
            public const string Month = "Month";
            public const string Note = "Note";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
        }
    }

    public class Sal_ItemForShopSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_ShopID)]
        public Nullable<System.Guid> ShopID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
