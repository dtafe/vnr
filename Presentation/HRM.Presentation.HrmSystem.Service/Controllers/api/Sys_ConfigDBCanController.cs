using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Service;
using VnResource.Helper.Data;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_ConfigDBCanController : ApiController
    {
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
        //// GET api/<controller>
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        public Sys_ConfigDBCanModel Get(string id)
        {
            var service = new Sys_AllSettingServices();
            string status = string.Empty;
            var listConfig = service.GetData<Sys_AllSettingEntity>(id, ConstantSql.hrm_cat_sp_get_GetAllSettings, UserLogin, ref status);
            Sys_ConfigDBCanModel config = new Sys_ConfigDBCanModel();
            foreach (var item in listConfig)
            {
                // Config Attendance
                // Sever 1
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.SERVERNAME.ToString())
                {
                    config.Can_ServerName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.USERID.ToString())
                {
                    config.Can_UserID1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.PASSWORD.ToString())
                {
                    config.Can_Password1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.DBNAME.ToString())
                {
                    config.Can_DBName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.TABLENAME.ToString())
                {
                    config.Can_TableName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.CARDCOLNAME.ToString())
                {
                    config.Can_CardColName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.DATACOLNAME.ToString())
                {
                    config.Can_DataColName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.MACHINECODE.ToString())
                {
                    config.Can_SerialNumber1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.TYPE.ToString())
                {
                    config.Can_In_Out1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATA.ToString())
                {
                    config.Can_FilterData1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATASPLIT.ToString())
                {
                    config.Can_FilterDataSplit1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.ISCODEEMP.ToString())
                {
                    config.Can_IsCodeEmp1 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.ISACTIVATED.ToString())
                {
                    config.Can_IsActive1 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }

                // Sever 2
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.SERVERNAME.ToString())
                {
                    config.Can_ServerName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.USERID.ToString())
                {
                    config.Can_UserID2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.PASSWORD.ToString())
                {
                    config.Can_Password2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.DBNAME.ToString())
                {
                    config.Can_DBName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.TABLENAME.ToString())
                {
                    config.Can_TableName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.CARDCOLNAME.ToString())
                {
                    config.Can_CardColName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.DATACOLNAME.ToString())
                {
                    config.Can_DataColName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.MACHINECODE.ToString())
                {
                    config.Can_SerialNumber2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.TYPE.ToString())
                {
                    config.Can_In_Out2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATA.ToString())
                {
                    config.Can_FilterData2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATASPLIT.ToString())
                {
                    config.Can_FilterDataSplit2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.ISCODEEMP.ToString())
                {
                    config.Can_IsCodeEmp2 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.ISACTIVATED.ToString())
                {
                    config.Can_IsActive2 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }
            }
            return config;
        }

        public Sys_ConfigDBCanModel Post(Sys_ConfigDBCanModel model)
        {
            Dictionary<object, object> dic = new Dictionary<object, object>();
            //SERVER1
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.SERVERNAME.ToString(), model.Can_ServerName1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.USERID.ToString(), model.Can_UserID1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.PASSWORD.ToString(), model.Can_Password1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.DBNAME.ToString(), model.Can_DBName1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.TABLENAME.ToString(), model.Can_TableName1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.CARDCOLNAME.ToString(), model.Can_CardColName1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.DATACOLNAME.ToString(), model.Can_DataColName1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.MACHINECODE.ToString(), model.Can_SerialNumber1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.TYPE.ToString(), model.Can_In_Out1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATA.ToString(), model.Can_FilterData1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATASPLIT.ToString(), model.Can_FilterDataSplit1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.ISCODEEMP.ToString(), model.Can_IsCodeEmp1);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_.ToString() + AppConfig.ISACTIVATED.ToString(), model.Can_IsActive1.ToString());
            //SERVER2                 
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.SERVERNAME.ToString(), model.Can_ServerName2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.USERID.ToString(), model.Can_UserID2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.PASSWORD.ToString(), model.Can_Password2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.DBNAME.ToString(), model.Can_DBName2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.TABLENAME.ToString(), model.Can_TableName2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.CARDCOLNAME.ToString(), model.Can_CardColName2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.DATACOLNAME.ToString(), model.Can_DataColName2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.MACHINECODE.ToString(), model.Can_SerialNumber2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.TYPE.ToString(), model.Can_In_Out2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATA.ToString(), model.Can_FilterData2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATASPLIT.ToString(), model.Can_FilterDataSplit2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.ISCODEEMP.ToString(), model.Can_IsCodeEmp2);
            dic.Add(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_.ToString() + AppConfig.ISACTIVATED.ToString(), model.Can_IsActive2.ToString());
            var service = new Sys_AllSettingServices();
            service.SaveData<Sys_AllSettingEntity>(dic, ConstantSql.hrm_cat_sp_get_UpdateAllSettingByName);
            return model;
        }
    }
}