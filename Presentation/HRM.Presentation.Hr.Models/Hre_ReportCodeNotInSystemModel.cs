using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportCodeNotInSystemModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_ReportCodeNotInSystem_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportCodeNotInSystem_Time)]
        public DateTime? Time { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportCodeNotInSystem_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportCodeNotInSystem_Machine)]
        public string Machine { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateTo)]
        public DateTime? DateTo { get; set; }
        public Guid ExportID { get; set; }
        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string Time = "Time";
            public const string Type = "Type";
            public const string Machine = "Machine";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string DateCreate = "DateCreate";
            public const string UserCreate = "UserCreate";
        }
    }
}
