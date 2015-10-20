using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Kai_CategoryMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string CategoryName { get; set; }

    }

    public class Kai_CategoryEntity : HRMBaseModel
    {
        public string Code { get; set; }
        public string CategoryName { get; set; }
        public string CatalogyType { get; set; }
        public string CatalogyTypeTranslate { get; set; }
        private Guid _id = Guid.Empty;
        public Guid Code_ID
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
