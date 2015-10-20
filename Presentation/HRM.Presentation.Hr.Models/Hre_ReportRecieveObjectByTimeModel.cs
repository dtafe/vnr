using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportRecieveObjectByTimeModel : BaseViewModel
    {
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureID { get; set; }
        public string Dept { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
