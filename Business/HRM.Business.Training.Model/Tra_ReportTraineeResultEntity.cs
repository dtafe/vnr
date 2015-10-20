using System;


namespace HRM.Business.Training.Models
{
    public class Tra_ReportTraineeResultEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string WorkPlaceName { get; set; }
        public string CodeEmp { get; set; }
        public System.Guid ProfileID { get; set; }
        public string ProfileName { get; set; }
        public System.Guid ClassID { get; set; }
        public string ClassName { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public Nullable<double> Result { get; set; }
        public Nullable<System.Guid> CertificateID { get; set; }
        public string CertificateName { get; set; }
        public string TeacherComment { get; set; }
        public string ProfileComment { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public string PracticeMath { get; set; }
        public string TheoreticalMath { get; set; }
        public string AttendanceMath { get; set; }
        public Nullable<double> CostCenter { get; set; }
        public Nullable<double> CostCenterCompanyPay { get; set; }
        public Nullable<System.DateTime> DateExpirationCertificate { get; set; }
        public Nullable<System.DateTime> DateExpireCertificate { get; set; }
        public Nullable<System.DateTime> CertificateDate { get; set; }

        public Nullable<System.DateTime> DateCertificate { get; set; }
        public Nullable<System.DateTime> CommitmentTimeWork { get; set; }
        public string CommitmentOther { get; set; }
        public Nullable<System.Guid> TrainLevelID { get; set; }
        public string TrainLevelName { get; set; }
        public Nullable<System.DateTime> DatePrintCard { get; set; }
        public string UserPrintCard { get; set; }
        public Nullable<System.DateTime> DateRequest { get; set; }
        public Nullable<System.DateTime> DateEndRequest { get; set; }
        public Nullable<bool> IsObsolete { get; set; }
        public string RankingName { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.Guid> ReasonID { get; set; }
        public string ReasonName { get; set; }
        public string JobTitleName { get; set; }
        public string Reason { get; set; }
        public Nullable<bool> IsCreateFromNewCertificate { get; set; }
        public Nullable<System.Guid> RankingID { get; set; }
        public string ResultXML { get; set; }
        public Guid? TraineeCertificateID { get; set; }
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
        public Guid? RequirementTrainID { get; set; }
        public string RequirementTrainName { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";

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
    public class Tra_ReportTraineeResultSearchEntity
    {
        public string ClassID { get; set; }
        public string CodeEmp { get; set; }
        public string CourseID { get; set; }

        public Guid? PlanID { get; set; }

        // ngày học
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Guid ExportId { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string OrgStructureID { get; set; }
        public string ProfileName { get; set; }
        public string Status { get; set; }
        public string ScoreTypeID { get; set; }
        public string RequirementTrainIDs { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}

