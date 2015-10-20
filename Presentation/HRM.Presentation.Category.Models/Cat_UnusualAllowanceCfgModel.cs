using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_UnusualAllowanceCfgModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_Name)]
        public string UnusualAllowanceCfgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_EDType)]
        public string EDType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_Amount)]
        public double Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_IsAddToHourlyRate)]
        public bool IsAddToHourlyRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_IsChargePIT)]
        public bool IsChargePIT { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_IsExcludePayslip)]
        public bool IsExcludePayslip { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_MethodCalculation)]
        public string MethodCalculation { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_Formula)]
        public string Formula { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_Type)]
        public string Type { get; set; }
        public string TypeView { get; set; }
        public string EDTypeView { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string UnusualAllowanceCfgName = "UnusualAllowanceCfgName";
            public const string EDType = "EDType";
            public const string Code = "Code";
            public const string Amount = "Amount";
            public const string IsAddToHourlyRate = "IsAddToHourlyRate";
            public const string IsChargePIT = "IsChargePIT";
            public const string IsExcludePayslip = "IsExcludePayslip";
            public const string MethodCalculation = "MethodCalculation";
            public const string Formula = "Formula";
            public const string Comment = "Comment";
            public const string Type = "Type";
            public const string TypeView = "TypeView";
            public const string EDTypeView = "EDTypeView";
        }
    }

    public class CatUnusualAllowanceCfgSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_Name)]
        public string UnusualAllowanceCfgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_Name)]
        public string Code { get; set; }
        public string EDType { get; set; }
        public string Type { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_UnusualAllowanceCfgMuitlModel
    {
        public Guid ID { get; set; }
        public string UnusualAllowanceCfgName { get; set; }
        public string Code { get; set; }
        public string UnusualAllowanceCfgNameCode { get; set; }
    }
    public class Cat_UnusualAllowanceCfgAmout
    {
        public Guid ID { get; set; }
        public string UnusualAllowanceCfgName { get; set; }
        public double Amount { get; set; }
    }
}
