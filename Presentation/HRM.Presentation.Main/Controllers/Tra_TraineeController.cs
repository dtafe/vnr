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
    public class Tra_TraineeController : MainBaseController
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
            return RemoveOrDeleteAndReturn<Tra_TraineeModel>(_hrm_Hr_Service, "api/Tra_Trainee/", selectedIds, ConstantPermission.Tra_Trainee, DeleteType.Remove.ToString());
        }

        public ActionResult Edit(string id)
        {
            var service = new RestServiceClient<Tra_TraineeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Tra_Trainee/", id);
            return View(result);
        }

        [HttpPost]
        public JsonResult GetDataAddPoint([DataSourceRequest] DataSourceRequest request, string ids)
        {
            Tra_AddPointModel result = new Tra_AddPointModel();
            List<Guid> selectedIds = ids.Split(',').Select(s => Guid.Parse(s)).ToList();

            if (selectedIds.Count > 0)
            {
                int _total = selectedIds.Count;
                int _totalPage = _total / 5 + 1;
                int _pageSize = 5;
                Tra_AddPointModel dataReturn = new Tra_AddPointModel();
                for (int _page = 1; _page <= _totalPage; _page++)
                {

                    int _skip = _pageSize * (_page - 1);
                    var _listCurrenPage = selectedIds.Skip(_skip).Take(_pageSize).ToList();
                    var _strDelIDs = string.Join(",", _listCurrenPage);
                    var service = new RestServiceClient<Tra_AddPointModel>(UserLogin);
                    service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                    dataReturn = service.Get(_hrm_Hr_Service, "api/Tra_AddPoint/", _strDelIDs);
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
            //var service = new RestServiceClient<Tra_AddPointModel>();
            //service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            //var result = service.Get(_hrm_Hr_Service, "api/Tra_AddPoint/", ids);



            return Json(result.Table.ToDataSourceResult(request));
        }

        public ActionResult AddPoint(string ids)
        {

            List<Guid> selectedIds = ids.Split(',').Select(s => Guid.Parse(s)).ToList();
            Tra_AddPointModel result = new Tra_AddPointModel();
          
            if (selectedIds.Count > 0)
            {
                int _total = selectedIds.Count;
                int _totalPage = _total / 5 + 1;
                int _pageSize = 5;
                Tra_AddPointModel dataReturn = new Tra_AddPointModel();
                for (int _page = 1; _page <= _totalPage; _page++)
                {
                    
                    int _skip = _pageSize * (_page - 1);
                    var _listCurrenPage = selectedIds.Skip(_skip).Take(_pageSize).ToList();
                    var _strDelIDs = string.Join(",", _listCurrenPage);
                    var service = new RestServiceClient<Tra_AddPointModel>(UserLogin);
                    service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                    dataReturn = service.Get(_hrm_Hr_Service, "api/Tra_AddPoint/", _strDelIDs);
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
            //var service = new RestServiceClient<Tra_AddPointModel>();
            //service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
          //  Tra_AddPointModel result = service.Get(_hrm_Hr_Service, "api/Tra_AddPoint/", ids);
            result.Ids = ids;
            return View(result);
        }

        public ActionResult Save([DataSourceRequest] DataSourceRequest request, Dictionary<string,object> model)
        {
            return Json(new object());
        }

        public ActionResult Tra_CostCenterDetailInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Tra_RequirementTrainDetailModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Tra_RequirementTrainDetail/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
    }
}