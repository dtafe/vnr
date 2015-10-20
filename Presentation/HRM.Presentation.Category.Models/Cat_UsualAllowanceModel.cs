using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_UsualAllowanceModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_UsualAllowanceName)]
        public string UsualAllowanceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_EDType)]
        public string EDType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_IsAddToHourlyRate)]
        public bool IsAddToHourlyRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_IsChargePIT)]
        public bool IsChargePIT { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_IsBaseOnWorkday)]
        public bool IsBaseOnWorkday { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_MinHours)]
        public double? MinHours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_UseSalaryByHours)]
        public bool? UseSalaryByHours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_MethodPayroll)]
        public string MethodPayroll { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_IsUseHoursSalary)]
        public bool? IsUseHoursSalary { get; set; }
       [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_GrossAmount)]
        public string Formula { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_GrossAmount)]
       public double GrossAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_MethodCalculation)]
        public string MethodCalculation { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_FormulaNoPIT)]
        public string FormulaNoPIT { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_Comment)]
        public string Comment { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        private Guid _id = Guid.Empty;
        public Guid UsualAllowance_ID
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
            public const string UsualAllowanceName = "UsualAllowanceName";
            public const string Code = "Code";
            public const string EDType = "EDType";
            public const string IsAddToHourlyRate = "IsAddToHourlyRate";
            public const string IsChargePIT = "IsChargePIT";
            public const string IsBaseOnWorkday = "IsBaseOnWorkday";
            public const string MinHours = "MinHours";
            public const string UseSalaryByHours = "UseSalaryByHours";
            public const string MethodPayroll = "MethodPayroll";
            public const string IsUseHoursSalary = "IsUseHoursSalary";
            public const string Formula = "Formula";
            public const string MethodCalculation = "MethodCalculation";
            public const string FormulaNoPIT = "FormulaNoPIT";
            public const string Comment = "Comment";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }

    public class Cat_UsualAllowanceSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_Code)]
        public string Code { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_UsualAllowanceName)]
        public string UsualAllowanceName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_UsualAllowanceMultiModel
    {
        public Guid ID { get; set; }
        public string UsualAllowanceName { get; set; }
    } 
}
