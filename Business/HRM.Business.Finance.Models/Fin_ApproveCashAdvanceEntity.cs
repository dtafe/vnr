using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRM.Business.Finance.Models
{
    public class Fin_ApproveCashAdvanceEntity : HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.Guid> TravelRequestID { get; set; }
        public Nullable<bool> IsEntertaiment { get; set; }
        public string CashAdvanceName { get; set; }
        public string Code { get; set; }
        public string Other { get; set; }
        public string ProfileName { get; set; }

        public string CodeEmp { get; set; }
        public string TravelRequestName { get; set; }
        public string Status { get; set; }

        public string Area { get; set; }
        public string Region { get; set; }
        public string Channel { get; set; }
        public string WorkPlaceName { get; set; }
        public string JobTitle { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureCode { get; set; }
        public string SupervisorName { get; set; }
        public string JobTitleName { get; set; }
        

    }
  
}
