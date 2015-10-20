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
    public class Med_DiseaseModel : BaseViewModel
    {
        public string DiseaseName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public partial class FieldNames
        {
           
        }
    }

    public class Med_DiseaseMultiModel
    {
        public Guid ID { get; set; }
        public string DiseaseName { get; set; }
    }
    
}
