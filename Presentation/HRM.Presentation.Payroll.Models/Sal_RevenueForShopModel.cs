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
    public class Sal_RevenueForShopModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_ShopID)]
        public Nullable<System.Guid> ShopID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_ShopID)]
        public string ShopName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_Target)]
        public Nullable<double> Target { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_Actual)]
        public Nullable<double> Actual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_Month)]
        public Nullable<System.DateTime> Month { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_KPIBonusID)]
        public Nullable<System.Guid> KPIBonusID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_KPIBonusID)]
        public string KPIBonusName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_DateTo)]
        public DateTime? DateTo { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_IsPass)]
        public bool? IsPass { get; set; }

        public int NoShiftLeader { get; set; }

        [DefaultValue(0)]
        public double Percent { get; set; }
        public string PrinceSL { get; set; }
        public double? TotalPrince { get; set; }
        public string StrOrg { get; set; }
        public string LineItemName { get; set; }
        public string ItemName { get; set; }
        public string Title { get; set; }

        public int TotalProfile { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ShopID = "ShopID";
            public const string ShopName = "ShopName";
            public const string KPIBonusName = "KPIBonusName";
            public const string Target = "Target";
            public const string Actual = "Actual";
            public const string Month = "Month";
            public const string Type = "Type";
            public const string Note = "Note";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
        }
    }

    public class Sal_RevenueForShopSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_ShopID)]
        public Nullable<System.Guid> ShopID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForShop_KPIBonusID)]
        public Nullable<System.Guid> KPIBonusID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
