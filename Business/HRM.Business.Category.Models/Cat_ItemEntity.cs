using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Cat_ItemEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> LineItemID { get; set; }
        public string LineItemName { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
        public string Note { get; set; }
    }

    public class Cat_ItemMultiEntity
    {
        public Guid ID { get; set; }
        public string LineItemName { get; set; }
     
    }
}
