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
    
    public partial class Cat_Shop
    {
        public Cat_Shop()
        {
            this.Cat_KPIBonusItem = new HashSet<Cat_KPIBonusItem>();
            this.Hre_Profile2 = new HashSet<Hre_Profile>();
            this.Hre_WorkHistory = new HashSet<Hre_WorkHistory>();
            this.Sal_LineItemForShop = new HashSet<Sal_LineItemForShop>();
            this.Sal_RevenueForShop = new HashSet<Sal_RevenueForShop>();
            this.Sal_ItemForShop = new HashSet<Sal_ItemForShop>();
            this.Sal_RevenueRecord = new HashSet<Sal_RevenueRecord>();
        }
    
        public System.Guid ID { get; set; }
        public string ShopName { get; set; }
        public string Code { get; set; }
        public string Rank { get; set; }
        public Nullable<double> SaleProduct { get; set; }
        public Nullable<double> NoCustomer { get; set; }
        public Nullable<double> MoneyCollectService { get; set; }
        public Nullable<double> NoCustomerService { get; set; }
        public Nullable<double> MainLineProduct { get; set; }
        public Nullable<double> PromoteProduct { get; set; }
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
        public Nullable<System.Guid> ShopLeaderID { get; set; }
        public Nullable<System.Guid> ShiftLeaderID { get; set; }
        public string Formular { get; set; }
        public string Formular1 { get; set; }
        public Nullable<int> NoShopLeader { get; set; }
        public Nullable<int> NoShiftLeader { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public Nullable<System.Guid> ShopGroupID { get; set; }
    
        public virtual ICollection<Cat_KPIBonusItem> Cat_KPIBonusItem { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure { get; set; }
        public virtual Cat_ShopGroup Cat_ShopGroup { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual Hre_Profile Hre_Profile1 { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile2 { get; set; }
        public virtual ICollection<Hre_WorkHistory> Hre_WorkHistory { get; set; }
        public virtual ICollection<Sal_LineItemForShop> Sal_LineItemForShop { get; set; }
        public virtual ICollection<Sal_RevenueForShop> Sal_RevenueForShop { get; set; }
        public virtual ICollection<Sal_ItemForShop> Sal_ItemForShop { get; set; }
        public virtual ICollection<Sal_RevenueRecord> Sal_RevenueRecord { get; set; }
    }
}
