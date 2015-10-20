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
    
    public partial class Tra_RequirementTrainingApprove
    {
        public System.Guid ID { get; set; }
        public string RequirementTrainApproveName { get; set; }
        public Nullable<System.Guid> UserInfoID { get; set; }
        public Nullable<System.Guid> RequirementTrainID { get; set; }
        public string Remark { get; set; }
        public string Description { get; set; }
        public Nullable<int> ApproveLevel { get; set; }
        public Nullable<System.DateTime> DateApprove { get; set; }
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
        public Nullable<bool> IsTrainer { get; set; }
    
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Tra_RequirementTrain Tra_RequirementTrain { get; set; }
    }
}
