using HRM.Presentation.Category.Models;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Web;
using HRM.Business.Category.Domain;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using HRM.Business.Main.Domain;
using System.Data.SqlTypes;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatShiftController : ApiController
    {
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
        /// <summary>
        /// Get All Cat_Shift for Multi Control
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CatShiftMultiModel> Get()
        {
            BaseService service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<CatShiftMultiModel>(string.Empty, ConstantSql.hrm_cat_sp_get_Shift_multi, UserLogin, ref status);

            if (listEntity != null)
            {
                var listModel = listEntity.Translate<CatShiftMultiModel>();
                return listModel;
            }

            return new List<CatShiftMultiModel>();
        }


        /// <summary>
        /// [Tam.Le] - Lấy dữ liệu Shift theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatShiftModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatShiftModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ShiftEntity>(id,ConstantSql.hrm_cat_sp_get_ShiftById ,ref status);
            DateTime? TimeCoOut = SqlDateTime.MinValue.Value;
            DateTime? TimeCoBreakIn = SqlDateTime.MinValue.Value;
            DateTime? TimeCoBreakOut = SqlDateTime.MinValue.Value;
            DateTime? TimeMinIn = SqlDateTime.MinValue.Value;
            DateTime? TimeMaxOut = SqlDateTime.MinValue.Value;
            if (entity != null)
            {
                model = entity.CopyData<CatShiftModel>();
                model.TimeCoOut = model.InTime.AddHours((double)model.CoOut);
                model.TimeCoBreakIn = model.InTime.AddHours((double)model.CoBreakIn);
                model.TimeCoBreakOut = model.InTime.AddHours((double)model.CoBreakOut);
                model.TimeMinIn = model.InTime.AddHours(-(double)model.MinIn);
                model.TimeMaxOut = model.TimeCoOut.Value.AddHours((double) model.MaxOut);
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tam.Le] - Xóa hoặc chuyển đổi trạng thái của Shift sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatShiftModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ShiftEntity, CatShiftModel>(id);
            return result;
        }

        /// <summary>
        /// [Tam.Le] - Xử lý thêm mới hoặc chỉnh sửa một Shift
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatShift")]
        public CatShiftModel Post([Bind]CatShiftModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatShiftModel>(model, "Cat_Shift", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            //CatShiftModel shiftModel = new CatShiftModel 
            //{ 
            //     ID = model.ID,
            //     InTime = model.InTime,
            //     IPCreate = model.IPCreate,
            //     IPUpdate = model.IPUpdate,
            //     NightTimeEnd = model.NightTimeEnd,
            //     NightTimeStart = model.NightTimeStart,
            //     ServerCreate = model.ServerCreate,
            //     ServerUpdate = model.ServerUpdate,
            //     ShiftBreakType = model.ShiftBreakType,
            //     ShiftName = model.ShiftName,
            //     Code = model.Code,
            //     CodeStatistic = model.CodeStatistic,
            //     FirstShiftID = model.FirstShiftID,
            //     LastShiftID = model.LastShiftID,
            //     WorkHours = model.WorkHours,
            //     IsBreakAsWork = model.IsBreakAsWork,
            //     IsDoubleShift = model.IsDoubleShift,
            //     IsNightShift = model.IsNightShift,
            //     IsNotApplyWorkHoursReal = model.IsNotApplyWorkHoursReal,
            //     IsTemporary = model.IsTemporary,
            //     UserCreate = model.UserCreate,
            //     UserUpdate = model.UserUpdate,
            //     CoOut = model.TimeCoOut.Value.Subtract(model.InTime).TotalHours,
            //     CoBreakIn = model.TimeCoBreakIn.Value.Subtract(model.InTime).TotalHours,
            //     CoBreakOut = model.TimeCoBreakOut.Value.Subtract(model.InTime).TotalHours,
            //     MaxOut = model.MaxOut,
            //     MinIn = model.MinIn
            //};
            model.CoOut = model.TimeCoOut.Value.Subtract(model.InTime).TotalHours;
            model.CoBreakIn = model.TimeCoBreakIn.Value.Subtract(model.InTime).TotalHours;
            model.CoBreakOut = model.TimeCoBreakOut.Value.Subtract(model.InTime).TotalHours;
            return service.UpdateOrCreate<Cat_ShiftEntity, CatShiftModel>(model);
        }
        
    }
}
