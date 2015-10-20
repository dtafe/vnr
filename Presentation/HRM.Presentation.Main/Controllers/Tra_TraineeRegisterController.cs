using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using Kendo.Mvc.Extensions;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using HRM.Presentation.Main.Controllers;
using HRM.Presentation.Training.Models;
using System.Data;
using Kendo.Mvc.UI;
namespace HRM.Presentation.Main.Web.Controllers
{
    public class Tra_TraineeRegisterController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Tra_TraineeRegisterModel>(_hrm_Hr_Service, "api/Tra_TraineeRegister/", selectedIds, ConstantPermission.Tra_Trainee, DeleteType.Remove.ToString());
        }

        public ActionResult Edit(string id)
        {
            var service = new RestServiceClient<Tra_TraineeRegisterModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Tra_TraineeRegister/", id);
            return View(result);
        }

        [HttpPost]
        public JsonResult GetDataAddPoint([DataSourceRequest] DataSourceRequest request, string ids)
        {
            DataTable dt = new DataTable();
            var service = new RestServiceClient<Tra_AddPointModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Tra_AddPoint/", ids);
            return Json(result.Table.ToDataSourceResult(request));
        }

        public ActionResult AddPoint(string ids)
        {
            var service = new RestServiceClient<Tra_AddPointModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            Tra_AddPointModel result = service.Get(_hrm_Hr_Service, "api/Tra_AddPoint/", ids);
            result.Ids = ids;
            return View(result);
        }

        public ActionResult Save([DataSourceRequest] DataSourceRequest request, Dictionary<string,object> model)
        {
            return Json(new object());
        }


        //public ActionResult UpdateStatusSelected(List<Guid> selectedIds, string status)
        //{
        //    Tra_TraineeRegisterModel model = new Tra_TraineeRegisterModel();
        //    model.Status = status;
        //    model.selecteIds = selectedIds;
        //    if (selectedIds.Count >= 0)
        //    {
        //        var service = new RestServiceClient<Tra_TraineeRegisterModel>();
        //        service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
        //        var dataReturn = service.Put(_hrm_Hr_Service, "api/Tra_TraineeRegister/", model);

        //        return Json(dataReturn, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(null);
        //}
        public ActionResult ReasonInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Tra_TraineeRegisterModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Tra_TraineeRegister/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
       
    }
}