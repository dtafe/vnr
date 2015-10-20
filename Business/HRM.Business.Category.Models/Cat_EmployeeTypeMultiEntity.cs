using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_EmployeeTypeMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string EmployeeTypeName { get; set; }
        public string Notes { get; set; }
        public int? CodeID { get; set; }
    }
}
