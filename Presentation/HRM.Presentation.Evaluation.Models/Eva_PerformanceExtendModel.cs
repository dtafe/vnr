using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_PerformanceExtendModel : BaseViewModel
    {
        private Guid _id = Guid.Empty;
        public Guid PerformanceExtendID
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
        public string V1 { get; set; }
        public string V2 { get; set; }
        public string V3 { get; set; }
        public string V4 { get; set; }
        public string V5 { get; set; }
        public string V6 { get; set; }
        public string V7 { get; set; }
        public string V8 { get; set; }
        public string V9 { get; set; }
        public string V10 { get; set; }
        public string V11 { get; set; }
        public string V12 { get; set; }
        public Nullable<double> V13 { get; set; }
        public Nullable<double> V14 { get; set; }
        //public string V14 { get; set; }
        public string V15 { get; set; }
        public string V16 { get; set; }
        public string V17 { get; set; }
        public string V18 { get; set; }
        public string V19 { get; set; }
        public string V20 { get; set; }
        public string V21 { get; set; }
        public string V22 { get; set; }
        public string V23 { get; set; }
        public string V24 { get; set; }
        public string V25 { get; set; }
        public string V26 { get; set; }
        public string V27 { get; set; }
        public string V28 { get; set; }
        public string V29 { get; set; }
        public string V30 { get; set; }
        public string V31 { get; set; }
        public string V32 { get; set; }
        public string V33 { get; set; }
        public string V34 { get; set; }
        public string V35 { get; set; }
        public string V36 { get; set; }
        public string V37 { get; set; }
        public string V38 { get; set; }
        public string V39 { get; set; }
        public string V40 { get; set; }
        public string V41 { get; set; }
        public string V42 { get; set; }
        public string V43 { get; set; }
        public string V44 { get; set; }
        public string V45 { get; set; }
        public string V46 { get; set; }
        public Nullable<System.DateTime> V47 { get; set; }
        public Nullable<System.DateTime> V48 { get; set; }
        public Nullable<System.DateTime> V49 { get; set; }
        public Nullable<System.DateTime> V50 { get; set; }
        public string V51 { get; set; }
        public string V52 { get; set; }
        public string V53 { get; set; }
        public string V54 { get; set; }
        public string V55 { get; set; }
        public string V56 { get; set; }
        public string V57 { get; set; }
        public string V58 { get; set; }
        public string V59 { get; set; }

        public partial class FieldNames
        {
            
        }
    }


    public class Eva_PerformanceExtendMultiModel
    {
        public Guid ID { get; set; }
        public string ProfileName { get; set; }
    }

}
