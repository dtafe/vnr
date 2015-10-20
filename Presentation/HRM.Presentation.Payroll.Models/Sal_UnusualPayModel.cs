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
    public class Sal_UnusualPayModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_ProfileID)]
        public System.Guid ProfileID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_ProfileID)]
         public string ProfileName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_MonthYear)]
         public Nullable<System.DateTime> MonthYear { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_DateStart)]
         public Nullable<System.DateTime> DateStart { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_DateEnd)]
         public Nullable<System.DateTime> DateEnd { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_Type)]
         public string Type { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_TypeOfIssuance)]
         public string TypeOfIssuance { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_Status)]
         public string Status { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_PaymentMethod)]
         public string PaymentMethod { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_DateOfPayment)]
         public Nullable<System.DateTime> DateOfPayment { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_ReceiptCode)]
         public string ReceiptCode { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_Amount)]
         public double Amount { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_AmountPaidPITCom)]
         public double AmountPaidPITCom { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_AmountPaidPITEmp)]
         public double AmountPaidPITEmp { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_Budget)]
         public Nullable<double> Budget { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_UserApproveID)]
         public Nullable<System.Guid> UserApproveID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_UserApproveID)]
         public string UserApproveIDName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_UserApproveID1)]
         public Nullable<System.Guid> UserApproveID1 { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_UserApproveID1)]
         public string UserApproveID1Name { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_Description)]
         public string Description { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_AttachFile)]
         public string AttachFile { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_CurrentcyID)]
         public Nullable<System.Guid> CurrentcyID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_CurrentcyID)]
         public string CurrencyName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_UnusualPay_IsSubSalary)]
         public Nullable<bool> IsSubSalary { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileName = "ProfileName";
            public const string ProfileID = "ProfileID";
            public const string MonthYear = "MonthYear";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Type = "Type";
            public const string TypeOfIssuance = "TypeOfIssuance";
            public const string Status = "Status";
            public const string PaymentMethod = "PaymentMethod";
            public const string DateOfPayment = "DateOfPayment";
            public const string ReceiptCode = "ReceiptCode";
            public const string Amount = "Amount";
            public const string AmountPaidPITCom = "AmountPaidPITCom";
            public const string AmountPaidPITEmp = "AmountPaidPITEmp";
            public const string Budget = "Budget";
            public const string UserApproveID = "UserApproveID";
            public const string UserApproveIDName = "UserApproveIDName";
            public const string UserApproveID1 = "UserApproveID1";
            public const string UserApproveID1Name = "UserApproveID1Name";
            public const string Description = "Description";
            public const string AttachFile = "AttachFile";
            public const string CurrentcyID = "CurrentcyID";
            public const string CurrencyName = "CurrencyName";
            public const string IsSubSalary = "IsSubSalary";
        }
    }
   
    public class Sal_UnusualPaySearchModel
    {

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
