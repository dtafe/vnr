using System;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Category.Models
{
    public interface IHRMBaseSearchModel
    {
        string UserName { get; set; }
    }
    public class BaseViewSearchModel : IHRMBaseSearchModel
    {
        public BaseViewSearchModel()
        {
        }
        [MaxLength(50)]
        public virtual string UserName { get; set; }
    }
    
}
