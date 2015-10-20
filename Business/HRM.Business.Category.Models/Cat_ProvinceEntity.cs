using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_ProvinceMultiEntity 
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string ProvinceName { get; set; }
    }
    public class Cat_ProvinceEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string Code { get; set; }
        public string ProvinceName { get; set; }
        public string Notes { get; set; }
        public Guid? CountryID { get; set; }
        public Guid? RegionID { get; set; }
        public int? Order { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
    }
}
