using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.UI.Controls.Menu;
using HRM.Infrastructure.Utilities;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Linq;
using HRM.Presentation.Main.Models;
using System.Xml.Linq;
using System;
using System.Collections;
using Kendo.Mvc.UI;
using System.Reflection;
namespace HRM.Presentation.Main.Controllers
{
    public class DashboardModel
    {
        public DashboardAlertsModel AlertModel { get; set; }
        public List<string> DashboardSetting { get; set; }
        public List<string> DashboardAll { get; set; }
    }

    public class HomeController : MainBaseController
    {
        readonly string ChildCheck = "ChildCheck";
        readonly string NodeParent = "NodeParent";
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Main_Web = ConstantPathWeb.Hrm_Main_Web;
        readonly string _hrm_Hr7_Service = ConstantPathWeb.Hrm_Hr7_Service;

        public ActionResult SetTempData(string controller)
        {
            TempData["PreController"] = controller;

            return null;
        }
        public ActionResult Index()
        {
            var sessionId = Session["userId"];
            if (sessionId == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        #region Menu - Module - Dashboard
        [HttpGet]
        public ActionResult ModuleOptionView()
        {
            var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
            var result = service.Get(_hrm_Sys_Service, "api/MenuOption/", Constant.HRM_CONFIG_MODULE_OPTION);
            return View(result);
        }

        [HttpPost]
        public ActionResult ModuleOptionView(string option)
        {
            Sys_AllSettingModel model = new Sys_AllSettingModel();
            model.Name = Constant.HRM_CONFIG_MODULE_OPTION;
            model.Value1 = option;
            return Json(SaveConfig(model));
        }

        [HttpGet]
        public ActionResult MenuOptionView()
        {
            var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
            var result = service.Get(_hrm_Sys_Service, "api/MenuOption/", Constant.HRM_CONFIG_MENU_OPTION);
            return View(result);
        }

        [HttpPost]
        public ActionResult MenuOptionView(string parent, string child)
        {
            if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(child))
            {
                parent = null;
                child = null;
            }
            Sys_AllSettingModel model = new Sys_AllSettingModel();
            model.Name = Constant.HRM_CONFIG_MENU_OPTION;
            model.Value1 = parent;
            model.Value2 = child;
            return Json(SaveConfig(model));
        }

        [HttpGet]
        public ActionResult DashboardOptionView()
        {

        

            DashboardModel dashboardModel = new DashboardModel();
            var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
            var result = service.Get(_hrm_Sys_Service, "api/MenuOption/", Constant.HRM_CONFIG_DASHBOARD_OPTION);
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.Value1))
                {
                    dashboardModel.DashboardSetting = result.Value1.Split(',').ToList();
                }
            }
            List<string> listString = new List<string>();
            IList<SelectListItem> listEnum = Enum.GetValues(typeof(DashboardOptionEnum))
            .Cast<DashboardOptionEnum>()
            .Select(x => new SelectListItem { Value = x.ToString() })
            .ToList();
            foreach (var item in listEnum)
            {
                listString.Add(item.Value);
            }
            dashboardModel.DashboardAll = listString;
            return View(dashboardModel);
        }


        [HttpPost]
        public ActionResult DashboardOptionView(string text)
        {
            Sys_AllSettingModel model = new Sys_AllSettingModel();
            model.Name = Constant.HRM_CONFIG_DASHBOARD_OPTION;
            model.Value1 = text;
            return Json(SaveConfig(model));
        }

        #endregion
        //Hàm translate cho javascript
        public string Translate(string text)
        {
            return text.TranslateString();
        }
        public ActionResult CreateTemplate()
        {
            return View();
        }
        public void CreateSession(string parent, string childCheck)
        {
            Session[NodeParent] = parent;
            Session[ChildCheck] = childCheck;
        }
        public string GetChildCheckSession()
        {
            if (Session[ChildCheck] != null && Session[ChildCheck] != "")
            {
                return Session[ChildCheck].ToString();
            }

            return null;
        }

        #region Get - Save Config
        public Sys_AllSettingModel GetConfig(string key)
        {
            var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
            var result = service.Get(_hrm_Sys_Service, "api/MenuOption/", key);
            return result;
        }

        public Sys_AllSettingModel SaveConfig(Sys_AllSettingModel model)
        {
            var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
            var dbModel = service.Get(_hrm_Sys_Service, "api/MenuOption/", model.Name);
            if (dbModel != null)
            {
                model.ID = dbModel.ID;
            }
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_AllSetting/", model);

            return model;
        }
        #endregion

        public ActionResult GetDataTreeview()
        {
            if (Session != null && Session[NodeParent] != null)
            {
                //Key của node cha đang được chọn
                var strParent = Session[NodeParent].ToString();
                //Tất cả dữ liệu của menu đang được chọn
                var menus = (XmlSiteMap)HttpContext.Cache["SiteMap"];
                if (menus != null && menus.RootNode != null && menus.RootNode.ChildNodes != null && !string.IsNullOrEmpty(strParent))
                {
                    //Load tất cả node con theo node cha strParent
                    var nodeParent = menus.RootNode.ChildNodes.FirstOrDefault(d => d.NopResource == strParent);
                    //Danh sách node con sẽ trả về
                    if (nodeParent != null)
                    {
                        var nodes = nodeParent.ChildNodes;
                        //Biến lưu tat cả các node con
                        var smNodes = new UI.Controls.Menu.SiteMapNode();
                        if (nodes != null)
                        {
                            object listChildNodes = null;
                            //Duyệt qua 4 cấp và lưu tat cả các node con
                            foreach (var node in nodes)
                            {
                                if (node.ChildNodes != null)
                                {
                                    var nodes1 = node.ChildNodes;
                                    if (nodes1.Count == 0)
                                    {
                                        smNodes.ChildNodes.Add(node);
                                    }
                                    foreach (var node1 in nodes1)
                                    {
                                        if (node1.ChildNodes != null)
                                        {
                                            var nodes2 = node1.ChildNodes;
                                            if (nodes2.Count == 0)
                                            {
                                                smNodes.ChildNodes.Add(node1);
                                            }
                                            foreach (var node2 in nodes2)
                                            {
                                                smNodes.ChildNodes.Add(node2);
                                            }
                                        }
                                    }
                                }
                            }
                            //Xử lý ghi nhớ các menu đã chọn trong config
                            var listChildCheck = new List<string>();
                            //Sesion này lưu tất cả các node đang chọn ở thời điểm hiện tại
                            if (Session[ChildCheck] != null && (string)Session[ChildCheck] != "")
                            {
                                listChildCheck = Session[ChildCheck].ToString().Split(',').ToList();
                            }
                            //Nếu hiện tài không có check thì xuống db để lấy config và kiểm tra
                            else
                            {
                                var sessionConfig = Session[Constant.HRM_CONFIG_MENU_OPTION];
                                Sys_AllSettingModel getConfig = null;
                                //Xử lý lưu config vào session đe không xuống db nhiều lần
                                if (sessionConfig != null)
                                {
                                    var modelConfig = (Sys_AllSettingModel)sessionConfig;
                                    if (!string.IsNullOrEmpty(modelConfig.Value1) && !string.IsNullOrEmpty(modelConfig.Value2))
                                    {
                                        getConfig = modelConfig;
                                    }
                                }
                                else
                                {
                                    getConfig = GetConfig(Constant.HRM_CONFIG_MENU_OPTION);
                                    Session[Constant.HRM_CONFIG_MENU_OPTION] = getConfig;
                                }
                                if (getConfig != null)
                                {
                                    var strValue1 = getConfig.Value1;
                                    var strValue2 = getConfig.Value2;
                                    if (!string.IsNullOrEmpty(strValue1))
                                    {
                                        var indexParent = strValue1.IndexOf(strParent, System.StringComparison.Ordinal);
                                        if (!string.IsNullOrEmpty(strValue2) && indexParent >= 0)
                                        {
                                            var splitValue2 = strValue2.Split('|');
                                            if (splitValue2.Length > indexParent)
                                            {
                                                listChildCheck = splitValue2[indexParent].Split(',').ToList();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Session[Constant.HRM_CONFIG_MENU_OPTION] = null;
                                }
                                Session[ChildCheck] = string.Join(",", listChildCheck);
                            }
                            //Lấy danh sách dữ liệu cần để trả về
                            listChildNodes = from e in smNodes.ChildNodes
                                             where !string.IsNullOrWhiteSpace(e.NopResource)
                                             select new
                                             {
                                                 id = e.NopResource,
                                                 Name = e.NopResource.TranslateString(),
                                                 hasChildren = false,
                                                 @checked = listChildCheck.Contains(e.NopResource)
                                             };
                            //Làm trống session sau mỗi lần xử lý
                            Session[NodeParent] = null;

                            return Json(listChildNodes, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return null;
        }

        public ActionResult Dashboard()
        {
            //   var isAllow = HasPermission(PrivilegeType.View, ConstantPermission.Sys_DataPermission);

            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_DataPermission);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}
            var alertModel = AlertsView();
            DashboardModel dashboardModel = new DashboardModel();
            dashboardModel.AlertModel = alertModel;
            var result = GetConfig(Constant.HRM_CONFIG_DASHBOARD_OPTION);
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.Value1))
                {
                    dashboardModel.DashboardSetting = result.Value1.Split(',').ToList();
                }
            }
            return View(dashboardModel);
        }

        /// <summary>
        /// [Hieu.Van] Lấy dữ liệu cho Alert
        /// </summary>
        /// <returns></returns>
        public DashboardAlertsModel AlertsView()
        {
            var service = new RestServiceClient<DashboardAlertsModel>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Dashboard/");

            return result;
        }

        /// <summary>
        /// [Hieu.Van] Lấy dữ liệu cho Information
        /// </summary>
        /// <returns></returns>
        public ActionResult MyInformationView()
        {
            return View();
        }

        /// <summary>
        /// [Hieu.Van] Lấy dữ liệu cho LeavesSummary
        /// </summary>
        /// <returns></returns>
        public ActionResult LeavesSummaryView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSubOfMenu(string siteName)
        {
            var siteMap = new XmlSiteMap();
            if (HttpContext.Cache["SiteMap"] == null)
            {
                siteMap.LoadFrom("~/Mvc.sitemap");
                HttpContext.Cache["SiteMap"] = siteMap;
            }
            else if (HttpContext.Cache["ChangeLanguage"] != null && HttpContext.Cache["ChangeLanguage"].ToString() == bool.TrueString)
            {
                siteMap.LoadFrom("~/Mvc.sitemap");
                HttpContext.Cache["SiteMap"] = siteMap;
                HttpContext.Cache["ChangeLanguage"] = false;
            }
            else
            {
                siteMap = (HRM.Presentation.UI.Controls.Menu.XmlSiteMap)HttpContext.Cache["SiteMap"];
            }
            var listMenu = siteMap.RootNode.ChildNodes.Where(w => w.ModuleName == siteName).FirstOrDefault();
            if (listMenu == null)
                return null;
            return Json(listMenu.ChildNodes);

        }

        /// <summary>
        /// [Hieu.Van] Hàm lấy html menu từ Sys_AllSetting
        /// </summary>
        /// <param name="enumKey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMenu(string enumKey)
        {
            //var baseService = new BaseService();
            //List<object> lstObject = new List<object>();
            //string status = string.Empty;
            //lstObject.Add(enumKey);
            //lstObject.Add(null);
            //lstObject.Add(null);

            //var result = baseService.GetData<Sys_AllSettingModel>(lstObject, ConstantSql.hrm_sys_sp_get_AllSetting, ref status);

            return Json(null);

        }

        #region Login action

        public ActionResult Login()
        {
            //var service = new RestServiceClient<LoginModel>();
            //service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            //var result = service.Get(_hrm_Sys_Service, "api/Login/", string.Empty);

            return View();
        }

        public ActionResult LoginSession()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Guid userId = Guid.Empty;
            if (Session[SessionObjects.UserId] != null)
            {
                Guid.TryParse(Session[SessionObjects.UserId].ToString(), out userId);
            }
            var service = new RestServiceClient<LoginModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/LogOut/", userId);
            Session.Abandon();

            return RedirectToAction("Login", new { Logout = true });
        }

        public void SetWidth(int width)
        {
            Session[SessionObjects.ScreenWidth] = width;
        }

        public void SetLanguageCode(string _languageCode)
        {
            Session[SessionObjects.LanguageCode] = _languageCode;
        }
        //[HttpPost]
        public ActionResult CheckLogin(LoginModel model)
        {
            //Xử lý user default



            //Goal(Xử lý login)
            //Steps :
            //    Step1  :  Lấy userName và pass , mã hoá Password (MD5)
            //    Step2  :  Login 
            //    Step3  :  Nếu login thành cong => lưu session 
            #region Tung.Ly : Xử lý login

            // Lấy UserName
            var userName = model.UserName;
            var password = model.Password;
            var passwordEncrypt = string.Empty;
            var isSucceed = false;
            var isRunScriptNew = false;
            var language = model.Language;
            //Mã hoá Pass
            passwordEncrypt = EncryptUtil.MD5(password);//mã hoá giống bản 7
            //  passwordEncrypt = DataHelper.Encrypt2MD5(password, "!@#");

            //Set ngôn ngữ
            Session[SessionObjects.LanguageCode + userName] = language;
            VnResource.Helper.Utility.LanguageHelper.LanguageCode = language;

            //step 2 : login
            if (LoginValidate(userName, passwordEncrypt, model.GoogleMail, model.GoogleProfileName, model.IsGoogleSign, out model))
            {

                //nếu login thành công
                TempData[SessionObjects.UserId] = model.ID;
                Session[SessionObjects.UserId] = model.ID;
                Session[SessionObjects.LoginUserName] = model.UserName;
                Session[SessionObjects.FullName] = model.FullName;
                Session[SessionObjects.UserInfoName] = model.UserInfoName;
                Session[SessionObjects.ProfileID] = model.ProfileID;
                Session[SessionObjects.HasMasterDataGroup] = model.HasMasterDataGroup;
                isSucceed = !model.IsRunScriptNew;
                isRunScriptNew = model.IsRunScriptNew;
            }
            #endregion

            if (!isSucceed && Common.UserNameSystem == model.UserName && Common.PasswordSystem == password)
            {

                Session[SessionObjects.UserId] = Common.UserNameSystem;
                Session[SessionObjects.LoginUserName] = Common.UserNameSystem;
                Session[SessionObjects.FullName] = Common.UserNameSystem;
                Session[SessionObjects.UserInfoName] = Common.UserNameSystem;
                Session[SessionObjects.ProfileID] = Common.ProfileID;
                

                if (model.IsRunScriptNew)
                    model = RunScript(model);// goi ham chay script moi
                
                //Session[SessionObjects.UserId] = Common.PasswordSystem;
                if (model.IsRunScriptNew && model.Execption != null && model.Execption.Count > 0)
                    return Json(new { succeed = true, IsRunScriptNew = model.IsRunScriptNew, Name = model.Name, Execption = model.Execption });
                else
                    return Json(new { succeed = true });

            }

   
            return Json(new { succeed = isSucceed, hasMasterDataGroup = model.HasMasterDataGroup, IsRunScriptNew = isRunScriptNew });
        }
        #endregion

        #region TestPerformace
        //[HttpPost]
        public ActionResult doTestPerformace(TestPerformanceModel model)
        {
            LoginModel _LoginModel = new LoginModel();
            var userName = model.UserName;
            var password = model.PassWord;
            var passwordEncrypt = string.Empty;
            passwordEncrypt = EncryptUtil.MD5(password);//mã hoá giống bản 7
            model.IsSusscess = true;
            if (LoginValidate(userName, passwordEncrypt, _LoginModel.GoogleMail, _LoginModel.GoogleProfileName, _LoginModel.IsGoogleSign, out _LoginModel))
            {
                model.IsSusscess = true;
            }
            else
            {
                model.IsSusscess = false;
            }

            return Json(model);
        }
        #endregion

        #region Methods
        string GetVesionLastNameFile()
        {
            try
            {
                var di = new DirectoryInfo(@Common.ScriptPath);
                var vesionLastNameFile = di.GetFiles().OrderByDescending(s => s.CreationTime).Select(s => s.Name).FirstOrDefault();
                return vesionLastNameFile;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        //son.vo - tạm thời comment lại do lỗi - 20150528
        LoginModel RunScript(LoginModel model)
        {
            var service = new RestServiceClient<LoginModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put<LoginModel>(_hrm_Sys_Service, "api/Login/", model);

            
            return result;
        }
        /// <summary> Validate login </summary>
        /// <param name="userName"></param>
        /// <param name="passwordEncrypt"></param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public bool LoginValidate(string userName, string passwordEncrypt, string googleMail, string googleProfileName, bool isGoogleSign, out LoginModel loginInfo)
        {
            var isSuccess = false;

            loginInfo = new LoginModel
            {
                UserName = userName,
                Password = passwordEncrypt,
                GoogleMail = googleMail,
                GoogleProfileName = googleProfileName,
                IsGoogleSign = isGoogleSign
            };
            var service = new RestServiceClient<LoginModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Post(_hrm_Sys_Service, "api/Login/", loginInfo);
            if (result != null)
            {
                if (result.IsActive)
                {
                    isSuccess = true;
                }
                loginInfo = result;
            }

            return isSuccess;
        }

        #endregion

        public ActionResult SaveFileUpload(IEnumerable<HttpPostedFileBase> files, string formatString)
        {
            var model = new UploadModel();
            if (files != null)
            {
                foreach (var file in files)
                {
                    var _fileName = file.FileName;
                    if (!string.IsNullOrEmpty(formatString))
                    {
                        string fileExt = Path.GetExtension(_fileName);
                        string filename = Path.GetFileNameWithoutExtension(_fileName);
                        //string fileSuffix = DateTime.Now.ToString("ddmmyyyyHHmmss");
                        _fileName = filename + formatString + fileExt;
                    }
                    var fileName = Common.TemplateURL + "\\" + _fileName;
                    fileName = fileName.Replace("/", "");
                    //var physicalPath = Path.Combine(Server.MapPath("~/"+Common.TemplateURL), fileName);
                    var physicalPath = Common.GetPath(fileName);
                    file.SaveAs(physicalPath);

                    string fileUrl = _hrm_Main_Web + fileName;
                    fileUrl = fileUrl.Replace("\\", "/");
                    model.FileName = _fileName; //file.FileName;
                    model.FileUrl = fileUrl;
                    model.Status = true;
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFile(string[] fileNames)
        {
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Common.GetPath(fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult GetFileInfo(string path, string fileNames)
        {
            if (!string.IsNullOrEmpty(fileNames))
            {
                List<FileInfoModel> listFileInfo = new List<FileInfoModel>();
                var fileNameSplip = fileNames.Split(',');
                foreach (var fileName in fileNameSplip)
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        FileInfoModel fileInfo = new FileInfoModel();
                        fileInfo.name = fileName;

                        var urlFile = path + fileName;
                        var fullPath = Common.GetPath(urlFile);
                        if (System.IO.File.Exists(fullPath))
                        {
                            fullPath = fullPath.Replace("/", "\\");
                            FileInfo info = new FileInfo(fullPath);
                            long size = info.Length;
                            fileInfo.size = size;
                        }
                        listFileInfo.Add(fileInfo);
                    }
                }

                return Json(listFileInfo, JsonRequestBehavior.AllowGet);
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult SaveImageUpload(IEnumerable<HttpPostedFileBase> files)
        {
            var model = new UploadModel();
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/" + Common.ImagesURL), fileName);
                    file.SaveAs(physicalPath);

                    string fileUrl = _hrm_Main_Web + Common.ImagesURL + fileName;
                    model.FileName = fileName;
                    model.FileUrl = fileUrl;
                    model.Status = true;
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VrnValidator(string screenName)
        {
            XElement xValidate = XElement.Load(Common.GetPath("VALIDATOR.XML"));
            var data = xValidate.Elements("Screen")
                .Where(w => w.Attributes().First().Value.Equals(screenName));
            if (data.Count() > 0)
            {
                var result = new Dictionary<string, string>();
                foreach (var item in data.Descendants().Attributes())
                {
                    result.Add(item.Name.LocalName, item.Value);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult GetFieldRequired(string tableName)
        {
            var xmlFileSpec = Common.GetPath(@"Settings\FIELD_INFO_SPEC.XML");
            var xmlFile = Common.GetPath(@"Settings\FIELD_INFO.XML");
            List<HRM.Infrastructure.Security.DataFieldInfo> listInfo = null;
            bool checkSpect = true;
            if (!System.IO.File.Exists(xmlFileSpec))
            {
                checkSpect = false;
            }
            if (checkSpect)
            {
                //Load file XML
                List<HRM.Infrastructure.Security.DataFieldInfo> listAllFieldInfoSpec = Common.GetDataFromXml<HRM.Infrastructure.Security.DataFieldInfo>(xmlFileSpec, "Field");
                listInfo = listAllFieldInfoSpec.Where(d => d.TableName == tableName).ToList();
            }
            if (System.IO.File.Exists(xmlFile) && (listInfo == null || listInfo.Count == 0))
            {
                List<HRM.Infrastructure.Security.DataFieldInfo> listAllFieldInfo = Common.GetDataFromXml<HRM.Infrastructure.Security.DataFieldInfo>(xmlFile, "Field");
                //Lọc theo table cần validate
                listInfo = listAllFieldInfo.Where(d => d.TableName == tableName).ToList();
            }
            if (listInfo != null && listInfo.Count > 0)
            {
                return Json(listInfo, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        private static List<string> ListResultPermission = new List<string>();
        public ActionResult LoadMenu()
        {
            //try
            //{
            #region Load Menu
            var siteMap = new HRM.Presentation.UI.Controls.Menu.XmlSiteMap();
            if (HttpContext.Cache["SiteMap"] == null)
            {
                var serviceMaster = new RestServiceClient<List<Rep_MasterModel>>(UserLogin);
                serviceMaster.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                siteMap.LoadFrom("~/Mvc.sitemap");

                var listMaster = serviceMaster.Get(_hrm_Sys_Service, "api/Rep_Control/", Guid.NewGuid());
                UI.Controls.Menu.SiteMapNode _tmp;
                if (listMaster != null && listMaster.Count > 0)
                {
                    foreach (var i in siteMap.RootNode.ChildNodes)
                    {
                        if (i.ChildNodes.Count > 0)
                        {
                            switch (i.HrmWebSite)
                            {
                                case "Hrm_Rec_Web":
                                    _tmp = CreateMenuForCustomReport(siteMap, listMaster, ModuleName.Recruitment, i.ChildNodes.FirstOrDefault().Alias);
                                    if (_tmp != null)
                                    {
                                        i.ChildNodes.Add(_tmp);
                                    }
                                    break;
                                case "Hrm_Eva_Web":
                                    _tmp = CreateMenuForCustomReport(siteMap, listMaster, ModuleName.Evaluation, i.ChildNodes.FirstOrDefault().Alias);
                                    if (_tmp != null)
                                    {
                                        i.ChildNodes.Add(_tmp);
                                    }
                                    break;
                                case "Hrm_Main_Web":
                                    _tmp = CreateMenuForCustomReport(siteMap, listMaster, ModuleName.HR, i.ChildNodes.FirstOrDefault().Alias);
                                    if (_tmp != null)
                                    {
                                        i.ChildNodes.Add(_tmp);
                                    }
                                    break;
                                case "Hrm_Att_Web":
                                    _tmp = CreateMenuForCustomReport(siteMap, listMaster, ModuleName.Attendance, i.ChildNodes.FirstOrDefault().Alias);
                                    if (_tmp != null)
                                    {
                                        i.ChildNodes.Add(_tmp);
                                    }
                                    break;
                                case "Hrm_Sal_Web":
                                    _tmp = CreateMenuForCustomReport(siteMap, listMaster, ModuleName.Salary, i.ChildNodes.FirstOrDefault().Alias);
                                    if (_tmp != null)
                                    {
                                        i.ChildNodes.Add(_tmp);
                                    }
                                    break;
                                case "Hrm_Can_Web":
                                    _tmp = CreateMenuForCustomReport(siteMap, listMaster, ModuleName.Canteen, i.ChildNodes.FirstOrDefault().Alias);
                                    if (_tmp != null)
                                    {
                                        i.ChildNodes.Add(_tmp);
                                    }
                                    break;
                                case "Hrm_Lau_Web":
                                    _tmp = CreateMenuForCustomReport(siteMap, listMaster, ModuleName.Laundry, i.ChildNodes.FirstOrDefault().Alias);
                                    if (_tmp != null)
                                    {
                                        i.ChildNodes.Add(_tmp);
                                    }
                                    break;
                                case "Hrm_Cat_Web":
                                    _tmp = CreateMenuForCustomReport(siteMap, listMaster, ModuleName.Category, i.ChildNodes.FirstOrDefault().Alias);
                                    if (_tmp != null)
                                    {
                                        i.ChildNodes.Add(_tmp);
                                    }
                                    break;
                                case "Hrm_Ins_Web":
                                    _tmp = CreateMenuForCustomReport(siteMap, listMaster, ModuleName.Insurance, i.ChildNodes.FirstOrDefault().Alias);
                                    if (_tmp != null)
                                    {
                                        i.ChildNodes.Add(_tmp);
                                    }
                                    break;
                                case "Hrm_Sys_Web":
                                    _tmp = CreateMenuForCustomReport(siteMap, listMaster, ModuleName.System, i.ChildNodes.FirstOrDefault().Alias);
                                    if (_tmp != null)
                                    {
                                        i.ChildNodes.Add(_tmp);
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }

                HttpContext.Cache["SiteMap"] = siteMap;
            }
            else
            {
                siteMap = (HRM.Presentation.UI.Controls.Menu.XmlSiteMap)HttpContext.Cache["SiteMap"];
            }
            #endregion

            #region Load Permission
            TempPermissionModel result = new TempPermissionModel();
            Guid userId = Guid.Empty;
            if (Session[SessionObjects.UserId] != null)
            {
                if (Session[SessionObjects.UserId].ToString() != Common.UserNameSystem)
                {
                    userId = (Guid)Session[SessionObjects.UserId];
                }
                else
                {
                    return View(new List<string>());
                }
            }
            PermissionModel permission = new PermissionModel();   //do không truyền kiểu string lên được nên mượn model có guid để truyền lên api
            permission.UserID = userId;

            var service = new RestServiceClient<TempPermissionModel>(UserLogin);
            if (HttpContext.Cache[SessionObjects.Permission + userId] == null)
            {
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                result = service.Put(_hrm_Sys_Service, "api/SysPermission/", permission);
                //lưu cache quyền của user
                if (result != null)
                {
                    HttpContext.Cache[SessionObjects.Permission + userId] = result;
                }
            }
            else
            {
                result = (TempPermissionModel)HttpContext.Cache[SessionObjects.Permission + userId];
            }
            #endregion

            if (result != null)
            {
                List<string> modelResult = GetPermissionCurrentUser(result.Data, siteMap.RootNode.ChildNodes.ToList()).Distinct().ToList();
                modelResult.Add(siteMap.RootNode.ControllerName);//Add Quyền View Của Trang Home
                modelResult.Add("Sys_CustomReport");
                return View(modelResult);
            }
            return View();
            //}
            //catch
            //{
            //    return View();
            //}
        }

        [HttpPost]
        public ActionResult GetPermissionForGeneralProfile()
        {
            Guid userId = Guid.Empty;
            if (Session[SessionObjects.UserId] != null)
            {
                if (Session[SessionObjects.UserId].ToString() != Common.UserNameSystem)
                {
                    userId = (Guid)Session[SessionObjects.UserId];
                }
                else
                {
                    return Json(new List<string>());
                }
            }
            PermissionModel permission = new PermissionModel();   //do không truyền kiểu string lên được nên mượn model có guid để truyền lên api
            permission.UserID = userId;
            TempPermissionModel result = new TempPermissionModel();
            var service = new RestServiceClient<TempPermissionModel>(UserLogin);
            if (HttpContext.Cache[SessionObjects.Permission + userId] == null)
            {
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                result = service.Put(_hrm_Sys_Service, "api/SysPermission/", permission);
                //lưu cache quyền của user
                HttpContext.Cache[SessionObjects.Permission + userId] = result;
            }
            else
            {
                result = (TempPermissionModel)HttpContext.Cache[SessionObjects.Permission + userId];
            }
            return Json(result.Data);
        }


        public UI.Controls.Menu.SiteMapNode CreateMenuForCustomReport(HRM.Presentation.UI.Controls.Menu.XmlSiteMap sitemap, List<Rep_MasterModel> ListMaster, ModuleName module, string alias)
        {
            List<Rep_MasterModel> item = ListMaster.Where(m => m.ReportStoreModule == module.ToString()).ToList();
            if (item != null && item.Count > 0)
            {
                //Add menu cha
                HRM.Presentation.UI.Controls.Menu.SiteMapNode node = new UI.Controls.Menu.SiteMapNode();
                node.ControllerName = "Sys_CustomReport";
                node.ActionName = "GeneralCustomReport";
                node.HrmWebSite = "Hrm_Sys_Web";
                //node.Alias = ConstantPathWeb.Hrm_Main_Web;
                node.RouteValues = sitemap.RootNode.ChildNodes.LastOrDefault().RouteValues;
                node.ImageUrl = "/Content/images/icons/submenu/menu/Att_ShiftTimeSheet.png";
                node.Title = "Báo Cáo Động";
                node.Visible = true;
                node.ChildNodes = new List<UI.Controls.Menu.SiteMapNode>();

                foreach (var t in item)
                {
                    HRM.Presentation.UI.Controls.Menu.SiteMapNode nodeitem = new UI.Controls.Menu.SiteMapNode();
                    nodeitem.ControllerName = "Sys_CustomReport";
                    nodeitem.ActionName = "GeneralCustomReport?id=" + t.ID;
                    nodeitem.HrmWebSite = "Hrm_Sys_Web";
                    nodeitem.Alias = alias;
                    //nodeitem.Alias = ConstantPathWeb.Hrm_Main_Web;
                    nodeitem.Url = "#" + nodeitem.HrmWebSite + "/" + nodeitem.ControllerName + "/" + nodeitem.ActionName;
                    nodeitem.RouteValues = sitemap.RootNode.ChildNodes.LastOrDefault().RouteValues;
                    nodeitem.ImageUrl = "/Content/images/icons/submenu/menu/Att_ShiftTimeSheet.png";
                    nodeitem.Title = t.ReportName;
                    nodeitem.Visible = true;
                    node.ChildNodes.Add(nodeitem);
                }
                return node;
            }
            return null;
        }

        //claer all cache
        public ActionResult ClearCache()
        {
            List<string> keys = new List<string>();

            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = HttpContext.Cache.GetEnumerator();

            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }


            // delete every key from cache
            for (int i = 0; i < keys.Count; i++)
            {
                HttpContext.Cache.Remove(keys[i]);
            }

            //foreach (System.Collections.DictionaryEntry entry in HttpContext.Cache){
            //    HttpContext.Cache.Remove(entry.Key.ToString());
            //}

            return Json("");
        }

        /// <summary>
        /// [Hien.Nguyen] - Hiện tại chưa có cách giải quyết bằng thuật toán Đệ Quy (Đệ Quy Tương Hỗ) nên làm cứng bằng 4 vòng lặp lồng nhau
        /// </summary>
        /// <param name="listPermission"></param>
        /// <param name="listNode"></param>
        /// <returns></returns>
        private List<string> GetPermissionCurrentUser(List<string> listPermission, List<HRM.Presentation.UI.Controls.Menu.SiteMapNode> listNode)
        {
            List<string> result = new List<string>();

            foreach (var i in listNode)
            {
                if (listPermission.Any(m => m != null && i.ControllerName != null && m.Trim().ToUpper() == i.ControllerName.Trim().ToUpper()))
                {
                    result.Add(i.ControllerName);
                }
                foreach (var j in i.ChildNodes)
                {
                    if (listPermission.Any(m => m != null && j.ControllerName != null && m.Trim().ToUpper() == j.ControllerName.Trim().ToUpper()))
                    {
                        result.Add(i.ControllerName);
                        result.Add(j.ControllerName);
                    }
                    foreach (var n in j.ChildNodes)
                    {
                        if (listPermission.Any(m => m != null && n.ControllerName != null && m.Trim().ToUpper() == n.ControllerName.Trim().ToUpper()))
                        {
                            result.Add(i.ControllerName);
                            result.Add(j.ControllerName);
                            result.Add(n.ControllerName);
                        }
                        foreach (var t in n.ChildNodes)
                        {
                            if (listPermission.Any(m => m != null && t.ControllerName != null && m.Trim().ToUpper() == t.ControllerName.Trim().ToUpper()))
                            {
                                result.Add(i.ControllerName);
                                result.Add(j.ControllerName);
                                result.Add(n.ControllerName);
                                result.Add(t.ControllerName);
                            }
                        }
                    }
                }
            }
            return result;
        }

        //public ActionResult GetVerSion(string Stringurl)
        //{
        //    string Build = "";
        //    Assembly assembly = Assembly.LoadFrom(Stringurl);
        //    Version ver = assembly.GetName().Version;
        //    Build = ver.Build.ToString();
        //    return Json(Build, JsonRequestBehavior.AllowGet);
        //}
        #region Login action

        public ActionResult TestPerformance()
        {
            TestPerformanceModel model = new TestPerformanceModel();
            model.Loop = 100;

            return View(model);
        }
        #endregion
    }

}

