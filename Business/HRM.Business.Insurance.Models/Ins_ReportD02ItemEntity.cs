using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    public class Ins_ReportD02ItemEntity : HRMBaseModel
    {
        public Guid? ReportD02ID { get; set; }
        public Guid? ProfileID { get; set; }
        public Nullable<double> OldBasicSalary { get; set; }
        public Nullable<double> NewBasicSalary { get; set; }
        public Nullable<double> RateSocialIns { get; set; }
        public Nullable<double> RateHealthIns { get; set; }
        public Nullable<double> RateUnEmpIns { get; set; }
        public bool? NotCardHealth { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime? MonthFrom { get; set; }
        public DateTime? MonthTo { get; set; }
        public DateTime? MonthConvertRecord { get; set; }
        public int? ItemOrder { get; set; }
        public string Comment { get; set; }

        public string JobName { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<double> AllowanceAdditional { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public Guid? SocialInsPlaceID { get; set; }
        public string SocialInsNo { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public bool? IsPayBack { get; set; }
        public Guid? PayBackID { get; set; }
    

    }

}
