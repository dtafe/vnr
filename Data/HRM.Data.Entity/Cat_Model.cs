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
    
    public partial class Cat_Model
    {
        public Cat_Model()
        {
            this.PUR_ColorModel = new HashSet<PUR_ColorModel>();
            this.Pur_MCAM = new HashSet<Pur_MCAM>();
            this.PUR_PaymentModel = new HashSet<PUR_PaymentModel>();
        }
    
        public System.Guid ID { get; set; }
        public string ModelType { get; set; }
        public string ModelCode { get; set; }
        public string ModelName { get; set; }
        public Nullable<System.Guid> ColorID { get; set; }
        public Nullable<System.DateTime> DateApply { get; set; }
        public Nullable<double> NormalPrice { get; set; }
        public Nullable<double> SpecialPrice { get; set; }
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
        public Nullable<double> WholePrice { get; set; }
        public string Note { get; set; }
    
        public virtual Cat_NameEntity Cat_NameEntity { get; set; }
        public virtual ICollection<PUR_ColorModel> PUR_ColorModel { get; set; }
        public virtual ICollection<Pur_MCAM> Pur_MCAM { get; set; }
        public virtual ICollection<PUR_PaymentModel> PUR_PaymentModel { get; set; }
    }
}
