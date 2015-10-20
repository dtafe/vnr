using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM.Presentation.EmpPortal.Models
{
    public class Sal_PayrollTableItemTempModel
    {
        public int ID { get; set; }
        public string ProfileName { get; set; }
        public string Value { get; set; }
        public string Description1 { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string Value = "Value";
            public const string Description1 = "Description1";
            
        }
    }
}