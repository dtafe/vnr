using HRM.Infrastructure.Utilities;
using System.Web.Mvc;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Sys_SynDataHrm7Controller : HRM.Presentation.HrmSystem.Web.Controllers.BaseController
    {
       // readonly string _hrm_hr7_Service = ConstantPathWeb.Hrm_Hr7_Service;
        readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Sys_SynDataHrm7/
        public ActionResult Index()
        {
            return View();
        }

        public void SynDataHrm7()
        {
            
            //var service = new RestServiceClient<Hre_DataImportModel>();
            //service.SetCookies(Request.Cookies, _hrm_hr7_Service);
            //var result = service.Get(_hrm_hr7_Service, "api/Hr_Profile/");


            ////if (result.ListWorkPlace.ToList() != null)
            ////{
            ////    var list = new List<CatWorkPlaceModel>();
            ////    foreach (var item in result.ListWorkPlace)
            ////    {
            ////        var data = new CatWorkPlaceModel();
            ////        data = result.ListWorkPlace.CopyData<CatWorkPlaceModel>();
            ////        list.Add(data);
            ////    }
            ////    service.SetCookies(Request.Cookies, _hrm_Hre_Service);
            ////    service.Post<CatWorkPlaceModel>(_hrm_Hre_Service, "api/Hre_DataImport/", list);
            ////}

            //if (result != null)
            //{
            //    //Hre_DataImportModel listDataImport = new Hre_DataImportModel();
            //    //listDataImport.ListProfile = new List<Hre_ProfileModel>();
            //    //listDataImport.ListProfile.AddRange(result);
            //    service.SetCookies(Request.Cookies, _hrm_Hre_Service);
            //    service.Post(_hrm_Hre_Service, "api/Hre_DataImport/", result);
            //}


            ////return View();
            ////return Json(result.ToDataSourceResult(request));
            ////var service = new RestServiceClient<HRM.Presentation.Hr.Models.Hre_ProfileModel>();
            ////service.SetCookies(Request.Cookies, _hrm_hr7_Service);
            ////var result = service.Get(_hrm_hr7_Service, "api/Hr_Profile/");
            
        }
    }
}
