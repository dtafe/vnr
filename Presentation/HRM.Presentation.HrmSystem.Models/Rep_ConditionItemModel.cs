using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Rep_ConditionItemModel : BaseViewModel
    {
         [DisplayName("Tên Điều Kiện")]
        public string ConditionItemName { get; set; }
            [DisplayName("Tên Nhóm Điều Kiện")]
        public string ConditionGroupName { get; set; }
             [DisplayName("Tên Nhóm Điều Kiện")]
        public Nullable<System.Guid> ConditionID { get; set; }
                [DisplayName("Loại Điều Kiện")]
        public string JoinType { get; set; }
            [DisplayName("Tên Bảng")]
        public string TableName { get; set; }
            [DisplayName("Tên Cột")]
        public string ColumnName { get; set; }
            [DisplayName("Toán Tử")]
        public string Operator { get; set; }
            [DisplayName("Giá Trị 1")]
        public string Value1 { get; set; }
            [DisplayName("Giá Trị 2")]
        public string Value2 { get; set; }
        [DisplayName("Loại Điều Kiện")]
        public string WhereType { get; set; }

        public partial class FieldNames
        {
            public const string ConditionItemName = "ConditionItemName";
            public const string ConditionGroupName = "ConditionGroupName";
            public const string WhereType = "WhereType";
            public const string ConditionID = "ConditionID";
            public const string JoinType = "JoinType";
            public const string TableName = "TableName";
            public const string ColumnName = "ColumnName";
            public const string Operator = "Operator";
            public const string Value1 = "Value1";
            public const string Value2 = "Value2";
        }
    }
}
