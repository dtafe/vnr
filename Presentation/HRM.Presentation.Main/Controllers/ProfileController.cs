using HRM.Presentation.Main.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    public class ProfileController : MainBaseController
    {
        //
        // GET: /Profile/
        public ActionResult Index()
        {
            return View();
        }
	}
}