using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_UnAllowCfgAmountModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_UnusualEDTypeID)]
        public Nullable<System.Guid> UnusualAllowanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnAllowCfgAmount_FromDate)]
        public Nullable<System.DateTime> FromDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnAllowCfgAmount_ToDate)]
        public Nullable<System.DateTime> ToDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnAllowCfgAmount_Amount)]
        public Nullable<double> Amount { get; set; }
        public string UnusualAllowanceCfgName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string UnusualAllowanceCfgName = "UnusualAllowanceCfgName";
            public const string FromDate = "FromDate";
            public const string ToDate = "ToDate";
            public const string Amount = "Amount";
        }
    }

    public class Cat_UnAllowCfgAmountSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_UnusualEDTypeID)]
        public Nullable<System.Guid> UnusualAllowanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_UnusualED_DateEffect)]
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
