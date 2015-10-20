using System;
using System.Web.Mvc;
using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Laundry.Models;
using VnResource.Helper.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{
    public class Lau_LaundryRecordController : MainBaseController
    {
        private readonly string _hrm_Lau_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Provider/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LaundryRecordInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Lau_LaundryRecordModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Lau_Service);
                var modelResult = service.Get(_hrm_Lau_Service, "api/LaundryRecord/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Lau_LaundryRecordModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Lau_Service);
            var result = service.Get(_hrm_Lau_Service, "api/LaundryRecord/", id);
            return View(result);
        }

        /// <summary>
        /// Chuyển trạng thái IsDelete = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Lau_LaundryRecordModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Lau_Service);
            var result = service.Remove(_hrm_Lau_Service, "api/LaundryRecord/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Lau_LaundryRecordModel>(_hrm_Lau_Service, "api/LaundryRecord/", selectedIds, ConstantPermission.LaundryRecord, DeleteType.Remove.ToString());
        }
        /// <summary>
        /// Xử lí thay doi trang thai cua ngày công
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>      
        public ActionResult SetStatusSelected(string selectedIds, string status)
        {
            var model = new Lau_LaundryRecordModel();
            if (selectedIds != null)
            {

                var service = new RestServiceClient<Lau_LaundryRecordModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Lau_Service);
                model.ProfileIDs = selectedIds;
                model.Status = status;
                service.Put(_hrm_Lau_Service, "api/LaundryRecord/", model);
            }
            return Json("");
        } 
       
    }
}
