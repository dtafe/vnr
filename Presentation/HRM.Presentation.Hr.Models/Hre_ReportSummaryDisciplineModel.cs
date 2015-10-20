using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportSummaryDisciplineModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_ReportProfileDiscipline_DisciplineType)]
        public string DisciplineTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }


        public int? Jan { get; set; }
        public int? Feb { get; set; }
        public int? Mar { get; set; }
        public int? Apr { get; set; }
        public int? May { get; set; }
        public int? Jun { get; set; }
        public int? Jul { get; set; }
        public int? Aug { get; set; }
        public int? Sep { get; set; }
        public int? Oct { get; set; }
        public int? Nov { get; set; }
        public int? Dec { get; set; }
        
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsChecked { get; set; }

        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }


        public partial class FieldNames
        {
            public const string DisciplineTypeName = "DisciplineTypeName";

            public const string Jan = "Jan";
            public const string Feb = "Feb";
            public const string Mar = "Mar";
            public const string Apr = "Apr";
            public const string May = "May";
            public const string Jun = "Jun";
            public const string Jul = "Jul";
            public const string Aug = "Aug";
            public const string Sep = "Sep";
            public const string Oct = "Oct";
            public const string Nov = "Nov";
            public const string Dec = "Dec";
        }

    }
}
