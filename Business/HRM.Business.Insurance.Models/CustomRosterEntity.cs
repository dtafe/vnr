using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    public class CustomRosterEntity
    {
        public Guid ProfileID { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public Guid? MonShiftID { get; set; }
        public Guid? TueShiftID { get; set; }
        public Guid? WedShiftID { get; set; }
        public Guid? ThuShiftID { get; set; }
        public Guid? FriShiftID { get; set; }
        public Guid? SatShiftID { get; set; }
        public Guid? SunShiftID { get; set; }
    }
}
