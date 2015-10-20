using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Kai_CategoryModel:BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Kai_Category_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_Category_CategoryName)]
        public string CategoryName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Kai_Category_CatalogyType)]
        public string CatalogyType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_Category_CatalogyType)]
         public string CatalogyTypeTranslate { get; set; }
        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string CategoryName = "CategoryName";
            public const string CatalogyType = "CatalogyType";
            public const string CatalogyTypeTranslate = "CatalogyTypeTranslate";
        }
    }

    public class Kai_CategorySearchModel
    {
       [DisplayName(ConstantDisplay.HRM_Kai_Category_CategoryName)]
        public string CategoryName { get; set; }
       [DisplayName(ConstantDisplay.HRM_Kai_Category_Code)]
       public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Kai_CategoryMultiModel
    {
        public Guid ID { get; set; }
        public string CategoryName { get; set; }
        public int TotalRow { get; set; }
    }
}
