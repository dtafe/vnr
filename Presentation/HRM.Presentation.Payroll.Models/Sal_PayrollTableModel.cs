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
    public class Sal_PayrollTableModel : BaseViewModel
    {
        public System.Guid ProfileID { get; set; }
        public System.DateTime MonthYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_CutOffDurationID)]
        public Nullable<System.Guid> CutOffDurationID { get; set; }
        public string Status { get; set; }
        public Nullable<double> BasicSalary { get; set; }
        public Nullable<double> GrossSalary { get; set; }
        public Nullable<double> HourlyRate { get; set; }
        public Nullable<double> PayableWorkDays { get; set; }
        public Nullable<double> WorkingSalary { get; set; }
        public Nullable<double> OvertimeSalary { get; set; }
        public Nullable<double> Allowance { get; set; }
        public Nullable<double> Deduction { get; set; }
        public Nullable<double> OtherEarning { get; set; }
        public Nullable<double> OtherDeduction { get; set; }
        public Nullable<double> AmountInsUnemployCom { get; set; }
        public Nullable<double> AmountInsUnemployEmp { get; set; }
        public Nullable<double> AmountInsHealthCom { get; set; }
        public Nullable<double> AmountInsHealthEmp { get; set; }
        public Nullable<double> AmountInsSocialCom { get; set; }
        public Nullable<double> AmountInsSocialEmp { get; set; }
        public double IncomeBeforeTax { get; set; }
        public int DependantCount { get; set; }
        public double IncomeTaxable { get; set; }
        public double AmountPaidPITEmp { get; set; }
        public double AmountPaidPITCom { get; set; }
        public double IncomeNET { get; set; }

        [DisplayName(ConstantDisplay.HRM_Sal_Grade_OrgStructureID)]
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public Nullable<System.Guid> JobTitleID { get; set; }
        public Nullable<System.Guid> EmployeeTypeID { get; set; }
        public Nullable<System.Guid> PayrollGroupID { get; set; }
        public Nullable<System.Guid> CostCentreID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileName)]
        public string ProfileIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_CutOffDurationID_IsWorking)]
        public bool isIncludeWorkingEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_CutOffDurationID_PaymentQuit)]
        public bool PaymentQuit { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Settlement)]
        public int? Settlement { get; set; }
        public Nullable<System.Guid> BankID { get; set; }
        public string AccountNo { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_LateEarlyRule_GradeName)]
        public string GradePayrollID { get; set; }
         public string CostCentreNamePayrollTable { get; set; }
    }
}
