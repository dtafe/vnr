using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportDetailShiftEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string GroupCode { get; set; }
        public string SectionCode { get; set; }
        public string DepartmentName { get; set; }
        public string DeptPath { get; set; }
        public DateTime? WorkDate { get; set; }
        public Guid? ShiftId { get; set; }
        public string ShiftName { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string Remark { get; set; }
        public string CodeAttendance { get; set; }
        public string LaborType { get; set; }
        public string EmployeeType { get; set; }
        public string Supervisor { get; set; }
        public string HighSupervisor { get; set; }
        public string WorkPlace { get; set; }
        public string WorkingPlace { get; set; }
        public string IDNo { get; set; }
        public DateTime? MonthYear { get; set; }
        public DateTime? DateHire { get; set; }
        public DateTime? DateQuit { get; set; }
        public DateTime? DateEndProbation { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Jobtitle { get; set; }
        public string BankAccountNo { get; set; }
        public string AccountNo { get; set; }
        public string AccountNo2 { get; set; }
        public string bankCode { get; set; }
        public string BankName { get; set; }
        public string SalaryRankName { get; set; }
        public string TaxCode { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterCode { get; set; }
        public string PayrollGroup { get; set; }
        public string SocialInsNo { get; set; }
        public List<int?> OrgStructureID { get; set; }
        public string UserExport { get; set; }
        public string PositionName { get; set; }
        public string JobtitleName { get; set; }
        public partial class FieldNames
        {

            public const string Date = "Date";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string PositionName = "PositionName";
            public const string JobtitleName = "JobtitleName";
            public const string Division = "Division";
            public const string Department = "Department";
            public const string SectionCode = "SectionCode";
            public const string TeamCode = "TeamCode";
            public const string ShiftName = "ShiftName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
            public const string OrgStructureName = "OrgStructureName";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string Remark = "Remark";
            public const string GroupCode = "GroupCode";
            public const string BranchCode = "BranchCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string DateExport = "DateExport";
            public const string WorkDate = "WorkDate";
            public const string DeptPath = "DeptPath";
            public const string MonthYear = "MonthYear";
            public const string DateHire = "DateHire";
            public const string DateQuit = "DateQuit";
            public const string DateEndProbation = "DateEndProbation";
            public const string SocialInsNo = "SocialInsNo";
            public const string PayrollGroup = "PayrollGroup";
            public const string CostCenter = "CostCenter";
            public const string CostCenterCode = "CostCenterCode";
            public const string TaxCode = "TaxCode";
            public const string EmployeeType = "EmployeeType";
            public const string LaborType = "LaborType";
            public const string CodeAttendance = "CodeAttendance";
            public const string Email = "Email";
            public const string WorkingPlace = "WorkingPlace";
            public const string WorkPlace = "WorkPlace";
            public const string Supervisor = "Supervisor";
            public const string HighSupervisor = "HighSupervisor";
            public const string IDNo = "IDNo";
            public const string SalaryRankName = "IDNo";
            public const string BankAccountNo = "BankAccountNo";
            public const string AccountNo = "AccountNo";
            public const string AccountNo2 = "AccountNo2";
            public const string BankName = "BankName";
            public const string bankCode = "bankCode";
            public const string UserExport = "UserExport";
            
        }
    }
}
