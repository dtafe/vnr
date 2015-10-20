using System;

namespace HRM.Business.Category.Models
{
    public class Cat_JobTypeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string JobTypeName { get; set; }
        public string Code { get; set; }
        public Nullable<System.Guid> RoleID { get; set; }
        public string RoleName { get; set; }
        public string Note { get; set; }
    }
    public class Cat_JobTypeMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string JobTypeName { get; set; }
    }
}

