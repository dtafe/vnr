using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_RevenueRecordModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_ShopID)]
        public Nullable<System.Guid> ShopID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_Month)]
        public Nullable<System.DateTime> Month { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_Amount)]
        public Nullable<double> Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_Note)]
        public string Note { get; set; }
        public Nullable<System.Guid> KPIBonusID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_ShopID)]
        public string ShopName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_Target)]
        public double? Taget { get; set; }
        public double? ShopTaget { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_Actual)]
        public double? Actual { get; set; }
        public double? ShopActual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_Value)]
        public double? Value { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_KPIBonusName)]
        public string KPIBonusName { get; set; }

        public int NoShiftLeader { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ShopID = "ShopID";
            public const string ShopName = "ShopName";
            public const string Month = "Month";
            public const string Type = "Type";
            public const string Amount = "Amount";
            public const string Note = "Note";
            public const string Taget = "Taget";
            public const string Actual = "Actual";
            public const string Value = "Value";
            public const string KPIBonusName = "KPIBonusName";
            public const string KPIBonusID = "KPIBonusID";
        }
    }

    public class Sal_RevenueRecordSearchModel {
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_Month)]
        public DateTime? Month { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueRecord_ShopID)]
        public Nullable<System.Guid> ShopID { get; set; }

        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    
    }

    public class Sal_RevenueRecordDisplayModel : BaseViewModel
    {
        public string ShopName { get; set; }
        public string ShopCode { get; set; }
        public string OtherName { get; set; }
        public double? Actual { get; set; }
        public double? Target { get; set; }
        public bool IsParent { get; set; }
        public DateTime Month { get; set; }

    }
}
