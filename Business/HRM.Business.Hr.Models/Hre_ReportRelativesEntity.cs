using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportRelativesEntity : HRMBaseModel
    {
        public string OrgStructureIDs { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public string RelativeTypeName { get; set; }
        public string RelativeName { get; set; }
        public string YearOfBirth { get; set; }
        public string Address { get; set; }
        public Guid? JobTitleID { get; set; }
        public Guid? ProfileID { get; set; }
        public string JobTitleName { get; set; }
        public Guid? PositionID { get; set; }
        public string PositionName { get; set; }
        public Guid? RelativeTypeID { get; set; }




        public Nullable<System.Guid> RankID { get; set; }
   
        public Nullable<System.Guid> WorkPlaceID { get; set; }


    
        public DateTime? RelativesOfBirth { get; set; }

    
        public string SalaryClassName { get; set; }
        public string WorkPlaceName { get; set; }
    }
}
