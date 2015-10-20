using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class Cat_TAMScanReasonMissModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_ReasonName)]
        public string TAMScanReasonMissName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_MealAllowanceTypeSettingName)]
        public Guid? MealAllowanceTypeSettingID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_MealAllowanceTypeSettingName)]
        public string MealAllowanceTypeSettingName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_UserUpdate)]
        public string UserUpdate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_DateUpdate)]
        public DateTime? DateUpdate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_IsFullPay)]
        public bool? IsFullPay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_IsForCMS)]
        public bool? IsForCMS { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_IsRemind)]
        public bool? IsRemind { get; set; }
        /// <summary>
        /// [Tam.Le] - 7.8.2014 - them field Amount lay tu table "Can_MealAllowanceTypeSetting" tu store
        /// </summary>
        /// <param name="amount_missing"></param>
        /// <returns></returns>
        public decimal? Amount { get; set; }

        public string MealAllowanceTypeSettingCode { get; set; }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {

            public const string TAMScanReasonMissName = "TAMScanReasonMissName";
            public const string Code = "Code";
            public const string MealAllowanceTypeSettingID = "MealAllowanceTypeSettingID";
            public const string MealAllowanceTypeSettingName = "MealAllowanceTypeSettingName";
            public const string IsFullPay = "IsFullPay";
            public const string IsForCMS = "IsForCMS";
            public const string IsRemind = "IsRemind";
            public const string Description = "Description";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";

        }
    }
    public class Cat_TAMScanReasonMissSearchModel
    {
     //   [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMissnName)]
        public string TAMScanReasonMissName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_TAMScanReasonMissMuitlModel {
        public Guid ID {get; set;}

        public string TAMScanReasonMissName { get; set; }

    }
}
