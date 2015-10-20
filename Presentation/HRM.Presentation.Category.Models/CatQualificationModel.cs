using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatQualificationModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Qualification_Code)]
        public string Code { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Qualification_Name)]
        public string QualificationName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Qualification_Notes)]
        public string Notes { get; set; }
         public partial class FieldNames
         {
             public const string ID = "ID";
             public const string Code = "Code";
             public const string QualificationName = "QualificationName";
             public const string Notes = "Notes";
             public const string UserCreate = "UserCreate";
         }
    }
    public class Cat_QualificationSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Qualification_Name)]
        public string QualificationName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class CatQualificationMultiModel
    {
        public Guid ID { get; set; }
        public string QualificationName { get; set; }
    }
    public class CatQualificationLevelMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
}
