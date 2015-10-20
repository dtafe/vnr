using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_ProductItemEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string ProductItemCode { get; set; }
        public Nullable<System.Guid> ProductID { get; set; }
        public string ProductItemName { get; set; }
        public string Code { get; set; }
        public Nullable<double> UnitPrice { get; set; }
        public Nullable<double> BonusPrice { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public Nullable<double> RateStage { get; set; }
        public Nullable<double> ProductionTime { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string ProductName { get; set; }
        public string CurrencyName { get; set; }

    }
    public class Cat_ProductItemMultiEntity
    {
        public Guid ID { get; set; }
        public string ProductItemName { get; set; }
    }

}
