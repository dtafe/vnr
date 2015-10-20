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
    public class CatExportItemModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ExportItem_Code)]
        [MaxLength(32)]
        public string ExportItemCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_ExportName)]
        public Guid? ExportID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ExportItem_DataField)]
        [MaxLength(32)]
        public string DataField { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ExportItem_ExcelField)]
        [MaxLength(32)]
        public string ExcelField { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ExportItem_FormatString)]
        [MaxLength(32)]
        public string FormatString { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ExportItem_Description)]
        [MaxLength(100)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ExportItem_IsFixedCell)]
        public bool? IsFixedCell { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_ExportName)]
        public string ExportName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ExportItem_GroupOrder)]
        public long? GroupOrder { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ExportItem_SubTotal)]
        public bool? SubTotal { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string ExportID = "ExportID";
            public const string DataField = "DataField";
            public const string ExcelField = "ExcelField";
            public const string FormatString = "FormatString";
            public const string Description = "Description";
            public const string IsFixedCell = "IsFixedCell";
            public const string ExportName = "ExportName";
            public const string GroupOrder = "GroupOrder";
            public const string SubTotal = "SubTotal";
        }
    }
}
