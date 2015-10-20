using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Category.Models
{
 
    public class Cat_OrgMoreInforContractEntity : HRMBaseModel
    {
        public Nullable<System.Guid> OrgMoreInforID { get; set; }
        public string ContactName { get; set; }
        public string Area { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Telephone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
    }
}
