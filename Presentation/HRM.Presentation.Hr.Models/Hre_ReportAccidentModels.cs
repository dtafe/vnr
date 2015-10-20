using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;
namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportAccidentModels : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Acciden_DateFrom)]
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public Guid? AccidentTypeID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Accident_AccidentTypeID)]
        public Guid ExportID { get; set; }
        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string OrgStructureID = "OrgStructureID";
            public const string AccidentTypeID = "AccidentTypeID";
        }
    }
}
