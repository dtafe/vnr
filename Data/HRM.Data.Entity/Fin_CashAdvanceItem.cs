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
    
    public partial class Fin_CashAdvanceItem
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CashAdvanceID { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Remark { get; set; }
        public string CashAdvanceItemName { get; set; }
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
        public string Status { get; set; }
    
        public virtual Cat_Currency Cat_Currency { get; set; }
        public virtual Fin_CashAdvance Fin_CashAdvance { get; set; }
    }
}
