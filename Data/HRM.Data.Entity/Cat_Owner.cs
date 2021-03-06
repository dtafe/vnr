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
    
    public partial class Cat_Owner
    {
        public Cat_Owner()
        {
            this.Cat_Owner1 = new HashSet<Cat_Owner>();
            this.Cat_PurchaseItems = new HashSet<Cat_PurchaseItems>();
            this.FIN_PurchaseRequest = new HashSet<FIN_PurchaseRequest>();
            this.FIN_PurchaseRequest1 = new HashSet<FIN_PurchaseRequest>();
            this.FIN_PurchaseRequest2 = new HashSet<FIN_PurchaseRequest>();
        }
    
        public System.Guid ID { get; set; }
        public string OwnerName { get; set; }
        public string OwnerType { get; set; }
        public Nullable<System.Guid> OwnerParentID { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public string Code { get; set; }
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
    
        public virtual ICollection<Cat_Owner> Cat_Owner1 { get; set; }
        public virtual Cat_Owner Cat_Owner2 { get; set; }
        public virtual ICollection<Cat_PurchaseItems> Cat_PurchaseItems { get; set; }
        public virtual ICollection<FIN_PurchaseRequest> FIN_PurchaseRequest { get; set; }
        public virtual ICollection<FIN_PurchaseRequest> FIN_PurchaseRequest1 { get; set; }
        public virtual ICollection<FIN_PurchaseRequest> FIN_PurchaseRequest2 { get; set; }
    }
}
