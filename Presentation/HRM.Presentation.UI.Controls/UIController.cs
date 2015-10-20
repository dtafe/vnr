using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HRM.Presentation.UI.Controls
{
    public class UIController
    {
        public static string GetPath(string path)
        {
            if (HttpContext.Current != null && HttpContext.Current.Server != null)
            {
                return HttpContext.Current.Server.MapPath("~/" + path);
            }

            return path;
        }
    }
}
