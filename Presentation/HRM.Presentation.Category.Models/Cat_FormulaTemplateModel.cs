using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Category.Models
{
    public class Cat_FormulaTemplateModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Element_Code)]
        [MaxLength(100)]
        public string ElementCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Element_ElementName)]
        [MaxLength(200)]
        public string ElementName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Element_Formula)]
        [MaxLength(6000)]
        public string Formula { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Element_OrderNumber)]
        public long? OrderNumber { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Category_Element_Invisible)]
        //public bool? Invisible { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Element_IsBold)]
        public bool? IsBold { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Category_Element_DisplayIndex)]
        //public long? DisplayIndex { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Element_Type)]
        [MaxLength(100)]
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Element_Description)]
        [MaxLength(500)]
        public string Description { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Category_Element_ColumnID)]
        //public Guid? ColumnID { get; set; }

        //[DisplayName(ConstantDisplay.HRM_System_DynamicColumn_ColumnName)]
        //public string ColumnName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LateEarlyRule_GradeName)]
        public Nullable<System.Guid> GradeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LateEarlyRule_GradeName)]
        public string GradeCfgName { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Category_Element_ElementType)]
        //public string ElementType { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Category_Element_ElementLevel)]
        //public string ElementLevel { get; set; }
        //public string MethodPayroll { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Category_GradePayroll_IsSalaryTime)]
        //public bool IsSalaryTime { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Category_GradePayroll_IsSalaryOvertime)]
        //public bool IsSalaryOvertime { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Category_GradePayroll_IsSalaryLeaveDay)]
        //public bool IsSalaryLeaveDay { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Category_GradePayroll_IsSalaryUsualAllowance)]
        //public bool IsSalaryUsualAllowance { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Category_GradePayroll_TabType)]
        //public string TabType { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ElementCode = "ElementCode";
            public const string ElementName = "ElementName";
            public const string Formula = "Formula";
            public const string OrderNumber = "OrderNumber";
            public const string Invisible = "Invisible";
            public const string IsBold = "IsBold";
            public const string DisplayIndex = "DisplayIndex";
            public const string Type = "Type";
            public const string Description = "Description";
            public const string ColumnID = "ColumnID";
            public const string ColumnName = "ColumnName";
            public const string GradeCfgName = "GradeCfgName";
            public const string ElementType = "ElementType";
            public const string GradePayrollID = "GradePayrollID";
            public const string ElementLevel = "ElementLevel";
            public const string MethodPayroll = "MethodPayroll";
            public const string IsSalaryTime = "IsSalaryTime";
            public const string IsSalaryOvertime = "IsSalaryOvertime";
            public const string IsSalaryLeaveDay = "IsSalaryLeaveDay";
            public const string IsSalaryUsualAllowance = "IsSalaryUsualAllowance";
            public const string TabType = "TabType";

        }
    }

    public class Cat_FormulaTemplateSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Element_ElementName)]
        [MaxLength(200)]
        public string ElementName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Element_Code)]
        [MaxLength(100)]
        public string ElementCode { get; set; }
        public string Cat_ElementType { get; set; }
        public Guid? GradePayrollID { get; set; }
        public string TabType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
