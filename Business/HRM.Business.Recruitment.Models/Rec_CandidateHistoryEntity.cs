using System;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_CandidateHistoryEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public System.Guid CandidateID { get; set; }
        public string CandidateName { get; set; }
        public string WordHistoryName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Position { get; set; }
        public string PositionLast { get; set; }
        public string SalaryLast { get; set; }
        public string Tasks { get; set; }
        public string JobDescription { get; set; }
        public string Phone { get; set; }
        public string BussinessType { get; set; }
        public string Salary { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateFinish { get; set; }
        public string Comment { get; set; }
        public string Code { get; set; }
        public string SupPosition { get; set; }
        public string SupName { get; set; }
        public string SupMobile { get; set; }
        public string SupEmail { get; set; }
        public string SupCompany { get; set; }
        public string SupComment { get; set; }
        public string SupRelation { get; set; }
        public string ResignReason { get; set; }
        public string Status { get; set; }
        public Nullable<bool> PassFilterResume { get; set; }
        public Nullable<int> LevelInterview { get; set; }
        public Nullable<System.DateTime> DateApply { get; set; }
    }
}
