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
    public class Sal_SalaryDepartmentModel : BaseViewModel
    {

        public Nullable<System.Guid> CutOffDurationID { get; set; }
        public System.DateTime MonthYear { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string OrgStructureString { get; set; }
        public string TypeCompute { get; set; }
        public Nullable<System.Guid> ProductionLineID { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_SAL_SalaryDeparment_Amount)]
        public double Amount { get; set; }
        public Nullable<double> TotalGroupHours { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public Nullable<bool> IsAll { get; set; }
        public string Note { get; set; }
        public string CutOffDurationName { get; set; }
        public string ProductionLineName { get; set; }
        public string OrgStructureName { get; set; }
        public string CurrencyName { get; set; }

        public Guid? PurchaseRequestID { get; set; }
        private Guid _id = Guid.Empty;
        public Guid Department_ID
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

        public Guid? ProductID { get; set; }
        public Guid? ProductItemID { get; set; }
        public double? QuantityCalculate { get; set; }
        public double? RateForPO { get; set; }
        public double? AmountAdjust { get; set; }
        public double? AmountAfterAdjust { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string MonthYear = "MonthYear";
            public const string DateFrom = "DateFrom";
            public const string TypeCompute = "TypeCompute";
            public const string DateTo = "DateTo";
            public const string OrgStructureID = "OrgStructureID";
            public const string Amount = "Amount";
            public const string ProductionLineID = "ProductionLineID";
            public const string TotalGroupHours = "TotalGroupHours";
            public const string CurrencyID = "CurrencyID";
            public const string IsAll = "IsAll";
            public const string Note = "Note";
            public const string CutOffDurationName = "CutOffDurationName";
            public const string ProductionLineName = "ProductionLineName";
            public const string OrgStructureName = "OrgStructureName";
            public const string CurrencyName = "CurrencyName";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";

            

        }
    }

    public class Sal_SalaryDepartmentSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        
    }
    public class Sal_SalaryDepartmentMultiModel {
        public Guid ID { get; set; }
        public string DepartmentName { get; set; }
    }
}
