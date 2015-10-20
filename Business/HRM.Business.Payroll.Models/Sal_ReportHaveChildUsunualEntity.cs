using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Sal_ReportHaveChildUsunualEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string OrgStructureName { get; set; }
        public string RelativeName { get; set; }
        public string UNIT { get; set; }
        public string DIVISION { get; set; }
        public string DEPARTMENT { get; set; }
        public string SECTION { get; set; }
        public DateTime? DateBorn { get; set; }
        public int? Age { get; set; }

        public int? MonthOfAge { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public partial class FieldNames
        {

            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string RelativeName = "RelativeName";
            public const string DateBorn = "DateBorn";
            public const string Age = "Age";
            public const string MonthOfAge = "MonthOfAge";
            public const string UNIT = "UNIT";
            public const string DIVISION = "DIVISION";
            public const string DEPARTMENT = "DEPARTMENT";
            public const string SECTION = "SECTION";


            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        
        }
    }
}
