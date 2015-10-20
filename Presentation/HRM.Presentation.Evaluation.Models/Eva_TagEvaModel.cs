using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_TagEvaModel : BaseViewModel
    {
       
        public partial class FieldNames
        {
            
        }
    }
    public class Eva_TagEvaMultiModel
    {
        public Guid ID { get; set; }
        public string TagEvaName { get; set; }
    }
  
}
