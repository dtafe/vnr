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
    public class Sal_UnusualPayItemModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPayItem_UnusualPayID)]
         public System.Guid UnusualPayID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPayItem_UnusualPayItemName)]
         public string UnusualPayItemName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPayItem_Code)]
         public string Code { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPayItem_Amount)]
         public double Amount { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPayItem_CurrencyID)]
         public Nullable<System.Guid> CurrencyID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPayItem_CurrencyID)]
         public string CurrencyName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPayItem_DateSubmit)]
         public Nullable<System.DateTime> DateSubmit { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPayItem_ReceiptNo)]
         public string ReceiptNo { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPayItem_AttachFile)]
         public string AttachFile { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPayItem_Element)]
         public string Element { get; set; }
         public int TotalRow { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string UnusualPayID = "UnusualPayID";
            public const string UnusualPayItemName = "UnusualPayItemName";
            public const string Code = "Code";
            public const string Amount = "Amount";
            public const string CurrencyID = "CurrencyID";
            public const string CurrencyName = "CurrencyName";
            public const string DateSubmit = "DateSubmit";
            public const string ReceiptNo = "ReceiptNo";
            public const string AttachFile = "AttachFile";
            public const string Element = "Element";
        }
    }

    public class Sal_UnusualPayItemSearchModel
    {
        public Guid ProfileID { get; set; }
        public Guid? CutOffDurationID { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
    }
}
