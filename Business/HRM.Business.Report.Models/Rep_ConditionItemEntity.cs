using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Report.Models
{
    public class Rep_ConditionItemEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string ConditionItemName { get; set; }
        public string ConditionGroupName { get; set; }
        public Nullable<System.Guid> ConditionID { get; set; }
        public string JoinType { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string Operator { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string WhereType { get; set; }
    }
}
