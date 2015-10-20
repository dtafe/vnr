using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System.Collections;
using System.Collections.Generic;
using HRM.Business.Main.Domain;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatImportModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_Code)]
        [StringLength(32, ErrorMessage = ConstantDisplay.HRM_Category_CatImport_Code_StringLength)]
        public string ImportCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_ImportName)]
        [StringLength(50, ErrorMessage = ConstantDisplay.HRM_Category_CatImport_ImportName_StringLength)]
        public string ImportName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_ObjectName)]
        [StringLength(32, ErrorMessage = ConstantDisplay.HRM_Category_CatImport_ObjectName_StringLength)]
        public string ObjectName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_SheetIndex)]
        public long? SheetIndex { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_StartRowIndex)]
        public long? StartRowIndex { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_StartColumnIndex)]
        public long? StartColumnIndex { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_TemplateFile)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_CatImport_TemplateFile_StringLength)]
        public string TemplateFile { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_Description)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_CatImport_Description_StringLength)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_IsProtected)]
        public bool IsProtected { get; set; }

        public string FormatDate { get; set; }
        public string Percent { get; set; }
        public string ProcessName { get; set; }
        public string ProcessNameView { get; set; }
        public bool IsImportAndSave { get; set; }
        public List<string> ListValueField { get; set; }
        public List<string> ListDisplayField { get; set; }
        public List<string> ListInvalidField { get; set; }
        public object ListInvalidData { get; set; }
        public string ProcessDuplicateData { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_ImportRosterType)]
        public string ImportRosterType { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_SkipRowNumbers)]
        public int SkipRowNumbers { get; set; }
        public object ListImportData { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_FollowData)]
        public bool FollowData { get; set; }

        public string UrlInvalidFileName { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ImportCode = "ImportCode";
            public const string ImportName = "ImportName";
            public const string ObjectName = "ObjectName";
            public const string SheetIndex = "SheetIndex";
            public const string StartRowIndex = "StartRowIndex";
            public const string StartColumnIndex = "StartColumnIndex";
            public const string TemplateFile = "TemplateFile";
            public const string Description = "Description";
            public const string IsProtected = "IsProtected";
            public const string FollowData = "FollowData";
        }
    }

    public class Cat_ImportSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_ImportName)]
        public string ImportName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_Code)]
        public string ImportCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_ObjectName)]
        public string ObjectName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class CatImportMultiModel
    {
        public Guid? ID { get; set; }
        public string ImportName { get; set; }
        public string ObjectName { get; set; }
    }

    public class CatSysTablesMultiModel
    {
        public Guid? ObjectID { get; set; }
        public string Name { get; set; }
    }

    public class CatChildFieldsMultiModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

}
