//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM.Data.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class MD_SCHEMAS1
    {
        public MD_SCHEMAS1()
        {
            this.MD_GROUPS = new HashSet<MD_GROUPS1>();
            this.MD_MIGR_WEAKDEP = new HashSet<MD_MIGR_WEAKDEP1>();
            this.MD_OTHER_OBJECTS = new HashSet<MD_OTHER_OBJECTS1>();
            this.MD_PACKAGES = new HashSet<MD_PACKAGES1>();
            this.MD_PRIVILEGES = new HashSet<MD_PRIVILEGES1>();
            this.MD_SEQUENCES = new HashSet<MD_SEQUENCES1>();
            this.MD_STORED_PROGRAMS = new HashSet<MD_STORED_PROGRAMS1>();
            this.MD_SYNONYMS = new HashSet<MD_SYNONYMS1>();
            this.MD_TABLES = new HashSet<MD_TABLES1>();
            this.MD_TABLESPACES = new HashSet<MD_TABLESPACES1>();
            this.MD_USER_DEFINED_DATA_TYPES = new HashSet<MD_USER_DEFINED_DATA_TYPES1>();
            this.MD_USERS = new HashSet<MD_USERS1>();
            this.MD_VIEWS = new HashSet<MD_VIEWS1>();
        }
    
        public decimal ID { get; set; }
        public decimal CATALOG_ID_FK { get; set; }
        public string NAME { get; set; }
        public string TYPE { get; set; }
        public string CHARACTER_SET { get; set; }
        public string VERSION_TAG { get; set; }
        public string NATIVE_SQL { get; set; }
        public string NATIVE_KEY { get; set; }
        public string COMMENTS { get; set; }
        public decimal SECURITY_GROUP_ID { get; set; }
        public System.DateTime CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATED_ON { get; set; }
        public string LAST_UPDATED_BY { get; set; }
    
        public virtual MD_CATALOGS1 MD_CATALOGS { get; set; }
        public virtual ICollection<MD_GROUPS1> MD_GROUPS { get; set; }
        public virtual ICollection<MD_MIGR_WEAKDEP1> MD_MIGR_WEAKDEP { get; set; }
        public virtual ICollection<MD_OTHER_OBJECTS1> MD_OTHER_OBJECTS { get; set; }
        public virtual ICollection<MD_PACKAGES1> MD_PACKAGES { get; set; }
        public virtual ICollection<MD_PRIVILEGES1> MD_PRIVILEGES { get; set; }
        public virtual ICollection<MD_SEQUENCES1> MD_SEQUENCES { get; set; }
        public virtual ICollection<MD_STORED_PROGRAMS1> MD_STORED_PROGRAMS { get; set; }
        public virtual ICollection<MD_SYNONYMS1> MD_SYNONYMS { get; set; }
        public virtual ICollection<MD_TABLES1> MD_TABLES { get; set; }
        public virtual ICollection<MD_TABLESPACES1> MD_TABLESPACES { get; set; }
        public virtual ICollection<MD_USER_DEFINED_DATA_TYPES1> MD_USER_DEFINED_DATA_TYPES { get; set; }
        public virtual ICollection<MD_USERS1> MD_USERS { get; set; }
        public virtual ICollection<MD_VIEWS1> MD_VIEWS { get; set; }
    }
}
