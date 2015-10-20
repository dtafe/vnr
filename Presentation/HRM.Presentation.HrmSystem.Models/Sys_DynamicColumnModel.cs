using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_DynamicColumnModel : BaseViewModel
    {
        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_System_DynamicColumn_ColumnName)]
        public string ColumnName { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_DynamicColumn_Code)]
        public string Code { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_DynamicColumn_Status)]
        public string Status { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_DynamicColumn_DataType)]
        public string DataType { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_System_DynamicColumn_Length)]
        public int? Length { get; set; }

        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_System_DynamicColumn_TableName)]
        public string TableName { get; set; }

        [MaxLength(1000)]
        [DisplayName(ConstantDisplay.HRM_System_DynamicColumn_Comment)]
        public string Comment { get; set; }

        public partial class FieldNames
        {
            public const string ColumnName = "ColumnName";
            public const string Code = "Code";
            public const string Status = "Status";
            public const string DataType = "DataType";
            public const string Length = "Length";
            public const string TableName = "TableName";
            public const string Comment = "Comment";
        }
    }
    public class Sys_DynamicColumnSearchModel
    {
        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_System_DynamicColumn_ColumnName)]
        public string ColumnName { get; set; }
       
    }
}