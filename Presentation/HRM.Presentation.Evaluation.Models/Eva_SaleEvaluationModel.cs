using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_SaleEvaluationModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleEvaluation_SaleEvaluationName)]
        public string SaleEvaluationName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleEvaluation_Year)]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Year { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleEvaluation_SalesTypeID)]
        public Guid? SalesTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleEvaluation_TagetNumber)]

        public Nullable<double> TagetNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleEvaluation_ResultNumber)]

        public Nullable<double> ResultNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleEvaluation_ResultPercent)]
        //[DisplayFormat(DataFormatString = "{0:P2}")]
        public double? ResultPercent { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SalesType_SalesTypeName)]
        public string SalesTypeName { get; set; }

        
        public string Note { get; set; }
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

         public double? TargetNumberSaleIn { get; set; }
         public double? ResultNumberSaleIn { get; set; }
         public double? ResultPercentSaleIn { get; set; }

         public string E_UNIT { get; set; }
         public string E_DIVISION { get; set; }
         public string E_DEPARTMENT { get; set; }
         public string E_SECTION { get; set; }
         public string E_TEAM { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CodeEmp = "CodeEmp";
            public const string OrgStructureName = "OrgStructureName";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

            public const string ProfileName = "ProfileName";
            public const string SaleEvaluationName = "SaleEvaluationName";
            public const string Year = "Year";
            public const string SalesTypeID = "SalesTypeID";
            public const string TagetNumber = "TagetNumber";
            public const string ResultNumber = "ResultNumber";
            public const string ResultPercent = "ResultPercent";
            public const string SalesTypeName = "SalesTypeName";
            public const string ProfileID = "ProfileID";
            public const string TargetNumberSaleIn = "TargetNumberSaleIn";
            public const string ResultNumberSaleIn = "ResultNumberSaleIn";
            public const string ResultPercentSaleIn = "ResultPercentSaleIn";
        }
    }

    public class Eva_SaleEvaluationSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
       
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
       
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }


    }
    public class Eva_SaleEvaluationByProfileSearchModel
    {
        public string ProfileID { get; set; }
        public string SaleType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Eva_SaleEvaluationPortalByProfileSearchModel
    {
        public string ProfileID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
