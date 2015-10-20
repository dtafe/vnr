using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_JobTitleEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string JobTitleName { get; set; }

        public string JobTitleCode { get; set; }
        public string Code { get; set; }
       
        public string JobTitleNameEn { get; set; }
      
        public Guid? OrgStructureID { get; set; }
      
        public string OrgStructureName { get; set; }
       
        public Guid? CostCentreID { get; set; }
      
        public string CostCentreName { get; set; }
      
        //public string JobTitleCode { get; set; }
    
        public string JobDescription { get; set; }
    
        public string Notes { get; set; }
       
        public double? SalaryMax { get; set; }
      
        public double? SalaryMin { get; set; }  
     
        public Guid? EmployeeTypeID { get; set; }
      
        public string EmployeeTypeName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

    }
    public class Cat_JobTitleMultiEntity
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string JobTitleName { get; set; }
    }
}
