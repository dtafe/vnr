using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportExpiryContractEntity : HRMBaseModel
    {
        public Nullable<System.DateTime> DateExtend { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureName { get; set; }
        public string Code { get; set; }
        public string CodeOSN { get; set; }
        public List<Guid?> OrgStructureID { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public string ContractTypeName { get; set; }
        public DateTime? DateSigned { get; set; }
        public DateTime? DateHire { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string NextContractName { get; set; }
        public DateTime? NextContractDateStart { get; set; }
        public DateTime? NextContractDateEnd { get; set; }
        public Guid? ContractTypeID { get; set; }
        public Guid ProfileID { get; set; }
        public string ContractResult { get; set; }
        public Guid? RankDetailForNextContract { get; set; }
        public string WorkPlaceName { get; set; }
        public Guid? SalaryClassID { get; set; }
        public string ClassRateName { get; set; }
        public Nullable<double> Salary { get; set; }
        public string ContractNo { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public string StatusEvaluation { get; set; }

        public double? MonthSenior { get; set; }
        // PC1
        public Nullable<System.Guid> AllowanceID1 { get; set; }
        public string AllowanceID1Name { get; set; }
        // Tiền PC1
        public Nullable<double> Allowance1 { get; set; }
        // Loại tiền tệ PC1
        public Nullable<System.Guid> CurenncyID2 { get; set; }
        public string CurenncyAllowance1Name { get; set; }
        //PC2
        public Nullable<System.Guid> AllowanceID2 { get; set; }
        public string AllowanceID2Name { get; set; }
        // Tiền PC2
        public Nullable<double> Allowance2 { get; set; }
        // Loại tiền tệ PC2
        public Nullable<System.Guid> CurenncyID3 { get; set; }
        public string CurenncyAllowance2Name { get; set; }
        // PC3
        public Nullable<System.Guid> AllowanceID3 { get; set; }
        public string AllowanceID3Name { get; set; }
        // Tiền PC3
        public Nullable<double> Allowance3 { get; set; }
        // Loại tiền tệ PC3
        public Nullable<System.Guid> CurenncyIDSalary { get; set; }
        public string CurenncyAllowance3Name { get; set; }
        //PC4
        public Nullable<System.Guid> AllowanceID4 { get; set; }
        public string AllowanceID4Name { get; set; }
        // Tiền PC4
        public Nullable<double> Allowance4 { get; set; }
        // Loại tiền tệ PC4
        public Nullable<System.Guid> CurenncyID4 { get; set; }
        public string CurenncyAllowance4Name { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public DateTime? DateEndNextContract { get; set; }
        public string TypeOfPass { get; set; }
        public string Remark { get; set; }
    }
}
