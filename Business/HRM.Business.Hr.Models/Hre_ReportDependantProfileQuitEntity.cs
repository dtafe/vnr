using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportDependantProfileQuitEntity : HRMBaseModel
    {
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string DependantName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateQuit { get; set; }
        public DateTime? MonthOfEffect { get; set; }
    }
}
