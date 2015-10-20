using System;

namespace HRM.Business.Category.Models
{
    public class Cat_RoleEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string RoleName { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
    }
    public class Cat_RoleMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string RoleName { get; set; }
    }
}

