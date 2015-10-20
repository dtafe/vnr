using System;


namespace HRM.Business.Category.Models
{
    public class Cat_ValueEntityEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string Type { get; set; }
        public string ValueEntityName { get; set; }
        public Nullable<double> Value { get; set; }
        public Nullable<System.DateTime> DateOfEffect { get; set; }
        public Nullable<double> Value2 { get; set; }
        public Nullable<double> Value3 { get; set; }
        public string ValueString { get; set; }
        public string ValueString2 { get; set; }
        public string ValueString3 { get; set; }
        public string Comment { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
    }

    
}

