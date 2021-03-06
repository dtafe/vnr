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
    
    public partial class Tra_Class
    {
        public Tra_Class()
        {
            this.Cat_NonConformingRecord = new HashSet<Cat_NonConformingRecord>();
            this.Tra_RequirementTrainDetail = new HashSet<Tra_RequirementTrainDetail>();
            this.Tra_Trainee = new HashSet<Tra_Trainee>();
        }
    
        public System.Guid ID { get; set; }
        public string ClassName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Teacher { get; set; }
        public Nullable<System.Guid> PlanID { get; set; }
        public System.Guid CourseID { get; set; }
        public string Code { get; set; }
        public string Topic { get; set; }
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
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public bool IsTrainingOut { get; set; }
        public string ContentTraining { get; set; }
        public string TrainingPlace { get; set; }
        public string Schedule { get; set; }
        public Nullable<System.DateTime> DateReminder { get; set; }
        public string EmailReminder { get; set; }
        public Nullable<System.DateTime> DatePlanStart { get; set; }
        public Nullable<System.DateTime> DatePlanEnd { get; set; }
        public string Status { get; set; }
        public Nullable<double> Duration { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<bool> IsDuringWork { get; set; }
        public Nullable<int> MassNumProfileRegisted { get; set; }
        public Nullable<int> MassNumProfileAttendanced { get; set; }
        public string MassResult { get; set; }
        public string MassAttendedSection { get; set; }
        public Nullable<System.DateTime> DateReminder2 { get; set; }
        public string EmailReminder2 { get; set; }
        public string TopicName { get; set; }
        public string AttachedFiles { get; set; }
        public string AmountUnit { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public string ClassType { get; set; }
        public string Formula { get; set; }
        public Nullable<System.Guid> RankingGroupID { get; set; }
        public Nullable<System.DateTime> ScheduleTimeStart { get; set; }
        public Nullable<System.DateTime> ScheduleTimeEnd { get; set; }
        public string TypeHandleFomular { get; set; }
        public Nullable<double> StandardScoreToPass { get; set; }
        public Nullable<bool> IsAutoCreated { get; set; }
        public Nullable<System.Guid> CertificateID { get; set; }
        public Nullable<double> ExpectedCost { get; set; }
        public string PlaceTrainner { get; set; }
        public Nullable<System.Guid> CurrencyID2 { get; set; }
    
        public virtual Cat_Currency Cat_Currency { get; set; }
        public virtual Cat_Currency Cat_Currency1 { get; set; }
        public virtual ICollection<Cat_NonConformingRecord> Cat_NonConformingRecord { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure { get; set; }
        public virtual Tra_Certificate Tra_Certificate { get; set; }
        public virtual Tra_Course Tra_Course { get; set; }
        public virtual Tra_Plan Tra_Plan { get; set; }
        public virtual Tra_RankingGroup Tra_RankingGroup { get; set; }
        public virtual ICollection<Tra_RequirementTrainDetail> Tra_RequirementTrainDetail { get; set; }
        public virtual ICollection<Tra_Trainee> Tra_Trainee { get; set; }
    }
}
