using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
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
        public Guid? CodeID { get; set; }

    }
}
