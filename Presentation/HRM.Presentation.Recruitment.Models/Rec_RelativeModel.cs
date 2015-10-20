using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Recruitment.Models
{
    public class Rec_RelativeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_Relative_RelativeName)]
        public string RelativeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Relative_CompanyName)]
        public string CompanyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Relative_Position)]
        public string Position { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_CandidateID)]
        public Nullable<System.Guid> CandidateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_CandidateID)]
        public string CandidateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Relative_RelationshipName)]
        public Nullable<System.Guid> RelationshipID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Relative_RelationshipName)]
        public string RelationshipName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Relative_BusinessRelation)]
        public string BusinessRelation { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Relative_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Relative_BussinessType)]
        public string BussinessType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Relative_Bussiness)]
        public string Bussiness { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string Phone { get; set; }

            [DisplayName(ConstantDisplay.HRM_HR_Dependant_DateOfBirth)]
        public string DateOrBirth { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string RelativeName = "RelativeName";
            public const string CompanyName = "CompanyName";
            public const string RelationshipName = "RelationshipName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Rec_RelativeByCandidateSearchModel
    {
        public Guid CandidateID { get; set; }
        public string Type { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    
}
