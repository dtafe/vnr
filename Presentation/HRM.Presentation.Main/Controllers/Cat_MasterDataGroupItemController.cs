using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using HRM.Infrastructure.Security;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using System.IO;
using System;
using System.Linq;
using HRM.Presentation.Main.Controllers;


namespace HRM.Presentation.Main.Web.Controllers
{

    public class Cat_MasterDataGroupItemController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Cat_MasterDataGroup/
        public ActionResult Index()
        {            
            return View();
        }


        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatImportModel>(_hrm_Hr_Service, "api/Cat_MasterDataGroupItem/", selectedIds, ConstantPermission.Cat_MasterDataGroupItem, DeleteType.Remove.ToString());

        }

        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Cat_MasterDataGroupItemModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_MasterDataGroupItem/", id);
            return View(result);
        }


    }
}