using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_DataGroupMultiEntity
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string DataGroupName { get; set; }
        public int OrderNumber { get; set; }
    }
    public class Cat_DataGroupEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string DataGroupName { get; set; }
        public int OrderNumber { get; set; }
        public string Notes { get; set; }
    }
}
