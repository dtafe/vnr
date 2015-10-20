using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM.Presentation.EmpPortal.Models
{
    public class LeavesSummaryModel : BaseModelPortal
    {
        public string LeavesType { get; set; }
        public int Entitled { get; set; }
        public int Taken { get; set; }
        public int Balance { get; set; }

        public partial class FieldNames
        {
            public const string LeavesType = "LeavesType";
            public const string Entitled = "Entitled";
            public const string Taken = "Taken";
            public const string Balance = "Balance";
        }
    }
}