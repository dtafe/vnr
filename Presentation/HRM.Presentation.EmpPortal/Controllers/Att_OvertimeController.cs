using HRM.Business.Attendance.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Category.Models;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Data;
using VnResource.Helper.Security;

namespace HRM.Presentation.EmpPortal.Controllers
{
    public class Att_OvertimeController : BasePortalController
    {
        readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        readonly string _hrm_Main_Web = ConstantPathWeb.Hrm_Main_Web;
        //
        // GET: /Att_Overtime/
        public ActionResult Approve()
        {
            return GetView();
        }

        public ActionResult OvertimeList()
        {
            return GetView();
        }

        public ActionResult Submit()
        {
            var service = new RestServiceClient<IEnumerable<CatOvertimeTypeMultiModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hre_Service);
            var result = service.Get(_hrm_Hre_Service, "api/CatOvertimeType/");
            ViewBag.Cat_OvertimeType = result;

            var service1 = new RestServiceClient<IEnumerable<Sys_UserMultiModel>>(UserLogin);
            service1.SetCookies(Request.Cookies, _hrm_Hre_Service);
            var result1 = service1.Get(_hrm_Sys_Service, "api/Sys_User/");
            ViewBag.Sys_User = result1;
            return GetView();
        }
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
            string status = string.Empty;
            var model = new Att_OvertimeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_OvertimeEntity>(id, ConstantSql.hrm_att_sp_get_OvertimeById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_OvertimeModel>(UserLogin);
            }
            model.ActionStatus = status;
            return GetOnlyView(model);
        }
        //private static Guid IdOvertime;
        //public string Save(Att_OvertimeModel model)
        //{
        //    if (IdOvertime != Guid.Empty)
        //    {
        //        model.ID = IdOvertime;
        //    }
        //    ActionService service = new ActionService(UserLogin);
        //    service.UpdateOrCreate<Att_OvertimeEntity, Att_OvertimeModel>(model);
        //    IdOvertime = model.ID;
        //    if (model.ID != Guid.Empty)
        //    {
        //        return model.ID.ToString();
        //    }
        //    return model.ActionStatus;
        //}



        public void SetStatusSelectedWithReason(string selectedIds, string status, string reason)
        {
            if (!string.IsNullOrEmpty(selectedIds))
            {
                selectedIds = Common.DotNetToOracle(selectedIds);

                BaseService service = new BaseService();
                string statusMessages = string.Empty;
                List<object> lstObj = new List<object>();
                lstObj.Add(selectedIds);
                lstObj.Add(status);
                lstObj.Add(reason);
                service.UpdateData<Att_OvertimeModel>(lstObj, ConstantSql.hrm_att_sp_Set_OvertimePortal_Status, ref statusMessages);
            }
        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            if (selectedIds.Count >= 0)
            {
                BaseService service = new BaseService();
                var status = string.Empty;
                for (int i = 0; i < selectedIds.Count; i++)
                {
                    status = service.Remove<Att_OvertimeEntity>(selectedIds[i]);
                    if (status.StartsWith("Error"))
                    {
                        break;
                    }
                }
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        [HttpPost]
        public string Edit(Att_OvertimeModel model)
        {
            model = Validate<Att_OvertimeModel>(model, "Att_Overtime");
            if (!string.IsNullOrEmpty(model.ActionStatus))
            {
                if (model != null)
                {
                    ActionService service = new ActionService(UserLogin);
                    service.UpdateOrCreate<Att_OvertimeEntity, Att_OvertimeModel>(model);
                    if (model.ID != Guid.Empty)
                    {
                        return model.ID.ToString();
                    }
                }
            }
            return model.ActionStatus;
        }

        public void SetStatusSelected(string selectedIds, string status)
        {
            if (!string.IsNullOrEmpty(selectedIds))
            {
                selectedIds = Common.DotNetToOracle(selectedIds);
                BaseService service = new BaseService();
                string statusMessages = string.Empty;
                List<object> lstObj = new List<object>();
                lstObj.Add(selectedIds);
                lstObj.Add(status);
                lstObj.Add(null);
                service.UpdateData<Att_OvertimeModel>(lstObj, ConstantSql.hrm_att_sp_Set_Overtime_Status, ref statusMessages);
            }
        }

        public ActionResult Save([Bind(Prefix = "models")]List<Att_OvertimeModel> attOvertime, [Bind(Prefix = "params")]Att_OvertimeModel model)
        {
            model = Validate<Att_OvertimeModel>(model, "Att_Overtime");
            if (!string.IsNullOrEmpty(model.ActionStatus))
            {
                if (attOvertime == null || attOvertime.Count() <= 0)
                {
                    attOvertime = new List<Att_OvertimeModel>();
                    if (model != null)
                        attOvertime.Add(model);
                }
                if (attOvertime != null)
                {
                    foreach (var item in attOvertime)
                    {
                        item.WorkDate = Common.JoinTimeInDate(item.WorkDate, item.WorkDateTime);
                        item.WorkDateRoot = item.WorkDate;
                        ActionService service = new ActionService(UserLogin);
                        service.UpdateOrCreate<Att_OvertimeEntity, Att_OvertimeModel>(item);
                    }
                }
            }
            return Json(attOvertime);
        }

        [HttpPost]
        public ActionResult CreateAnalysis([Bind(Prefix = "models")]List<Att_OvertimeModel> attOvertime, [Bind(Prefix = "params")]Att_OvertimeModel model)
        {
            Att_OvertimeModel result = new Att_OvertimeModel();
            var lstAttOvertime = new List<Att_OvertimeModel>();
            if (attOvertime == null || attOvertime.Count() <= 0)
            {
                attOvertime = new List<Att_OvertimeModel>();

                if (model != null)
                    attOvertime.Add(model);
            }
            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hre_Service);

            if (attOvertime != null)
            {
                var user = Session[SessionObjects.UserId].ToString();
                attOvertime.ForEach(s => s.UserRegister = s.UserCreate = s.UserUpdate = s.UserLoginID = user);
                attOvertime.ForEach(s => s.Host = _hrm_Main_Web);


                result = service.Post(_hrm_Hre_Service, "api/Att_Overtime/", attOvertime);
            }
            return Json(result);
        }

    }
}