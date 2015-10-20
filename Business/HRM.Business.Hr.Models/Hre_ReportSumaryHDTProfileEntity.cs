using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportSumaryHDTProfileEntity : HRMBaseModel
    {
        public string ProfileName { get; set; }
        public Guid? HDTJobGroupID { get; set; }
        public string HDTJobGroupCode { get; set; }
        public string HDTJobGroupName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? MonthInsurance { get; set; }
        public DateTime? DateSearchFrom { get; set; }
        public DateTime? DateSearchTo { get; set; }
        public string CodeSearch { get; set; }
        public string CodeEmp { get; set; }
        public string Dept { get; set; }
        public string Unit { get; set; }
        public string Part { get; set; }
        public string Line { get; set; }
        public string Session { get; set; }
        public string Type { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string Dept = "Dept";
            public const string Type = "Type";
            public const string ProfileName = "ProfileName";
            public const string HDTJobGroupCode = "HDTJobGroupCode";
            public const string HDTJobGroupName = "HDTJobGroupName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string MonthInsurance = "MonthInsurance";
            public const string DateSearchFrom = "DateSearchFrom";
            public const string DateSearchTo = "DateSearchTo";
            public const string CodeSearch = "CodeSearch";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
}
