using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_ProfileQualificationEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public Guid? ProfileID { get; set; }

        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [MaxLength(150)]
        public string ProfileName { get; set; }

        public Guid? QualificationTypeID { get; set; }

        public Guid? QualifiTypeID { get; set; }

        [MaxLength(150)]
        public string FieldOfTraining { get; set; }

        [MaxLength(150)]
        public string CertificateName { get; set; }

        public DateTime? GraduationDate { get; set; }

        [MaxLength(150)]
        public string QualificationName { get; set; }

        [MaxLength(1000)]
        public string TrainingPlace { get; set; }

        [MaxLength(1000)]
        public string TrainingAddress { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateFinish { get; set; }

        [MaxLength(150)]
        public string Rank { get; set; }

        public Guid? SpecialLevelID { get; set; }

        public string Notes { get; set; }

        public Guid? PositionID { get; set; }

        [MaxLength(150)]
        public string PositionName { get; set; }

        public Guid? JobTitleID { get; set; }

        [MaxLength(150)]
        public string JobTitleName { get; set; }

        public List<Guid?> OrgStructureID { get; set; }

        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        [MaxLength(150)]
        public string NameEntityName { get; set; }

        private Guid _id = Guid.Empty;
        public Guid Qualification_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

    }
}
