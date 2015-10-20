using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;


namespace HRM.Presentation.Category.Models
{
    public class PUR_ColorModelModel : BaseViewModel
    {
        public Guid? ModelID { get; set; }
        public string ColorCode { get; set; }
        public string Color { get; set; }
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ColorCode = "ColorCode";
            public const string ModelID = "ModelID";
            public const string Color = "Color";
            public const string Note = "Note";
            public const string UserCreate = "UserCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

}
