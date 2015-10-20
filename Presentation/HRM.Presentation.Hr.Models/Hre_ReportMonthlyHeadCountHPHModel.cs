using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportMonthlyHeadCountHPHModel : BaseViewModel
    {
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }
        public string PositionName { get; set; }
        public int? FirstOfMonth { get; set; }
        public int? NewHiring { get; set; }
        public int? Resign { get; set; }
        public int? TransferredToOther { get; set; }
        public int? JoinedTheTeam { get; set; }
        public int? TotalEmployees { get; set; }
        public int? AttritionRate { get; set; }
        public int? HeadcountBudget { get; set; }
        public string Vacancy { get; set; }
        public string Note { get; set; }
        public string Level { get; set; }
        public partial class FieldNames
        {
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";
            public const string PositionName = "PositionName";
            public const string FirstOfMonth = "FirstOfMonth";
            public const string NewHiring = "NewHiring";
            public const string Resign = "Resign";
            public const string TransferredToOther = "TransferredToOther";
            public const string JoinedTheTeam = "JoinedTheTeam";
            public const string TotalEmployees = "TotalEmployees";
            public const string AttritionRate = "AttritionRate";
            public const string HeadcountBudget = "HeadcountBudget";
            public const string Vacancy = "Vacancy";
            public const string Note = "Note";
            public const string Level = "Level";
        }
    }
    public class Hre_ReportMonthlyHeadCountHPHModelSearch
    {
        public DateTime MonthYear { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

    }
}
