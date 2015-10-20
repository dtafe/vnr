using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatOrgStructureTypeMultiModel
    {
        public Guid ID { get; set; }
        public string OrgStructureTypeName { get; set; }
        public string OrgStructureTypeCode { get; set; }
    }
    public class CatOrgStructureTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_OrgStructureTypeName)]
        public string OrgStructureTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_Code)]
        public string OrgStructureTypeCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_Icon)]
        public string Icon { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_Description)]
        public string Description { get; set; }



        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string OrgStructureTypeName = "OrgStructureTypeName";
            public const string OrgStructureTypeCode = "OrgStructureTypeCode";
            public const string Icon = "Icon";
            public const string Description = "Description";
        }

    }

    public class CatOrgStructureTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_OrgStructureTypeName)]
        public string OrgStructureTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_Code)]
        public string OrgStructureTypeCode { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }

    }
}
