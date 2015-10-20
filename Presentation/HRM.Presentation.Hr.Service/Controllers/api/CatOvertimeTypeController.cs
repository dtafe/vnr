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
using System;
using System.Linq;
using HRM.Business.Main.Domain;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatOvertimeTypeController : ApiController
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
        /// <summary>
        /// [Hien.Nguyen] Get All Cat_OrverTimeType for Multi Control
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CatOvertimeTypeMultiModel> Get()
        {
            BaseService service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<CatOvertimeTypeMultiModel>(string.Empty, ConstantSql.hrm_cat_sp_get_OvertimeType_multi, UserLogin, ref status);

            if (listEntity != null)
            {
                var listModel = listEntity.Translate<CatOvertimeTypeMultiModel>();
                return listModel;
            }

            return new List<CatOvertimeTypeMultiModel>();
        }



        /// <summary>
        /// [Tin.Nguyen] - Lấy dữ liệu Loại Ngày Nghỉ(Cat_LeaveDayType) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatOvertimeTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatOvertimeTypeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_OvertimeTypeEntity>(id,ConstantSql.hrm_cat_sp_get_OvertimeTypeById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatOvertimeTypeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Loại Thiết Bị(Cat_Currency) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatOvertimeTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_OvertimeTypeEntity, CatOvertimeTypeModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Loại Thiết Bị(Currency)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatOvertime")]
        public CatOvertimeTypeModel Post([Bind]CatOvertimeTypeModel model)
        {
            if (model.TypeTemp == EnumDropDown.OverTimeType.E_HOLIDAY.ToString())
            {
                model.IsHoliday = true;
                model.IsWeeked = false;
                model.IsWorkDay = false;
                model.IsNightShift = false;
            }
            else if (model.TypeTemp == EnumDropDown.OverTimeType.E_HOLIDAY_NIGHTSHIFT.ToString())
            {
                model.IsHoliday = true;
                model.IsWeeked = false;
                model.IsWorkDay = false;
                model.IsNightShift = true;
            }
            else if (model.TypeTemp == EnumDropDown.OverTimeType.E_WEEKEND.ToString())
            {
                model.IsHoliday = false;
                model.IsWeeked = true;
                model.IsWorkDay = false;
                model.IsNightShift = false;
            }
            else if (model.TypeTemp == EnumDropDown.OverTimeType.E_WEEKEND_NIGHTSHIFT.ToString())
            {
                model.IsHoliday = false;
                model.IsWeeked = true;
                model.IsWorkDay = false;
                model.IsNightShift = true;
            }
            else if (model.TypeTemp == EnumDropDown.OverTimeType.E_WORKDAY.ToString())
            {
                model.IsHoliday = false;
                model.IsWeeked = false;
                model.IsWorkDay = true;
                model.IsNightShift = false;
            }
            else if (model.TypeTemp == EnumDropDown.OverTimeType.E_WORKDAY_NIGHTSHIFT.ToString())
            {
                model.IsHoliday = false;
                model.IsWeeked = false;
                model.IsWorkDay = true;
                model.IsNightShift = true;
            }
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatOvertimeTypeModel>(model, "Cat_OvertimeType", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_OvertimeTypeEntity, CatOvertimeTypeModel>(model);
        }
    }
}