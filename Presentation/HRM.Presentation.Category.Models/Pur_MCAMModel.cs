using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;


namespace HRM.Presentation.Category.Models
{
    public class Pur_MCAMModel : BaseViewModel
    {
       
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Fin_PurchaseRequest_DateRequest)]
        public DateTime? DateRequest { get; set; }
         [DisplayName(ConstantDisplay.HRM_Cat_Model_ModelCode)]
        public Guid? ModelID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_PaymentMethodID)]
        public Guid? PaymentMethodID { get; set; }
        public DateTime? StartDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Pur_MCAM_ReceivePlace)]
        public string ReceivePlace { get; set; }
        public string Status { get; set; }
        public DateTime? EndDate { get; set; }
        public string VehicleIdNo { get; set; }
        public string EngineNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Pur_MCAM_ReceiveDate)]
        public DateTime? ReceiveDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_ColorID)]
        public Guid? ColorID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Pur_MCAM_ReceivePlace)]
        public Guid? RecevivePlaceID { get; set; }
        public DateTime? LiquidationDate { get; set; }
        public string Note { get; set; }
        public string NotMeetConditionReason { get; set; }
        public double? InterestRate { get; set; }
        public double? AmountPayment { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Pur_MCAM_AmountDeposit)]
        public double? AmountDeposit { get; set; }
       [DisplayName(ConstantDisplay.HRM_Category_Pur_MCAM_AmountRemain)]
        
        public double? AmountRemain { get; set; }
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_IDCardNo)]
        public string IDNo { get; set; }
          [DisplayName(ConstantDisplay.HRM_Cat_Model_ModelType)]
        public string ModelType { get; set; }
         [DisplayName(ConstantDisplay.HRM_Cat_Model_ModelCode)]
        public string ModelCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_ModelName)]
        public string ModelName { get; set; }
        public string PaymentmethodCode { get; set; }
          [DisplayName(ConstantDisplay.HRM_Cat_Model_DeadlinePayment)]
        public double? DeadlinePayment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Pur_MCAM_OrgStructureName)]
        public string OrgStructureName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Pur_MCAM_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Pur_MCAM_GrossAmount)]
        public double? GrossAmount { get; set; }
        public string Cat_contractType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_ColorID)]
        public string ColorCode { get; set; }
        public string WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        public Guid? ContractTypeID { get; set; }
        public string CurrencyCode {get;set;}
        public Guid? ColorModelID { get; set; }
        public string Color { get; set; }
        public string CurrencyAmountRemain { get; set; }
        public string CurrencyAmountDeposit { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string IDNo = "IDNo";
            public const string ProfileName = "ProfileName";
            public const string StartDate = "StartDate";
            public const string EndDate = "EndDate";
            public const string ReceiveDate = "ReceiveDate";
            public const string ModelName = "ModelName";
            public const string ModelType = "ModelType";
            public const string Status = "Status";
            public const string ColorCode = "ColorCode";
            public const string Color = "Color";
            public const string PaymentmethodCode = "PaymentmethodCode";
            public const string DeadlinePayment = "DeadlinePayment";
            public const string AmountDeposit = "AmountDeposit";
            public const string AmountRemain = "AmountRemain";
            public const string ReceivePlace = "ReceivePlace";
            public const string OrgStructureName = "OrgStructureName";
            public const string GrossAmount = "GrossAmount";
            public const string Cat_contractType = "Cat_contractType";
             
        }
    }
    public class Pur_MCAMSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Pur_MCAM_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public string WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_ModelType)]
        public string ModelType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Model_ModelName)]
        public string ModelName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
