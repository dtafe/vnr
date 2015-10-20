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
    public class Att_ChangeRosterGroupModel : BaseViewModel
    {
        public DateTime? Month { get; set; }
        public string OrgStrucutreName { get; set; }
    }
  
    
}
