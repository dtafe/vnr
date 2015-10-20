using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatLaundryModel : BaseViewModel
    {
        [MaxLength(100)]
        [DisplayName(ConstantDisplay.HRM_Category_Laundry_LaundryName)]
        public string LaundryName { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_HR_Category_Laundry_Code)]
        public string Code { get; set; }

        [MaxLength(500)]
        [DisplayName(ConstantDisplay.HRM_Category_Laundry_Description)]
        public string Description { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Category_Laundry_LaundryPrice)]
        public double LaundryPrice { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string LaundryName = "LaundryName";
            public const string LaundryPrice = "LaundryPrice";
            public const string Description = "Description";
        }
    }

    public class CatLaundrySearchModel
    {
        public string LaundryName { get; set; }
        public string LaundryCode { get; set; }        
    }
    public class CatLaundryModelMultiModel
    {
        public Guid ID { get; set; }
        public string LaundryName { get; set; }
    }
}
