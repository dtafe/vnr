
namespace HRM.Business.HrmSystem.Models
{
    public class Sys_ResourceEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string ResourceType { get; set; }
        public string ResourceName { get; set; }
        public string ModuleName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}
