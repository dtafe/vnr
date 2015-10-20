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
    
    public partial class FIN_PurchaseRequestItem
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> PurchaseRequestID { get; set; }
        public Nullable<System.Guid> CostCenterID { get; set; }
        public Nullable<System.Guid> ProjectID { get; set; }
        public Nullable<System.Guid> ItemID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> UnitPrice { get; set; }
        public Nullable<double> Amount { get; set; }
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
        public Nullable<double> AmountConvert { get; set; }
    
        public virtual Cat_CateCode Cat_CateCode { get; set; }
        public virtual Cat_MasterProject Cat_MasterProject { get; set; }
        public virtual Cat_PurchaseItems Cat_PurchaseItems { get; set; }
        public virtual FIN_PurchaseRequest FIN_PurchaseRequest { get; set; }
    }
}