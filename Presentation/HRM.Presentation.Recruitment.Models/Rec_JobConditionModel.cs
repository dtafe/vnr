using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Recruitment.Models
{
    public class Rec_JobConditionModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_ConditionName)]
        public string ConditionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_ConditionType)]
        public string ConditionType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_Value1)]
        public string Value1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_Value2)]
        public string Value2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_ConditionBrand)]
        public string ConditionBrand { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_ValueType)]
        public string ValueType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_Note)]
        public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_Value1)]
        public double?  Value1_Number { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_Value1)]
        public string Value1_Text { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_Value2)]
        public double? Value2_Number { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_Value1)]
        public bool? Value1_bool { get; set; }
        public string DiseaseIDs { get; set; }
        public string EducationLevelIDs { get; set; }
        public string PProvinceIDs { get; set; }
        public object ArrDisease { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_ValueType)]
        public string ValueTypeView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_ConditionName)]
        public string ConditionNameView { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_ConditionType)]
        public string ConditionTypeView { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobCondition_ConditionBrand)]
        public string ConditionBrandView { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ConditionName = "ConditionName";
            public const string ConditionNameView = "ConditionNameView";
            public const string ConditionTypeView = "ConditionTypeView";
            public const string ConditionBrandView = "ConditionBrandView";
            public const string ValueTypeView = "ValueTypeView";
            public const string Code = "Code";
            public const string ConditionType = "ConditionType";
            public const string Value1 = "Value1";
            public const string Value2 = "Value2";
            public const string ConditionBrand = "ConditionBrand";
            public const string ValueType = "ValueType";
            public const string Note = "Note";
        }
    }

    public class Rec_JobConditionByVacancySearchModel
    {
        public string JobConditionIDs { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Rec_JobConditionSearchModel
    {
        public string ConditionName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    
}
