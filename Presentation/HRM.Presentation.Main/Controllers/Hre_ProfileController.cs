using System.IO;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.HrmSystem.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.Attendance.Models;
using System;
using System.Linq;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.Xml.Linq;
using System.Xml;
using HRM.Presentation.Payroll.Models;
using HRM.Presentation.Main.Controllers;
namespace HRM.Presentation.Main.Controllers
{

    public class Hre_ProfileController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Main_Web = ConstantPathWeb.Hrm_Main_Web;
        readonly string _hrm_Eva_Service = ConstantPathWeb.Hrm_Hre_Service;
        // GET: /Hre_Profile/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Hre_Profile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        public ActionResult RelativeInfo(string id, string ProfileName)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Relatives);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Relatives);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_RelativesModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_Relatives/", id1);
                if (modelResult.ProfileID == Guid.Empty)
                    modelResult.ProfileID = id1;
                    modelResult.ProfileName = ProfileName;
                return View(modelResult);
            }
            else
            {

                if (Request["profileID"] != null)
                {
                    string profileid = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(profileid))
                    {
                        ViewBag.profileID = profileid;
                    }
                }
                return View();
            }
        }

        public ActionResult CodeAlocalInfo(string id, string ProfileName)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sal_CodeAlocal);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sal_CodeAlocal);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_CodeAlocalModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_CodeAlocal/", id1);
                if (modelResult.ProfileID == Guid.Empty)
                    modelResult.ProfileID = id1;
                modelResult.ProfileName = ProfileName;
                return View(modelResult);
            }
            else
            {
                if (Request["profileID"] != null)
                {
                    string profileid = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(profileid))
                    {
                        ViewBag.profileID = profileid;
                    }
                }
                return View();
            }
        }

        public ActionResult Sal_CostCentreSalInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sal_CostCentreSal);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sal_CostCentreSal);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_CostCentreSalModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_CostCentreSal/", id1);
                if (modelResult.ProfileID == Guid.Empty || modelResult.ProfileID==null)
                    modelResult.ProfileID = id1;
                //modelResult.ProfileName = ProfileName;
                return View(modelResult);
            }
            else
            {
                if (Request["profileID"] != null)
                {
                    string profileid = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(profileid))
                    {
                        ViewBag.profileID = profileid;
                    }
                }
                return View();
            }
        }

        public ActionResult RemoveSelectedCodeAlocal(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sal_CodeAlocalModel>(_hrm_Hr_Service, "api/Sal_CodeAlocal/", selectedIds, ConstantPermission.Sal_CodeAlocal, DeleteType.Remove.ToString());
        }
        public ActionResult RemoveSelectedSal_CostCentreSal(Guid[] selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sal_CostCentreSalModel>(_hrm_Hr_Service, "api/Sal_CostCentreSal/", string.Join(",",selectedIds), ConstantPermission.Sal_CostCentreSal, DeleteType.Remove.ToString());
        }
        /// <summary>
        /// [Tho.Bui]: Tab thông tin Đảng và Đoàn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public ActionResult Hre_PartyAndUnionView(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetByProfileId(id));
        }

        /// <summary>
        /// Get profile ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public Hre_ProfilePartyUnionModel GetByProfileId(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return null;
            }
            else
            {
                var service = new RestServiceClient<Hre_ProfilePartyUnionModel>(UserLogin);
                var modeltemp = new Hre_ProfilePartyUnionModel();
                modeltemp.ProfileID = (Guid)id;
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_ProfilePartyUnion/", modeltemp);
                return result;
            }
        }

        //public ActionResult Hre_ProfileProcessWorkView(Guid? id)
        //{
        //    bool isAccess;
        //    if (id != Guid.Empty)
        //    {
        //        isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
        //    }
        //    else
        //    {
        //        isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
        //    }
        //    if (!isAccess)
        //    {
        //        return PartialView("AccessDenied");
        //    }
        //    return id == Guid.Empty ? View() : View(GetById(id));
        //}
        public ActionResult Hre_ProfileQualificationView(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        public ActionResult Hre_ProfileBasicView(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        public ActionResult HreHDTJobInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_HDTJob);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_HDTJob);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);


                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_HDTJob/", id1);
                if (modelResult.ProfileID == null)
                    modelResult.ProfileID = id1;
                return View(modelResult);

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
        public ActionResult WorkHistoryInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_WorkHistory);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_WorkHistory);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_WorkHistoryModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);


                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_WorkHistory/", id1);
                if (modelResult.ProfileID == Guid.Empty)
                    modelResult.ProfileID = id1;
                return View(modelResult);

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
        public ActionResult VisaInfoInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_VisaInfo);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_VisaInfo);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_VisaInfoModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);


                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_VisaInfo/", id1);
                if (modelResult.ProfileID == null)
                    modelResult.ProfileID = id1;
                return View(modelResult);

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
        public ActionResult CandidateHistoryInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_CandidateHistory);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_CandidateHistory);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_CandidateHistoryModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);


                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_CandidateHistory/", id1);
                if (modelResult.ProfileID == Guid.Empty)
                    modelResult.ProfileID = id1;
                return View(modelResult);

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
        //public ActionResult Hre_ProfileBasic(Guid? id)
        //{
        //    bool isAccess;
        //    if (id != Guid.Empty)
        //    {
        //        isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
        //    }
        //    else
        //    {
        //        isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
        //    }
        //    if (!isAccess)
        //    {
        //        return PartialView("AccessDenied");
        //    }
        //    return id == Guid.Empty ? View() : View(GetById(id));
        //

        //public ActionResult Hre_ProfilePersonal(Guid? id)
        //{
        //    bool isAccess;
        //    if (id != Guid.Empty)
        //    {
        //        isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
        //    }
        //    else
        //    {
        //        isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
        //    }
        //    if (!isAccess)
        //    {
        //        return PartialView("AccessDenied");
        //    }
        //    return id == Guid.Empty ? View() : View(GetById(id));
        //}

        public ActionResult Hre_ProfilePersonalView(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        public ActionResult Hre_ProfileWorkHistoryView(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_WorkHistory);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_WorkHistory);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        public ActionResult Hre_ProfileLaborForeignView(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_LaborForeign);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_LaborForeign);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        //public ActionResult Hre_ProfileContact(Guid? id)
        //{
        //    bool isAccess;
        //    if (id != Guid.Empty)
        //    {
        //        isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
        //    }
        //    else
        //    {
        //        isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
        //    }
        //    if (!isAccess)
        //    {
        //        return PartialView("AccessDenied");
        //    }
        //    return id == Guid.Empty ? View() : View(GetById(id));
        //}

        public ActionResult Hre_ProfileContactView(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_ProfileRelativeView(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Relatives);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Relatives);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }



        //public ActionResult Hre_ProfileQuit(Guid? id)
        //{
        //    bool isAccess;
        //    if (id != Guid.Empty)
        //    {
        //        isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
        //    }
        //    else
        //    {
        //        isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
        //    }
        //    if (!isAccess)
        //    {
        //        return PartialView("AccessDenied");
        //    }
        //    return id == Guid.Empty ? View() : View(GetById(id));
        //}

        public ActionResult Hre_ProfileQuitView(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

       
        public Hre_ProfileModel GetById(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return null;
            }
            else
            {
                var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/", (Guid)id);
                return result;
            }
        }

        public Att_AttendanceTableModel GetById(Guid id, Guid cutoffId)
        {
            var model = new ProfileIdAndCutOffIdModel()
            {
                ProfileId = id,
                CutOffId = cutoffId
            };
            var service = new RestServiceClient<Att_AttendanceTableModel>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Att_AttendanceTable/", model);
            return result;
        }

        

        /// <summary>
        /// Lấy tất cả dữ liệu trong table Hre_Profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_ProfileModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/");
            
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Hre_ProfileModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_Profile/", model);
               
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }


        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/", id);
            //if (result.StatusVerify == "Invalid")
            //{
            //    ViewBag.MsgInsert = "Test message";
            //}
            //else
            //{
                ViewBag.MsgInsert = "Insert success";
            //}
            return View(result);
        }

       
        /// <summary>
        /// Chuyển trạng thái IsDelete = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Hre_Profile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Hre_Profile/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            if(selectedIds != null)
            {
                var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                RemoveOrDeleteAndReturn<Hre_ProfileModel>(_hrm_Hr_Service, "api/Hre_Profile/", selectedIds, ConstantPermission.Hre_Profile, DeleteType.Remove.ToString());
            }
            return Json("");
        }

        //public ActionResult RemoveSelected(List<Guid> selectedIds)
        //{

        //    var user = new List<Hre_ProfileModel>(UserLogin);
        //    if (selectedIds != null)
        //    {
        //        var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
        //        service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
        //        foreach (var id in selectedIds)
        //        {
        //            service.Delete(_hrm_Hr_Service, "api/Hre_Profile/", id);
        //        }
        //    }
        //    return Json("");
        //}



        public ActionResult RemoveSelectedWorkHistory(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_WorkHistoryModel>(_hrm_Hr_Service, "api/Hre_WorkHistory/", selectedIds, ConstantPermission.Hre_WorkHistory, DeleteType.Remove.ToString());
        }

        public ActionResult RemoveSelectedRelative(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_RelativesModel>(_hrm_Hr_Service, "api/Hre_Relatives/",string.Join(",",selectedIds), ConstantPermission.Hre_Relatives, DeleteType.Remove.ToString());
        }

       
        

        public ActionResult UploadImage(IEnumerable<HttpPostedFileBase> files)
        {           
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var physicalPath = Path.Combine(Server.MapPath("~/Content/images/profiles/"), fileName);
                        file.SaveAs(physicalPath);
                    }
                    return Json(fileName);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        /// <summary>
        /// Tab Bảng Công Chi Tiết
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cutOffId"></param>
        /// <returns></returns>
        public ActionResult Hre_ProfileAttendanceDetail(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Att_AttendanceTableItem);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Att_AttendanceTableItem);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            //return id == Guid.Empty || cutOffId == Guid.Empty ? View() : View(GetById(id, cutOffId));
            //if (!string.IsNullOrEmpty(id))
            //{
            //    var idtemp = Common.ConvertToGuid(id);
            //    var service = new RestServiceClient<Att_AttendanceTableModel>(UserLogin);
            //    service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            //    var result = service.Post(_hrm_Hr_Service, "api/Att_AttendanceTable/", idtemp);
            //    return View(result);
            //}
            //else
            //{
            return View();
            //}
        }

        public ActionResult Hre_ProfileGradeAttendance()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sal_BasicSalary);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }

        public ActionResult Hre_Roster([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Att_Roster);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Att_Roster);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (id != Guid.Empty)
            {
                var service = new RestServiceClient<IEnumerable<Att_RosterModel>>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Post(_hrm_Hr_Service, "api/Att_Roster/", id);
                return Json(result.ToDataSourceResult(request));
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Tab Tăng Ca Chi Tiết
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult Hre_OvertimeDetail(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Att_Overtime);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            ViewBag.ProfileId = id;
            return View();
        }
        public ActionResult GetOvertimeByProfileId([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Att_Overtime);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<IEnumerable<Att_OvertimeModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Att_OvertimeCustom/", id);
                
            return Json(result.ToDataSourceResult(request));
            //return View(result);
        }


        /// <summary>
        /// Tab Khen Thưởng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_RewardDetail([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
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
            if (id != Guid.Empty)
            {
                var service = new RestServiceClient<IEnumerable<Hre_RewardModel>>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_RewardCustom/", id);
                return Json(result.ToDataSourceResult(request));
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Tab Kỹ Luật
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_DisciplineDetail([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Discipline);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Discipline);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (id != Guid.Empty)
            {
                var service = new RestServiceClient<IEnumerable<Hre_DisciplineModel>>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_DisciplineCustom/", id);
                return Json(result.ToDataSourceResult(request));
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Tab Người Thân
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_RelativesDetail([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Relatives);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Relatives);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (id != Guid.Empty)
            {
                var service = new RestServiceClient<IEnumerable<Hre_RelativesModel>>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_RelativesCustom/", id);
                return Json(result.ToDataSourceResult(request));
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Tab công việc nặng nhọc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_HDTJobDetail([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_HDTJob);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_HDTJob);
            //}
         
            if (id != Guid.Empty)
            {
                var service = new RestServiceClient<IEnumerable<Hre_HDTJobModel>>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_HDTJobCustom/", id);
                return Json(result.ToDataSourceResult(request));
            }
            else
            {
                return View();
            }
        }


        //public ActionResult GetByProfileIdGrid([DataSourceRequest] DataSourceRequest request, int id)
        //{
        //    var service = new RestServiceClient<IEnumerable<Att_OvertimeModel>>(UserLogin);
        //    service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
        //    var result = service.Get(_hrm_Hr_Service, "api/Att_OvertimeCustom/", id);
        //    return Json(result.ToDataSourceResult(request));
        
        
        //}


        /// <summary>
        /// Tab WorkDay
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_WorkDayDetail(Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Att_Workday);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Att_Workday);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (id != Guid.Empty)
            {
                var service = new RestServiceClient<Att_AttendanceTableModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Post(_hrm_Hr_Service, "api/Att_WorkDayCustom/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy ds workday by profileid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetWorkDayByProfileId([DataSourceRequest] DataSourceRequest request, Guid id)
        {
           
            var service = new RestServiceClient<IEnumerable<Att_WorkdayModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Att_WorkDayCustom/", id);

            return Json(result.ToDataSourceResult(request));
            //return View(result);
        }

        public ActionResult ComputeAttendance(Att_AttendanceTableModel model)
        {
          
            var service = new RestServiceClient<Att_AttendanceTableModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Post(_hrm_Hr_Service, "api/Att_ComputeAttendance/", model);
            return PartialView(result);
        }

        /// <summary>
        /// Lấy ds TamScanLog by profileid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetTAMScanLogByProfileId([DataSourceRequest] DataSourceRequest request, Guid id)
        {
         
            var service = new RestServiceClient<IEnumerable<Att_TAMScanLogModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Att_TAMScanLog/", id);

            return Json(result.ToDataSourceResult(request));
            //return View(result);
        }

        /// <summary>
        /// [Hieu.Van] 03/06/2014 - View cho Tab TamSanLog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_TAMScanLogView(Guid id)
        {
           
            return View();
        }

        /// <summary>
        /// [Hieu.Van] 03/06/2014 - View cho Tab Roster
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_RosterView(Guid id)
        {
           
            return View();
        }

        public ActionResult Hre_AnnualDetailView(Guid id)
        {
            return View();
        }

        /// <summary>
        /// [Hieu.Van] 03/06/2014 - View cho Tab LeaveDay
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_LeaveDayView(Guid id)
        {
          
            return View();
        }
        public ActionResult Hre_CompensationDetailView(Guid id)
        {
            return View();
        }

        /// <summary>
        /// [Hieu.Van] 03/06/2014 - View cho Tab WorkDay
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_WorkDayView(Guid id)
        {
           
            return View();
        }

        /// <summary>
        /// [Hieu.Van] 03/06/2014 - View cho Tab OverTime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_OvertimeView(Guid id)
        {
           
            return View();
        }

        public ActionResult Hre_WorkHistory([DataSourceRequest] DataSourceRequest request, Guid id)
        {
           
            return View();
        }

        public ActionResult Hre_HDTJob([DataSourceRequest] DataSourceRequest request, Guid id)
        {
          
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        //public ActionResult Hre_CardHistory([DataSourceRequest] DataSourceRequest request, Guid id)
        //{
        //    var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Hre_CardHistory);
        //    if (!isAccess)
        //    {
        //        return PartialView("AccessDenied");
        //    }
        //    return View();
        //}
        public ActionResult Hre_CardHistory(Guid? id)
        {
            
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_ProfileInsuranceView(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_ProfileInsurance(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        public ActionResult Hre_ProfileLaborForeign(Guid? id)
        {
            
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_Contract(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        #region [hien.Nguyen] Cải thiện tốc độ load trang edit
        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_ProfileBasic(Guid? id)
        {
          
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_ProfilePersonal(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_ProfileContact(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        ///// <summary>
        ///// Edit một Record
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult Hre_ProfileLaborForeign(Guid? id)
        //{
        //    var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
        //    if (!isAccess)
        //    {
        //        return PartialView("AccessDenied");
        //    }
        //    var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
        //    service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
        //    var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/", (Guid)id);
        //    //if (result.StatusVerify == "Invalid")
        //    //{
        //    //    ViewBag.MsgInsert = "Test message";
        //    //}
        //    //else
        //    //{
        //    ViewBag.MsgInsert = "Insert success";
        //    //}
        //    return View(result);
        //}
        ///// <summary>
        ///// Edit một Record
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult Hre_ProfileInsurance(Guid? id)
        //{
        //    var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
        //    if (!isAccess)
        //    {
        //        return PartialView("AccessDenied");
        //    }
        //    var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
        //    service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
        //    var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/", (Guid)id);
        //    //if (result.StatusVerify == "Invalid")
        //    //{
        //    //    ViewBag.MsgInsert = "Test message";
        //    //}
        //    //else
        //    //{
        //    ViewBag.MsgInsert = "Insert success";
        //    //}
        //    return View(result);
        //}
        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_ProfileQuit(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_ProfileWorkHistory(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        #endregion

        #region [Tung.Ly 20141020] - đánh giá
        
        #region Doanh Số Mua Vào

        public ActionResult Hre_SaleEvaluation(Guid id)
        {
           
            ViewBag.profileId = id;

            var service = new RestServiceClient<IEnumerable<Eva_SalesTypeMultiModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Eva_Service);
            var salesType = service.Get(_hrm_Eva_Service, "api/Eva_SalesType/");
            ViewData["Eva_SalesType"] = salesType;

            return View();
        }

        #endregion

        #region Doanh Số Bán Ra
        public ActionResult Hre_SaleEvaluationOut(Guid id)
        {
           
            ViewBag.profileId = id;

            var service = new RestServiceClient<IEnumerable<Eva_SalesTypeMultiModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Eva_Service);
            var salesType = service.Get(_hrm_Eva_Service, "api/Eva_SalesType/");
            ViewData["Eva_SalesType"] = salesType;

            return View();
        }
        #endregion

        #region Doanh Số 4S

        public ActionResult Hre_SaleEvaluation4S(Guid id)
        {
          

            ViewBag.profileId = id;

            var service = new RestServiceClient<IEnumerable<Eva_SalesTypeMultiModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Eva_Service);
            var salesType = service.Get(_hrm_Eva_Service, "api/Eva_SalesType/");
            ViewData["Eva_SalesType"] = salesType;

            return View();
        }
        #endregion


        #region [Tin.Nguyen] - Lương
        public ActionResult Hre_BasicSalary()
        {
           
            return View();
        }

        public ActionResult Hre_Paysips() 
        {
            
            return View();
        }
        public ActionResult Hre_PaysCommission()
        {
            return View();
        }
        public ActionResult Hre_UnusualED()
        {
            
            return View();
        }

        public ActionResult Hre_UnusualAllowanceED()
        {

            return View();
        }

        public ActionResult Hre_UnusualPay()
        {
           
            return View();
        }

        public ActionResult Hre_GradeAndAllowance()
        {
           
            return View();
        }
        public ActionResult Hre_SalaryInformationView(Guid id)
        {
           
            ViewBag.profileId = id;

            var service = new RestServiceClient<Sal_SalaryInformationModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var salaryInfo = service.Get(_hrm_Hr_Service, "api/Sal_SalaryInformationCustom/", id);
            //ViewData["Sal_SalaryInformation"] = salaryInfo;

            return View(salaryInfo);
        }

        #endregion

        public ActionResult CreateInlineSaleEvaluation([Bind(Prefix = "models")] Eva_SaleEvaluationModel model)
        {
            var service = new RestServiceClient<Eva_SaleEvaluationModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = new Eva_SaleEvaluationModel();
            result = service.Post(_hrm_Eva_Service, "api/Eva_SaleEvaluation/", model);
            return Json(result);
        }

        public ActionResult RemoveSaleEvaluation(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Eva_SaleEvaluationModel>(_hrm_Eva_Service, "api/Eva_SaleEvaluation/", selectedIds, ConstantPermission.Eva_SaleEvaluation, DeleteType.Remove.ToString());
        }

        public ActionResult Hre_SaleEvaluationInfo(string id,string profileId)
        {
           

            if (!string.IsNullOrEmpty(profileId))
            {
                var profile = GetById(Common.ConvertToGuid(profileId));
                if (profile != null)
                {
                    ViewBag.profileId = profile.ID;
                    ViewBag.profileName = profile.ProfileName;
                }
            }
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Eva_SaleEvaluationModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Eva_Service);
                var modelResult = service.Get(_hrm_Eva_Service, "api/Eva_SaleEvaluation/", id1);
                //modelResult.ProfileID = Guid.Parse(id);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Hre_SaleEvaluation4SInfo(string id, string profileId)
        {


            if (!string.IsNullOrEmpty(profileId))
            {
                var profile = GetById(Common.ConvertToGuid(profileId));
                if (profile != null)
                {
                    ViewBag.profileId = profile.ID;
                    ViewBag.profileName = profile.ProfileName;
                }
            }

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Eva_SaleEvaluationModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Eva_Service);
                var modelResult = service.Get(_hrm_Eva_Service, "api/Eva_SaleEvaluation/", id1);
                //modelResult.ProfileID = Guid.Parse(id);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        
        
        #endregion

        #region [Tho.Bui] Các quá trình công tác
        public ActionResult Hre_RewardView(Guid? id)
        {
          
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_DisciplineView(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_AccidentView(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_CandidateHistoryView(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        #endregion

        public ActionResult ComputeWorkdayPopUp()
        {
            return View();
        }
        public ActionResult TAMScanLogInfo(string id)
        {
          
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Att_TAMScanLogModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Att_TAMScanLog/", id1);
                if (modelResult.ProfileID == null)
                    modelResult.ProfileID = id1;
                return View(modelResult);
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

        public ActionResult RosterInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Att_RosterModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Att_Roster/", id1);
                if (modelResult.ProfileID == null)
                    modelResult.ProfileID = id1;
                return View(modelResult);
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

        public ActionResult LeavedayInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Att_LeaveDayModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Att_Leaveday/", id1);
                if (modelResult.ProfileID == null)
                    modelResult.ProfileID = id1;
                return View(modelResult);
            }
            else
            {
                if (Request["profileID"] != null)
                {
                    string profileID = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(profileID))
                    {
                        ViewBag.profileID = profileID;
                    }
                }
                return View();
            }
        }

        public ActionResult OvertimeInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Att_Overtime/", id1);
                if (modelResult.ProfileID == null)
                    modelResult.ProfileID = id1;
                return View(modelResult);
            }
            else
            {
                if (Request["profileID"] != null)
                {
                    string profileID = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(profileID))
                    {
                        ViewBag.profileID = profileID;
                    }
                }
                return View();
            }
        }

        public ActionResult GradeAndAllowanceInfo(Guid id)
        {
            if (id != Guid.Empty)
            {
                var service = new RestServiceClient<Sal_GradeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_Grade/", id);
                return View(modelResult);
            }
            return View();
        }

        public ActionResult DependantInfo(string id, string ProfileName)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_DependantModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);

                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_Dependant/", id1);
                if (modelResult.ProfileID == Guid.Empty)
                    modelResult.ProfileID = id1;
                modelResult.ProfileName = ProfileName;
                return View(modelResult);
            }
            else
            {
                if (Request["profileID"] != null)
                {
                    string profileid = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(profileid))
                    {
                        ViewBag.profileID = profileid;
                    }
                }
                return View();
            }
        }

        public ActionResult Hre_ProfileDependantView(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult RemoveSelectedDependant(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_DependantModel>(_hrm_Hr_Service, "api/Hre_Dependant/", selectedIds, ConstantPermission.Hre_Dependant, DeleteType.Remove.ToString());
        }

        public ActionResult GradeAttendanceInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Att_GradeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Att_Grade/", id1);
                if (modelResult.ProfileID == null)
                    modelResult.ProfileID = id1;
                return View(modelResult);
            }
            else
            {
                if (Request["profileID"] != null)
                {
                    string profileID = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(profileID))
                    {
                        ViewBag.profileID = profileID;
                    }
                }
                return View();
            }
        }
        //[Tho.Bui] Lí do từ chối
        public ActionResult CreateReasonDeny(string selectedItems)
        {
            if (!string.IsNullOrEmpty(selectedItems))
            {
                ViewData["lstIDs"] = selectedItems;
            }
            return View();
        }

        //[Tho.Bui]-Cập nhật phòng ban
        public ActionResult UpdateOrgProfile(string selectedItems)
        {
            if (!string.IsNullOrEmpty(selectedItems))
            {
                ViewData["lstIDs"] = selectedItems;
            }
            return View();
        }
        public ActionResult Hre_UnusualAllowanceView(Guid? id)
        {
            if(id != Guid.Empty)
            {
                ViewBag.ProfileID = id;
            }
            return View();
        }

        public ActionResult Hre_UnusualEDChildCareView(Guid? id)
        {
            if (id != Guid.Empty)
            {
                ViewBag.ProfileID = id;
            }
            return View();
        }

        public ActionResult Hre_AppendixContractView(Guid? id)
        {
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        #region Đào Tạo + đánh giá
        public ActionResult Hre_TrainningResultView(Guid? id)
        {
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_PerformanceResultView(Guid? id)
        {
            return id == Guid.Empty ? View() : View(GetById(id));
        } 
        #endregion

        public ActionResult MPInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_MPModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_MP/", id1);
                if (modelResult.ProfileID == Guid.Empty)
                    modelResult.ProfileID = id1;
                return View(modelResult);
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

        public ActionResult Hre_ProfileMPView(Guid? id)
        {
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult RemoveSelectedMP(Guid[] selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_MPModel>(_hrm_Hr_Service, "api/Hre_MP/", string.Join(",", selectedIds), ConstantPermission.Hre_MP, DeleteType.Remove.ToString());
        }
    }
}