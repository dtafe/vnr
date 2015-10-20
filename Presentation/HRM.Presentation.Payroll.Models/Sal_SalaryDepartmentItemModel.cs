using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_SalaryDepartmentItemModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_DepartmentID)]
        public System.Guid SalaryDepartmentID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_ProfileID)]
        public System.Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_PaidWorkHours)]
        public double PaidWorkHours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Rate)]
        public double Rate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_DateFrom)]
        public Nullable<System.DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_DateTo)]
        public Nullable<System.DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime1TypeID)]
        public Nullable<System.Guid> Overtime1TypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime1Hours)]
        public Nullable<double> Overtime1Hours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime2TypeID)]
        public Nullable<System.Guid> Overtime2TypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime2Hours)]
        public Nullable<double> Overtime2Hours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime3TypeID)]
        public Nullable<System.Guid> Overtime3TypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime3Hours)]
        public Nullable<double> Overtime3Hours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime4TypeID)]
        public Nullable<System.Guid> Overtime4TypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime4Hours)]
        public Nullable<double> Overtime4Hours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime5TypeID)]
        public Nullable<System.Guid> Overtime5TypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime5Hours)]
        public Nullable<double> Overtime5Hours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime6TypeID)]
        public Nullable<System.Guid> Overtime6TypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime6Hours)]
        public Nullable<double> Overtime6Hours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_AmoutSalary)]
        public Nullable<double> AmoutSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_CurrencyName)]
        public Nullable<System.Guid> CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime1TypeID)]
        public string OvertimeTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime2TypeID)]
        public string OvertimeTypeName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime3TypeID)]
        public string OvertimeTypeName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime4TypeID)]
        public string OvertimeTypeName3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime5TypeID)]
        public string OvertimeTypeName4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_Overtime6TypeID)]
        public string OvertimeTypeName5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_CurrencyName)]
        public string CurrencyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_SalaryDepartmentItem_DepartmentID)]
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
        public string DepartmentName { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string PaidWorkHours = "PaidWorkHours";
            public const string Rate = "Rate";
            public const string CodeEmp = "CodeEmp";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string Overtime1TypeID = "Overtime1TypeID";
            public const string Overtime1Hours = "Overtime1Hours";
            public const string Overtime2TypeID = "Overtime2TypeID";
            public const string Overtime2Hours = "Overtime2Hours";
            public const string Overtime3TypeID = "Overtime3TypeID";
            public const string Overtime3Hours = "Overtime3Hours";
            public const string Overtime4TypeID = "Overtime4TypeID";
            public const string Overtime4Hours = "Overtime4Hours";
            public const string Overtime5TypeID = "Overtime5TypeID";
            public const string Overtime5Hours = "Overtime5Hours";
            public const string Overtime6TypeID = "Overtime6TypeID";
            public const string Overtime6Hours = "Overtime6Hours";
            public const string AmoutSalary = "AmoutSalary";
            public const string OvertimeTypeName = "OvertimeTypeName";
            public const string OvertimeTypeName1 = "OvertimeTypeName1";
            public const string OvertimeTypeName2 = "OvertimeTypeName2";
            public const string OvertimeTypeName3 = "OvertimeTypeName3";
            public const string OvertimeTypeName4 = "OvertimeTypeName4";
            public const string OvertimeTypeName5 = "OvertimeTypeName5";
            public const string CurrencyName = "CurrencyName";
            public const string DepartmentName = "DepartmentName";
            public const string Status = "Status";
        }
    }

}
