using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Xml;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Rep_MasterModel : BaseViewModel
    {   
        [DisplayName(ConstantDisplay.FIN_ClaimItem_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.Rep_Master_LinkObject)]
        public string LinkObject { get; set; }
        [DisplayName(ConstantDisplay.Rep_Master_ObjectMain)]
        public string ObjectMain { get; set; }
        [DisplayName(ConstantDisplay.Rep_Master_ReportMappingID)]
        public Guid? ReportMappingID { get; set; }
        [DisplayName(ConstantDisplay.Rep_Master_ReportMappingID)]
        public string ReportName { get; set; }
        [DisplayName(ConstantDisplay.Rep_Master_ReportStoreModule)]
        public string ReportStoreModule { get; set; }
        public Guid? ResourceID { get; set; }
        public string ScriptExcute { get; set; }
        public string ScriptParams { get; set; }
        public List<string> ListColumn { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportID { get; set; }
        public XmlNodeList NodeList { get; set; }
        public List<string> ListObject { get; set; }
        public partial class FieldNames
        {
            public const string ReportName = "ReportName";
            public const string LinkObject = "LinkObject";
            public const string ObjectMain = "ObjectMain";
            public const string Description = "Description";
        }
    }


    public class Rep_MasterModelSearch
    {
        [DisplayName("Tên Báo Cáo")]
        public string ReportName { get; set; }
    }

    /// <summary>
    /// Model để lưu cấu hình các control
    /// </summary>
    public class Rep_ControlModel:BaseViewModel
    {
        public string Name { get; set; }
        /// <summary>
        /// Tên hiển thị 1
        /// </summary>
        //public string DisplayName1 { get; set; }
        /// <summary>
        /// Tên hiển thị 2
        /// </summary>
        //public string DisplayName2 { get; set; }
        /// <summary>
        /// Giá trị mặt định 1
        /// </summary>
        //public string DefaultValue1 { get; set; }
        /// <summary>
        /// Giá trị mặt định 2
        /// </summary>
        //public string DefaultValue2 { get; set; }
        /// <summary>
        /// Tên tham số 1
        /// </summary>
        //public string ParamerterName1 { get; set; }
        /// <summary>
        /// Tên tham số 2
        /// </summary>
        //public string ParamerterName2 { get; set; }
        /// <summary>
        /// Tên đối tượng chọn combobox
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Text hiển thị khi chọn type là combobox
        /// </summary>
        public string TextField { get; set; }
        /// <summary>
        /// Value hiển thị khi chọn type là combobox
        /// </summary>
        public string ValueField { get; set; }
        /// <summary>
        /// Dữ liệu cho combobox là Enum
        /// </summary>
        public bool IsEnum { get; set; }
        /// <summary>
        /// Loại Control
        /// </summary>
        public string ControlType { get; set; }
        public Guid MasterID { get; set; }
        public Guid ConditionItemID { get; set; }
        public string ConditionName { get; set; }

        public partial class FieldNames
        {
            public const string Name = "Name";
            //public const string DisplayName1 = "DisplayName1";
            //public const string DisplayName2 = "DisplayName2";
            //public const string DefaultValue1 = "DefaultValue1";
            //public const string DefaultValue2 = "DefaultValue2";
            //public const string ParamerterName1 = "ParamerterName1";
            //public const string ParamerterName2 = "ParamerterName2";
            public const string TableName = "TableName";
            public const string TextField = "TextField";
            public const string ValueField = "ValueField";
            public const string IsEnum = "IsEnum";
            public const string ControlType = "ControlType";
            public const string MasterID = "MasterID";
            public const string ConditionItemID = "ConditionItemID";
            public const string ConditionName = "ConditionName";
        }
    }
}
