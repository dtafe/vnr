using System;


namespace HRM.Business.Category.Models
{
    public class Cat_RateInsuranceEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public double HealthInsCompRate { get; set; }
        public double HealthInsEmpRate { get; set; }
        public double SocialInsCompRate { get; set; }
        public double SocialInsEmpRate { get; set; }
        public double UnemployInsCompRate { get; set; }
        public double UnemployInsEmpRate { get; set; }
        public Nullable<System.DateTime> ApplyFrom { get; set; }
    }

   
}

