using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Category.Models
{
    public class Cat_AbilityTileEntity : HRMBaseModel
    {
        public Nullable<System.Guid> RankID { get; set; }
        public string RankName { get; set; }
        public string AbilityTitleVNI { get; set; }
        public string AbilityTitleEng { get; set; }
        public string Code { get; set; }
    }
    public class Cat_AbilityTileMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string Code { get; set; }
    }
}
