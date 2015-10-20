using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    /// <summary>Quyết toán bảo hiểm</summary>
    public class Ins_InsuranceSettlementEntity : HRMBaseModel
    {
        public Guid ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public bool? IsSettlement { get; set; }
        public string InsuranceSettlementType { get; set; }
        public string ReceiveSocialInsType { get; set; }
        public bool? IsReceiveSocialIns { get; set; }     
        public DateTime? DateHire { get; set; }
        public DateTime? DateQuit { get; set; }
        public Guid? OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public int TotalRow { get; set; }
        public string ProfileIds { get; set; }
        public string OrgStructureIDs { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


    }
   
}
