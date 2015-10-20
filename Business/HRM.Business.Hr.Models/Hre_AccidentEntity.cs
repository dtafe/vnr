using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Business.Hr.Models
{
    public class Hre_AccidentEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public Guid ProfileID { get; set; }
        [MaxLength(150)]
        public string ProfileName { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(1000)]
        public string Place { get; set; }
        public Guid? AccidentTypeID { get; set; }
        public double? FirstMoney { get; set; }
        public double? RealMoney { get; set; }
        public double? CompanyPay { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        [MaxLength(150)]
        public string CodeEmp { get; set; }
        [MaxLength(150)]
        public string PositionName { get; set; }
        public Guid? PositionID { get; set; }
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        public Guid? JobTitleID { get; set; }
        [MaxLength(500)]
        public string OrgStructureName { get; set; }
        public string OrgStructureID { get; set; }
        //public List<Guid?> OrgStructureID { get; set; }

        private Guid _id = Guid.Empty;
        public string AccidentTypeName { get; set; }
        public string Notes { get; set; }
        public Guid Accident_ID
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
