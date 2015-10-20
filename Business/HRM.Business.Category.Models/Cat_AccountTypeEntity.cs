using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_AccountTypeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string AccountTypeName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

       
    }
    public class Cat_AccountTypeMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string AccountTypeName { get; set; }
    }
}
