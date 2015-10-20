using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_VersionModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
    }

}
