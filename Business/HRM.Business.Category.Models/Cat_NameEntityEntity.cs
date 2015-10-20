using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Category.Models
{
    public class Cat_NameEntityEntity : HRMBaseModel
    {

        public string Code { get; set; }
        public string NameEntityName { get; set; }
        public string NameEntityType { get; set; }
        public string EnumType { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public Nullable<int> Order { get; set; }
    }
    public class Cat_NameEntityMultiEntity
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string NameEntityName { get; set; }
        public string NameEntityType { get; set; }
    }

    public class Cat_CutOffDurationTypeMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

    public class Cat_ComputingLevelMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

    public class Cat_ComputingTypeMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

    public class Cat_LanguageTypeMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
    public class Cat_LanguageLevelMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
    public class Cat_LanguageSkillMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

    public class Cat_EducationLevelMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
    public class CatObjectKPIMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

    public class Cat_GraduatedLevelMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
    public class Cat_LockObjectMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

    public class Cat_JobVancancyResonMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
     }

    public class Tra_CostCenterMultiEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }

}

