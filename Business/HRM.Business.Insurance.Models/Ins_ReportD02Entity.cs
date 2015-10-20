using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    public class Ins_ReportD02Entity : HRMBaseModel
    {
        public string ReportD02Name { get; set; }
        public DateTime? DateReport { get; set; }
        public DateTime? DateMonth { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? SociaInsCountPro { get; set; }
        public double? SociaInsTotalSalary { get; set; }
        public int? HealthInsCountPro { get; set; }
        public double? HealthInsTotalSalary { get; set; }
        public int? UnEmpInsCountPro { get; set; }
        public double? UnEmpInsTotalSalary { get; set; }
        public double? MaxSalary { get; set; }
        public double? MinSalary { get; set; }
        public DateTime? DateOfEffectMax { get; set; }
        public DateTime? DateOfEffectMin { get; set; }
        public string Type { get; set; }
    }

}
