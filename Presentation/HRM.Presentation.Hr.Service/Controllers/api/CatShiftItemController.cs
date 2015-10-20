using Antlr.Runtime.Misc;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Data.SqlTypes;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatShiftItemController : ApiController
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
        /// <summary>
        /// [Son.Vo] - Lấy dữ liệu ShiftItem(Cat_ShiftItem) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatShiftItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatShiftItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ShiftItemEntity>(id, ConstantSql.hrm_cat_sp_get_ShiftItemById, ref status);
            if (entity != null && entity.ID != Guid.Empty)
            {
                
                model = entity.CopyData<CatShiftItemModel>();
                DateTime temp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, model.Intime.Value.Hour, model.Intime.Value.Minute, model.Intime.Value.Second);
              //  model.From = SqlDateTime.MinValue.Value;
                model.From = temp.AddHours(model.CoFrom);
              //  model.To = SqlDateTime.MinValue.Value;
                model.To = temp.AddHours(model.CoTo);
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của ShiftItem(Cat_ShiftItem) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatShiftItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ShiftItemEntity, CatShiftItemModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một ShiftItem(Cat_ShiftItem)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatShiftItem")]
        public CatShiftItemModel Post([Bind]CatShiftItemModel model)
        {
            #region Validate
            string message = string.Empty;
            string status = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatShiftItemModel>(model, "Cat_ShiftItem", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            
       
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ShiftEntity>(model.ShiftID, ConstantSql.hrm_cat_sp_get_ShiftById, ref status);
            if(model.OvertimeTypeID != null){
                if(model.From.Value.Day > entity.InTime.Day){
                    DateTime temp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, entity.InTime.Hour, entity.InTime.Minute, entity.InTime.Second);
                    model.From.Value.AddDays(1);
                    model.CoFrom = model.From.Value.Subtract(temp).TotalHours;
                }
                if(model.To.Value.Day > entity.InTime.Day){
                    DateTime temp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, entity.InTime.Hour, entity.InTime.Minute, entity.InTime.Second);
                    model.To.Value.AddDays(1);
                    model.CoTo = model.To.Value.Subtract(temp).TotalHours;
                }
            }
       //     double d = model.From.Value.Hour + model.From.Value.Minute / 100;
            if(model.From != null || model.To != null){
                DateTime temp = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day, entity.InTime.Hour,entity.InTime.Minute,entity.InTime.Second);

                model.CoFrom = model.From.Value.Subtract(temp).TotalHours;
                model.CoTo = model.To.Value.Subtract(temp).TotalHours;
            }
            
            
            return service.UpdateOrCreate<Cat_ShiftItemEntity, CatShiftItemModel>(model);
        }

    }


}