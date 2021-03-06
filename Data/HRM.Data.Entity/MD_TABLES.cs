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
    
    public partial class MD_TABLES
    {
        public MD_TABLES()
        {
            this.MD_COLUMNS = new HashSet<MD_COLUMNS>();
            this.MD_CONSTRAINTS = new HashSet<MD_CONSTRAINTS>();
            this.MD_INDEXES = new HashSet<MD_INDEXES>();
            this.MD_PARTITIONS = new HashSet<MD_PARTITIONS>();
        }
    
        public decimal ID { get; set; }
        public decimal SCHEMA_ID_FK { get; set; }
        public string TABLE_NAME { get; set; }
        public string NATIVE_SQL { get; set; }
        public string NATIVE_KEY { get; set; }
        public string QUALIFIED_NATIVE_NAME { get; set; }
        public string COMMENTS { get; set; }
        public decimal SECURITY_GROUP_ID { get; set; }
        public System.DateTime CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATED_ON { get; set; }
        public string LAST_UPDATED_BY { get; set; }
    
        public virtual ICollection<MD_COLUMNS> MD_COLUMNS { get; set; }
        public virtual ICollection<MD_CONSTRAINTS> MD_CONSTRAINTS { get; set; }
        public virtual ICollection<MD_INDEXES> MD_INDEXES { get; set; }
        public virtual ICollection<MD_PARTITIONS> MD_PARTITIONS { get; set; }
        public virtual MD_SCHEMAS MD_SCHEMAS { get; set; }
    }
}
