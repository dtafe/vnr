using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;

using HRM.Infrastructure.Security;
using VnResource.Helper.Security;
using System.IO;
using System;
using System.Linq;
using HRM.Presentation.Training.Models;
using System.Data;
namespace HRM.Presentation.Main.Controllers
{
    public class Att_ChangeRosterGroupController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult GradeInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Att_GradeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Att_Grade/", id1);
                return View(modelResult);
            }
            else
            {

                return View();
            }
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttGrade
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_GradeModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_Grade/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttGrade
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_GradeModel model)
        {
            var service = new RestServiceClient<Att_GradeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_Grade/", model);
            return Json(result);
        }

        public ActionResult Remove(Guid id)
        {
            return View();
            var service = new RestServiceClient<Att_GradeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Att_Grade/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(Att_GradeModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_GradeModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_Grade/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Att_Grade_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_GradeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_Grade/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_GradeModel AttGrade)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_GradeModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_Grade/", AttGrade);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Att_Grade_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Att_GradeModel>(_Hrm_Hre_Service, "api/Att_Grade/", selectedIds, ConstantPermission.Att_Grade, DeleteType.Remove.ToString());
        }



        [HttpPost]
        public JsonResult GetDataChangeRosterGroup([DataSourceRequest] DataSourceRequest request, string ids)
        {
            Att_ChangeRosterGroupTableModel result = new Att_ChangeRosterGroupTableModel();
            List<Guid> selectedIds = new List<Guid>();

            if (!string.IsNullOrEmpty(ids))
            {
                selectedIds = ids.Split(',').Select(s => Guid.Parse(s)).ToList();
                if (selectedIds.Count > 0)
                {
                    int _total = selectedIds.Count;
                    int _totalPage = _total / 5 + 1;
                    int _pageSize = 5;
                    Att_ChangeRosterGroupTableModel dataReturn = new Att_ChangeRosterGroupTableModel();
                    for (int _page = 1; _page <= _totalPage; _page++)
                    {

                        int _skip = _pageSize * (_page - 1);
                        var _listCurrenPage = selectedIds.Skip(_skip).Take(_pageSize).ToList();
                        var _strDelIDs = string.Join(",", _listCurrenPage);
                        var service = new RestServiceClient<Att_ChangeRosterGroupTableModel>(UserLogin);
                        service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                        dataReturn = service.Get(_Hrm_Hre_Service, "api/Att_ChangeRosterGroupTable/", _strDelIDs);
                        if (result.Table == null)
                        {
                            result.Table = new DataTable();
                            if (result.Table.Rows.Count == 0)
                            {
                                result.Table = dataReturn.Table.Clone();
                            }

                        }
                        if (dataReturn != null)
                        {
                            foreach (DataRow i in dataReturn.Table.Rows)
                            {
                                result.Table.ImportRow(i);
                            }
                        }

                        //    result.Table.ImportRow((DataTable)dataReturn);
                        //  result.Table.ImportRow(dataReturn);

                    }
                }
            }
            else {

                var service = new RestServiceClient<Att_ChangeRosterGroupTableModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                result = service.Get(_Hrm_Hre_Service, "api/Att_ChangeRosterGroupTable/", "Create");
            }
           
            //var service = new RestServiceClient<Tra_AddPointModel>();
            //service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            //var result = service.Get(_hrm_Hr_Service, "api/Tra_AddPoint/", ids);



            return Json(result.Table.ToDataSourceResult(request));
        }

        public ActionResult Edit(string ids)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Accident);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }

        public ActionResult DataRosterGroup(string ids)
        {
            List<Guid> selectedIds = new List<Guid>();
            if (!string.IsNullOrEmpty(ids))
            {

                selectedIds = ids.Split(',').Select(s => Guid.Parse(s)).ToList();
            }

            Att_ChangeRosterGroupTableModel result = new Att_ChangeRosterGroupTableModel();

            if (selectedIds.Count > 0)
            {
                int _total = selectedIds.Count;
                int _totalPage = _total / 5 + 1;
                int _pageSize = 5;
                Att_ChangeRosterGroupTableModel dataReturn = new Att_ChangeRosterGroupTableModel();
                for (int _page = 1; _page <= _totalPage; _page++)
                {

                    int _skip = _pageSize * (_page - 1);
                    var _listCurrenPage = selectedIds.Skip(_skip).Take(_pageSize).ToList();
                    var _strDelIDs = string.Join(",", _listCurrenPage);
                    var service = new RestServiceClient<Att_ChangeRosterGroupTableModel>(UserLogin);
                    service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                    dataReturn = service.Get(_Hrm_Hre_Service, "api/Att_ChangeRosterGroupTable/", _strDelIDs);
                    if (result.Table == null)
                    {
                        result.Table = new DataTable();
                        if (result.Table.Rows.Count == 0)
                        {
                            result.Table = dataReturn.Table.Clone();
                        }

                    }
                    if (dataReturn != null)
                    {
                        foreach (DataRow i in dataReturn.Table.Rows)
                        {
                            result.Table.ImportRow(i);
                        }
                    }

                    //    result.Table.ImportRow((DataTable)dataReturn);
                    //  result.Table.ImportRow(dataReturn);

                }
            }
            else {
                var service = new RestServiceClient<Att_ChangeRosterGroupTableModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                result = service.Get(_Hrm_Hre_Service, "api/Att_ChangeRosterGroupTable/", "Create");
            }
            if (result != null)
            {
                result.Ids = string.Empty;
            }



            return View(result);
        }
	}
}