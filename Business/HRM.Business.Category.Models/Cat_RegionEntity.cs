using System;

namespace HRM.Business.Category.Models
{

    public class Cat_RegionEntity : HRM.Business.BaseModel.HRMBaseModel
    {
       
        public string Code { get; set; }

     
        public string RegionName { get; set; }

        public string Notes { get; set; }
        public Nullable<double> MinSalary { get; set; }
        public Nullable<double> MaxSalary { get; set; }
    }
}
