using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_ProfileComputingLevelEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public Guid ProfileID { get; set; }

        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [MaxLength(150)]
        public string ProfileName { get; set; }


  

        public Guid SpecialLevelID { get; set; }
        public Guid SpecialTypeID { get; set; }
      
        [MaxLength(150)]
        public string ComputingType { get; set; }

        public string ComputingLevel { get; set; }
        private Guid _id = Guid.Empty;
        public Guid Computing_ID
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
    }
}
