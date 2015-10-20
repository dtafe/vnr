using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportProfileNotContractEntity : HRMBaseModel
    {
        public DateTime? DateHireFrom { get; set; }
        public DateTime? DateHireTo { get; set; }
        public string ProfileName { get; set; }
        public Guid? ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string CodeAttendance { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureID { get; set; }
        public Guid? JobTitleID { get; set; }
        public string JobTitleName { get; set; }
        public Guid? PositionID { get; set; }
        public string PositionName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime? DateHire { get; set; }
        public DateTime? DateEndProbation { get; set; }
        public DateTime? DateUpdate { get; set; }
        public string CellPhone { get; set; }
        public string EmployeeTypeName { get; set; }
        public Guid? EmpTypeID { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

    }
}
