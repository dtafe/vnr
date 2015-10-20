using System;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Canteen.Models
{
    public class Can_BackPayEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Guid? ProfileID { get; set; }

        public string ProfileName { get; set; }

        public DateTime? MonthYear { get; set; }

        public double? Count { get; set; }

        public double? CountByFomular { get; set; }

        public double? AmountByFomular { get; set; }
        
        public double? Amount { get; set; }

        public Guid? MealAllowanceTypeSettingID { get; set; }
        public Guid? MealAllowanceTypeID { get; set; }
        public string MealAllowanceTypeName { get; set; }

        public int TotalRow { get; set; }

        [MaxLength(1000)]
        public string Note { get; set; }
    }
}
