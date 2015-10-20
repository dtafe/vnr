using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Attendance.Models
{
    public class CatDayOffModel:BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_DateOff)]
        public DateTime? DateOff { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_Type)]
        [MaxLength(50)]
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_Status)]
        [MaxLength(500)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_Comments)]
        [MaxLength(500)]
        public string Comments { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_OrgStructureID)]
        public int? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_IsLeaveDay)]
        public bool? IsLeaveDay { get; set; }
    }

}
