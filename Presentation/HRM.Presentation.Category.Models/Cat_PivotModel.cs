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
    public class Cat_PivotMultiModel
    {
        public Guid ID { get; set; }
        public string PivotName { get; set; }
    }

    public class Cat_PivotModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Export_Code)]
        [MaxLength(32)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_ExportName)]
        [MaxLength(50)]
        public string PivotName { get; set; }


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

        public string PivotFileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_Import_TemplateFile)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_CatImport_TemplateFile_StringLength)]
        public string TemplateFile { get; set; }

        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string PivotName = "PivotName";
           
            public const string Description = "Description";
            public const string IsProtected = "IsProtected";
            public const string IsReadOnly = "IsReadOnly";
            public const string GroupOrder = "GroupOrder";
            public const string IsColumnDynamic = "IsColumnDynamic";
        }
    }

    public class Cat_PivotSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Export_ExportName)]
        public string PivotName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Export_Code)]
        public string Code { get; set; }

      
    }
}
