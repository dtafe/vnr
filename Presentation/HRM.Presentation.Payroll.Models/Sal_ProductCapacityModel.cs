using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_ProductCapacityModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string ProductCapacityName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_ProductName)]
        public Nullable<System.Guid> ProductID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_ProductName)]
        public string ProductName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_ProductItemName)]
        public Nullable<System.Guid> ProductItemID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_ProductItemName)]
        public string ProductItemName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_OrgStructureName)]
        public Nullable<System.Guid> DepartmentID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_DateFrom)]
        public Nullable<System.DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_DateTo)]
        public Nullable<System.DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_MinCapacity)]
        public Nullable<double> MinCapacity { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_MaxCapacity)]
        public Nullable<double> MaxCapacity { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_Rate)]
        public Nullable<double> Rate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_Formula)]
        public string Formula { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_Notes)]
        public string Notes { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string Code { get; set; }


        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string ProductCapacityName = "ProductCapacityName";
            public const string OrgStructureName = "OrgStructureName";
            public const string ProductName = "ProductName";
            public const string ProductItemName = "ProductItemName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string MinCapacity = "MinCapacity";
            public const string MaxCapacity = "MaxCapacity";
            public const string Rate = "Rate";
            public const string Formula = "Formula";
            public const string Notes = "Notes";
        }
    }

    public class Sal_ProductCapacitySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_ProductName)]
        public Nullable<System.Guid> ProductID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_ProductItemName)]
        public Nullable<System.Guid> ProductItemID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_DateFrom)]
        public Nullable<System.DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ProductCapacity_DateTo)]
        public Nullable<System.DateTime> DateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

    }
}
