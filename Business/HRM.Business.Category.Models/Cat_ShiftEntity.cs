using System;

namespace HRM.Business.Category.Models
{
    public class Cat_ShiftMultiEntity 
    {
        public Guid ID { get; set; }
        public string ShiftName { get; set; }
    }
    public class Cat_ShiftEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string ShiftName { get; set; }
        public double CoBreakIn { get; set; }
        public double CoBreakOut { get; set; }
        public string Code { get; set; }
        public string CodeStatistic { get; set; }
        public double CoOut { get; set; }
        public Nullable<System.Guid> FirstShiftID { get; set; }
        public Nullable<System.Guid> LastShiftID { get; set; }
        public DateTime InTime { get; set; }
        public double MaxOut { get; set; }
        public double MinIn { get; set; }
        public Nullable<System.DateTime> CoStartOTNightWithOutShift { get; set; }
        public Nullable<System.DateTime> CoEndOTNightWithOutShift { get; set; }
        public Nullable<bool> IsBreakAsWork { get; set; }
        public Nullable<double> ReduceNightShift { get; set; }
        public Nullable<bool> IsNotApplyWorkHoursReal { get; set; }
        public Nullable<double> HourStartOvertime { get; set; }
        public Nullable<bool> IsDoubleShift { get; set; }
        public bool IsNightShift { get; set; }
        public bool IsTemporary { get; set; }
        public Nullable<double> WorkHours { get; set; }
        public Nullable<double> StdWorkHours { get; set; }
        public string ShiftBreakType { get; set; }
        public Nullable<System.DateTime> NightTimeStart { get; set; }
        public Nullable<System.DateTime> NightTimeEnd { get; set; }
        public DateTime? TimeCoOut { get; set; }
        public DateTime? TimeCoBreakIn { get; set; }
        public DateTime? TimeCoBreakOut { get; set; }

        public Nullable<System.Guid> ShopGroupID { get; set; }

        public Nullable<System.DateTime> FirstIn1 { get; set; }
        public Nullable<System.DateTime> FirstIn2 { get; set; }
        public Nullable<System.DateTime> FirstOut1 { get; set; }
        public Nullable<System.DateTime> FirstOut2 { get; set; }
        public Nullable<System.DateTime> LastIn1 { get; set; }
        public Nullable<System.DateTime> LastIn2 { get; set; }
        public Nullable<System.DateTime> LastOut1 { get; set; }
        public Nullable<System.DateTime> LastOut2 { get; set; }
        public Nullable<double> InOutDynamic { get; set; }
        public double? udAvailableHours { get; set; }



        //public string UserUpdate { get; set; }
        //public DateTime? DateUpdate { get; set; }
        //public int TotalRow { get; set; }

        //public string ShiftName { get; set; }
        //public string Code { get; set; }
        //public System.DateTime InTime { get; set; }
        //public double? CoOut { get; set; }
        //public double? CoBreakIn { get; set; }
        //public double? CoBreakOut { get; set; }
        //public double? MinIn { get; set; }
        //public double? MaxOut { get; set; }
        //public bool IsNightShift { get; set; }
        //public bool IsTemporary { get; set; }
        //public Nullable<double> WorkHours { get; set; }
        //public string ShiftBreakType { get; set; }
        //public Nullable<System.DateTime> NightTimeStart { get; set; }
        //public Nullable<System.DateTime> NightTimeEnd { get; set; }
        //public Nullable<bool> IsBreakAsWork { get; set; }
        //public Nullable<double> ReduceNightShift { get; set; }
        //public Nullable<bool> IsNotApplyWorkHoursReal { get; set; }
        //public Nullable<double> HourStartOvertime { get; set; }
        //public Nullable<bool> IsDoubleShift { get; set; }
        //public Nullable<System.Guid> LastShiftID { get; set; }
    }
}
