using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;


namespace HRM.Business.Hr.Models
{
    public class Hre_ProfilePartyUnionEntity : HRMBaseModel
    {
        public Guid ProfileID { get; set; }
        public string ProfileName { get; set; }
        public bool? IsYouthUnionist { get; set; }
        public DateTime? YouthUnionEnrolledDate { get; set; }
        public Guid? YouthUnionPositionID { get; set; }
        public bool? IsCommunistPartyMember { get; set; }
        public DateTime? CommunistPartyEnrolledDate { get; set; }
        public Guid? CommunistPartyPositionID { get; set; }
        public string CommunistPartyCardNo { get; set; }
        public Guid? WoundedSoldierTypeID { get; set; }
        public Guid? PoliticalLevelID { get; set; }
        public bool? IsTradeUnionist { get; set; }
        public DateTime? TradeUnionistEnrolledDate { get; set; }
        public Guid? TradeUnionistPositionID { get; set; }
        public bool? IsSelfDefenceMilitia { get; set; }
        public DateTime? SelfDefenceMilitiaEnrolledDate { get; set; }
        public System.DateTime? SelfDefenceMilitiaEndDate { get; set; }
        public System.Guid? SelfDefenceMilitiaPositionID { get; set; }
        public bool? IsVeteranUnionist { get; set; }
        public System.DateTime? VeteranUnionEnrolledDate { get; set; }
        public string VeteranUnionCardNo { get; set; }
        public Guid? VeteranUnionPositionID { get; set; }
        public DateTime? RevolutionEnrolledDate { get; set; }
        public DateTime? RevolutionEndDate { get; set; }
        public Guid? ArmedForceTypeID { get; set; }
        public string RelationWithMartyr { get; set; }
        public string Biography { get; set; }
        public DateTime? CommunistPartyReserveDate { get; set; }
        public bool? IsRelationWithMartyr { get; set; }

        public string CommunistPartyPositionName { get; set; }
        public string PoliticalLevelName { get; set; }
        public string TradeUnionistPositionName { get; set; }
        public string YouthUnionPositionName { get; set; }
        public string ArmedForceTypesName { get; set; }
        public string SelfDefenceMilitiaPositionName { get; set; }
        public string WoundedSoldierTypesName { get; set; }
        public string VeteranUnionPositionName { get; set; }
        public DateTime? DateHire { get; set; }
        public Guid? JobTitleID { get; set; }
        public Guid? PositionID { get; set; }
        public Guid? OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
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
        public string PoliticalTheory { get; set; }
        public string PoliticalTheoryView { get; set; }
    }
}
