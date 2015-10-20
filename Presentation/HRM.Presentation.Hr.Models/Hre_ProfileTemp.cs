using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ProfileTemp : BaseViewModel
    {
        public string ProfileName { get; set; }

        private string _ProfileIDs;
        public Guid? ProfileID { get; set; }
        public string OrgStructureIDs { get; set; }
        public string TempProfileIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile)]
        public string ProfileIDs
        {
            get
            {
                return _ProfileIDs;
            }
            set
            {
                _ProfileIDs = value;
            }
        }
        private Guid? _SalaryRankID;
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_SalaryRankID)]
        public Guid? SalaryRankID
        {
            get
            {
                return _SalaryRankID;
            }
            set
            {
                _SalaryRankID = value;
            }
        }
        private string _SalaryClassName;
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_SalaryClassName)]
        public string SalaryClassName
        {
            get
            {
                return _SalaryClassName;
            }
            set
            {
                _SalaryClassName = value;
            }
        }
        private string _BasicSalary;
        [DisplayName(ConstantDisplay.HR_BasicSalary)]
        public string BasicSalary
        {
            get
            {
                return _BasicSalary;
            }
            set
            {
                _BasicSalary = value;
            }
        }
        private string _AbilityTitle;

        [DisplayName(ConstantDisplay.HRM_Hre_ProfileTemp_AbilityTitle)]
        public string AbilityTitle
        {
            get
            {
                return _AbilityTitle;
            }
            set
            {
                _AbilityTitle = value;
            }
        }
        private string _Note;
        public string Note
        {
            get
            {
                return _Note;
            }
            set
            {
                _Note = value;
            }
        }
        private Guid? _ContractTypeID;
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractTypeID)]
        public Guid? ContractTypeID
        {
            get
            {
                return _ContractTypeID;
            }
            set
            {
                _ContractTypeID = value;
            }
        }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_DateOfEffect)]
        public DateTime? DateOfEffect { get; set; }
        private DateTime? _DateHire;
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_DateHire)]
        public DateTime? DateHire
        {
            get
            {
                return _DateHire;
            }
            set
            {
                _DateHire = value;
            }
        }
        private DateTime? _DateEndProbation;
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateEndProbation)]
        public DateTime? DateEndProbation
        {
            get
            {
                return _DateEndProbation;
            }
            set
            {
                _DateEndProbation = value;
            }
        }
        //    private Guid? _GradePayrollID;
        //[DisplayName(ConstantDisplay.HRM_Sal_Grade_GradePayrollID)]
        //    public Guid? GradePayrollID
        //    {
        //        get
        //        {
        //            return _GradePayrollID;
        //        }
        //        set
        //        {
        //            _GradePayrollID = value;
        //        }

        //    }

        private Guid? _GradePayrollID;
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_GradePayrollID)]
        public Guid? GradePayrollID
        {
            get
            {
                return _GradePayrollID;
            }
            set
            {
                _GradePayrollID = value;
            }
        }
        private Guid? _GradeAttendanceID;
        [DisplayName(ConstantDisplay.HRM_Att_Grade_GradeAttendanceID)]
        public Guid? GradeAttendanceID
        {
            get
            {
                return _GradeAttendanceID;
            }
            set
            {
                _GradeAttendanceID = value;
            }
        }
        private Guid? _OrgStructureID;
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public Guid? OrgStructureID
        {
            get
            {
                return _OrgStructureID;
            }
            set
            {
                _OrgStructureID = value;
            }
        }

        private Guid? _WorkPlaceID;
        [DisplayName(ConstantDisplay.HRM_HR_Contract_WorkPlaceID)]
        public Guid? WorkPlaceID
        {
            get
            {
                return _WorkPlaceID;
            }
            set
            {
                _WorkPlaceID = value;
            }
        }

        public int? OrderNumber { get; set; }
        public bool IsRank { get; set; }
        public bool IsRankDetail { get; set; }

        private double? _InsuranceSalary;
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_InsuranceAmount)]
        public double? InsuranceSalary
        {
            get
            {
                return _InsuranceSalary;
            }
            set
            {
                _InsuranceSalary = value;
            }
        }

        private Guid? _JobTitleID;
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_JobtitleID)]
        public Guid? JobTitleID
        {
            get
            {
                return _JobTitleID;
            }
            set
            {
                _JobTitleID = value;
            }
        }

        private Guid? _PositionID;
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_PositionID)]
        public Guid? PositionID
        {
            get
            {
                return _PositionID;
            }
            set
            {
                _PositionID = value;
            }
        }

        private Guid? _AllowanceID1;
        [DisplayName(ConstantDisplay.HRM_HR_ProfileTemp_AllowanceID1)]
        public Guid? AllowanceID1
        {
            get
            {
                return _AllowanceID1;
            }
            set
            {
                _AllowanceID1 = value;
            }
        }

        private double? _Allowance1;
        public double? Allowance1
        {
            get
            {
                return _Allowance1;
            }
            set
            {
                _Allowance1 = value;
            }
        }


        private Guid? _AllowanceID2;
        [DisplayName(ConstantDisplay.HRM_HR_ProfileTemp_AllowanceID2)]
        public Guid? AllowanceID2
        {
            get
            {
                return _AllowanceID2;
            }
            set
            {
                _AllowanceID2 = value;
            }
        }

        private double? _Allowance2;
        public double? Allowance2
        {
            get
            {
                return _Allowance2;
            }
            set
            {
                _Allowance2 = value;
            }
        }

        private Guid? _AllowanceID3;
        [DisplayName(ConstantDisplay.HRM_HR_ProfileTemp_AllowanceID3)]
        public Guid? AllowanceID3
        {
            get
            {
                return _AllowanceID3;
            }
            set
            {
                _AllowanceID3 = value;
            }
        }

        private double? _Allowance3;
        public double? Allowance3
        {
            get
            {
                return _Allowance3;
            }
            set
            {
                _Allowance3 = value;
            }
        }

        private Guid? _AllowanceID4;
        [DisplayName(ConstantDisplay.HRM_HR_ProfileTemp_AllowanceID4)]
        public Guid? AllowanceID4
        {
            get
            {
                return _AllowanceID4;
            }
            set
            {
                _AllowanceID4 = value;
            }
        }

        private double? _Allowance4;
        public double? Allowance4
        {
            get
            {
                return _Allowance4;
            }
            set
            {
                _Allowance4 = value;
            }
        }


        private Guid? _AllowanceID5;
        [DisplayName(ConstantDisplay.HRM_HR_ProfileTemp_AllowanceID5)]
        public Guid? AllowanceID5
        {
            get
            {
                return _AllowanceID5;
            }
            set
            {
                _AllowanceID5 = value;
            }
        }

        private double? _Allowance5;
        public double? Allowance5
        {
            get
            {
                return _Allowance5;
            }
            set
            {
                _Allowance5 = value;
            }
        }

        private Guid? _CurrencyID;
        [DisplayName(ConstantDisplay.HRM_Category_Grade_CurrencyID)]
        public Guid? CurrencyID
        {
            get
            {
                return _CurrencyID;
            }
            set
            {
                _CurrencyID = value;
            }
        }

        private string _CodeEmp;
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp
        {
            get
            {
                return _CodeEmp;
            }
            set
            {
                _CodeEmp = value;
            }
        }

    }
}
