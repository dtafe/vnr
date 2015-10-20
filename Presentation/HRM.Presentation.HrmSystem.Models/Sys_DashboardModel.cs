using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_DashboardModel : BaseViewModel
    {
        public int ExpiryContract { get; set; }
        public int EndProbation { get; set; }
        public int BirthDay { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string OrgStructureID { get; set; }
        public class FieldsName
        {
            public const string ExpiryContract = "ExpiryContract";
            public const string EndProbation = "EndProbation";
            public const string BirthDay = "BirthDay";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
        }
    }
}
