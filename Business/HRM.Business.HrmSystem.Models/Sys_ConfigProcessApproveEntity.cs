using System;


namespace HRM.Business.HrmSystem.Models
{
     public class Sys_ConfigProcessApproveEntity : HRM.Business.BaseModel.HRMBaseModel 
    {

        public string ConfigProcessApproveName { get; set; }
        public Nullable<System.Guid> DeptID { get; set; }
        public string Function { get; set; }
        public string CurrentStatus { get; set; }
        public string NextStatus { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public string TypeInCharge { get; set; }
        public string InCharge { get; set; }
        public string NextStatusFormular { get; set; } 
        public string OrgStructureName { get; set; }
       
    }
}
