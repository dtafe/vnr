using System;


namespace HRM.Business.Payroll.Models
{
    public class Sal_SalaryDepartmentItemEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public System.Guid SalaryDepartmentID { get; set; }
        public System.Guid ProfileID { get; set; }
        public double PaidWorkHours { get; set; }
        public double Rate { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public Nullable<System.Guid> Overtime1TypeID { get; set; }
        public Nullable<double> Overtime1Hours { get; set; }
        public Nullable<System.Guid> Overtime2TypeID { get; set; }
        public Nullable<double> Overtime2Hours { get; set; }
        public Nullable<System.Guid> Overtime3TypeID { get; set; }
        public Nullable<double> Overtime3Hours { get; set; }
        public Nullable<System.Guid> Overtime4TypeID { get; set; }
        public Nullable<double> Overtime4Hours { get; set; }
        public Nullable<System.Guid> Overtime5TypeID { get; set; }
        public Nullable<double> Overtime5Hours { get; set; }
        public Nullable<System.Guid> Overtime6TypeID { get; set; }
        public Nullable<double> Overtime6Hours { get; set; }
        public Nullable<double> AmoutSalary { get; set; }
        public string Status { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string DepartmentName { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string OvertimeTypeName { get; set; }
        public string OvertimeTypeName1 { get; set; }
        public string OvertimeTypeName2 { get; set; }
        public string OvertimeTypeName3 { get; set; }
        public string OvertimeTypeName4 { get; set; }
        public string OvertimeTypeName5 { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<double> OT1AmountSalary { get; set; }
        public Nullable<double> OT2AmountSalary { get; set; }
        public Nullable<double> OT3AmountSalary { get; set; }
        public Nullable<double> OT4AmountSalary { get; set; }
        public Nullable<double> OT5AmountSalary { get; set; }
        public Nullable<double> StdAmountSalary { get; set; }
        public Nullable<double> DayAmountSalary { get; set; }
        public Nullable<bool> IsNightShift { get; set; }
        public Nullable<double> NightShiftHours { get; set; }
        public Nullable<double> NightAmountSalary { get; set; }

    }

 
}

