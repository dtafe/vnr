using System;


namespace HRM.Business.Category.Models
{
    public class Cat_ProductEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string ProductName { get; set; }
        public string Unit { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public double BonusPerUnit { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.Guid> ProductTypeID { get; set; }
        public string ProductTypeName { get; set; }
        public string Code { get; set; }
        public Nullable<double> ProductionTime { get; set; }
        public string Note { get; set; }
        
    }

    public class Cat_ProductMultiEntity 
    {
        public Guid ID { get; set; }
        public string ProductName { get; set; }
    }

}

