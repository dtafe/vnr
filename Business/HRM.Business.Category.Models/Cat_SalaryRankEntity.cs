using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Category.Models
{
    public class Cat_SalaryRankEntity:HRMBaseModel
    {
        public Guid? SalaryClassID { get; set; }
        public string SalaryClassName { get; set; }
        public Guid? SalaryClassTypeID { get; set; }
        public string SalaryClassTypeName { get; set; }
        public string SalaryRankName { get; set; }
        public double? Rate { get; set; }
        public double? SalaryStandard { get; set; }
        public double? SalaryMin { get; set; }
        public double? SalaryMax { get; set; }
        public DateTime? DateOfEffect { get; set; }
        public string AbilityTitleENG { get; set; }
        public string AbilityTitleVNI { get; set; }
        public string Code { get; set; }
        public string SalaryClassCode { get; set; }
        public int? OrderNumber { get; set; }
    }

    public class Cat_SalaryRankMultiEntity
    {
        public Guid ID { get; set; }
        public string SalaryRankName { get; set; }
    } 
}
