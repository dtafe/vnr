using HRM.Presentation.Service;
using System;
namespace HRM.Presentation.Category.Models
{
    public class Cat_ExportModel: BaseViewModel
    {
        public Guid ID { get; set; }
        public string ExportName { get; set; }
    }
}
