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
    
    public partial class Cat_SourceAds
    {
        public Cat_SourceAds()
        {
            this.Rec_Candidate = new HashSet<Rec_Candidate>();
            this.Rec_CandidateSourceAds = new HashSet<Rec_CandidateSourceAds>();
            this.Rec_JobVacancy = new HashSet<Rec_JobVacancy>();
            this.Rec_RecruitmentHistory = new HashSet<Rec_RecruitmentHistory>();
        }
    
        public System.Guid ID { get; set; }
        public string SourceAdsName { get; set; }
        public string Description { get; set; }
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
    
        public virtual ICollection<Rec_Candidate> Rec_Candidate { get; set; }
        public virtual ICollection<Rec_CandidateSourceAds> Rec_CandidateSourceAds { get; set; }
        public virtual ICollection<Rec_JobVacancy> Rec_JobVacancy { get; set; }
        public virtual ICollection<Rec_RecruitmentHistory> Rec_RecruitmentHistory { get; set; }
    }
}
