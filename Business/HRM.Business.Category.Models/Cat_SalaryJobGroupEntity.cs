using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_SalaryJobGroupEntity
    {
        public Guid ID { get; set; }
        public string SalaryJobGroupName { get; set; }
    }

    public class Cat_SalaryJobGroupMultiEntity
    {
        public Guid ID { get; set; }
        public string SalaryJobGroupName { get; set; }
    }
}
