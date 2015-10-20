using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_NameEntityModel : BaseViewModel
    {
        public Guid NameEntityID
        {
            get;
            set;
        }
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_NameEntityName)]
        public string NameEntityName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_NameEntityType)]
        public string NameEntityType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_EnumType)]
        public string EnumType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_Icon)]
        public string Icon { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_Order)]
        public Nullable<int> Order { get; set; }
        public string Eva_KPIIds { get; set; }
        public Guid Eva_KPIID { get; set; }
        public bool IsAddKPI { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LockObject_EnumType)]
        public string ObjectName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_VehicleName)]
        public string VehicleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_CostSourceName)]
        public string CostSourceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Salary_CountAnalyzeHoldSalary_Name)]
        public string CountAnalyzeHoldSalary { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string NameEntityName = "NameEntityName";
            public const string NameEntityType = "NameEntityType";
            public const string EnumType = "EnumType";
            public const string Description = "Description";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }

    public class Cat_NameEntitySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_NameEntityName)]
        public string NameEntityName { get; set; }
        public string NameEntityType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_NameEntityByKPISearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_NameEntity_NameEntityName)]
        public string NameEntityName { get; set; }
        public string NameEntityType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_NameEntityMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
        public int TotalRow { get; set; }
    }

}
