using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRM.Business.Hr.Models
{
    public class Hre_AddressEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        [MaxLength(1000)]
        public string AddressName { get; set; }
        [MaxLength(150)]
        public string AddressCode { get; set; }
        public Guid? CountryID { get; set; }
        [MaxLength(150)]
        public string CountryName { get; set; }
        public Guid? ProvinceID { get; set; }
        [MaxLength(150)]
        public string ProvinceName { get; set; }
        public Guid? DistrictID { get; set; }
        [MaxLength(150)]
        public string DistrictName { get; set; }
    }
}
