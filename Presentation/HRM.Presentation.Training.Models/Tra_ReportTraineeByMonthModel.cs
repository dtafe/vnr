﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_ReportTraineeByMonthModel : BaseViewModel
    {
        public Nullable<System.DateTime> Month { get; set; }
        public Guid? CourseID { get; set; }
        public string CourseName { get; set; }
        public string ProfileName { get; set; }
        public string SectionName { get; set; }
        public string CodeEmp { get; set; }
        public string ClassName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string Notes { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public Guid ExportId { get; set; }
        public string WorkPlace { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public partial class FieldNames
        {
            public const string WorkPlace = "WorkPlace";
            public const string Month = "Month";
            public const string CourseID = "CourseID";
            public const string CourseName = "CourseName";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string ClassName = "ClassName";
            public const string StartDate = "StartDate";
            public const string Notes = "Notes";
            public const string SectionName = "SectionName";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
}
