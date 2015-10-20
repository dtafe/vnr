using System;

namespace HRM.Business.Canteen.Models
{
    public class Can_MealAllowanceToMoneyEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Guid? ProfileID { get; set; }
        public string ProfileName { get; set; }      
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        ///// <summary> Loại phụ cấp </summary>
        //public int MealAllowanceType { get; set; }
        ///// <summary> Tên Loại phụ cấp </summary>
        //public string MealAllowanceTypeName { get; set; }
        public Guid? MealAllowanceTypeID { get; set; }
        public string MealAllowanceTypeSettingName { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string CodeEmp { get; set; }

        public int TotalRow { get; set; }
    }
    public class Can_MealAllowanceToMoneyMultiEntity
    {
        public Guid ID { get; set; }
        public string MealAllowanceToMoneyName { get; set; }       
    }
}
