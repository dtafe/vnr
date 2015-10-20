using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_ForeignKeyMultiModel
    {
        public string CONSTRAINT_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        public string R_CONSTRAINT_NAME { get; set; }
    }

    public class Sys_AllSettingModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_System_AllSetting_Name)]
        [StringLength(150, ErrorMessage = ConstantDisplay.HRM_System_AllSetting_Name_StringLength)]
        [UIHint("ComboBox")]
        public string Name { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_AllSetting_Value1)]
        public string Value1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_AllSetting_Value2)]
        public string Value2 { get; set; }

        public string Value3 { get; set; }
        public string Value4 { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_AllSetting_ModuleId)]
        [UIHint("ComboBox")]
        public string ModuleName { get; set; }

        public Guid UserID { get; set; }

        public double? SalaryAverage { get; set; }
        public double? WorkHour { get; set; }

        public partial class FieldNames
        {
            public const string Name = "Name";
            public const string Value1 = "Value1";
            public const string Value2 = "Value2";
            public const string ModuleName = "ModuleName";
        }

    }

    public class Sys_AllSettingSearchModel
    {
        public string Name { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Sys_AllSettingReturnModel
    {
        public bool Status { get; set; }
        public string DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }


}
