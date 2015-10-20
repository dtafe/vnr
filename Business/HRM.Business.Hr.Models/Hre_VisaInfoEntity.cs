using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_VisaInfoEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        [MaxLength(50)]
        public string VisaInfoName { get; set; }
        public System.Guid? CountryID { get; set; }
        public string CountryName { get; set; }
        public Guid? ProfileID { get; set; }
        public string ProfileName { get; set; }
        public System.DateTime? DateStart { get; set; }
        public System.DateTime? DateEnd { get; set; }

        public string Notes { get; set; }
        private Guid _id = Guid.Empty;
        public Guid Visa_ID
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
