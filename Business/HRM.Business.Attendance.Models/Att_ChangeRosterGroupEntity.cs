using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ChangeRosterGroupEntity : HRMBaseModel
    {
        public string OrgStructureName { get; set; }
        public DateTime? Month { get; set; }
        public string DayOfMonth { get; set; }
        public string DayOfWeek { get; set; }

        
        public partial class FieldNames
        {
            public static string ID = "ID";
            public static string OrgStructureName = "OrgStructureName";
            public static string Month = "Month";
            public static string DayOfMonth = "DayOfMonth";
            public static string DayOfWeek = "DayOfWeek";

        }




    }

}
