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
    public class Cat_PivotItemModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Export_Code)]
        [MaxLength(32)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Export_ExportName)]
        public Guid? PivotID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Pivot_PivotSheetIndex)]
        public int? PivotSheetIndex { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Pivot_PivotHeaderRowIndex)]
        public int? PivotHeaderRowIndex { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Pivot_PivotColumnStart)]
        public int? PivotColumnStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Pivot_PivotColumnEnd)]
        public int? PivotColumnEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Pivot_PivotColumnName)]
        public string PivotColumnName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Pivot_StartRowIndex)]
        public int? StartRowIndex { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Pivot_StartColumnIndex)]
        public int? StartColumnIndex { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Pivot_SkipColumnNumber)]
        public int? SkipRowNumbers { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Pivot_TargetSheetIndex)]
        public int? TargetSheetIndex { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Pivot_TargetSheetName)]
        public string TargetSheetName { get; set; }

        public string Description { get; set; }



        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string PivotSheetIndex = "PivotSheetIndex";
            public const string PivotHeaderRowIndex = "PivotHeaderRowIndex";
            public const string PivotColumnStart = "PivotColumnStart";
            public const string PivotColumnEnd = "PivotColumnEnd";
            public const string PivotColumnName = "PivotColumnName";
            public const string StartRowIndex = "StartRowIndex";
            public const string StartColumnIndex = "StartColumnIndex";
            public const string SkipRowNumbers = "SkipRowNumbers";
            public const string TargetSheetIndex = "TargetSheetIndex";
            public const string TargetSheetName = "TargetSheetName";
            public const string Description = "Description";
            public const string Code = "Code";

        }
    }
}
