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
    public class CatShiftModel:BaseViewModel
    {

       [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftName)]
        [MaxLength(200)]
        public string ShiftName { get; set; }

       [DisplayName(ConstantDisplay.HRM_Category_Shift_Code)]
        [MaxLength(32)]
        public string Code { get; set; }

       [DisplayName(ConstantDisplay.HRM_Category_Shift_InTime)]
       public DateTime? InTime { get; set; }

       [DisplayName(ConstantDisplay.HRM_Category_Shift_CoOut)]
       public float? CoOut { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_CoBreakIn)]
       public float? CoBreakIn { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_CoBreakOut)]
        public float? CoBreakOut { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_MinIn)]
        public float? MinIn { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_MaxOut)]
        public float? MaxOut { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_IsNightShift)]
        public bool? IsNightShift { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_IsTemporary)]
        public bool? IsTemporary { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_WorkHours)]
        public float? WorkHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftBreakType)]
        [MaxLength(50)]
        public string ShiftBreakType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_NightTimeStart)]
        public DateTime? NightTimeStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_NightTimeEnd)]
        public DateTime? NightTimeEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_IsBreakAsWork)]
        public bool? IsBreakAsWork { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_ReduceNightShift)]
        public float? ReduceNightShift { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_IsNotApplyWorkHoursReal)]
        public bool? IsNotApplyWorkHoursReal { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_HourStartOvertime)]
        public float? HourStartOvertime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_FirstShiftID)]
        public int? FirstShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_LastShiftID)]
        public int? LastShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_CodeStatistic)]
        [MaxLength(100)]
        public string CodeStatistic { get; set; }
    }
}
