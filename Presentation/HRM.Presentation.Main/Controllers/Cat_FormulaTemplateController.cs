using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    public class Cat_FormulaTemplateController : Controller
    {
        //
        // GET: /Cat_FormulaTemplate/
        public ActionResult Index(string GradeID)
        {
            return View();
        }

        public ActionResult GridFormulaTemplate()
        {
            return View();
        }
	}
}