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
    
    public partial class MD_CONSTRAINT_DETAILS1
    {
        public decimal ID { get; set; }
        public string REF_FLAG { get; set; }
        public decimal CONSTRAINT_ID_FK { get; set; }
        public Nullable<decimal> COLUMN_ID_FK { get; set; }
        public Nullable<decimal> COLUMN_PORTION { get; set; }
        public string CONSTRAINT_TEXT { get; set; }
        public decimal DETAIL_ORDER { get; set; }
        public decimal SECURITY_GROUP_ID { get; set; }
        public System.DateTime CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATED_ON { get; set; }
        public string LAST_UPDATED_BY { get; set; }
    
        public virtual MD_COLUMNS1 MD_COLUMNS { get; set; }
        public virtual MD_CONSTRAINTS1 MD_CONSTRAINTS { get; set; }
    }
}
