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
    
    public partial class Hre_BusinessTrip
    {
        public Hre_BusinessTrip()
        {
            this.Hre_BusinessTripCosting = new HashSet<Hre_BusinessTripCosting>();
            this.Hre_BusinessTripProfile = new HashSet<Hre_BusinessTripProfile>();
        }
    
        public System.Guid ID { get; set; }
        public string Code { get; set; }
        public string BusinessTripName { get; set; }
        public Nullable<System.DateTime> DateStartIntend { get; set; }
        public Nullable<System.DateTime> DateEndIntend { get; set; }
        public Nullable<System.DateTime> DateStartReal { get; set; }
        public Nullable<System.DateTime> DateEndReal { get; set; }
        public Nullable<double> SummaryCost { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> UserApproveID { get; set; }
        public Nullable<System.Guid> UserConfirmID { get; set; }
        public string ConfirmComment { get; set; }
        public string ApproveComment { get; set; }
        public string RefuseComment { get; set; }
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
        public string Type { get; set; }
        public Nullable<System.Guid> BusinessTripPurposeID { get; set; }
        public string Place { get; set; }
        public Nullable<int> TotalDay { get; set; }
        public Nullable<int> TotalPeople { get; set; }
        public Nullable<bool> IsFinish { get; set; }
        public Nullable<System.Guid> ProvinceID { get; set; }
        public Nullable<System.Guid> DistrictID { get; set; }
        public Nullable<int> TotalDayReal { get; set; }
        public Nullable<double> AdvancePayment { get; set; }
        public Nullable<System.Guid> CurrencyAdvancePaymentID { get; set; }
        public Nullable<System.Guid> CountryID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string NoDecide { get; set; }
    
        public virtual Cat_BusinessTripPurpose Cat_BusinessTripPurpose { get; set; }
        public virtual Cat_Country Cat_Country { get; set; }
        public virtual Cat_Currency Cat_Currency { get; set; }
        public virtual Cat_Currency Cat_Currency1 { get; set; }
        public virtual Cat_District Cat_District { get; set; }
        public virtual Cat_Province Cat_Province { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual ICollection<Hre_BusinessTripCosting> Hre_BusinessTripCosting { get; set; }
        public virtual ICollection<Hre_BusinessTripProfile> Hre_BusinessTripProfile { get; set; }
    }
}
