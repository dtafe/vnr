//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM.Data.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class MIGR_DATATYPE_TRANSFORM_MAP
    {
        public MIGR_DATATYPE_TRANSFORM_MAP()
        {
            this.MIGR_DATATYPE_TRANSFORM_RULE = new HashSet<MIGR_DATATYPE_TRANSFORM_RULE>();
        }
    
        public decimal ID { get; set; }
        public decimal PROJECT_ID_FK { get; set; }
        public string MAP_NAME { get; set; }
        public decimal SECURITY_GROUP_ID { get; set; }
        public System.DateTime CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATED_ON { get; set; }
        public string LAST_UPDATED_BY { get; set; }
    
        public virtual MD_PROJECTS MD_PROJECTS { get; set; }
        public virtual ICollection<MIGR_DATATYPE_TRANSFORM_RULE> MIGR_DATATYPE_TRANSFORM_RULE { get; set; }
    }
}
