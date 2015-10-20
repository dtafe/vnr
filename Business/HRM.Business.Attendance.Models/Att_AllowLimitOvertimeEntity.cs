using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Attendance.Models
{
    public class Att_AllowLimitOvertimeEntity
    {
        public Guid ID { get; set; }
        public int TotalRow { get; set; }
        public Guid? ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? LevelAllowLimit { get; set; }
        public string Type { get; set; }

        public partial class FieldNames
        {
            public static string ID = "ID";
            public static string ProfileID = "ProfileID";
            public static string ProfileName = "ProfileName";
            public static string CodeEmp = "CodeEmp";
            public static string DateStart = "DateStart";
            public static string DateEnd = "DateEnd";
            public static string LevelAllowLimit = "LevelAllowLimit";
            public static string Type = "Type";
        }
    }
}
