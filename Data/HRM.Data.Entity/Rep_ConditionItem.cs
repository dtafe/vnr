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
    
    public partial class Rep_ConditionItem
    {
        public System.Guid ID { get; set; }
        public string ConditionItemName { get; set; }
        public Nullable<System.Guid> ConditionID { get; set; }
        public string JoinType { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string Operator { get; set; }
        public string ServerUpdate { get; set; }
        public string ServerCreate { get; set; }
        public string UserUpdate { get; set; }
        public string UserCreate { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    
        public virtual Rep_Condition Rep_Condition { get; set; }
    }
}
