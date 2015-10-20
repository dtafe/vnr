using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_PayrollTableEntity : HRMBaseModel
    {
        public System.Guid ProfileID { get; set; }
        public System.DateTime MonthYear { get; set; }
        public Nullable<System.Guid> CutOffDurationID { get; set; }
        public string CutOffDurationName { get; set; }

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
        public bool? IsPaid { get; set; }
        public int? OrderOrg { get; set; }
        public double IncomeBeforeTax { get; set; }
        public int DependantCount { get; set; }
        public double IncomeTaxable { get; set; }
        public double AmountPaidPITEmp { get; set; }
        public double AmountPaidPITCom { get; set; }
        public double IncomeNET { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public Nullable<System.Guid> JobTitleID { get; set; }
        public Nullable<System.Guid> EmployeeTypeID { get; set; }
        public Nullable<System.Guid> PayrollGroupID { get; set; }
        public Nullable<System.Guid> CostCentreID { get; set; }
        public string Value { get; set; }
        public string ElementType { get; set; }
        public Nullable<System.Guid> BankID { get; set; }
        public string AccountNo { get; set; }
        public string CostCentreNamePayrollTable { get; set; }

    }
}
