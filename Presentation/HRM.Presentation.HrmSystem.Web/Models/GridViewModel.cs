using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Web.Models
{
    public class GridViewModel
    {
        [Required]
        public string Name { get; set; }

        public ConfigViewModel GridModel { get; set; }
    }
}