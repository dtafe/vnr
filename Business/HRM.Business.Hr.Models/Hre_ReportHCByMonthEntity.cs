using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportHCByMonthEntity : HRMBaseModel
    {
        public string Division { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Position { get; set; }
        public int? FirstOfMonth { get; set; }
        public int? NewHiring { get; set; }
        public int? Resign { get; set; }
        public int? Transfer { get; set; }
        public int? Join { get; set; }
        public int? TotalEmployeeLastMonth { get; set; }
        public string HeadcountBudget { get; set; }
        public string Vacancy { get; set; }
        public Guid ExportID { get; set; }
        public double? AttritionRate { get; set; }
        public string Note { get; set; }
        public string Level { get; set; }

        public partial class FieldNames
        {
            public const string Division = "Division";
            public const string Department = "Department";
            public const string Section = "Section";
            public const string Position = "Position";
            public const string FirstOfMonth = "FirstOfMonth";
            public const string NewHiring = "NewHiring";
            public const string Resign = "Resign";
            public const string Transfer = "Transfer";
            public const string Join = "Join";
            public const string TotalEmployeeLastMonth = "TotalEmployeeLastMonth";
            public const string HeadcountBudget = "HeadcountBudget";
            public const string Vacancy = "Vacancy";
            public const string ExportID = "ExportID";
            public const string AttritionRate = "AttritionRate";
            public const string Note = "Note";
            public const string Level = "Level";
        }
    }
}
