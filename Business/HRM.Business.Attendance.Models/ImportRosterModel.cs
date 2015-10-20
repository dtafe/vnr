using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class ImportRosterModel
    {
        public Guid ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string CodeOrg { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string MonShift { get; set; }
        public string TueShift { get; set; }
        public string WedShift { get; set; }
        public string ThuShift { get; set; }
        public string FriShift { get; set; }
        public string SatShift { get; set; }
        public string SunShift { get; set; }
        public Guid? MonShiftID { get; set; }
        public Guid? TueShiftID { get; set; }
        public Guid? WedShiftID { get; set; }
        public Guid? ThuShiftID { get; set; }
        public Guid? FriShiftID { get; set; }
        public Guid? SatShiftID { get; set; }
        public Guid? SunShiftID { get; set; }
        public DateTime? MonDate { get; set; }
        public DateTime? TueDate { get; set; }
        public DateTime? WedDate { get; set; }
        public DateTime? ThuDate { get; set; }
        public DateTime? FriDate { get; set; }
        public DateTime? SatDate { get; set; }
        public DateTime? SunDate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public partial class FieldNames
        {
            public const string MonShift = "MonShift";
            public const string TueShift = "TueShift";
            public const string WedShift = "WedShift";
            public const string ThuShift = "ThuShift";
            public const string FriShift = "FriShift";
            public const string SatShift = "SatShift";
            public const string SunShift = "SunShift";
            public const string Description = "Description";
            public const string ProfileName = "ProfileName";
            public const string CodeOrg = "CodeOrg";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
        }
    }
}
