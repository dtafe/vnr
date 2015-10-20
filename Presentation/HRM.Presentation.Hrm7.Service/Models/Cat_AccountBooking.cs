//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM.Presentation.Hrm7.Service.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cat_AccountBooking
    {
        public System.Guid ID { get; set; }
        public string PayrollElement { get; set; }
        public string CostCenter { get; set; }
        public string LaborType { get; set; }
        public string AccCredit { get; set; }
        public string AccDebit { get; set; }
        public string Description { get; set; }
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
        public string Description2 { get; set; }
        public Nullable<System.Guid> AccountTypeID { get; set; }
        public Nullable<System.Guid> BalAccountTypeID { get; set; }
    
        public virtual Cat_AccountType Cat_AccountType { get; set; }
        public virtual Cat_AccountType Cat_AccountType1 { get; set; }
    }
}
