using System;

namespace HRM.Business.Training.Models
{
    public class Tra_TraineeRegisterEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.Guid> ClassID { get; set; }
        public string Status { get; set; }
        public Nullable<System.Guid> UserApproveID1 { get; set; }
        public Nullable<System.Guid> UserApproveID2 { get; set; }
        public string Description { get; set; }
        public string Reason1 { get; set; }
        public string Reason2 { get; set; }

        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string OrgStructureName { get; set; }
        public string CourseName { get; set; }
        public string ClassName { get; set; }
        public string Code { get; set; }
        public string TypeError { get; set; }
        public string UserApproveName1 { get; set; }
        public string UserApproveName2 { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string ClassCode { get; set; }
        public string StatusView { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CodeEmp = "CodeEmp";
            public const string TraineeName = "TraineeName";
            public const string UserApproveName1 = "UserApproveName1";
            public const string UserApproveName2 = "UserApproveName2";
            public const string ProfileName = "ProfileName";
            public const string TypeError = "TypeError";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";



        }
    }
}

