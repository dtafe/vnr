using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_UniformEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public Guid? ProfileID { get; set; }

        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [MaxLength(150)]
        public string ProfileName { get; set; }

        [MaxLength(150)]
        public string UniformName { get; set; }

        public DateTime? DateOfIssue { get; set; }

        public DateTime? DateExpire { get; set; }

        public DateTime? DateOfReIssue { get; set; }

        [MaxLength(1000)]
        public string Note { get; set; }

        public string Size { get; set; }

        public Guid? PositionID { get; set; }

        [MaxLength(150)]
        public string PositionName { get; set; }

        public Guid? JobTitleID { get; set; }

        [MaxLength(150)]
        public string JobTitleName { get; set; }

        public List<Guid?> OrgStructureID { get; set; }

        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

    }
}
