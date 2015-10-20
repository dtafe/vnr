using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace HRM.Presentation.Training.Models
{
    public class Tra_AddPointModel : BaseViewModel
    {
        public DataTable Table { get; set; }
        public string Ids { get; set; }
        public List<Guid> lstIds { get; set; }
    }
}
