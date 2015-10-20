using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportProfileWaitingStopWorkingEntity : HRMBaseModel
    {
        public string ProfileName { get; set; }


        public string CodeEmp { get; set; }


        public DateTime? DateHire { get; set; }

        [MaxLength(150)]
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
        public string UnitName { get; set; }
        public string UnitCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string SectionName { get; set; }
        public string SectionCode { get; set; }

        public string TeamName { get; set; }
        public string TeamCode { get; set; }
        public Guid ResignReasonID { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }



        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string UnitName = "UnitName";
            public const string UnitCode = "UnitCode";
            public const string DepartmentName = "DepartmentName";
            public const string DepartmentCode = "DepartmentCode";
            public const string SectionName = "SectionName";
            public const string SectionCode = "SectionCode";
            public const string TeamName = "TeamName";
            public const string TeamCode = "TeamCode";
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


            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }


}
