using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_ReportTraineeResultModel : BaseViewModel
    {
        public string WorkPlaceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ProfileID)]
        public System.Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ProfileID)]
        public string ProfileName { get; set; }
        public Nullable<System.Guid> RequirementTrainID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_RequirementTrainNames)]
        public string RequirementTrainName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ClassID)]
        public System.Guid ClassID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ClassID)]
        public string ClassName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_Status)]
        public string Status { get; set; }
        public string StatusView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_Result)]
        public Nullable<double> Result { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_CertificateID)]
        public Nullable<System.Guid> CertificateID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_CertificateID)]
        public string CertificateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_TeacherComment)]
        public string TeacherComment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ProfileComment)]
        public string ProfileComment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_OrgStructureID)]
        public Nullable<System.Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_OrgStructureID)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_PracticeMath)]
        public string PracticeMath { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_TheoreticalMath)]
        public string TheoreticalMath { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_AttendanceMath)]
        public string AttendanceMath { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_CostCenter)]
        public Nullable<double> CostCenter { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_CostCenterCompanyPay)]
        public Nullable<double> CostCenterCompanyPay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_DateExpirationCertificate)]
        public Nullable<System.DateTime> DateExpirationCertificate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_DateExpirationCertificate)]
        public Nullable<System.DateTime> DateExpireCertificate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_CertificateDate)]
        public Nullable<System.DateTime> CertificateDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_CertificateDate)]
        public Nullable<System.DateTime> DateCertificate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_CommitmentTimeWork)]
        public Nullable<System.DateTime> CommitmentTimeWork { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_CommitmentOther)]
        public string CommitmentOther { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_TrainLevelID)]
        public Nullable<System.Guid> TrainLevelID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_TrainLevelID)]
        public string TrainLevelName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_DatePrintCard)]
        public Nullable<System.DateTime> DatePrintCard { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_UserPrintCard)]
        public string UserPrintCard { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_DateRequest)]
        public Nullable<System.DateTime> DateRequest { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_DateEndRequest)]
        public Nullable<System.DateTime> DateEndRequest { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_IsObsolete)]
        public Nullable<bool> IsObsolete { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_RankingName)]
        public string RankingName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_DateStart)]
        public Nullable<System.DateTime> DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ReasonID)]
        public Nullable<System.Guid> ReasonID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ReasonID)]
        public string ReasonName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_Reason)]
        public string JobTitleName { get; set; }
        public string Reason { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_IsCreateFromNewCertificate)]
        public Nullable<bool> IsCreateFromNewCertificate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_RankingID)]
        public Nullable<System.Guid> RankingID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ResultXML)]
        public string ResultXML { get; set; }
        public Guid? TraineeCertificateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public string CourseName { get; set; }

        public Guid CourseID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
        public string ClassCode { get; set; }
        public string CourseCode { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public Nullable<System.Guid> ClassOldID { get; set; }
        public string TrainingPlace { get; set; }
        public string TrainerOtherList { get; set; }
        public string TeacherName { get; set; }
        public Nullable<double> Score1 { get; set; }
        public Nullable<double> Score2 { get; set; }
        public Nullable<double> Score3 { get; set; }
        public partial class FieldNames
        {
            public const string ClassCode = "ClassCode";
            public const string CourseCode = "CourseCode";
            public const string StartDate = "StartDate";
            public const string EndDate = "EndDate";
            public const string CourseName = "CourseName";
            public const string ClassID = "ClassID";
            public const string JobTitleName = "JobTitleName";
            public const string CodeEmp = "CodeEmp";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string ResultXML = "ResultXML";
            public const string RankingName = "RankingName";
            public const string IsCreateFromNewCertificate = "IsCreateFromNewCertificate";
            public const string Reason = "Reason";
            public const string ReasonName = "ReasonName";
            public const string DateStart = "DateStart";
            public const string IsObsolete = "IsObsolete";
            public const string DateEndRequest = "DateEndRequest";
            public const string DateRequest = "DateRequest";
            public const string UserPrintCard = "UserPrintCard";
            public const string DatePrintCard = "DatePrintCard";
            public const string TrainLevelName = "TrainLevelName";
            public const string CommitmentOther = "CommitmentOther";
            public const string CommitmentTimeWork = "CommitmentTimeWork";
            public const string CertificateDate = "CertificateDate";
            public const string DateExpirationCertificate = "DateExpirationCertificate";
            public const string CostCenterCompanyPay = "CostCenterCompanyPay";
            public const string CostCenter = "CostCenter";
            public const string AttendanceMath = "AttendanceMath";
            public const string TheoreticalMath = "TheoreticalMath";
            public const string PracticeMath = "PracticeMath";
            public const string OrgStructureName = "OrgStructureName";
            public const string ProfileComment = "ProfileComment";
            public const string TeacherComment = "TeacherComment";
            public const string CertificateName = "CertificateName";
            public const string Result = "Result";
            public const string ClassName = "ClassName";
            public const string ProfileName = "ProfileName";
            public const string DateCertificate = "DateCertificate";
            public const string DateExpireCertificate = "DateExpireCertificate";
            public const string TraineeCertificateID = "TraineeCertificateID";
            public const string RequirementTrainName = "RequirementTrainName";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string Score1 = "Score1";
            public const string Score2 = "Score2";
            public const string Score3 = "Score3";
        }
    }

    public class Tra_ReportTraineeResultSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ClassID)]
        public string ClassID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public string CourseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_PlanName)]

        public Guid? PlanID { get; set; }

        // ngày học
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_OrgName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_ReportTraineeResult_ScoreTypeID)]
        public string ScoreTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_RequirementTrainNames)]
        public string RequirementTrainIDs { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }


}
