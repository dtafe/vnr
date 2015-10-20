using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportRecieveObjectByTimeEntity : HRMBaseModel
    {
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureID { get; set; }
        public string Dept { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public string Unit { get; set; }
        public string Part { get; set; }
        public string Line { get; set; }
        public string Session { get; set; }


        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Dept = "Dept";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string Unit = "Unit";
            public const string Part = "Part";
            public const string Line = "Line";
            public const string Session = "Session";
        }

    }
}
