
namespace HRM.Business.HrmSystem.Models
{
    public class Sys_DynamicColumnEntity : HRM.Business.BaseModel.HRMBaseModel
    {       
        public string ColumnName { get; set; }
       
        public string Code { get; set; }
       
        public string Status { get; set; }
        
        public string DataType { get; set; }
       
        public int? Length { get; set; }
      
        public string TableName { get; set; }
    
        public string Comment { get; set; }
    }
}
