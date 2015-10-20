using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_TrainLevelModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_TrainLevel_TrainLevelName)]
        public string TrainLevelName { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Recruitment_Notes)]
        public double Expiredate { get; set; }

        public Nullable<System.Guid> CertificateID { get; set; }
             [DisplayName(ConstantDisplay.HRM_Category_TrainLevel_Description)]
        public string Description { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string TrainLevelName = "TrainLevelName";
            public const string Expiredate = "Expiredate";
            public const string CertificateID = "CertificateID";
            public const string Description = "Description";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }

    public class Cat_TrainLevelSearchModel
    {
        public string TrainLevelName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_TrainLevelMultiModel
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string TrainLevelName { get; set; }
    }

}
