using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace HRM.Business.Finance.Models
{
    public class FIN_HistoryApproveECLAIMEntity : HRMBaseModel
    {       
        public Guid? ProfileID { get; set; }
        public string ApprovedType { get; set; }
        public Guid? CashAdvanceID { get; set; }
        public Guid? ClaimID     { get; set; }
        public Guid? TravelRequestID { get; set; }
        public string TravelRequestName { get; set; }
        public DateTime? DateApproved { get; set; }
        public Guid? UserApproveClaimID { get; set; }
        public int? OrderNo { get; set; }
        public string Note { get; set; }
        public List<Guid> ApprovedIDs { get; set; }
        public string ProfileName { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public string ApprovedName { get; set; }
        public string ApprovedTypeView { get; set; }
        public string UserApproveClaimName { get; set; }
    }
   
}
