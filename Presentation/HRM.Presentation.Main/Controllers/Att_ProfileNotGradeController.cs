using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_ProfileNotGradeController : Controller
    {
        //
        // GET: /Att_ProfileNotGrade/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Att_ProfileNotGradeInfo(List<string> id)
        {
            return View();
        }
	}

}