using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Rep_ConditionModel : BaseViewModel
    {
         [DisplayName("Tên Nhóm Điều Kiện")]
        public string ConditionGroupName { get; set; }
         [DisplayName("Tên Báo Cáo")]
        public Nullable<System.Guid> MasterID { get; set; }
         [DisplayName("Ghi Chú")]
        public string Notes { get; set; }
         [DisplayName("STT")]
        public Nullable<int> OrderNumber { get; set; }
        [DisplayName("Loại Điều Kiện")]
        public string WhereType { get; set; }

        public partial class FieldNames
        {
            public const string ConditionGroupName = "ConditionGroupName";
            public const string MasterID = "MasterID";
            public const string Notes = "Notes";
            public const string OrderNumber = "OrderNumber";
            public const string WhereType = "WhereType";
        }
    }

    public class Rep_ConditionSearchModel
    {
         [DisplayName("Loại Điều Kiện")]
        public Guid? MasterID { get; set; }
    }
}
