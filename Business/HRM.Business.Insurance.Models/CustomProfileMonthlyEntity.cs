using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    public class CustomProfileMonthlyEntity
    {
        public Guid? ProfileID { get; set; }
        public double? SalaryInsurance { get; set; }
        public bool? IsSocialInsurance { get; set; }
        public DateTime MonthYear { get; set; }
        public double? MoneyHDTJob { get; set; }
        public bool? IsPregnant { get; set; }
        public bool? IsDecreaseWorkingDays { get; set; }
    }

}
