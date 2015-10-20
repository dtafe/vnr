using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Models
{
    public class EmployeeModel : BaseViewModel
    {
        public string Code { get; set; }
        public string UserName { get; set; }
        [UIHint("Test")]
        public string LoginName { get; set; }
        public string LDAPDatasource { get; set; }

        public bool? IsActivate { get; set; }
    }
}