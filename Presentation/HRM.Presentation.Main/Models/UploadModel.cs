using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Main.Models
{
    public class UploadModel
    {
        public UploadModel()
        {
            Status = false;
        }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public bool Status { get; set; }
    }
}
