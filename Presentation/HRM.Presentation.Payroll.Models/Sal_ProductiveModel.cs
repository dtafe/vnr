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
    public class Sal_ProductiveModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Nullable<System.Guid> ProfileID { get; set; }
            [DisplayName(ConstantDisplay.HRM_Category_Product_ProductName)]
        public Nullable<System.Guid> ProductID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AllowLimitOvertime_DateStart)]
        public Nullable<System.DateTime> StartDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AllowLimitOvertime_DateEnd)]
        public Nullable<System.DateTime> EndDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Quantity)]
        public Nullable<double> Quantity { get; set; }
          [DisplayName(ConstantDisplay.HRM_Att_CutOffDuration)]
        public Nullable<System.Guid> CutOffDurationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_Note)]
        public string Note { get; set; }
          [DisplayName(ConstantDisplay.FIN_ClaimItem_CurrencyName)]
        public Nullable<System.Guid> CurrencyID { get; set; }
          [DisplayName(ConstantDisplay.HRM_Category_ProductItem_ProductItemName)]
        public Nullable<System.Guid> ProductItemID { get; set; }
          [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
           [DisplayName(ConstantDisplay.HRM_Category_Product_ProductName)]
        public string ProductName { get; set; }
              [DisplayName(ConstantDisplay.HRM_Category_ProductItem_ProductItemName)]
        public string ProductItemName { get; set; }
                  [DisplayName(ConstantDisplay.FIN_ClaimItem_CurrencyName)]
        public string CurrencyName { get; set; }
        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string ProductName = "ProductName";
            public const string StartDate = "StartDate";
            public const string EndDate = "EndDate";
            public const string Quantity = "Quantity";
            public const string CutOffDurationID = "CutOffDurationID";
            public const string Note = "Note";
            public const string CurrencyName = "CurrencyName";
            public const string ProductItemName = "ProductItemName";
        }
    }

    public class Sal_ProductiveSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Product_ProductName)]
        public Nullable<System.Guid> ProductID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_ProductItemName)]
        public Nullable<System.Guid> ProductItemID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AllowLimitOvertime_DateStart)]
        public Nullable<System.DateTime> StartDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AllowLimitOvertime_DateEnd)]
        public Nullable<System.DateTime> EndDate { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }


}
