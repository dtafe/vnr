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
    
    public partial class Cat_ExchangeRate
    {
        public System.Guid ID { get; set; }
        public System.Guid CurrencyBaseID { get; set; }
        public System.Guid CurrencyDestID { get; set; }
        public double SellingRate { get; set; }
        public Nullable<double> BuyingRate { get; set; }
        public System.DateTime MonthOfEffect { get; set; }
        public string Comment { get; set; }
        public string UserCreate { get; set; }
        public string ServerCreate { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public string UserUpdate { get; set; }
        public string ServerUpdate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
        public string Type { get; set; }
    
        public virtual Cat_Currency Cat_Currency { get; set; }
        public virtual Cat_Currency Cat_Currency1 { get; set; }
    }
}