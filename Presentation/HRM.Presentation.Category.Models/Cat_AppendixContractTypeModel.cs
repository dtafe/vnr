using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatAppendixContractTypeMultiModel
    {
        public string AppendixContractName { get; set; }
        public Guid ID { get; set; }
        
    }
    public class Cat_AppendixContractTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_AppendixContractType_Name)]
        public string AppendixContractName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_AppendixContractType_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_AppendixContractType_ReportName)]
        public Guid? ReportID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_AppendixContractType_ReportName)]
        public string ReportName { get; set; }
        public Guid? ExportID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_AppendixContractType_ExportName)]
        public string ExportName { get; set; }
        public partial class FieldNames
        
        {
            public const string ID = "ID";
            public const string AppendixContractName = "AppendixContractName";
            public const string Code = "Code";
            public const string ExportName = "ExportName";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }
    public class Cat_AppendixContractTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_AppendixContractType_Name)]
        public string AppendixContractName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }


}
