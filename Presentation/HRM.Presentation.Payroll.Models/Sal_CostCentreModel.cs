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
    public class SalCostCentreModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_Payroll_Costcentre_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_CostCentre_CostCentreName)]
        public string CostCentreName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Costcentre_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_CostCentre_Code)]
        public Nullable<System.Guid> CostCentreID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Costcentre_ProfileID)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Costcentre_DateStart)]
        public Nullable<System.DateTime> DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Costcentre_Rate)]
        public Nullable<double> Rate { get; set; }
           [DisplayName(ConstantDisplay.HRM_Payroll_Costcentre_Rate)]
        public double Rate_Money { get; set; }
           [DisplayName(ConstantDisplay.HRM_Payroll_CodeAlocal_ElementType)]
           public string ElementType { get; set; }
           [DisplayName(ConstantDisplay.HRM_Payroll_CodeAlocal_ElementType)]
           public string ElementName { get; set; }
        //public virtual ICollection<Cat_JobTitle> Cat_JobTitles { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string CostCentreID = "CostCentreID";
            public const string DateStart = "DateStart";
            public const string ProfileName = "ProfileName";
            public const string CostCentreName = "CostCentreName";
            public const string Rate = "Rate";
            public const string UserCreate = "UserCreate";
            public const string DateUpdate = "DateUpdate";
            public const string CodeEmp = "CodeEmp";
            public const string ElementType = "ElementType";
        }
    }
    public class SalCostCentreMultiModel
    {
        public Guid ID { get; set; }
        public string CostCentreName { get; set; }
    }

    public class SalCostCentreSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_Costcentre_ProfileID)]
        public string ProfileName { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
