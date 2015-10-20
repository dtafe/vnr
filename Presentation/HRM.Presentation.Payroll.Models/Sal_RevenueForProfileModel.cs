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
    public class Sal_RevenueForProfileModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_ShopID)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_RevenueForProfile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_Target)]
        public Nullable<double> Target { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_Actual)]
        public Nullable<double> Actual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_Month)]
        public Nullable<System.DateTime> Month { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_Note)]
        public string Note { get; set; }
        public string CodeEmp { get; set; }
        public string Type { get; set; }
        public string TypeTranslate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_ItemForShop_DateTo)]
        public DateTime? DateTo { get; set; }
      
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ShopName = "ShopName";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
          
            public const string Target = "Target";
            public const string Actual = "Actual";
            public const string Month = "Month";
            public const string Note = "Note";
            public const string Type = "Type";
            public const string TypeTranslate = "TypeTranslate";
            public const string CodeEmp = "CodeEmp";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
        }
    }

    public class Sal_RevenueForProfileSearchModel
    {

        public string ProfileName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
