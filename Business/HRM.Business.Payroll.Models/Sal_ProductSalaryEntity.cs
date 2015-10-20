using HRM.Business.BaseModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_ProductSalaryEntity : HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.Guid> ProductID { get; set; }
        public Nullable<System.Guid> ProductItemID { get; set; }
        public Nullable<System.DateTime> MonthYear { get; set; }
        public Nullable<double> QtyPrevious { get; set; }
        public Nullable<double> QtyActual { get; set; }
        public Nullable<double> QtySalary { get; set; }
        public Nullable<double> QtyNext { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Product_ProductName)]
        public string ProductName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_ProductItemName)]
        public string ProductItemName { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_CurrencyName)]
        public string CurrencyName { get; set; }
    }
}
