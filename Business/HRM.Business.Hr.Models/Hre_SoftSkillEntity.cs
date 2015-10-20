using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_SoftSkillEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public Guid ProfileID { get; set; }
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [MaxLength(1000)]
        public string TrainingPlace { get; set; }
        [MaxLength(150)]
        public string Certificate { get; set; }
        [MaxLength(150)]
        public string SoftSkillName { get; set; }
        [MaxLength(150)]
        public string SoftSkillType { get; set; }
        [MaxLength(50)]
        public string SoftSkillLevel { get; set; }
        public DateTime? TrainingFrom { get; set; }
        public DateTime? TrainingTo { get; set; }
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [MaxLength(150)]
        public string PositionName { get; set; }
        public Guid? PositionID { get; set; }
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        public Guid? JobTitleID { get; set; }
        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        public Guid? OrgStructureID { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

    }
}
