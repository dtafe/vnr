using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_SalaryJobGroupModel : BaseViewModel
    {
       
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string SalaryClassName = "SalaryClassName";
            public const string Description = "Description";
            public const string Code = "Code";
            public const string SalaryClassTypeName = "SalaryClassTypeName";
        }
    }



    public class Cat_SalaryJobGroupMultiModel
    {
        public Guid ID { get; set; }
        public string SalaryJobGroupName { get; set; }
    }
}
