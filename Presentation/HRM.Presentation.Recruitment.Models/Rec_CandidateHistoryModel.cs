using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HRM.Presentation.Recruitment.Models
{
    public class Rec_CandidateHistoryModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_JoiningDate)]
        public Nullable<System.DateTime> JoiningDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_CandidateID)]
        public System.Guid CandidateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_CandidateName)]
        public string CandidateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_WordHistoryName)]
        public string WordHistoryName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_CompanyName)]
        public string CompanyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_CompanyAddress)]
        public string CompanyAddress { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_Position)]
        public string Position { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_PositionLast)]
        public string PositionLast { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_SalaryLast)]
        public string SalaryLast { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_Tasks)]
        public string Tasks { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_JobDescription)]
        public string JobDescription { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_Phone)]
        public string Phone { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_BussinessType)]
        public string BussinessType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_Salary)]
        public string Salary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_DateStart)]
        public Nullable<System.DateTime> DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_DateFinish)]
        public Nullable<System.DateTime> DateFinish { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_SupPosition)]
        public string SupPosition { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_SupName)]
        public string SupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_SupMobile)]
        public string SupMobile { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_SupEmail)]
        public string SupEmail { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_SupCompany)]
        public string SupCompany { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_SupComment)]
        public string SupComment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_SupRelation)]
        public string SupRelation { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_ResignReason)]
        public string ResignReason { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_PassFilterResume)]
        public Nullable<bool> PassFilterResume { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateHistory_LevelInterview)]
        public Nullable<int> LevelInterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_DateApply)]
        public Nullable<System.DateTime> DateApply { get; set; }
        public partial class FieldNames
        {
            public const string DateApply = "DateApply";
            public const string CandidateID = "CandidateID";
            public const string CandidateName = "CandidateName";
            public const string WordHistoryName = "WordHistoryName";
            public const string CompanyName = "CompanyName";
            public const string CompanyAddress = "CompanyAddress";
            public const string Position = "Position";
            public const string PositionLast = "PositionLast";
            public const string SalaryLast = "SalaryLast";
            public const string Tasks = "Tasks";
            public const string JobDescription = "JobDescription";
            public const string Phone = "Phone";
            public const string BussinessType = "BussinessType";
            public const string Salary = "Salary";
            public const string DateStart = "DateStart";
            public const string DateFinish = "DateFinish";
            public const string Comment = "Comment";
            public const string Code = "Code";
            public const string SupPosition = "SupPosition";
            public const string SupName = "SupName";
            public const string SupMobile = "SupMobile";
            public const string SupEmail = "SupEmail";
            public const string SupCompany = "SupCompany";
            public const string SupComment = "SupComment";
            public const string SupRelation = "SupRelation";
            public const string ResignReason = "ResignReason";
            public const string Status = "Status";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }

    public class Rec_CandidateHistorySearchModel
    {
        public string CandidateName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Rec_CandidateHistoryMultiModel
    {
        public Guid ID { get; set; }
        public string CandidateName { get; set; } 
        public int TotalRow { get; set; }
    }

}
