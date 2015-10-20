using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_UnitModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_Category_Unit_UnitName)]
        public string UnitName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Unit_Code)]
        public string Code { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Unit_Note)]

        public string Note { get; set; }
        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string UnitName = "UnitName";
            public const string Code = "Code";
            public const string Note = "Note";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Cat_UnitSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Unit_Code)]
        public string Code { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Unit_UnitName)]
        public string UnitName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_UnitMultiModel
    {
        public Guid ID { get; set; }
        public string UnitName { get; set; }
        public int TotalRow { get; set; }
    }

}
