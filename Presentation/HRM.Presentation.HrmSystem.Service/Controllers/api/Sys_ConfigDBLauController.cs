using System.Net.Http.Formatting;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VnResource.Helper.Data;
using VnResource.Helper.Security;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_ConfigDBLauController : ApiController
    {
        #region MyRegion
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    //var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    //userLogin = headerValues.FirstOrDefault();
                    userLogin = "son.vo";
                }
                return userLogin;
            }
        }
        #endregion
        //// GET api/<controller>
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        public Sys_ConfigDBLauModel Get(string id)
        {
            var service = new Sys_AllSettingServices();
            string status = string.Empty;
            var listConfig = service.GetData<Sys_AllSettingEntity>(id, ConstantSql.hrm_cat_sp_get_GetAllSettings, UserLogin, ref status);
            Sys_ConfigDBLauModel config = new Sys_ConfigDBLauModel();
            foreach (var item in listConfig)
            {
                // Sever 1
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.SERVERNAME.ToString())
                {
                    config.Lau_ServerName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.USERID.ToString())
                {
                    config.Lau_UserID1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.PASSWORD.ToString())
                {
                    config.Lau_Password1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.DBNAME.ToString())
                {
                    config.Lau_DBName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.TABLENAME.ToString())
                {
                    config.Lau_TableName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.CARDCOLNAME.ToString())
                {
                    config.Lau_CardColName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.DATACOLNAME.ToString())
                {
                    config.Lau_DataColName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.MACHINECODE.ToString())
                {
                    config.Lau_SerialNumber1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.TYPE.ToString())
                {
                    config.Lau_In_Out1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATA.ToString())
                {
                    config.Lau_FilterData1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATASPLIT.ToString())
                {
                    config.Lau_FilterDataSplit1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.ISCODEEMP.ToString())
                {
                    config.Lau_IsCodeEmp1 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.ISACTIVATED.ToString())
                {
                    config.Lau_IsActive1 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }

                // Sever 2
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.SERVERNAME.ToString())
                {
                    config.Lau_ServerName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.USERID.ToString())
                {
                    config.Lau_UserID2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.PASSWORD.ToString())
                {
                    config.Lau_Password2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.DBNAME.ToString())
                {
                    config.Lau_DBName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.TABLENAME.ToString())
                {
                    config.Lau_TableName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.CARDCOLNAME.ToString())
                {
                    config.Lau_CardColName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.DATACOLNAME.ToString())
                {
                    config.Lau_DataColName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.MACHINECODE.ToString())
                {
                    config.Lau_SerialNumber2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.TYPE.ToString())
                {
                    config.Lau_In_Out2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATA.ToString())
                {
                    config.Lau_FilterData2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATASPLIT.ToString())
                {
                    config.Lau_FilterDataSplit2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.ISCODEEMP.ToString())
                {
                    config.Lau_IsCodeEmp2 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.ISACTIVATED.ToString())
                {
                    config.Lau_IsActive2 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }
            }
            return config;
        }

        public Sys_ConfigDBLauModel Post(Sys_ConfigDBLauModel model)
        {
            Dictionary<object, object> dic = new Dictionary<object, object>();
            //SERVER1
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.SERVERNAME.ToString(), model.Lau_ServerName1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.USERID.ToString(), model.Lau_UserID1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.PASSWORD.ToString(), model.Lau_Password1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.DBNAME.ToString(), model.Lau_DBName1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.TABLENAME.ToString(), model.Lau_TableName1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.CARDCOLNAME.ToString(), model.Lau_CardColName1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.DATACOLNAME.ToString(), model.Lau_DataColName1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.MACHINECODE.ToString(), model.Lau_SerialNumber1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.TYPE.ToString(), model.Lau_In_Out1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATA.ToString(), model.Lau_FilterData1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATASPLIT.ToString(), model.Lau_FilterDataSplit1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.ISCODEEMP.ToString(), model.Lau_IsCodeEmp1);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_.ToString() + AppConfig.ISACTIVATED.ToString(), model.Lau_IsActive1.ToString());
            //SERVER2
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.SERVERNAME.ToString(), model.Lau_ServerName2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.USERID.ToString(), model.Lau_UserID2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.PASSWORD.ToString(), model.Lau_Password2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.DBNAME.ToString(), model.Lau_DBName2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.TABLENAME.ToString(), model.Lau_TableName2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.CARDCOLNAME.ToString(), model.Lau_CardColName2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.DATACOLNAME.ToString(), model.Lau_DataColName2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.MACHINECODE.ToString(), model.Lau_SerialNumber2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.TYPE.ToString(), model.Lau_In_Out2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATA.ToString(), model.Lau_FilterData2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATASPLIT.ToString(), model.Lau_FilterDataSplit2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.ISCODEEMP.ToString(), model.Lau_IsCodeEmp2);
            dic.Add(AppConfig.HRM_SYS_LAU_TAMSCANLOG_2_.ToString() + AppConfig.ISACTIVATED.ToString(), model.Lau_IsActive2.ToString());

            var service = new Sys_AllSettingServices();
            service.SaveData<Sys_AllSettingEntity>(dic, ConstantSql.hrm_cat_sp_get_UpdateAllSettingByName);
            return model;
        }
    }
}