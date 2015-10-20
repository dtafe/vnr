using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;


namespace HRM.Presentation.Hr.Models
{
    public class Hre_ProfilePartyUnionModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PoliticalTheory)]
        public string PoliticalTheory { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PoliticalTheory)]
        public string PoliticalTheoryView { get; set; }
        public Guid JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_IsYouthUnionist)]
        public bool? IsYouthUnionist { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_YouthUnionEnrolledDate)]
        [DataType(DataType.Date)]
        public DateTime? YouthUnionEnrolledDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_YouthUnionPositionID)]
        public Guid? YouthUnionPositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_YouthUnionPositionName)]
        public string YouthUnionPositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_IsCommunistPartyMember)]
        public bool? IsCommunistPartyMember { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_CommunistPartyEnrolledDate)]
        [DataType(DataType.Date)]
        public DateTime? CommunistPartyEnrolledDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_CommunistPartyPositionID)]
        public Guid? CommunistPartyPositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_CommunistPartyPositionName)]
        public string CommunistPartyPositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_CommunistPartyCardNo)]
        public string CommunistPartyCardNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_WoundedSoldierTypeID)]
        public Guid? WoundedSoldierTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_WoundedSoldierTypesName)]
        public string WoundedSoldierTypesName { get; set; }
        

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_PoliticalLevelID)]
        public Guid? PoliticalLevelID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_PoliticalLevelName)]
        public string PoliticalLevelName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_IsTradeUnionist)]
        public bool? IsTradeUnionist { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_TradeUnionistEnrolledDate)]
        [DataType(DataType.Date)]
        public DateTime? TradeUnionistEnrolledDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_TradeUnionistPositionID)]
        public Guid? TradeUnionistPositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_TradeUnionistPositionName)]
        public string TradeUnionistPositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_IsSelfDefenceMilitia)]
        public bool? IsSelfDefenceMilitia { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_SelfDefenceMilitiaEnrolledDate)]
        [DataType(DataType.Date)]
        public DateTime? SelfDefenceMilitiaEnrolledDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_SelfDefenceMilitiaEndDate)]
        [DataType(DataType.Date)]
        public DateTime? SelfDefenceMilitiaEndDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_SelfDefenceMilitiaPositionID)]
        public Guid? SelfDefenceMilitiaPositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_SelfDefenceMilitiaPositionName)]
        public string SelfDefenceMilitiaPositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_IsVeteranUnionist)]
        public bool? IsVeteranUnionist { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_VeteranUnionEnrolledDate)]
        [DataType(DataType.Date)]
        public DateTime? VeteranUnionEnrolledDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_VeteranUnionCardNo)]
        public string VeteranUnionCardNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_VeteranUnionPositionID)]
        public Guid? VeteranUnionPositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_VeteranUnionPositionName)]
        public string VeteranUnionPositionName { get; set; }

        

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_RevolutionEnrolledDate)]
        [DataType(DataType.Date)]
        public DateTime? RevolutionEnrolledDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_RevolutionEndDate)]
        [DataType(DataType.Date)]
        public DateTime? RevolutionEndDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_ArmedForceTypeID)]
        public Guid? ArmedForceTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_ArmedForceTypesName)]
        public string ArmedForceTypesName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_RelationWithMartyr)]
        public string RelationWithMartyr { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_Biography)]
        public string Biography { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_CommunistPartyReserveDate)]
        [DataType(DataType.Date)]
        public DateTime? CommunistPartyReserveDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_IsRelationWithMartyr)]
        public bool? IsRelationWithMartyr { get; set; }

        private Guid _id = Guid.Empty;
        public Guid PartyUnion_ID
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
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string IsYouthUnionist = "IsYouthUnionist";
            public const string YouthUnionEnrolledDate = "YouthUnionEnrolledDate";
            public const string YouthUnionPositionID = "YouthUnionPositionID";
            public const string IsCommunistPartyMember = "IsCommunistPartyMember";
            public const string CommunistPartyEnrolledDate = "CommunistPartyEnrolledDate";
            public const string CommunistPartyPositionID = "CommunistPartyPositionID";
            public const string CommunistPartyCardNo = "CommunistPartyCardNo";
            public const string WoundedSoldierTypeID = "WoundedSoldierTypeID";
            public const string PoliticalLevelID = "PoliticalLevelID";
            public const string IsTradeUnionist = "IsTradeUnionist";
            public const string TradeUnionistEnrolledDate = "TradeUnionistEnrolledDate";
            public const string TradeUnionistPositionID = "TradeUnionistPositionID";
            public const string IsSelfDefenceMilitia = "IsSelfDefenceMilitia";
            public const string SelfDefenceMilitiaEnrolledDate = "SelfDefenceMilitiaEnrolledDate";
            public const string SelfDefenceMilitiaEndDate = "SelfDefenceMilitiaEndDate";
            public const string SelfDefenceMilitiaPositionID = "SelfDefenceMilitiaPositionID";
            public const string IsVeteranUnionist = "IsVeteranUnionist";
            public const string VeteranUnionEnrolledDate = "VeteranUnionEnrolledDate";
            public const string VeteranUnionCardNo = "VeteranUnionCardNo";
            public const string VeteranUnionPositionID = "VeteranUnionPositionID";
            public const string RevolutionEnrolledDate = "RevolutionEnrolledDate";
            public const string RevolutionEndDate = "RevolutionEndDate";
            public const string ArmedForceTypeID = "ArmedForceTypeID";
            public const string ServerUpdate = "ServerUpdate";
            public const string ServerCreate = "ServerCreate";
            public const string UserUpdate = "UserUpdate";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserLockID = "UserLockID";
            public const string DateLock = "DateLock";
            public const string IsDelete = "IsDelete";
            public const string IPCreate = "IPCreate";
            public const string IPUpdate = "IPUpdate";
            public const string RelationWithMartyr = "RelationWithMartyr";
            public const string Biography = "Biography";
            public const string CommunistPartyReserveDate = "CommunistPartyReserveDate";
            public const string IsRelationWithMartyr = "IsRelationWithMartyr";
            public const string CommunistPartyPositionName = "CommunistPartyPositionName";
            public const string PoliticalLevelName = "PoliticalLevelName";
            public const string TradeUnionistPositionName = "TradeUnionistPositionName";
            public const string ArmedForceTypesName = "ArmedForceTypesName";
            public const string WoundedSoldierTypesName = "WoundedSoldierTypesName";
            public const string VeteranUnionPositionName = "VeteranUnionPositionName";
            public const string SelfDefenceMilitiaPositionName = "SelfDefenceMilitiaPositionName";
            public const string YouthUnionPositionName = "YouthUnionPositionName";
            public const string ProfileName = "ProfileName";
            public const string PartyUnion_ID = "PartyUnion_ID";

            public const string JobTitleID = "JobTitleID";
            public const string PositionID = "PositionID";
            public const string OrgStructureID = "OrgStructureID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";

            
        }
    }
    public class Hre_ProfilePartyUnionSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Uniform_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_DateTo)]
        public DateTime? DateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
