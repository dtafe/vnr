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
    
    public partial class MD_SYNONYMS
    {
        public decimal ID { get; set; }
        public decimal SCHEMA_ID_FK { get; set; }
        public string NAME { get; set; }
        public decimal SYNONYM_FOR_ID { get; set; }
        public string FOR_OBJECT_TYPE { get; set; }
        public string PRIVATE_VISIBILITY { get; set; }
        public string NATIVE_SQL { get; set; }
        public string NATIVE_KEY { get; set; }
        public string COMMENTS { get; set; }
        public decimal SECURITY_GROUP_ID { get; set; }
        public System.DateTime CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATED_ON { get; set; }
        public string LAST_UPDATED_BY { get; set; }
    
        public virtual MD_SCHEMAS MD_SCHEMAS { get; set; }
    }
}
