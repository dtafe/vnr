using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;
namespace HRM.Business.Canteen.Models
{
    public class Can_MealAllowanceTypeSettingEntity : HRMBaseModel
    {
        public string MealAllowanceTypeSettingCode { get; set; }
        public string MealAllowanceTypeSettingName { get; set; }
        
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public double? Amount { get; set; }
        public bool? Standard { get; set; }
        public bool? IsMealAllowanceToMoney { get; set; }
        public string Note { get; set; }
    }
    public class Can_MealAllowanceTypeSettingMultiEntity
    {
        public Guid ID { get; set; }
        public string MealAllowanceTypeSettingName { get; set; }
    }
}
