
using System;
namespace HRM.Business.HrmSystem.Models
{
    public class Sys_BookmarkEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Guid ID { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public Guid UserID { get; set; }
    }
}
