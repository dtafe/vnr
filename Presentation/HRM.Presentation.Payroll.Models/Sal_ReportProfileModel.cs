using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_ReportProfileModel : BaseViewModel
    {
       
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        [DataType(DataType.Date)]
        public DateTime? DateHire { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        public DateTime? DateOfBirth { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        [MaxLength(50)]
        public string Gender { get; set; }



        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlaceOfBirth)]
        [MaxLength(150)]
        public string PlaceOfBirth { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        [MaxLength(50)]
        public string IDNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDDateOfIssue)]
        [DataType(DataType.Date)]
        public DateTime? IDDateOfIssue { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDPlaceOfIssue)]
        public string IDPlaceOfIssue { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        [MaxLength(50)]
        public string Cellphone { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HomePhone)]
        [MaxLength(50)]
        public string HomePhone { get; set; }


        #region Địa chỉ tạm trú/ thường trú

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAddressID)]
        public string PAddress { get; set; }

        #endregion
      
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ResignReason)]
        public Guid? ResReasonID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ResignReason)]
        public string ResignReasonName { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        [DataType(DataType.Date)]
        public DateTime? DateQuit { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_EducationLevel)]
        public string EducationLevelName { get; set; }


        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_GraduatedLevelID)]
        public string GraduatedLevelName { get; set; }
  
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SalaryClassName)]
        public string SalaryClassName { get; set; }
        //public string UnitName { get; set; }
        //public string UnitCode { get; set; }
        //public string DepartmentName { get; set; }
        //public string DepartmentCode { get; set; }
        //public string SectionName { get; set; }
        //public string SectionCode { get; set; }

        //public string TeamName { get; set; }
        //public string TeamCode { get; set; }


        public string UnitNameOrg { get; set; }
        public string DivisionNameOrg { get; set; }
        public string DepartmentNameOrg { get; set; }
        public string SectionNameOrg { get; set; }
        public string TeamNameOrg { get; set; }


        public Guid ResignReasonID { get; set; }

        public string TypeOfStop { get; set; }

        //
        public string StatusSyn { get; set; }
        public double? BasicSalary { get; set; }
        public string WorkingPlace { get; set; }
        public Nullable<bool> IsUnEmpInsurance { get; set; }
        public Nullable<bool> IsSocialInsurance { get; set; }
        public Nullable<bool> IsHealthInsurance { get; set; }
        public Nullable<bool> IsTradeUnionist { get; set; }
        public Nullable<System.DateTime> DateStop { get; set; }
        //ngay bat dau hd
        public Nullable<System.DateTime> DateStart { get; set; }
        //ngay ke thuc hd
        public Nullable<System.DateTime> DateEnd { get; set; }

        public string WorkPlaceName { get; set; }
        public string SalaryRankName { get; set; }
        //public string CostCentreName { get; set; }
        public string CostCentreCode { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            //public const string UnitName = "UnitName";
            //public const string UnitCode = "UnitCode";
            //public const string DepartmentName = "DepartmentName";
            //public const string DepartmentCode = "DepartmentCode";
            //public const string SectionName = "SectionName";
            //public const string SectionCode = "SectionCode";
            //public const string TeamName = "TeamName";
            //public const string TeamCode = "TeamCode";

            public const string UnitNameOrg = "UnitNameOrg";
            public const string DepartmentNameOrg = "DepartmentNameOrg";
            public const string SectionNameOrg = "SectionNameOrg";
            public const string TeamNameOrg = "TeamNameOrg";

            public const string PositionName = "PositionName";
            public const string DateOfBirth = "DateOfBirth";
            public const string Gender = "Gender";
            public const string PlaceOfBirth = "PlaceOfBirth";
            //noi cap cmnd
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            //thuong tru
            public const string PAddress = "PAddress";
            public const string HomePhone = "HomePhone";
            //dt ca nhan
            public const string Cellphone = "Cellphone";
            //so cmnv
            public const string IDNo = "IDNo";
            //ngay cap
            public const string IDDateOfIssue = "IDDateOfIssue";
            //ngay vao lam
            public const string DateHire = "DateHire";
            //hso luong
            public const string SalaryClassName = "SalaryClassName";
            //trinh do van hoa
            public const string GraduatedLevelName = "GraduatedLevelName";
            //tring do hoc van
            public const string EducationLevelName = "EducationLevelName";
            //ngay lam viec cuoi cung
            public const string DateQuit = "DateQuit";
            //ly do nghi viec
            public const string TypeOfStop = "TypeOfStop";
            //ly do nghi chi tiet
            public const string ResignReasonName = "ResignReasonName";

            public const string BasicSalary = "BasicSalary";
            public const string WorkingPlace = "WorkingPlace";
            public const string IsUnEmpInsurance = "IsUnEmpInsurance";
            public const string IsSocialInsurance = "IsSocialInsurance";
            public const string IsHealthInsurance = "IsHealthInsurance";
            public const string IsTradeUnionist = "IsTradeUnionist";
            public const string DateStop = "DateStop";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string StatusSyn = "StatusSyn";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string SalaryRankName = "SalaryRankName";
            public const string CostCentreCode = "CostCentreCode";


            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
    public class Sal_ReportProfileSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_CutOffDurationID)]
        public Guid CutOffDurationID { get; set; }

        public Guid UserID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    

}
