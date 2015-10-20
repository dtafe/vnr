using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatProductTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ProductType_TypeName)]
        public string TypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductType_Description)]
        public string Description { get; set; }
        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string TypeName = "TypeName";
            public const string Description = "Description";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class CatProductTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ProductType_TypeName)]
        public string TypeName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class CatProductTypeMultiModel
    {
        public Guid ID { get; set; }
        public string TypeName { get; set; }
    }
}
