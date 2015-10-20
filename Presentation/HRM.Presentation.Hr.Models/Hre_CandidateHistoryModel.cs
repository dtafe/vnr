using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_CandidateHistoryModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Nullable<System.Guid> ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_WordHistoryName)]
        public string WordHistoryName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_CompanyName)]
        public string CompanyName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_CompanyAddress)]
        public string CompanyAddress { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_Position)]
        public string Position { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_PositionLast)]
        public string PositionLast { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_SalaryLast)]
        public Nullable<double> SalaryLast { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_Tasks)]
        public string Tasks { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_JobDescription)]
        public string JobDescription { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_Phone)]
        public string Phone { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_BussinessType)]
        public string BussinessType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_Salary)]
        public string Salary { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_DateStart)]
        public Nullable<System.DateTime> DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_DateFinish)]
        public Nullable<System.DateTime> DateFinish { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_Comment)]
        public string Comment { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_SupPosition)]
        public string SupPosition { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_SupName)]
        public string SupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_SupMobile)]
        public string SupMobile { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_SupEmail)]
        public string SupEmail { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_SupCompany)]
        public string SupCompany { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_SupComment)]
        public string SupComment { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_SupRelation)]
        public string SupRelation { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ResignReason)]
        public string ResignReason { get; set; }

        private Guid _id = Guid.Empty;
        public Guid CandidateHistory_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
        public Nullable<System.Guid> CandidateID { get; set; }
        public string CandidateName { get; set; }
        public Nullable<System.Guid> CountryID { get; set; }
        public string CountryName { get; set; }
        public Nullable<System.Guid> ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_LastPositionID)]
        public Nullable<System.Guid> LastPositionID { get; set; }
        public string LastPositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_FirstPositionID)]
        public Nullable<System.Guid> FirstPositionID { get; set; }
        public string FirstPositionName { get; set; }
        public string PositionFirst { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SalaryFirst)]
        public Nullable<double> SalaryFirst { get; set; }
        public partial class FieldNames
        {
            public const string SalaryFirst = "SalaryFirst";
            public const string PositionFirst = "PositionFirst";
            public const string CountryName = "CountryName";
            public const string ProvinceName = "ProvinceName";
            public const string LastPositionName = "LastPositionName";
            public const string FirstPositionName = "FirstPositionName";
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
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
            public const string CandidateHistory_ID = "CandidateHistory_ID";
            public const string CandidateName = "CandidateName";
        }
    
    }

    public class Hre_ProfileCandidateHistorySearchModel
    {
        public string OrgStructureID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public Guid? JobTitleID { get; set; }
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyIDs)]
        public Guid? JobVacancyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RecruitmentCampaignName)]
        public Guid? RecruitmentCampaignID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public DateTime? DateApplyFrom { get; set; }
        public DateTime? DateApplyTo { get; set; }
        public DateTime? WorkDateFrom { get; set; }
        public DateTime? WorkDateTo { get; set; }
        public DateTime? DateExamFrom { get; set; }
        public DateTime? DateExamTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        public DateTime? DateOfBirthFrom { get; set; }
        public DateTime? DateOfBirthTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string Mobile { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_CandidateHistory_CompanyName)]
        public string CompanyName { get; set; }
        public Nullable<double> SalaryLastFrom { get; set; }
        public Nullable<double> SalaryLastTo { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
