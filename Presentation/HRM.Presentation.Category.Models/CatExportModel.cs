using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatExportMultiModel
    {
        public Guid ID { get; set; }
        public string ExportName { get; set; }
    }

    public class CatExportModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Export_Code)]
        [MaxLength(32)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_ExportName)]
        [MaxLength(50)]
        public string ExportName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_ScreenName)]
        [MaxLength(50)]
        public string ScreenName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_ObjectName)]
        [MaxLength(32)]
        public string ObjectName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_SheetIndex)]
        public long? SheetIndex { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_StartRowIndex)]
        public long? StartRowIndex { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_StartColumnIndex)]
        public long? StartColumnIndex { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_TemplateFile)]
        [MaxLength(100)]
        public string TemplateFile { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_Description)]
        [MaxLength(100)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_IsProtected)]
        public bool IsProtected { get; set; }

        public Guid? ExportID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ExportItem_GroupOrder)]
        public int? GroupOrder { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Export_IsColumnDynamic)]
        public bool? IsColumnDynamic { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Export_IsSmartMarkers)]
        public bool? IsSmartMarkers { get; set; }
        public bool? IsExportWord { get; set; }
        public bool? IsGroup { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Export_IsReadOnly)]
        public bool? IsReadOnly { get; set; }
        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string ExportName = "ExportName";
            public const string ScreenName = "ScreenName";
            public const string ObjectName = "ObjectName";
            public const string SheetIndex = "SheetIndex";
            public const string StartRowIndex = "StartRowIndex";
            public const string StartColumnIndex = "StartColumnIndex";
            public const string TemplateFile = "TemplateFile";
            public const string Description = "Description";
            public const string IsProtected = "IsProtected";
            public const string GroupOrder = "GroupOrder";
            public const string IsColumnDynamic = "IsColumnDynamic";
        }
    }

    public class CatExportSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Export_ExportName)]
        public string ExportName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_ScreenName)]
        public string ScreenName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_ObjectName)]
        public string ObjectName { get; set; }
    }
}
