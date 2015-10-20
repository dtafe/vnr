using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_ReportEvalutionDataModel : BaseViewModel
    {



        [DisplayName(ConstantDisplay.HRM_Evaluation_NameEntityObject)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_EvalutionData_Year)]
        public Nullable<System.DateTime> Year { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_EvalutionData_Time)]
        public Nullable<int> Time { get; set; }
        public string Type { get; set; }
        public Nullable<int> C1 { get; set; }
        public Nullable<int> C2 { get; set; }
        public Nullable<int> C3 { get; set; }
        public Nullable<int> C4 { get; set; }
        public Nullable<int> C5 { get; set; }
        public Nullable<int> C6 { get; set; }
        public Nullable<int> C7 { get; set; }
        public Nullable<int> C8 { get; set; }
        public Nullable<int> C9 { get; set; }
        public Nullable<int> C10 { get; set; }
        public Nullable<int> C11 { get; set; }
        public Nullable<int> C12 { get; set; }
        public Nullable<int> C13 { get; set; }
        public Nullable<int> C14 { get; set; }
        public Nullable<int> C15 { get; set; }
        public Nullable<int> C16 { get; set; }

        public string C17 { get; set; }
        public string C18 { get; set; }
        public double? C19 { get; set; }
        public double? C20 { get; set; }
        public double? C21 { get; set; }
        public string C22 { get; set; }
        public double? C23 { get; set; }
        public double? C24 { get; set; }
        public double? C25 { get; set; }
        public double? C26 { get; set; }
        public double? C27 { get; set; }
        public double? C28 { get; set; }
        public double? C29 { get; set; }
        public double? C30 { get; set; }
        public string C31 { get; set; }



        public string UnitName { get; set; }
        public string UnitAbb { get; set; }
        public string DivisionName { get; set; }
        public string DivisionAbb { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentAbb { get; set; }
        public string GroupCode { get; set; }
        public string LocationByGroupCode { get; set; }
        public string ActualWokingPlace { get; set; }
        public DateTime ReportEvalutionDataNode { get; set; }
        public int Age { get; set; }
        public DateTime Entering { get; set; }
        public double? ServiceYear { get; set; }
        public string PayRankSalary { get; set; }
        public double? BasicSalary { get; set; }
        public string Rank { get; set; }
        public string RankZone { get; set; }
        public string JobTitle { get; set; }
        public string QualificationAbilitytitle { get; set; }
        public double? BasicSalary1 { get; set; }

        public string AreaAllowance { get; set; }
        public string SpecialAllowance { get; set; }
        public string DrivingAllowance { get; set; }

        public double? AdjustmentAllowanceForVP { get; set; }
        public double? ChildCareAllowance { get; set; }
        public double? TotalSalaryAllowance { get; set; }


        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

  
        [DisplayName(ConstantDisplay.HRM_Eva_EvalutionData_Year)]
        public int YearEvalution { get; set; }

        public Guid ExportId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_EvalutionData_DateHistory)]
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_EvalutionData_DateHistory)]
        public int HistoryYearEvalution { get; set; }

        //public Nullable<int> TotalC1C2 { get; set; }
        //public Nullable<int> TotalC3C4C5C6C7 { get; set; }
        //public Nullable<int> TotalC1C2C3C4C5C6C7 { get; set; }
        //public Nullable<int> TotalC8C9 { get; set; }
        public string TimeEvalutionData { get; set; }
        public string TimeEvalutionDataCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_EvalutionData_Time)]
        public Nullable<Guid> TimesGetDataID { get; set; }
        ////ngay bat dau danh gia
        //public Nullable<DateTime> DateStartEvalution { get; set; }
        ////ngay ket thuc danh gia
        //public Nullable<DateTime> DateStartEvalution { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_RankID)]
        public Nullable<System.Guid> RankID { get; set; }


        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string Year = "Year";
            public const string Time = "Time";
            public const string C1 = "C1";
            public const string C2 = "C2";
            public const string C3 = "C3";
            public const string C4 = "C4";
            public const string C5 = "C5";
            public const string C6 = "C6";
            public const string C7 = "C7";
            public const string C8 = "C8";
            public const string C9 = "C9";
            public const string C10 = "C10";
            public const string C11 = "C11";
            public const string C12 = "C12";
            public const string C13 = "C13";
            public const string C14 = "C14";
            public const string C15 = "C15";
            public const string C16 = "C16";
            public const string OrgStructureID = "OrgStructureID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";


            public const string C17 = "C17";
            public const string C18 = "C18";
            public const string C19 = "C19";
            public const string C20 = "C20";
            public const string C21 = "C21";
            public const string C22 = "C22";
            public const string C23 = "C23";
            public const string C24 = "C24";
            public const string C25 = "C25";
            public const string C26 = "C26";
            public const string C27 = "C27";
            public const string C28 = "C28";
            public const string C29 = "C29";
            public const string C30 = "C30";
            public const string C31 = "C31";
            public const string UnitName = "UnitName";
            public const string UnitAbb = "UnitAbb";
            public const string DivisionName = "DivisionName";
            public const string DivisionAbb = "DivisionAbb";
            public const string DepartmentName = "DepartmentName";
            public const string DepartmentAbb = "DepartmentAbb";

            public const string GroupCode = "GroupCode";
            public const string LocationByGroupCode = "LocationByGroupCode";
            public const string ActualWokingPlace = "ActualWokingPlace";
            public const string ReportEvalutionDataNode = "ReportEvalutionDataNode";
            public const string Age = "Age";
            public const string Entering = "Entering";
            public const string ServiceYear = "ServiceYear";
            public const string PayRankSalary = "PayRankSalary";
            public const string BasicSalary = "BasicSalary";
            public const string Rank = "Rank";
            public const string RankZone = "RankZone";
            public const string JobTitle = "JobTitle";
            public const string QualificationAbilitytitle = "QualificationAbilitytitle";
            public const string BasicSalary1 = "BasicSalary1";

            public const string AreaAllowance = "AreaAllowance";
            public const string SpecialAllowance = "SpecialAllowance";
            public const string DrivingAllowance = "DrivingAllowance";
            public const string AdjustmentAllowanceForVP = "AdjustmentAllowanceForVP";
            public const string ChildCareAllowance = "ChildCareAllowance";
            public const string TotalSalaryAllowance = "TotalSalaryAllowance";

            //public const string TotalC1C2 = "TotalC1C2";
            //public const string TotalC3C4C5C6C7 = "TotalC3C4C5C6C7";
            //public const string TotalC1C2C3C4C5C6C7 = "TotalC1C2C3C4C5C6C7";
            //public const string TotalC8C9 = "TotalC8C9";
            public const string TimeEvalutionData = "TimeEvalutionData";


        }
    }
 
}
