using System;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_SalesTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SalesType_SalesTypeName)]
        public string SalesTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Comment)]
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SalesType_SalesTypeGroup)]
        public string SalesTypeGroup { get; set; }

        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string SalesTypeName = "SalesTypeName";
            public const string Note = "Note";
            public const string SalesTypeGroup = "SalesTypeGroup";
        }
    }

    public class Eva_SalesTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_SalesType_SalesTypeName)]
        public string SalesTypeName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Eva_SalesTypeMultiModel
    {
        public Guid ID { get; set; }
        public string SalesTypeName { get; set; }
    }
}
