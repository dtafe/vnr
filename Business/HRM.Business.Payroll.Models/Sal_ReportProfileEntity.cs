using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Sal_ReportProfileEntity : HRMBaseModel
    {
        public string ProfileName { get; set; }


        public string CodeEmp { get; set; }


        public DateTime? DateHire { get; set; }

   
        public string PositionName { get; set; }

        public DateTime? DateOfBirth { get; set; }


        public string Gender { get; set; }



        public string PlaceOfBirth { get; set; }


        public string IDNo { get; set; }
        public DateTime? IDDateOfIssue { get; set; }

        public string IDPlaceOfIssue { get; set; }


        public string Cellphone { get; set; }

        public string HomePhone { get; set; }


        #region Địa chỉ tạm trú/ thường trú

        public string PAddress { get; set; }

        #endregion

        public Guid? ResReasonID { get; set; }

        public string ResignReasonName { get; set; }


        public DateTime? DateQuit { get; set; }


        public string EducationLevelName { get; set; }


        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }

        public string GraduatedLevelName { get; set; }

        public string SalaryClassName { get; set; }
        public string UnitNameOrg { get; set; }
        //public string UnitCode { get; set; }
        public string DepartmentNameOrg { get; set; }
        //public string DepartmentCode { get; set; }
        public string SectionNameOrg { get; set; }
        //public string SectionCode { get; set; }

        public string TeamNameOrg { get; set; }
        //public string TeamCode { get; set; }
        public Guid ResignReasonID { get; set; }

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
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string UnitNameOrg = "UnitNameOrg";
            //public const string UnitCode = "UnitCode";
            public const string DepartmentNameOrg = "DepartmentNameOrg";
            //public const string DepartmentCode = "DepartmentCode";
            public const string SectionNameOrg = "SectionNameOrg";
            //public const string SectionCode = "SectionCode";
            public const string TeamNameOrg = "TeamNameOrg";
            //public const string TeamCode = "TeamCode";
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
        }
    }
}
