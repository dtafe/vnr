using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_RptExceptionDataEntity
    {
        public Guid ID { get; set; }
        public Guid InOutID { get; set; }
        public Guid ShiftID { get; set; }
        public Guid ProfileID { get; set; }
        public String CodeEmp { get; set; }
        public String ProfileName { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public string EntityType { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string DataType { get; set; }
        public string Exception { get; set; }
        public string Description { get; set; }
        public DateTime DateExport { get; set; }
        public string UserExport { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string InOutID = "InOutID";
            public const string ShiftID = "ShiftID";
            public const string ProfileID = "ProfileID";
            public const string EntityType = "EntityType";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Position = "Position";
            public const string Department = "Department";
            public const string Section = "Section";
            public const string Date = "Date";
            public const string DataType = "DataType";
            public const string Exception = "Exception";
            public const string Description = "Description";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
        }
    }
}
