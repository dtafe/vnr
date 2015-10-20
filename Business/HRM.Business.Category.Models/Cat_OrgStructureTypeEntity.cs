using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Category.Models
{
    public class Cat_OrgStructureTypeMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string OrgStructureTypeName { get; set; }
        public string OrgStructureTypeCode { get; set; }
    }

    public class Cat_OrgStructureTypeEntity : HRMBaseModel
    {
        public System.Guid ID { get; set; }
        public int TotalRow { get; set; }
        public string OrgStructureTypeName { get; set; }
        public string OrgStructureTypeCode { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string ServerUpdate { get; set; }
        public string ServerCreate { get; set; }
        public string UserUpdate { get; set; }
        public string UserCreate { get; set; }
}
   
}
