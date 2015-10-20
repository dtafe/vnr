using HRM.Business.HrmSystem.Domain;
using HRM.Business.Main.Domain;
//using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.HrmSystem.Models;
using VnResource.Helper.Data;
using System;


namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_ConfigDBController : ApiController
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
        public Sys_ConfigDBModel Get(string id)
        {
            var service = new Sys_AllSettingServices();
            string status = string.Empty;
            var listConfig = service.GetData<Sys_AllSettingEntity>(id, ConstantSql.hrm_cat_sp_get_GetAllSettings, UserLogin, ref status);
            Sys_ConfigDBModel config = new Sys_ConfigDBModel();
            foreach (var item in listConfig)
            {
                // Config Attendance
                // Sever 1
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.SERVERNAME.ToString())
                {
                    config.Att_ServerName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.USERID.ToString())
                {
                    config.Att_UserID1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.PASSWORD.ToString())
                {
                    config.Att_Password1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.DBNAME.ToString())
                {
                    config.Att_DBName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.TABLENAME.ToString())
                {
                    config.Att_TableName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.CARDCOLNAME.ToString())
                {
                    config.Att_CardColName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.DATACOLNAME.ToString())
                {
                    config.Att_DataColName1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.MACHINECODE.ToString())
                {
                    config.Att_SerialNumber1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.TYPE.ToString())
                {
                    config.Att_In_Out1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATA.ToString())
                {
                    config.Att_FilterData1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATASPLIT.ToString())
                {
                    config.Att_FilterDataSplit1 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.ISCODEEMP.ToString())
                {
                    config.Att_IsCodeEmp1 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.ISACTIVATED.ToString())
                {
                    config.Att_IsActive1 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }

                // Sever 2
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.SERVERNAME.ToString())
                {
                    config.Att_ServerName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.USERID.ToString())
                {
                    config.Att_UserID2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.PASSWORD.ToString())
                {
                    config.Att_Password2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.DBNAME.ToString())
                {
                    config.Att_DBName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.TABLENAME.ToString())
                {
                    config.Att_TableName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.CARDCOLNAME.ToString())
                {
                    config.Att_CardColName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.DATACOLNAME.ToString())
                {
                    config.Att_DataColName2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.MACHINECODE.ToString())
                {
                    config.Att_SerialNumber2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.TYPE.ToString())
                {
                    config.Att_In_Out2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATA.ToString())
                {
                    config.Att_FilterData2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATASPLIT.ToString())
                {
                    config.Att_FilterDataSplit2 = item.Value1;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.ISCODEEMP.ToString())
                {
                    config.Att_IsCodeEmp2 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }
                if (item.Name == AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.ISACTIVATED.ToString())
                {
                    config.Att_IsActive2 = item.Value1 == bool.TrueString ? true : false;
                    continue;
                }
            }
            return config;
        }

        public Sys_ConfigDBModel Post(Sys_ConfigDBModel model)
        {
            Dictionary<object, object> dic = new Dictionary<object, object>();
            //SERVER1
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.SERVERNAME.ToString() , model.Att_ServerName1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.USERID.ToString() , model.Att_UserID1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.PASSWORD.ToString() , model.Att_Password1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.DBNAME.ToString(), model.Att_DBName1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.TABLENAME.ToString() , model.Att_TableName1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.CARDCOLNAME.ToString() , model.Att_CardColName1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.DATACOLNAME.ToString(), model.Att_DataColName1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.MACHINECODE.ToString() , model.Att_SerialNumber1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.TYPE.ToString() , model.Att_In_Out1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATA.ToString() , model.Att_FilterData1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATASPLIT.ToString() , model.Att_FilterDataSplit1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.ISCODEEMP.ToString() , model.Att_IsCodeEmp1);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.ISACTIVATED.ToString(), model.Att_IsActive1.ToString());
            //SERVER2
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.SERVERNAME.ToString() , model.Att_ServerName2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.USERID.ToString() , model.Att_UserID2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.PASSWORD.ToString() , model.Att_Password2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.DBNAME.ToString(), model.Att_DBName2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.TABLENAME.ToString() , model.Att_TableName2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.CARDCOLNAME.ToString() , model.Att_CardColName2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.DATACOLNAME.ToString(), model.Att_DataColName2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.MACHINECODE.ToString() , model.Att_SerialNumber2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.TYPE.ToString() , model.Att_In_Out2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATA.ToString() , model.Att_FilterData2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATASPLIT.ToString() , model.Att_FilterDataSplit2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.ISCODEEMP.ToString() , model.Att_IsCodeEmp2);
            dic.Add(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.ISACTIVATED.ToString(), model.Att_IsActive2.ToString());
            var service = new BaseService();
            var isSuccess = service.SaveData<Sys_AllSettingEntity>(dic, ConstantSql.hrm_cat_sp_get_UpdateAllSettingByName);
            return model;
        }
    }
}