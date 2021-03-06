﻿using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_ReportKaizenGeneralModel 
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_OrgStructureTypeName)]
        public string TypeOrg { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }

        public string ReportName { get; set; }
        public ExportFileType ExportType { get; set; }


        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }
    }

}
