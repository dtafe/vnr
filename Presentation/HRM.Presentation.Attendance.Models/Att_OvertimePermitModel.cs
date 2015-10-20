using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_OvertimePermitModel 
    {
        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_limitHour_ByDay)]
        public double? limitHour_ByDay { get; set; }
        public double? limitHour_ByDay_Lev1 { get; set; }
        public double? limitHour_ByDay_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_limitHour_ByWeek)]
        public double? limitHour_ByWeek { get; set; }
        public double? limitHour_ByWeek_Lev1 { get; set; }
        public double? limitHour_ByWeek_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_limitHour_ByMonth)]
        public double? limitHour_ByMonth { get; set; }
        public double? limitHour_ByMonth_Lev1 { get; set; }
        public double? limitHour_ByMonth_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_limitHour_ByYear)]
        public double? limitHour_ByYear { get; set; }
        public double? limitHour_ByYear_Lev1 { get; set; }
        public double? limitHour_ByYear_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_limitColor)]
        public string limitColor { get; set; }
        public string limitColor_Lev1 { get; set; }
        public string limitColor_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_IsAllowOverLimit_Normal)]
        public bool? IsAllowOverLimit_Normal { get; set; } //bật cờ này lên thì mới áp dụng những bình thường vượt trần 
        public bool? IsAllowOverLimit_Normal_Lev1 { get; set; }
        public bool? IsAllowOverLimit_Normal_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_IsAllowOverLimit_AllowOver)]
        public bool? IsAllowOverLimit_AllowOver { get; set; } //bật cờ này lên thì mới áp dụng những có trong ds cho phép vượt trần vượt trần 
        public bool? IsAllowOverLimit_AllowOver_Lev1 { get; set; }
        public bool? IsAllowOverLimit_AllowOver_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_IsAllowSplit)]
        public bool? IsAllowSplit { get; set; }//bật cờ này lên thì mới áp dụng cắt giờ làm việc theo ngày 
    }
}
