using System;


namespace HRM.Business.Training.Models
{
    public class Tra_TraineeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public System.Guid ProfileID { get; set; }
        public string WorkPlaceName { get; set; }
        public string ProfileName { get; set; }
        public string JobTitleName { get; set; }
        public string CodeEmp { get; set; }
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
        public Nullable<System.DateTime> DateCertificate { get; set; }
        public Nullable<System.DateTime> DateExpireCertificate { get; set; }
        public Nullable<System.DateTime> DateExpirationCertificate { get; set; }
        public Nullable<System.DateTime> CertificateDate { get; set; }
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
         public Nullable<System.Guid> TraineeRegisterID { get; set;} 
        public string ReasonName { get; set; }
        public string Reason { get; set; }
        public Nullable<bool> IsCreateFromNewCertificate { get; set; }
        public Nullable<System.Guid> RankingID { get; set; }

        public Guid? TraineeCertificateID { get; set; }
        
        public string ResultXML { get; set; }
        public string CourseName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Guid CourseID { get; set; }
        public string Code { get; set; }
        public string CourseCode { get; set; }
        public string ClassCode { get; set; }
        public Nullable<System.Guid> ClassOldID { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public string ClassNameOld { get; set; }
        public string TrainingPlace { get; set; }
        public string TrainerOtherList { get; set; }
        public Guid? RequirementTrainID { get; set; }
        public string RequirementTrainName { get; set; }
    }
    public class Tra_TraineeChangeClassEntity
    {
        public string CodeEmp{ get; set; }
        public string ClassOldCode { get; set; }
        public string ClassNewCode { get; set; }
    }
}

