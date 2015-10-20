using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_AnalyzeAnnualModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_AnalyzeAnnual_Config)]
        [MaxLength(150)]
        public string Config { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AnalyzeAnnual_Compute)]
        [MaxLength(1000)]
        public string Compute { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AnalyzeAnnual_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructure { get; set; }

        public partial class FieldNames
        {
            public const string Config = "Config";
            public const string Compute = "Compute";
            public const string DateFrom = "DateFrom";
            public const string OrgStructure = "OrgStructure";
        }
    }

}
