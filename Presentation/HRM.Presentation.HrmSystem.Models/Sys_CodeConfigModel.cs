using HRM.Infrastructure.Utilities;
using System;
using System.ComponentModel;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_CodeConfigSearchModel
    {
        public string ObjectName { get; set; }
    }

    public class Sys_CodeConfigModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_ObjectName)]
        public string ObjectName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_FieldName)]
        public string FieldName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_Formula)]
        public string Formula { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_Ordinal)]
        public int? Ordinal { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_IsOnInitObject)]
        public bool? IsOnInitObject { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_IsResetByDay)]
        public bool? IsResetByDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_IsResetByMonth)]
        public bool? IsResetByMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_IsResetByYear)]
        public bool? IsResetByYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_IsResetByUser)]
        public bool? IsResetByUser { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_IsResetByField)]
        public bool? IsResetByField { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_ResetByField1)]
        public string ResetByField1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_ResetByField2)]
        public string ResetByField2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_CodeConfig_Description)]
        public string Description { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ObjectName = "ObjectName";
            public const string FieldName = "FieldName";
            public const string Formula = "Formula";
            public const string Ordinal = "Ordinal";
            public const string IsOnInitObject = "IsOnInitObject";
            public const string IsResetByDay = "IsResetByDay";
            public const string IsResetByMonth = "IsResetByMonth";
            public const string IsResetByYear = "IsResetByYear";
            public const string IsResetByUser = "IsResetByUser";
            public const string IsResetByField = "IsResetByField";
            public const string ResetByField1 = "ResetByField1";
            public const string ResetByField2 = "ResetByField2";
            public const string Description = "Description";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }

    }
}
