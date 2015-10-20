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
    
    public partial class MD_CONNECTIONS1
    {
        public MD_CONNECTIONS1()
        {
            this.MD_ADDITIONAL_PROPERTIES = new HashSet<MD_ADDITIONAL_PROPERTIES1>();
            this.MD_CATALOGS = new HashSet<MD_CATALOGS1>();
            this.MD_DERIVATIVES = new HashSet<MD_DERIVATIVES1>();
            this.MD_MIGR_DEPENDENCY = new HashSet<MD_MIGR_DEPENDENCY1>();
            this.MIGR_GENERATION_ORDER = new HashSet<MIGR_GENERATION_ORDER1>();
            this.MD_MIGR_PARAMETER = new HashSet<MD_MIGR_PARAMETER1>();
            this.MD_MIGR_WEAKDEP = new HashSet<MD_MIGR_WEAKDEP1>();
        }
    
        public decimal ID { get; set; }
        public decimal PROJECT_ID_FK { get; set; }
        public string TYPE { get; set; }
        public string HOST { get; set; }
        public Nullable<decimal> PORT { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string DBURL { get; set; }
        public string NAME { get; set; }
        public string NATIVE_SQL { get; set; }
        public string STATUS { get; set; }
        public Nullable<decimal> NUM_CATALOGS { get; set; }
        public Nullable<decimal> NUM_COLUMNS { get; set; }
        public Nullable<decimal> NUM_CONSTRAINTS { get; set; }
        public Nullable<decimal> NUM_GROUPS { get; set; }
        public Nullable<decimal> NUM_ROLES { get; set; }
        public Nullable<decimal> NUM_INDEXES { get; set; }
        public Nullable<decimal> NUM_OTHER_OBJECTS { get; set; }
        public Nullable<decimal> NUM_PACKAGES { get; set; }
        public Nullable<decimal> NUM_PRIVILEGES { get; set; }
        public Nullable<decimal> NUM_SCHEMAS { get; set; }
        public Nullable<decimal> NUM_SEQUENCES { get; set; }
        public Nullable<decimal> NUM_STORED_PROGRAMS { get; set; }
        public Nullable<decimal> NUM_SYNONYMS { get; set; }
        public Nullable<decimal> NUM_TABLES { get; set; }
        public Nullable<decimal> NUM_TABLESPACES { get; set; }
        public Nullable<decimal> NUM_TRIGGERS { get; set; }
        public Nullable<decimal> NUM_USER_DEFINED_DATA_TYPES { get; set; }
        public Nullable<decimal> NUM_USERS { get; set; }
        public Nullable<decimal> NUM_VIEWS { get; set; }
        public string COMMENTS { get; set; }
        public decimal SECURITY_GROUP_ID { get; set; }
        public System.DateTime CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATED_ON { get; set; }
        public string LAST_UPDATED_BY { get; set; }
    
        public virtual ICollection<MD_ADDITIONAL_PROPERTIES1> MD_ADDITIONAL_PROPERTIES { get; set; }
        public virtual ICollection<MD_CATALOGS1> MD_CATALOGS { get; set; }
        public virtual MD_PROJECTS1 MD_PROJECTS { get; set; }
        public virtual ICollection<MD_DERIVATIVES1> MD_DERIVATIVES { get; set; }
        public virtual ICollection<MD_MIGR_DEPENDENCY1> MD_MIGR_DEPENDENCY { get; set; }
        public virtual ICollection<MIGR_GENERATION_ORDER1> MIGR_GENERATION_ORDER { get; set; }
        public virtual ICollection<MD_MIGR_PARAMETER1> MD_MIGR_PARAMETER { get; set; }
        public virtual ICollection<MD_MIGR_WEAKDEP1> MD_MIGR_WEAKDEP { get; set; }
    }
}
