using HRM.Business.BaseModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_JobConditionEntity : HRMBaseModel
    {
        public string PProvinceIDs { get; set; }
        public string ConditionName { get; set; }
        public string Code { get; set; }
        public string ConditionType { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string ConditionBrand { get; set; }
        public string ValueType { get; set; }
        public string Note { get; set; }
        public double? Value1_Number { get; set; }
        public string Value1_Text { get; set; }
        public double? Value2_Number { get; set; }
        public string DiseaseIDs { get; set; }
        public string EducationLevelIDs { get; set; }
        public object ArrDisease { get; set; }
        public string ValueTypeView { get; set; }
        public string ConditionNameView { get; set; }
        public string ConditionTypeView { get; set; }
        public string ConditionBrandView { get; set; }
        public string EnumTranslate { get; set; }
    }
    
}
