using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportHDTJobNotDateEndModel : BaseViewModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string OrgStructureIDs { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string Dept { get; set; }
        public string HDTJobTypeCode { get; set; }
        public string HDTJobTypeName { get; set; }
        public string HDTJobGroupName { get; set; }
        public string StatusView { get; set; }
        public Guid ExportId { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string ValueFields { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string OrgStructureName = "OrgStructureName";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string Dept = "Dept";
            public const string HDTJobTypeCode = "HDTJobTypeCode";
            public const string HDTJobTypeName = "HDTJobTypeName";
            public const string HDTJobGroupName = "HDTJobGroupName";
            public const string StatusView = "StatusView";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
}
