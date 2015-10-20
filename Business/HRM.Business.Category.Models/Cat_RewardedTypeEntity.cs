using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;

namespace HRM.Business.Category.Models
{
    public class Cat_RewardedTypeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string RewardedTypeName { get; set; }
        public string Code { get; set; }
        public string Notes { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_RewardedTypeMultiEntity
    {
        public Guid ID { get; set; }
        public string RewardedTypeName { get; set; }
    }
   
}

