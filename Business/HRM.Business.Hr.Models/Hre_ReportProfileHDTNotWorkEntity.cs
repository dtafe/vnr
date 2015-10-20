using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportProfileHDTNotWorkEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public string OrgStructureIDs { get; set; }
        public Guid? ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string HDTJobGroupCode { get; set; }
        public string HDTJobTypeCode { get; set; }
        public double? Price { get; set; }
        public DateTime? FirstIntime { get; set; }
        public DateTime? LastOutTime { get; set; }
        public string LeaveDayTypeCode { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }



        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string OrgStructureIDs = "OrgStructureIDs";
            public const string ProfileName = "ProfileName";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string HDTJobGroupCode = "HDTJobGroupCode";
            public const string HDTJobTypeCode = "HDTJobTypeCode";
            public const string Price = "Price";
            public const string FirstIntime = "FirstIntime";
            public const string LastOutTime = "LastOutTime";
            public const string LeaveDayTypeCode = "LeaveDayTypeCode";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
}
