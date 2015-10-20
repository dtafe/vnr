using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_UnusualPayItemEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public System.Guid UnusualPayID { get; set; }
        public string UnusualPayItemName { get; set; }
        public string Code { get; set; }
        public double Amount { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<System.DateTime> DateSubmit { get; set; }
        public string ReceiptNo { get; set; }
        public string AttachFile { get; set; }
        public string Element { get; set; }
    }
}
