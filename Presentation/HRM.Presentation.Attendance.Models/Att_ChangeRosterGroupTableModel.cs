using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.Data;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_ChangeRosterGroupTableModel : BaseViewModel
    {
        public DataTable Table { get; set; }
        public string Ids { get; set; }
        public List<Guid> lstIds { get; set; }

        public List<string> lstType { get; set; }

        public DateTime? DateStart { get; set; }
        public string OrgStructureID { get; set; }

    }
  
    
}
