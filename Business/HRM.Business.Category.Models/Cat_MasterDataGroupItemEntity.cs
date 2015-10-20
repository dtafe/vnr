using System;
using System.Collections.Generic;

namespace HRM.Business.Category.Models
{
    public class Cat_MasterDataGroupItemEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Guid? MasterDataGroupID { get; set; }
        public string ObjectName { get; set; }
        public Guid? ObjectID { get; set; }
        public List<Guid?> ObjectIDs { get; set; }
        public string ObjectIDStr { get; set; }
    }

}
