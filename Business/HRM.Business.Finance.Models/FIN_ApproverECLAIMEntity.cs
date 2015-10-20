using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace HRM.Business.Finance.Models
{
    public class FIN_ApproverECLAIMEntity : HRMBaseModel
    {       
        public Guid? ProfileID { get; set; }
        public string ApprovedType { get; set; }
        public Guid? ApprovedID { get; set; }
        public Guid? UserApproveClaimID { get; set; }
        public int? OrderNo { get; set; }
        public string Note { get; set; }
        public List<Guid> ApprovedIDs { get; set; }

        public string ProfileName { get; set; }
        public string ApprovedName { get; set; }
        public string ApprovedTypeView { get; set; }
    }
   
}
