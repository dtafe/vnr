using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_ShopGroupEntity : HRMBaseModel
    {
        public string ShopGroupName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        [DefaultValue(0)]
        public int CountShop { get; set; }
    }

    public class Cat_ShopGroupMultiEntity
    {
        public Guid ID { get; set; }
        public string ShopGroupName { get; set; }
    }
}
