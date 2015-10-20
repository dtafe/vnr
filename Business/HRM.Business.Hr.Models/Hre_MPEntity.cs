using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRM.Business.Hr.Models
{
    public class Hre_MPEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string MPRank { get; set; }
        public string MPRankView { get; set; }
        public string Notes { get; set; }
    }
}
