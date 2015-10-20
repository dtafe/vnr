using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;

namespace HRM.Business.Category.Models
{
    public class Cat_PerformanceTypeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string PerformanceTypeName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
    
}

