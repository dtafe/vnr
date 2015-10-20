using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Insurance.Models
{
    /// <summary>Quyết toán bảo hiểm</summary>
    public class Ins_InsuranceSettlementModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceSettlementType)]
        public string InsuranceSettlementType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Insurance_IsReceiveSocialIns)]
        public string ReceiveSocialInsType { get; set; }       

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public DateTime? DateHire { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceSettlementType)]
         public bool? IsSettlement { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_IsReceiveSocialIns)]
        public bool? IsReceiveSocialIns { get; set; }     

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        public DateTime? DateQuit { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        public DateTime? DateQuitFrom { get; set; }
        public DateTime? DateQuitTo { get; set; }

        public int TotalRow { get; set; }
        public string ProfileIds { get; set; }
        public string OrgStructureIDs { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string InsuranceSettlementType = "InsuranceSettlementType";
            public const string DateHire = "DateHire";
            public const string DateQuit = "DateQuit";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string IsSettlement = "IsSettlement";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string ReceiveSocialInsType = "ReceiveSocialInsType";

        }
    }
    public class Ins_InsuranceSettlementSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }       
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }      
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceSettlementType)]
        public bool? IsSettlement { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_IsReceiveSocialIns)]
        public bool? IsReceiveSocialIns { get; set; }        
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        public DateTime? DateQuitFrom { get; set; }
        public DateTime? DateQuitTo { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
   
}
