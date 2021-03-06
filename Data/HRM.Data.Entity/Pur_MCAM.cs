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
    
    public partial class Pur_MCAM
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.DateTime> DateRequest { get; set; }
        public Nullable<System.Guid> ModelID { get; set; }
        public Nullable<System.Guid> PaymentMethodID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string ReceivePlace { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string VehicleIdNo { get; set; }
        public string EngineNo { get; set; }
        public Nullable<System.DateTime> ReceiveDate { get; set; }
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
        public Nullable<System.Guid> ColorID { get; set; }
        public Nullable<System.Guid> ReceivePlaceID { get; set; }
        public Nullable<System.DateTime> LiquidationDate { get; set; }
        public string Note { get; set; }
        public string NotMeetConditionReason { get; set; }
        public Nullable<double> InterestRate { get; set; }
        public Nullable<double> AmountPayment { get; set; }
        public Nullable<double> AmountDeposit { get; set; }
        public Nullable<double> AmountRemain { get; set; }
        public string StatusCheck { get; set; }
        public Nullable<System.Guid> ColorModelID { get; set; }
    
        public virtual Cat_Model Cat_Model { get; set; }
        public virtual Cat_NameEntity Cat_NameEntity { get; set; }
        public virtual Cat_PaymentMethod Cat_PaymentMethod { get; set; }
        public virtual Cat_ReceivePlace Cat_ReceivePlace { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual PUR_ColorModel PUR_ColorModel { get; set; }
    }
}
