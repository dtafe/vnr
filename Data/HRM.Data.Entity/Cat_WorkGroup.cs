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
    
    public partial class Cat_WorkGroup
    {
        public Cat_WorkGroup()
        {
            this.Hre_Profile = new HashSet<Hre_Profile>();
        }
    
        public System.Guid ID { get; set; }
        public string WorkGroupName { get; set; }
        public Nullable<System.Guid> MonShiftID { get; set; }
        public Nullable<System.Guid> TueShiftID { get; set; }
        public Nullable<System.Guid> WedShiftID { get; set; }
        public Nullable<System.Guid> ThuShiftID { get; set; }
        public Nullable<System.Guid> FriShiftID { get; set; }
        public Nullable<System.Guid> SatShiftID { get; set; }
        public Nullable<System.Guid> SunShiftID { get; set; }
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
    
        public virtual Cat_Shift Cat_Shift { get; set; }
        public virtual Cat_Shift Cat_Shift1 { get; set; }
        public virtual Cat_Shift Cat_Shift2 { get; set; }
        public virtual Cat_Shift Cat_Shift3 { get; set; }
        public virtual Cat_Shift Cat_Shift4 { get; set; }
        public virtual Cat_Shift Cat_Shift5 { get; set; }
        public virtual Cat_Shift Cat_Shift6 { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile { get; set; }
    }
}
