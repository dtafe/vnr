using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_CertificateEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }

        [MaxLength(50)]
        public string CertificateCode { get; set; }

        [MaxLength(150)]
        public string CertificateName { get; set; }

        [MaxLength(1000)]
        public string Place { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public Guid? CourseID { get; set; }

        public Guid? PositionID { get; set; }

        [MaxLength(150)]
        public string PositionName { get; set; }

        public Guid? JobTitleID { get; set; }

        [MaxLength(150)]
        public string JobTitleName { get; set; }

        public Guid? OrgStructureID { get; set; }

        [MaxLength(150)]
        public string OrgStructureName { get; set; }
    }
}
