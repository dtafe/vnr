using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_CodeAlocalMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string CodeAlocalName { get; set; } 
    }
    public class Sal_CodeAlocalEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string CodeAlocalName { get; set; }
        public string Code { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<double> Rate { get; set; }
        private Guid _id = Guid.Empty;
        public Guid CodeAlocal_ID
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
