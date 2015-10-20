using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    /// <summary>Truy Lĩnh bảo hiểm</summary>
    public class Ins_InsuranceSalaryPaybackEntity : HRMBaseModel
    {
        public Guid? ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public DateTime? MonthYear { get; set; }
        public DateTime? FromMonthEffect { get; set; }
        public DateTime? ToMonthEffect { get; set; }
        public double? InsSalary { get; set; }
        public double? InsSalaryPayBack { get; set; }
        public double? InsSalaryAdjust { get; set; }
        public double? AmoutHDTIns { get; set; }
        public double? AmoutHDTInsPayBack { get; set; }
        public string JobtitleName { get; set; }
        public bool? IsSocialIns { get; set; }
        public bool? IsMedicalIns { get; set; }
        public bool? IsUnemploymentIns { get; set; }
        public double? SocialInsEmpRate { get; set; }
        public double? HealthInsEmpRate { get; set; }
        public double? UnemployEmpRate { get; set; }
        public double? SocialInsComRate { get; set; }
        public double? HealthInsComRate { get; set; }
        public double? UnemployComRate { get; set; }
        public string Note { get; set; }
        public Guid? TypeID { get; set; }
        public string Comment { get; set; }
        public string CommentName { get; set; }
        public bool? IsCallPayBack { get; set; }
        public string TypeCode { get; set; }
        public string StatusCode { get; set; }
        public string TypeName { get; set; }
        public string StatusName { get; set; }
        public Guid? SocialInsPlaceID { get; set; }

    }

}
