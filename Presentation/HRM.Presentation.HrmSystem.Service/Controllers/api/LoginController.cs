using System.Threading;
using HRM.Business.HrmSystem.Domain;
using HRM.Presentation.HrmSystem.Models;
using System.Web.Http;
using System.Collections.Generic;
using System.IO;
using HRM.Infrastructure.Utilities;
using System.Linq;
using System;
using HRM.Business.Hr.Models;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class LoginController : ApiController
    {

        #region MyRegion
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    userLogin = headerValues.FirstOrDefault();
                }
                return userLogin;
            }
        }
        #endregion
       
        public LoginModel Post(LoginModel user)       
        {

            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
            {
                var service = new Sys_UserServices();
                var model = new LoginModel();
                var status = string.Empty;
                var userInfo = service.Login(user.UserName, user.Password,user.GoogleMail,user.GoogleProfileName,user.IsGoogleSign);

             

                if (userInfo != null)
                {

                    var profileInfo = service.GetData<Hre_ProfileEntity>(userInfo.ProfileID, ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref status).FirstOrDefault();
            
                    model = new LoginModel
                    {
                        UserName = userInfo.UserLogin,
                        Password = userInfo.Password,
                        HasMasterDataGroup = userInfo.HasMasterDataGroup,
                        FullName = profileInfo != null ? profileInfo.ProfileName : string.Empty,
                        UserInfoName = userInfo.UserInfoName,
                        IsActive = userInfo.IsActivate,
                        ProfileID = userInfo.ProfileID,
                        ID = userInfo.ID,
                        IsRunScriptNew = userInfo.IsRunScriptNew,
                        VesionLastName = userInfo.VesionLastName,
                    };
                    if (userInfo.DateChangePasssword == null)
                        model.IsFirstLogin = true;

                    return model;
                }
            }
            return null;
        }
        public LoginModel Put(LoginModel model)
        {
            //ErrorModel errors = new ErrorModel();
            model.Name = new List<string>();
            model.Execption = new List<string>();

            var service = new Sys_SqlCommanderServices();
            string strKey = "GO";
            string strKeyName = "@";
            string strVersion = "";
            #region Cap nhat Script
            if (!string.IsNullOrEmpty(@Common.ScriptPath))
            {
                var directScript = new DirectoryInfo(@Common.ScriptPath);
                var filesScript = directScript.GetFiles();
                if (filesScript != null && filesScript.Count() > 0)
                {
                    var files = filesScript.OrderByDescending(s => s.Name).ToList();
                    var fileInfo = files.Where(d => Path.GetFileNameWithoutExtension(d.Name) == model.VesionLastName).FirstOrDefault();
                    files = files.Skip(files.IndexOf(fileInfo)).ToList();

                    foreach (var file in files)
                    {
                        StreamReader stream = file.OpenText();
                        string scripts = stream.ReadToEnd();
                        stream.Close();
                        
                        scripts = scripts.Replace("\t", " ").Replace("\n", " ").Replace("\r", " ").Replace(";","");
                        //scripts = scripts.Replace(@"\", " ");
                        //for (int i = 0; i < scripts.Length; i++)
                        //{
                        //    if ((int)scripts[i] == 34)
                        //    {
                        //        //scripts = scripts.Replace(scripts[i], ' ');
                        //        scripts = scripts.Remove(i, 1);
                        //        scripts = scripts.Insert(i, " ");
                        //    }
                        //}
                        while (scripts.Contains("  "))
                        {
                            scripts = scripts.Replace("  ", " ");
                        }
                        string[] arrScript;
                        if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                        {
                            strKey = "/ ";
                        }
                        else
                        {
                            strKey = "GO ";
                        }
                        scripts = scripts.Replace(strKey, " ");
                        arrScript = scripts.Split(new string[] { "--End--" }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> lstScript = new List<string>();
                        foreach (var item in arrScript)
                        {
                            string strExecption = service.ExecuteStringCommand(item);
                            if (!string.IsNullOrEmpty(strExecption))
                            {
                                model.Execption.Add(strExecption);
                                model.Name.Add(file.Name);
                            }
                        }
                        strVersion = Path.GetFileNameWithoutExtension(file.Name);
                    }
                }
            }
            #endregion
            #region Cap nhat Store
            if (!string.IsNullOrEmpty(@Common.StorePath))
            {
                var directStore = new DirectoryInfo(@Common.StorePath);
                var lstfileStore = directStore.GetFiles();
                if (lstfileStore.Count() > 0)
                {
                    FileInfo fileStore = lstfileStore.FirstOrDefault();
                    StreamReader stream = fileStore.OpenText();
                    string scriptStore = stream.ReadToEnd();
                    stream.Close();
                    scriptStore = scriptStore.Replace("\t", " ").Replace("\n", " ").Replace("\r", " ").Replace(";", "");
                    while (scriptStore.Contains("  "))
                    {
                        scriptStore = scriptStore.Replace("  ", " ");
                    }
                    string[] arrStore;
                    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                    {
                        strKey = "/ ";
                        strKeyName = "(";
                    }
                    else
                    {
                        strKey = "GO ";
                        strKeyName = "@";
                        scriptStore = scriptStore.Replace("ALTER PROCEDURE", "CREATE PROCEDURE");
                        scriptStore = scriptStore.Replace("alter procedure", "CREATE PROCEDURE");
                        scriptStore = scriptStore.Replace("ALTER procedure", "CREATE PROCEDURE");
                        scriptStore = scriptStore.Replace("alter PROCEDURE", "CREATE PROCEDURE");
                        scriptStore = scriptStore.Replace("create PROCEDURE", "CREATE PROCEDURE");
                        scriptStore = scriptStore.Replace("ALTER FUNCTION", "CREATE FUNCTION");
                        scriptStore = scriptStore.Replace("alter function", "CREATE FUNCTION");
                        scriptStore = scriptStore.Replace("alter FUNCTION", "CREATE FUNCTION");
                        scriptStore = scriptStore.Replace("ALTER function", "CREATE FUNCTION");
                        scriptStore = scriptStore.Replace("create FUNCTION", "CREATE FUNCTION");
                        
                    }
                    scriptStore = scriptStore.Replace(strKey, " ");
                    arrStore = scriptStore.Split(new string[] { "--End--" }, StringSplitOptions.RemoveEmptyEntries);
                    //string strScriptRoot = scriptStore;
                    foreach (var item in arrStore)
                    {
                        string strExecption = service.ExecuteStringCommand(item);
                        if (!string.IsNullOrEmpty(strExecption))
                        {
                            model.Execption.Add(strExecption);
                            model.Name.Add(item.Substring(0, scriptStore.IndexOf(strKeyName)));
                        }
                    }

                    //while (scriptStore.Contains(strKey))
                    //{
                    //    string strScript = scriptStore.Substring(0, scriptStore.IndexOf(strKey));
                    //    scriptStore = scriptStore.Substring(scriptStore.IndexOf(strKey) + strKey.Length);
                    //    string strExecption = service.ExecuteStringCommand(strScript);
                    //    if (!string.IsNullOrEmpty(strExecption))
                    //    {
                    //        model.Execption.Add(strExecption);
                    //        model.Name.Add(strScript.Substring(0, scriptStore.IndexOf(strKeyName)));
                    //    }
                    //}
                    if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                    {
                        scriptStore = scriptStore.Replace("CREATE PROCEDURE", "ALTER PROCEDURE");
                        scriptStore = scriptStore.Replace("CREATE FUNCTION", "ALTER FUNCTION");
                        //string strScript = strScriptRoot.Substring(0, strScriptRoot.IndexOf(strKey));
                        //strScriptRoot = strScriptRoot.Substring(strScriptRoot.IndexOf(strKey) + strKey.Length);
                        arrStore = scriptStore.Split(new string[] { "--End--" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in arrStore)
                        {
                            string strExecption = service.ExecuteStringCommand(item);
                            if (!string.IsNullOrEmpty(strExecption))
                            {
                                model.Execption.Add(strExecption);
                                model.Name.Add(item.Substring(0, item.IndexOf(strKeyName)));
                            }
                        }

                    }
                }
            }
            #endregion
            if (model.Name.Count == 0)
            {
                string strUpdateVersion = "";
                strUpdateVersion = "INSERT INTO \"Sys_Version\"(ID, \"Name\", \"Value\", \"Note\", \"UserCreate\", \"DateCreate\") VALUES (sys_guid(), '" + strVersion + "', '" + strVersion + "', '', 'son.vo', sysdate) ";
                service.ExecuteStringCommand(strUpdateVersion);
            }
            return model;
        }
        #region Method

 
        #endregion
    }
}