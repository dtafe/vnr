using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Category.Models
{
    public class Cat_AccidentTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_AccidentType_AccidentTypeName)]
        public string AccidentTypeName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_AccidentType_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_AccidentType_Type)]
        public string Type { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string AccidentTypeName = "AccidentTypeName";
            public const string Code = "Code";
            public const string Type = "Type";
        }
    }
    public class Cat_AccidentTypeSearchModel
    {
         [DisplayName(ConstantDisplay.HRM_Category_AccidentType_AccidentTypeName)]
        public string AccidentTypeName { get; set; }
         public bool? IsExport { get; set; }
         public string ValueFields { get; set; }
    }

    public class Cat_AccidentTyNameMuliModel
    {
        public Guid ID { get; set; }
        public string AcccidentTypeName { get; set; }
    }
    public class Cat_AccidentTypeMutiModel
    {
        public Guid ID { get; set; }
        public string AccidentTypeName { get; set; }
    }
}
