using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatShiftMultiModel 
    {
        public Guid ID { get; set; }
        public string ShiftName { get; set; }
    }
    public class CatShiftModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Shift_StdWorkHours)]
        public Nullable<double> StdWorkHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftName)]
        [MaxLength(200)]
        public string ShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_Code)]
        [MaxLength(32)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_InTime)]
        public DateTime InTime { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Category_Shift_CoOut)]
        public double CoOut { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Category_Shift_CoBreakIn)]
        public double CoBreakIn { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Category_Shift_CoBreakOut)]
        public double CoBreakOut { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Category_Shift_MinIn)]
        public double MinIn { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Category_Shift_MaxOut)]
        public double MaxOut { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_IsNightShift)]
        public bool IsNightShift { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_IsTemporary)]
        public bool? IsTemporary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shift_IsDoubleShift)]
        public bool? IsDoubleShift { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Category_Shift_WorkHours)]
        public double? WorkHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftBreakType)]
        [MaxLength(50)]
        public string ShiftBreakType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_NightTimeStart)]
        public DateTime? NightTimeStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_NightTimeEnd)]
        public DateTime? NightTimeEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_IsBreakAsWork)]
        public bool? IsBreakAsWork { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Category_Shift_ReduceNightShift)]
        public double? ReduceNightShift { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_IsNotApplyWorkHoursReal)]
        public bool? IsNotApplyWorkHoursReal { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Category_Shift_HourStartOvertime)]
        public double? HourStartOvertime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_FirstShiftID)]
        public Guid? FirstShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_LastShiftID)]
        public Guid? LastShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_CodeStatistic)]
        [MaxLength(100)]
        public string CodeStatistic { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_CoStartOTNightWithOutShift)]
        public DateTime? CoStartOTNightWithOutShift { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_CodeStatistic)]
        public DateTime? CoEndOTNightWithOutShift { get; set; }

          [DisplayName(ConstantDisplay.HRM_Category_Shift_TimeCoOut)]
        public DateTime? TimeCoOut { get; set; }

          [DisplayName(ConstantDisplay.HRM_Category_Shift_TimeCoBreakIn)]
        public DateTime? TimeCoBreakIn { get; set; }

          [DisplayName(ConstantDisplay.HRM_Category_Shift_TimeCoBreakOut)]
        public DateTime? TimeCoBreakOut { get; set; }
        public DateTime? TimeMinIn { get; set; }
        public DateTime? TimeMaxOut { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_FirstIn1)]
        public Nullable<System.DateTime> FirstIn1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_FirstIn2)]
        public Nullable<System.DateTime> FirstIn2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_FirstOut1)]
        public Nullable<System.DateTime> FirstOut1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_FirstOut2)]
        public Nullable<System.DateTime> FirstOut2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_LastIn1)]
        public Nullable<System.DateTime> LastIn1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_LastIn2)]
        public Nullable<System.DateTime> LastIn2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_LastOut1)]
        public Nullable<System.DateTime> LastOut1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_LastOut2)]
        public Nullable<System.DateTime> LastOut2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_InOutDynamic)]
        public Nullable<double> InOutDynamic { get; set; }


        private Guid _id = Guid.Empty;
        public Guid Shift_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }


        //[DisplayName(ConstantDisplay.HRM_Category_Shift_UserUpdate)]
        //public string UserUpdate { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Category_Shift_DateUpdate)]
        //public DateTime? DateUpdate { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ShiftName = "ShiftName";
            public const string Code = "Code";
            public const string InTime = "InTime";
            public const string CoOut = "CoOut";
            public const string CoBreakIn = "CoBreakIn";
            public const string CoBreakOut = "CoBreakOut";
            public const string MinIn = "MinIn";
            public const string MaxOut = "MaxOut";
            public const string IsNightShift = "IsNightShift";
            public const string IsTemporary = "IsTemporary";
            public const string WorkHours = "WorkHours";
            public const string StdWorkHours = "StdWorkHours";
            public const string ShiftBreakType = "ShiftBreakType";
            public const string NightTimeStart = "NightTimeStart";
            public const string NightTimeEnd = "NightTimeEnd";
            public const string IsBreakAsWork = "IsBreakAsWork";
            public const string ReduceNightShift = "ReduceNightShift";
            public const string IsNotApplyWorkHoursReal = "IsNotApplyWorkHoursReal";
            public const string HourStartOvertime = "HourStartOvertime";
            public const string FirstShiftID = "FirstShiftID";
            public const string LastShiftID = "LastShiftID";
            public const string CodeStatistic = "CodeStatistic";
            public const string UserUpdate = "UserUpdate";
            public const string TimeCoBreakIn = "TimeCoBreakIn";
            public const string TimeCoBreakOut = "TimeCoBreakOut";
            public const string TimeCoOut = "TimeCoOut";
            public const string DateUpdate = "DateUpdate";
            
        }

    }
    public class CatShiftSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftName)]
        [MaxLength(200)]
        public string ShiftName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_Code)]
        public string Code { get; set; }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
