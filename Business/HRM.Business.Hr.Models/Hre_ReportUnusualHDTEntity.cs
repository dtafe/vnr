using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportUnusualHDTEntity : HRMBaseModel
    {
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string Dept { get; set; }
        public string HDTJobTypeCode { get; set; }
        public string HDTJobTypeName { get; set; }
        public double? Price { get; set; }
        public string TimeScan { get; set; }
        public double? PriceRecieve { get; set; }
        // không đk nhưng nhận
        public bool? NotRegister { get; set; }
        // có đk nhưng nhận
        public bool? HaveRegister { get; set; }
        // nhận sai
        public bool? RevieveWrong { get; set; }
        // Đk Search
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string OrgStructureIDs { get; set; }
        public Guid? ProfileID { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }



        public partial class FieldNames
        {
            public const string Dept = "Dept";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string HDTJobTypeCode = "HDTJobTypeCode";
            public const string HDTJobTypeName = "HDTJobTypeName";
            public const string Price = "Price";
            public const string TimeScan = "TimeScan";
            public const string PriceRecieve = "PriceRecieve";
            public const string NotRegister = "NotRegister";
            public const string HaveRegister = "HaveRegister";
            public const string RevieveWrong = "RevieveWrong";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
}
