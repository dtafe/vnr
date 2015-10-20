using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_CardHistoryEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public Guid? ProfileID { get; set; }
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [MaxLength(50)]
        public string CardCode { get; set; }
        public DateTime? DateEffect { get; set; }
        [MaxLength(150)]
        public string PositionName { get; set; }
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

    }
}
