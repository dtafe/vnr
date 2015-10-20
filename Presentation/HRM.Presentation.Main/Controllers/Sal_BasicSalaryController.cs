﻿using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Sal_BasicSalaryController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        // GET: BasicSalary
        public ActionResult Index()
        
        {

            return View();
        }

        public ActionResult BasicSalaryInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Reward);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Reward);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_BasicSalaryModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Sal_BasicSalary/", id1);
                if (result.ProfileID == Guid.Empty)
                    result.ProfileID = id1;
                return View(result);
            }
            else
            {
                if (Request["profileID"] != null)
                {
                    string aaa = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(aaa))
                    {

                        ViewBag.profileID = aaa;

                    }

                }
                return View();
            }
        }
        public ActionResult Create()
        {

            return View();
        }

        public ActionResult Sal_BasicSalaryInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sal_CostCentre);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sal_CostCentre);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_BasicSalaryModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_BasicSalary/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        //public ActionResult RemoveSelected(List<Guid> selectedIds)
        //{
        //    //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Sal_BasicSalary);
        //    //if (!isAccess)
        //    //{
        //    //    return PartialView(ConstantMessages.AccessDenied);
        //    //}
        //    if (selectedIds != null)
        //    {
        //        var service = new RestServiceClient<Sal_BasicSalaryModel>();
        //        service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
        //        service.Delete(_hrm_Hr_Service, "api/Sal_BasicSalary/",String.Join(",",selectedIds));
        //    }
        //    return Json("");

        //}

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sal_BasicSalaryModel>(_hrm_Hr_Service, "api/Sal_BasicSalary/", string.Join(",", selectedIds), ConstantPermission.Sal_BasicSalary, DeleteType.Remove.ToString());
        }


        public ActionResult BasicSalaryPending()
        {
            return View();
        }

        /// <summary>
        /// Xử lí thay doi trang thai 
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>    
        /// 
        public ActionResult SetStatusSelected(List<Guid> selectedIds, string status, string userApproved)
        {
            var model = new Sal_BasicSalaryModel();
            if (selectedIds != null)
            {
                var service = new RestServiceClient<Sal_BasicSalaryModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                model.IDs = String.Join(",", selectedIds);
                model.Status = status;
                //Guid Approved = Guid.Empty;
                //Guid.TryParse(userApproved, out Approved);
                //if (Approved == Guid.Empty)
                //{
                //    model.ActionStatus = "NoPermission";
                //    return Json(model);
                //}
                model = service.Put(_hrm_Hr_Service, "api/Sal_BasicSalary/", model);
            }
            return Json(model);
        }
    }
}