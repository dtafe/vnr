using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportDependantProfileQuitModel : BaseViewModel
    {
        public string OrgStructureID { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public DateTime? DateQuitFrom { get; set; }
        public DateTime? DateQuitTo { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string DependantName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateQuit { get; set; }
        public DateTime? MonthOfEffect { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string ValueFields { get; set; }
        
        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string DependantName = "DependantName";
            public const string DateOfBirth = "DateOfBirth";
            public const string DateQuit = "DateQuit";
            public const string MonthOfEffect = "MonthOfEffect";
        }
    }
}
