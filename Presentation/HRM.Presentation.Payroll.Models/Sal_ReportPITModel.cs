using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_ReportPITSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_Year)]
        public int? Year { get; set; }        
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_StatusEmployee)]
        public string StatusEmployees { get; set; }
        
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }
    }
    public class Sal_ReportPITModel : BaseViewModel
    {
        public Double? CountPersonalVN { get; set; }
        public Double? CountPersonalEN { get; set; }
        public Double? BeforeTaxPersonalVN { get; set; }
        public Double? BeforeTaxPersonalVN_NoContract { get; set; }
        public Double? BeforeTaxPersonalEN { get; set; }
        public Double? IncomeTaxablePersonalVN { get; set; }
        public Double? IncomeTaxablePersonalVN_NoContract { get; set; }
        public Double? IncomeTaxablePersonalEN { get; set; }
        public Double? PitEmpPersonalVN { get; set; }
        public Double? PitEmpPersonalVN_NoContract { get; set; }
        public Double? PitEmpPersonalEN { get; set; }
        public int? Year { get; set; }

        public partial class FieldNames
        {
             public const string CountPersonalEN = "CountPersonalEN";
             public const string BeforeTaxPersonalVN = "BeforeTaxPersonalVN";
             public const string BeforeTaxPersonalVN_NoContract = "BeforeTaxPersonalVN_NoContract";
             public const string BeforeTaxPersonalEN             = "BeforeTaxPersonalEN            ";
             public const string IncomeTaxablePersonalVN = "IncomeTaxablePersonalVN";
             public const string IncomeTaxablePersonalVN_NoContract = "IncomeTaxablePersonalVN_NoContract";
             public const string IncomeTaxablePersonalEN = "IncomeTaxablePersonalEN";
             public const string PitEmpPersonalVN = "PitEmpPersonalVN";
             public const string PitEmpPersonalVN_NoContract = "PitEmpPersonalVN_NoContract";
             public const string PitEmpPersonalEN = "PitEmpPersonalEN";
             public const string Year = "Year";
        }
    }    

}
