using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_CodeAlocalModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_Payroll_CodeAlocal_CodeAlocalName)]
         public string CodeAlocalName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_CodeAlocal_Code)]
         public string Code { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_CodeAlocal_ProfileID)]
         public Nullable<System.Guid> ProfileID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_CodeAlocal_ProfileID)]
         public string ProfileName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_CodeAlocal_DateStart)]
         public Nullable<System.DateTime> DateStart { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_CodeAlocal_Rate)]
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
        [DisplayName(ConstantDisplay.HRM_Payroll_CodeAlocal_ElementType)]
         public string ElementType { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CodeAlocalName = "CodeAlocalName";
            public const string Code = "Code";
            public const string ProfileName = "ProfileName";
            public const string DateStart = "DateStart";
            public const string Rate = "Rate";
            public const string ElementType = "ElementType";
        }
    }
}
