using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_WorkHistoryEntity : HRMBaseModel
    {
        public Nullable<System.Guid> AbilityTileID { get; set; }
        //public int TotalRow { get; set; }
        [MaxLength(50)]
        public string WorkHistoryCode { get; set; }
        public Guid ProfileID { get; set; }
        public DateTime DateEffective { get; set; }
        public string DateEffectiveFormat { get; set; }
        public Guid? OrganizationStructureID { get; set; }
        public Guid? PositionID { get; set; }
        public Guid? CostCentreID { get; set; }
        public string CostCentreName { get; set; }
        [MaxLength(150)]
        public string LaborType { get; set; }
        public Guid? EmployeeTypeID { get; set; }
        public  string EmployeeTypeName { get; set; }
        public Guid? PayrollGroupID { get; set; }
        [MaxLength(1000)]
        public string WorkLocation { get; set; }

        public string Supervisor { get; set; }

        public string DecisionNo { get; set; }

        public Guid? JobTitleID { get; set; }
        public Guid? SalaryClassID { get; set; }
        public string SalaryClassName { get; set; }

        [MaxLength(150)]
        public string Status { get; set; }

        [MaxLength(150)]
        public string Note { get; set; }

        [MaxLength(150)]
        public string ProfileName { get; set; }

        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        [MaxLength(150)]
        public string OrgStructureNameEN { get; set; }

        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [MaxLength(150)]
        public string JobTitleNameEN { get; set; }

        [MaxLength(150)]
        public string PositionName { get; set; }

        [MaxLength(150)]
        public string PositionNameEN { get; set; }

        public string OrgOld { get; set; }

        public string JobTitleOld { get; set; }

        public string PositionOld { get; set; }

        public string PayrollGroupOld { get; set; }

        public string WorkLocationOld { get; set; }

        public string JRType { get; set; }

        public string UserApprove { get; set; }

        public string Rromotion { get; set; }

        public string RequirCondit { get; set; }

        public string Description { get; set; }

        public string Role { get; set; }
        public string FormType { get; set; }

        private Guid _id = Guid.Empty;
        public Guid WorkHistory_ID
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
        public Nullable<int> Duration { get; set; }
        public Nullable<System.DateTime> DateNotice { get; set; }
        public string DateNoticeFormat { get; set; }
        public string StatusView { get; set; }

        public Nullable<System.Guid> TypeOfTransferID { get; set; }

        public string NameEntityName { get; set; }
        public Nullable<System.DateTime> DateComeBack { get; set; }
        public string DateComeBackFormat { get; set; }
        public string TypeOfTransferName { get; set; }
        public string SalaryClassNameOld { get; set; }
        public Nullable<System.Guid> OrgStructureOldID { get; set; }
        public string OrgStructureOldName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public string UnitNameOld { get; set; }
        public string DivisionNameOld { get; set; }
        public string DepartmentNameOld { get; set; }
        public string SectionNameOld { get; set; }
        public Nullable<System.Guid> CostSourceID { get; set; }
        public string CostSourceName { get; set; }
        public string HighSupervisor { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public string WorkPlace { get; set; }
        public string WorkPlaceOld { get; set; }
        public double? Salary { get; set; }
        public double? Allowance1 { get; set; }
        public double? Allowance2 { get; set; }
        public double? Allowance3 { get; set; }
        public double? Allowance4 { get; set; }
        public string DateNow { get; set; }
        public string DateNow_Day { get; set; }
        public string DateNow_Month { get; set; }
        public string DateNow_Year { get; set; }
        public string DateNow_Hour { get; set; }

        public string GraveName { get; set; }
        public string Gender { get; set; }
        public string GenderView { get; set; }
    }
}
