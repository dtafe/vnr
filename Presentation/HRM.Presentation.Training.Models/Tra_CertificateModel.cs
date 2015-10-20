using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_CertificateModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_CertificateName)]
        public string CertificateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_Place)]
        public string Place { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_CourseID)]
        public Nullable<System.Guid> CourseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_CourseID)]
        public string CourseName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_TopicName)]
        public Nullable<System.Guid> TopicID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_TopicName)]
        public string TopicName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_Description)]
        public string Description { get; set; }

        public string UnitTraining { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CertificateName = "CertificateName";
            public const string Code = "Code";
            public const string Place = "Place";
            public const string CourseName = "CourseName";
            public const string TopicName = "TopicName";
            public const string Description = "Description";
            public const string UnitTraining = "UnitTraining";
        }
    }

    public class Tra_CertificateSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_CertificateName)]
        public string CertificateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_CourseID)]
        public Nullable<System.Guid> CourseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_TopicName)]
        public Nullable<System.Guid> TopicID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Tra_CertificateMultiModel
    {
        public Guid ID { get; set; }
        public string CertificateName { get; set; }
        public int TotalRow { get; set; }
    }

}
