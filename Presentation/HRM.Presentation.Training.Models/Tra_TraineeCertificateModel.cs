using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{

    public class Tra_TraineeCertificateModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeCertificate_TraineeID)]
        public Nullable<System.Guid> TraineeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeCertificate_TraineeID)]
        public string TraineeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeCertificate_CertificateID)]
        public Nullable<System.Guid> CertificateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeCertificate_CertificateID)]
        public string CertificateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeCertificate_DateCertificate)]
        public Nullable<System.DateTime> DateCertificate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeCertificate_DateExpireCertificate)]
        public Nullable<System.DateTime> DateExpireCertificate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeCertificate_PlaceCertificate)]
        public string PlaceCertificate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeCertificate_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeCertificate_UserApprove)]
        public string UserApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeCertificate_DateApprove)]
        public Nullable<System.DateTime> DateApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeCertificate_IsNonWaring)]
        public Nullable<bool> IsNonWaring { get; set; }
        public Guid? ClassIDTemp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_CourseName)]
        public string CourseName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ClassName)]
        public string ClassName { get; set; }
             [DisplayName(ConstantDisplay.HRM_Tra_Class_Teacher)]
        public string TeacherName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }



        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string TraineeName = "TraineeName";
            public const string CertificateName = "CertificateName";
            public const string DateCertificate = "DateCertificate";
            public const string DateExpireCertificate = "DateExpireCertificate";
            public const string PlaceCertificate = "PlaceCertificate";
            public const string Note = "Note";
            public const string UserApprove = "UserApprove";
            public const string DateApprove = "DateApprove";
            public const string IsNonWaring = "IsNonWaring";
            public const string ClassIDTemp = "ClassIDTemp";
            public const string OrgStructureName = "OrgStructureName";
            public const string CourseName = "CourseName";
            public const string ClassName = "ClassName";
            public const string TeacherName = "TeacherName";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }

    public class Tra_TraineeCertificateSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public Guid? CourseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_JobtitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_General_DateFrom)]
        public Nullable<System.DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_General_DateTo)]
        public Nullable<System.DateTime> DateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Tra_TraineeCertificateExpireSearchModel
    {
        public Nullable<System.DateTime> DateExpireCertificateFrom { get; set; }
        public Nullable<System.DateTime> DateExpireCertificateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
