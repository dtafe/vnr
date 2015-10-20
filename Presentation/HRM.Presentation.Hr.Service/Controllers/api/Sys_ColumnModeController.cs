using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using Newtonsoft.Json;
using VnResource.Helper.Data;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Sys_ColumnModeController : ApiController
    {
        #region UserLogin
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
        //public GridBuilderModel Put(GridBuilderModel model)
        //{
        //    if (model != null)
        //    {
        //        if (model.UserID != Guid.Empty && !string.IsNullOrEmpty(model.GridControlName))
        //        {
        //            ActionService service = new ActionService(UserLogin);
        //            List<object> lstObj = new List<object>() { model.UserID, model.GridControlName };
        //            var status = string.Empty;
        //            var entity = service.GetDataByListParameter<Sys_ColumnModeEntity>(lstObj,
        //                ConstantSql.hrm_sys_sp_get_ColumnMode, ref status);
        //            if (entity != null)
        //            {
        //                model = entity.CopyData<GridBuilderModel>();
        //                model.StringXml = entity.ColumnMode;
        //            }
        //            model.ActionStatus = status;
        //            return model;
        //        }
        //    }

        //    return null;
        //}
        public GridBuilderModel DeleteOrRemove(string id)
        {
            var model = new GridBuilderModel();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var idItem = id.Split(',');
                    var userId = idItem[0];
                    var gridName = idItem[1];
                    var service = new BaseService();
                    if (userId != null && gridName != null && gridName != "" && userId != "")
                    {
                        model.ActionStatus = service.DeleteColumMode(new Guid(userId), gridName);
                    }
                }
                catch (Exception ex)
                {
                    model.ActionStatus = NotificationType.Error + "," + ex.Message;
                }
            }
            return model;
        }
        public GridBuilderModel Post(GridBuilderModel model)
        {
            if (model != null)
            {
                ActionService service = new ActionService(UserLogin);
                List<object> lstObj = new List<object>() { model.UserID, model.GridControlName };
                var status = string.Empty;
                var entity = service.GetDataByListParameter<Sys_ColumnModeEntity>(lstObj, ConstantSql.hrm_sys_sp_get_ColumnMode, ref status);
                model.ActionStatus = status;
                if (entity != null && model.IsGet)
                {
                    model = entity.CopyData<GridBuilderModel>();
                    model.StringXml = entity.ColumnMode;
                    return model;
                }

                if (!string.IsNullOrEmpty(model.StringXml))
                {
                    XmlDocument doc = JsonConvert.DeserializeXmlNode(model.StringXml);
                    var baseService = new BaseService();
                    Sys_ColumnModeEntity newEntity = new Sys_ColumnModeEntity()
                    {
                        UserInfoID = model.UserID,
                        GridControlName = model.GridControlName,
                        ColumnMode = doc.InnerXml + "|" + model.PageSize
                    };
                    if (entity != null)
                    {
                        newEntity.ID = entity.ID;
                        baseService.Edit<Sys_ColumnModeEntity>(newEntity);
                    }
                    else
                    {
                        baseService.Add<Sys_ColumnModeEntity>(newEntity);
                    }

                    return model;
                }
            }
            return model;
        }
    }
}
