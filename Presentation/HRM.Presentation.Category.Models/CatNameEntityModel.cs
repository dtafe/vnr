using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatNameEntityModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_Category_NameEntityCode)]
        public string Code { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_NameEntityName)]
        public string NameEntityName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_NameEntityType)]
        public string NameEntityType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_NameEntityEnumType)]
        public string EnumType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_NameEntityDescription)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_NameEntityIcon)]
        public string Icon { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_NameEntityOrder)]
        public Nullable<int> Order { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string NameEntityName = "NameEntityName";
            public const string Code = "Code";
            public const string EnumType = "EnumType";
            public const string NameEntityType = "NameEntityType";
            public const string Description = "Description";
            public const string Icon = "Icon";
            public const string Order = "Order";
        }
        
    }
    public class Cat_EducationLevelSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_EducationLevel_Name)]
        public string NameEntityName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_TypeOfTransferSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_TypeOfTransfer_Name)]
        public string NameEntityName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_GraduatedLevelSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_GraduatedLevel_Name)]
        public string NameEntityName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_LevelSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_NameEntityName)]
        public string NameEntityName { get; set; }
        public string NameEntityType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_BlackListResonSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_NameEntityName)]
        public string NameEntityName { get; set; }
        public string NameEntityType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_TimeEvalutionDataSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_NameEntityName)]
        public string NameEntityName { get; set; }
        public string NameEntityType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_TypeOfStopSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_NameEntityName)]
        public string NameEntityName { get; set; }
        public string NameEntityType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_CutOffDurationTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_CutOffDurationType_Name)]
        public string NameEntityName { get; set; }
        public string NameEntityType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class CatNameEntityMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
        public string Code { get; set; }
        public string NameEntityType { get; set; }
    }

    public class Cat_CutOffDurationTypeMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

    public class CatComputingLevelMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

    public class CatComputingTypeMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

    public class CatLanguageTypeMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
    public class CatLanguageLevelMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
    public class CatLanguageSkillMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
    public class CatEducationLevelMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
    public class CatObjectKPIMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

    public class CatGraduatedLevelMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

    public class CatLockObjectMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
    public class CatJobVancancyResonMultiModel 
    {
        public Guid ID { get; set; } 
        public string NameEntityName { get; set; }
    }

    public class Cat_DisciplineReasonSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_DisciplineReason)]
        public string NameEntityName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_AreaPostJobSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_AreaPostJob)]
        public string NameEntityName { get; set; }
        public string NameEntityType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
