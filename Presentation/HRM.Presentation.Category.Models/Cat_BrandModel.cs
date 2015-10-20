using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_BrandModel : BaseViewModel
    {
        public string BrandName { get; set; }
        public string Code { get; set; }

        public string Note { get; set; }
        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string LineItemName = "LineItemName";
            public const string BrandName = "BrandName";
            public const string Code = "Code";
            public const string Note = "Note";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Cat_BrandSearchModel
    {
        public string Code { get; set; }
        public string BrandName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_BrandMultiModel
    {
        public Guid ID { get; set; }
        public string BrandName { get; set; }
        public int TotalRow { get; set; }
    }

}
