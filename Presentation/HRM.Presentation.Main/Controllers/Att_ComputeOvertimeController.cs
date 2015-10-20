using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Category.Models;
using HRM.Presentation.HrmSystem.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System;
using System.Linq;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_ComputeOvertimeController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        readonly string _hrm_Main_Web = ConstantPathWeb.Hrm_Main_Web;

        public ActionResult Index()
        {
            var serviceCat_Shift = new RestServiceClient<IEnumerable<CatShiftMultiModel>>(UserLogin);
            serviceCat_Shift.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var catShift = serviceCat_Shift.Get(_Hrm_Hre_Service, "api/CatShift/");
            ViewData["Cat_Shift"] = catShift;

            #region [Hien.Nguyen] get data for selectbox edit inline on gird
            var service = new RestServiceClient<IEnumerable<CatOvertimeTypeMultiModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/CatOvertimeType/");
            ViewData["Cat_OvertimeType"] = result;

            var serviceSys = new RestServiceClient<IEnumerable<Sys_UserMultiModel>>(UserLogin);
            serviceSys.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var resultSys = serviceSys.Get(_hrm_Sys_Service, "api/Sys_User/");
            ViewData["Sys_UserInfo"] = resultSys;

            var serviceCat_LeaveDayType = new RestServiceClient<IEnumerable<CatLeaveDayTypeModel>>(UserLogin);
            serviceCat_LeaveDayType.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var resultCat_LeaveDayType = serviceCat_LeaveDayType.Get(_Hrm_Hre_Service, "api/CatLeaveDayType/");
            ViewData["Cat_LeaveDayType"] = resultCat_LeaveDayType;



            #endregion

            return View();
        }
        public ActionResult ComputeOvertime()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetComputeOvertimeList([DataSourceRequest] DataSourceRequest request, Att_ComputeOvertimeModel model)
        {
            var service = new RestServiceClient<IEnumerable<Att_OvertimeModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Post(_Hrm_Hre_Service, "api/Att_ComputeOvertime/", model);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Create([Bind(Prefix = "totalRecord")] List<Att_OvertimeModel> TotalAtt_OvertimeModel, [Bind(Prefix = "models")] List<Att_OvertimeModel> updatedAtt_OvertimeModel)
        {
            List<Att_OvertimeModel> lstReturn = new List<Att_OvertimeModel>();
            Att_OvertimeModel _return = new Att_OvertimeModel();

            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            if (updatedAtt_OvertimeModel != null)
            {

                for (int i = 0; i < updatedAtt_OvertimeModel.Count; i++)
                {
                    #region [Hien.Nguyen] xử lý gộp time vào datetime
                    //Nếu date == null
                    if (updatedAtt_OvertimeModel[i].InTime == null)
                    {
                        updatedAtt_OvertimeModel[i].InTime = updatedAtt_OvertimeModel[i].WorkDate;//gán bằng với workdate
                        if (updatedAtt_OvertimeModel[i].TempTimeIn != null)//Nếu giờ khác null thì gán giờ vào
                        {
                            updatedAtt_OvertimeModel[i].InTime = new DateTime(updatedAtt_OvertimeModel[i].InTime.Value.Year, updatedAtt_OvertimeModel[i].InTime.Value.Month,
                                updatedAtt_OvertimeModel[i].InTime.Value.Day, int.Parse(updatedAtt_OvertimeModel[i].TempTimeIn.Split(':').ToList()[0]),
                                 int.Parse(updatedAtt_OvertimeModel[i].TempTimeIn.Split(new char[] { ':' }).ToList()[1]), int.Parse(updatedAtt_OvertimeModel[i].TempTimeIn.Split(':').ToList()[2]));
                        }
                    }
                    else//Nếu date != null 
                    {
                        if (updatedAtt_OvertimeModel[i].TempTimeIn != null)//Nếu time khác null thì gán time vào cho date
                        {
                            updatedAtt_OvertimeModel[i].InTime = new DateTime(updatedAtt_OvertimeModel[i].InTime.Value.Year, updatedAtt_OvertimeModel[i].InTime.Value.Month,
                               updatedAtt_OvertimeModel[i].InTime.Value.Day, int.Parse(updatedAtt_OvertimeModel[i].TempTimeIn.Split(':').ToList()[0]),
                                int.Parse(updatedAtt_OvertimeModel[i].TempTimeIn.Split(':').ToList()[1]), int.Parse(updatedAtt_OvertimeModel[i].TempTimeIn.Split(':').ToList()[2]));
                        }
                    }
                    if (updatedAtt_OvertimeModel[i].OutTime == null)
                    {
                        updatedAtt_OvertimeModel[i].OutTime = updatedAtt_OvertimeModel[i].WorkDate;//gán bằng với workdate
                        if (updatedAtt_OvertimeModel[i].TempTimeOut != null)//Nếu giờ khác null thì gán giờ vào
                        {
                            updatedAtt_OvertimeModel[i].OutTime = new DateTime(updatedAtt_OvertimeModel[i].OutTime.Value.Year, updatedAtt_OvertimeModel[i].OutTime.Value.Month,
                                updatedAtt_OvertimeModel[i].OutTime.Value.Day, int.Parse(updatedAtt_OvertimeModel[i].TempTimeOut.Split(':').ToList()[0]),
                                int.Parse(updatedAtt_OvertimeModel[i].TempTimeOut.Split(':').ToList()[1]), int.Parse(updatedAtt_OvertimeModel[i].TempTimeOut.Split(':').ToList()[2]));
                        }
                    }
                    else
                    {
                        if (updatedAtt_OvertimeModel[i].TempTimeOut != null)//Nếu time khác null thì gán time vào cho date
                        {
                            updatedAtt_OvertimeModel[i].OutTime = new DateTime(updatedAtt_OvertimeModel[i].OutTime.Value.Year, updatedAtt_OvertimeModel[i].OutTime.Value.Month,
                               updatedAtt_OvertimeModel[i].OutTime.Value.Day, int.Parse(updatedAtt_OvertimeModel[i].TempTimeOut.Split(':').ToList()[0]),
                               int.Parse(updatedAtt_OvertimeModel[i].TempTimeOut.Split(':').ToList()[1]), int.Parse(updatedAtt_OvertimeModel[i].TempTimeOut.Split(':').ToList()[2]));
                        }
                    }
                    #endregion
                    #region [Kiet.Chung] Compare lại code bảng 7 update luỹ kế
                    Att_OvertimeModel DataChange = updatedAtt_OvertimeModel[i];
                    double denta = DataChange.ApproveHours.HasValue ? DataChange.ApproveHours.Value - DataChange.RegisterHours : 0 - DataChange.RegisterHours;
                    List<Att_OvertimeModel> lstUpdate = TotalAtt_OvertimeModel.Where(d => d.ProfileID == DataChange.ProfileID && d.WorkDate.Date >= DataChange.WorkDate.Date).ToList();
                    foreach (var item in lstUpdate)
                    {
                        if (item.WorkDate == DataChange.WorkDate && item.ID == DataChange.ID)
                        {
                            item.RegisterHours = item.RegisterHours + denta;
                        }
                        if (item.WorkDate.Date == DataChange.WorkDate.Date)
                        {
                            if (item.udHourByDate != null)
                                item.udHourByDate = item.udHourByDate + denta;
                        }
                        if (item.WorkDate.Month == DataChange.WorkDate.Month && item.WorkDate.Year == DataChange.WorkDate.Year)
                        {
                            if (item.udHourByMonth != null)
                                item.udHourByMonth = item.udHourByMonth + denta;
                        }
                        if (item.WorkDate.Year == DataChange.WorkDate.Year)
                        {
                            if (item.udHourByYear != null)
                                item.udHourByYear = item.udHourByYear + denta;
                        }

                        lstReturn.Add(item);
                    }
                    // Cần bind lại Gird sau khi update
                    #endregion
                }

                // Lấy nhân viên ko thuộc edit list add vào return
                List<Guid> _lstProId = lstReturn.Select(s => s.ProfileID).Distinct().ToList();
                var temp = TotalAtt_OvertimeModel.Where(d => !_lstProId.Contains(d.ProfileID)).ToList();
                if (temp != null && temp.Count > 0)
                    lstReturn.AddRange(temp);

                foreach (var rs in updatedAtt_OvertimeModel)
                {
                    var lstUp = TotalAtt_OvertimeModel.Where(d => d.ProfileID == rs.ProfileID && d.WorkDate.Date < rs.WorkDate.Date).ToList();
                    if (lstUp != null && lstUp.Count > 0)
                    {
                        lstReturn.AddRange(lstUp);
                    }
                }
            }

            DataSourceRequest request = new DataSourceRequest() {
                Page = 1,
                PageSize = 50
            };
            return Json(lstReturn.ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult CreateAnalysis([Bind(Prefix = "models")]List<Att_OvertimeModel> attOvertime,[Bind(Prefix = "params")]Att_OvertimeModel model)
        {
            Att_OvertimeModel result = new Att_OvertimeModel();
            var  lstAttOvertime = new List<Att_OvertimeModel>();
            if (attOvertime == null || attOvertime.Count() <= 0)
            {
                attOvertime = new List<Att_OvertimeModel>();
                
                if (model!=null)
                    attOvertime.Add(model);
            }
            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);

            if (attOvertime != null)
            {
                var user = Session[SessionObjects.UserId].ToString();
                attOvertime.ForEach(s => s.UserRegister = s.UserCreate = s.UserUpdate = s.UserLoginID = user);
                attOvertime.ForEach(s => s.Host = _hrm_Main_Web);

                result = service.Post(_Hrm_Hre_Service, "api/Att_Overtime/", attOvertime);
           
            }
            return Json(result);
        }

       

    }
}