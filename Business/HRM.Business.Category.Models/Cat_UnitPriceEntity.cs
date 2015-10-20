using System;

namespace HRM.Business.Category.Models
{
    public class Cat_UnitPriceEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> RoleID { get; set; }
        public string RoleName { get; set; }
        public Nullable<System.Guid> JobTypeID { get; set; }
        public string JobTypeName { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Note { get; set; }
    }
}

