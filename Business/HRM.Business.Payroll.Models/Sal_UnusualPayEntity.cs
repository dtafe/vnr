using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_UnusualPayEntity : HRMBaseModel
    {
        public System.Guid ProfileID { get; set; }
        public string ProfileName { get; set; }
        public Nullable<System.DateTime> MonthYear { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public string Type { get; set; }
        public string TypeOfIssuance { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public Nullable<System.DateTime> DateOfPayment { get; set; }
        public string ReceiptCode { get; set; }
        public double Amount { get; set; }
        public double AmountPaidPITCom { get; set; }
        public double AmountPaidPITEmp { get; set; }
        public Nullable<double> Budget { get; set; }
        public Nullable<System.Guid> UserApproveID { get; set; }
        public string UserApproveIDName { get; set; }
        public Nullable<System.Guid> UserApproveID1 { get; set; }
        public string UserApproveID1Name { get; set; }
        public string Description { get; set; }
        public string AttachFile { get; set; }
        public Nullable<System.Guid> CurrentcyID { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<bool> IsSubSalary { get; set; }
    }
}
