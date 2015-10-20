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
    public class Sal_ReportHaveChildUnusualModel : BaseViewModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string OrgStructureName { get; set; }
        public string RelativeName { get; set; }
        public DateTime? DateBorn { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Age { get; set; }

        public int? MonthOfAge { get; set; }
        public string OrgStructureID { get; set; }
        public Guid ExportId { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }

        public partial class FieldNames
        {

            public const string Date = "Date";
         
            public const string Rank = "Rank";
            public const string Target = "Target";
            public const string Actual = "Actual";
            public const string CompletionRate = "CompletionRate";
    
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";

        }
    }



}
