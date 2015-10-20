using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    /// <summary></summary>
    public class Ins_ReportEmpHardJob2ndEntity : HRMBaseModel
    {
        public int Stt { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string JobName { get; set; }
        public string HDTJobType { get; set; }
        public int DaysNonHDTJob { get; set; }
        public DateTime? MonthYear { get; set; }
        public double? AmountHDTIns { get; set; }        
        public bool? IsExport { get; set; }
        public Guid ExportId { get; set; }
        public string OrgStructureID { get; set; }
    }

}
