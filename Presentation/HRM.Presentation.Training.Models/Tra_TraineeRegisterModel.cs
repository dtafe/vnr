using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Training.Models
{
    public class Tra_TraineeRegisterModel : BaseViewModel
    {
        public string CodeEmp { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ClassID)]
        public Nullable<System.Guid> ClassID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ClassID)]
        public string ClassName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeRegister_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeRegister_UserApproveName1)]
        public Nullable<System.Guid> UserApproveID1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeRegister_UserApproveName2)]
        public Nullable<System.Guid> UserApproveID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeRegister_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeRegister_Reason1)]
        public string Reason1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeRegister_Reason2)]
        public string Reason2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeRegister_UserApproveName1)]
        public string UserApproveName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeRegister_UserApproveName2)]
        public string UserApproveName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeRegister_Status)]
        public string StatusView { get; set; }

        public Nullable<System.DateTime> StartDate { get; set; }

        public Nullable<System.DateTime> EndDate { get; set; }
        public string ClassCode { get; set; }

         public string E_UNIT { get; set; }
         public string E_DIVISION { get; set; }
         public string E_DEPARTMENT { get; set; }
         public string E_TEAM { get; set; }
         public string E_SECTION { get; set; }

         public List<Guid> selecteIds { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string TraineeTopicID = "TraineeTopicID";
            public const string Status = "Status";
            public const string UserApproveName1 = "UserApproveName1";
            public const string UserApproveName2 = "UserApproveName2";
            public const string ClassCode = "ClassCode";
            public const string StatusView = "StatusView";
           
            public const string Type = "Type";
            public const string Reason2 = "Reason2";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string CourseName = "CourseName";
            public const string ClassName = "ClassName";
            public const string Reason1 = "Reason1";
            public const string StartDate = "StartDate";
            public const string EndDate = "EndDate";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
    public class Tra_TraineeRegisterSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_OrgName)]
        public string OrgStructureID { get; set; }



        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ClassID)]
        public string ClassID { get; set; }

        

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public string Status { get; set; }


        public bool IsCreateTemplate { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
    }
    
}
