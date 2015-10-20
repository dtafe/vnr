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
    
    public partial class Sal_ItemForShop
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> ShopID { get; set; }
        public Nullable<System.Guid> ItemID { get; set; }
        public Nullable<double> Target { get; set; }
        public Nullable<double> Actual { get; set; }
        public Nullable<System.DateTime> Month { get; set; }
        public string Note { get; set; }
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
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public Nullable<bool> IsPass { get; set; }
    
        public virtual Cat_Item Cat_Item { get; set; }
        public virtual Cat_Shop Cat_Shop { get; set; }
    }
}
