using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatEmployeeTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_EmployeeType_EmployeeTypeName)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_EmployeeType_EmployeeTypeName_StringLength)]
        [Required(ErrorMessage = ConstantDisplay.HRM_Category_EmployeeType_EmployeeTypeName_Required)]     
        public string EmployeeTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_EmployeeType_Notes)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_EmployeeType_Notes_StringLength)]       
        public string Notes { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_EmployeeType_CodeID)]
        public int? CodeID { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
        
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string EmployeeTypeName = "EmployeeTypeName";
            public const string Notes = "Notes";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class CatEmployeeTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_EmployeeType_EmployeeTypeName)]
        public string EmployeeTypeName { get; set; }
        public bool IsExport { get; set; }

        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string EmployeeTypeName = "EmployeeTypeName";
        }
    }

    public class CatEmployeeTypeMultiModel 
    {
        public Guid ID { get; set; }
        public string EmployeeTypeName { get; set; }
    }
}
