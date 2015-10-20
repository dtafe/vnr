using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_ComputeBackPayModel : BaseViewModel
    {
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_ProfileName)]       
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_ProfileID)]       
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Can_ComputeBackPay_Month)]       
        public string MonthYear { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_MealAllowanceTypeSettingID)]    
        public string Can_MealAllowanceTypeSetting { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_MealAllowanceTypeSettingID)]
        public Guid? MealAllowanceTypeSettingID { get; set; }
        
        public bool? IsFullPay { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Can_ComputeBackPay_Type)]       
        public string Type { get; set; }

        /// <summary> tổng số lần không quẹt thẻ
        /// </summary>
        [DisplayName(ConstantDisplay.HRM_Canteen_Can_ComputeBackPay_Total)]       
        public double Total { get; set; }

        /// <summary> Tổng số lần không quẹt thẻ (gồm cả số lần theo công thức)
        /// </summary>
        [DisplayName(ConstantDisplay.HRM_Canteen_Can_ComputeBackPay_Summary)]
        public double Summary { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Can_ComputeBackPay_TotalSupport)]
        public double TotalSupport { get; set; }

        /// <summary> Số tiền trợ cấp chuẩn (với standard là true)
        /// </summary>
        [DisplayName(ConstantDisplay.HRM_Canteen_Can_ComputeBackPay_Amount)]       
        public double Amount { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_CountByFomular)]       
        public double? CountByFomular { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_AmountByFomular)]       
        public double? AmountByFomular { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_Note)]       
        public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileName = "ProfileName";
            public const string ProfileID = "ProfileID";
            public const string MonthYear = "MonthYear";
            public const string Note = "Note";
            public const string Summary = "Summary";
            public const string CountByFomular = "CountByFomular";
            public const string AmountByFomular = "AmountByFomular";
            public const string Can_MealAllowanceTypeSetting = "Can_MealAllowanceTypeSetting";
            public const string MealAllowanceTypeSettingID = "MealAllowanceTypeSettingID";
            public const string Total = "Total";
            public const string TotalSupport = "TotalSupport";
            public const string IsFullPay = "IsFullPay";
            public const string Amount = "Amount";
            public const string Type = "Type";
            public const string OrgStructureID = "OrgStructureID";
        }
    }
    public class Can_ComputeBackPaySearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Can_ComputeBackPay_Month)]       
        public DateTime? MonthYear { get; set; }

         [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
    }
   
}
