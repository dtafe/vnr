using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_ModelModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Cat_Model_ModelType)]
        public string ModelType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_ModelCode)]
        public string ModelCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_ModelName)]
        public string ModelName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_ColorID)]
        public Guid? ColorID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_ColorName)]
        public string ColorName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_DateApply)]
        public DateTime DateApply { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_NormalPrice)]
        public double? NormalPrice { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_SpecialPrice)]
        public double? SpecialPrice { get; set; }
        public double? WholePrice { get; set; }
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public Guid? Model_ID { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ModelCode = "ModelCode";
            public const string ModelName = "ModelName";
            public const string ModelType = "ModelType";
            public const string NormalPrice = "NormalPrice";
            public const string SpecialPrice = "SpecialPrice";
            public const string DateApply = "DateApply";
            
        }
    }

    public class Cat_ModelSearchModel
    {
        public string ModelName { get; set; }
        public string ModelType { get; set; }
        public string ModelCode { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_ModelMultiModel
    {
        public Guid ID { get; set; }
        public string ModelName { get; set; }
    }
}
