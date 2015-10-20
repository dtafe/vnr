using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_VillageEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string VillageName { get; set; }
        public Nullable<System.Guid> ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public string Notes { get; set; }
        public Nullable<System.Guid> DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string Code { get; set; }
    }

    public class Cat_VillageMultiEntity
    {
        public Guid ID { get; set; }
        public string VillageName { get; set; }
    }
}
