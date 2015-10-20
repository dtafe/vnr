using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_TAMScanReasonMissEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string TAMScanReasonMissName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Guid? MealAllowanceTypeSettingID { get; set; }
        public string MealAllowanceTypeSettingName { get; set; }
        public bool? IsForCMS { get; set; }
        public bool? IsRemind { get; set; }
        public bool? IsFullPay { get; set; }
        public string UserUpdate { get; set; }
        public DateTime? DateUpdate { get; set; }
        /// <summary>
        /// [Tam.Le] - 7.8.2014 - them field Amount lay tu table "Can_MealAllowanceTypeSetting" tu store
        /// </summary>
        /// <param name="amount_missing"></param>
        /// <returns></returns>
        public decimal? Amount { get; set; }
    }
    public class Cat_TAMScanReasonMissMultiEntity
    {
        public Guid ID { get; set; }
        public string TAMScanReasonMissName { get; set; }
    }
}
