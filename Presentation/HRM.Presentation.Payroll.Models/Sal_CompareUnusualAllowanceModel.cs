using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_CompareUnusualAllowanceModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Sal_CompareUnusualAllowance_CutOffDurationID)]
        public Guid CutOffDurationID { get; set; }
        public string OrgStructureID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
       [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public Guid? WorkingPlaceID { get; set; }
        public string StatusSyn { get; set; }
        public string UnusualAllowanceCfgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public string WorkingPlace { get; set; }
        public Guid ExportId { get; set; }
       
        public partial class FieldNames
        {
            public const string UnusualAllowanceCfgName = "UnusualAllowanceCfgName";

        }
    }

    

}
