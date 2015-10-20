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
    
    public partial class MD_CATALOGS
    {
        public MD_CATALOGS()
        {
            this.MD_SCHEMAS = new HashSet<MD_SCHEMAS>();
        }
    
        public decimal ID { get; set; }
        public decimal CONNECTION_ID_FK { get; set; }
        public string CATALOG_NAME { get; set; }
        public string DUMMY_FLAG { get; set; }
        public string NATIVE_SQL { get; set; }
        public string NATIVE_KEY { get; set; }
        public string COMMENTS { get; set; }
        public decimal SECURITY_GROUP_ID { get; set; }
        public System.DateTime CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATED_ON { get; set; }
        public string LAST_UPDATED_BY { get; set; }
    
        public virtual MD_CONNECTIONS MD_CONNECTIONS { get; set; }
        public virtual ICollection<MD_SCHEMAS> MD_SCHEMAS { get; set; }
    }
}
