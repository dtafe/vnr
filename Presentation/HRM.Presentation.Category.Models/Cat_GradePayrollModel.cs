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
    public class Cat_GradePayrollModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_GradeCfgName)]
        public string GradeCfgName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_IsProductSalary)]
        public bool? IsProductSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_IsMoneySalary)]
        public bool? IsMoneySalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_IsFormulaSalary)]
        public bool? IsFormulaSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_SalaryDateClose)]
        public DateTime? SalaryDateClose { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_SalaryDateClose_SalaryTimeTypeClose)]
        public Nullable<int> SalaryDayClose { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_SalaryTimeTypeClose)]
        public string SalaryTimeTypeClose { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_IsDeptSalary)]
        public Nullable<bool> IsDeptSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_IsCommissionSalary)]
        public Nullable<bool> IsCommissionSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_FormulaSalaryIns)]
        public string FormulaSalaryIns { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_FormulaJobNameIns)]
        public string FormulaJobNameIns { get; set; }

        private Guid _id = Guid.Empty;
        public Guid GradePayroll_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;

            }
        }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string GradeCfgName = "GradeCfgName";
            public const string IsMoneySalary = "IsMoneySalary";
            public const string IsProductSalary = "IsProductSalary";
            public const string IsFormulaSalary = "IsFormulaSalary";
            public const string Description = "Description";

        }
    }
    public class Cat_GradePayrollSearchlModel
    {

        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_GradeCfgName)]
        public string GradeCfgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_Code)]
        public string Code { get; set; }
    }

    public class Cat_GradePayrollMultiModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_GradeCfgName)]
        public Guid ID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradePayroll_GradeCfgName)]
        public string GradeCfgName { get; set; }
    }

}
