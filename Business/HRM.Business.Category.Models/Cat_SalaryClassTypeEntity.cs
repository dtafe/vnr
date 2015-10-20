using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;

namespace HRM.Business.Category.Models
{
    public class Cat_SalaryClassTypeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string SalaryClassTypeName { get; set; }
        public string Description { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_SalaryClassTypeMultiEntity
    {
        public Guid ID { get; set; }
        public string SalaryClassTypeName { get; set; }
    }
}

