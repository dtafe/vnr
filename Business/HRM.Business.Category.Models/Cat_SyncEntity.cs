using System;


namespace HRM.Business.Category.Models
{
    public class Cat_SyncEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string SyncName { get; set; }
        public bool? IsFromInner { get; set; }
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string DatabaseName { get; set; }
        public string OuterTable { get; set; }
        public string InnerTable { get; set; }
        public string Description { get; set; }
    
    }

}

