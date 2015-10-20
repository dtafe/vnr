using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    public class DownloadController : Controller
    {
        //
        // GET: /Download/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult File(string path)
        {
            return View();
        }
	}
}