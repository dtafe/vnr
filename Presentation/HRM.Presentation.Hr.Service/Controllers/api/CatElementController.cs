using Antlr.Runtime.Misc;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Service;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatElementController : ApiController
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
        /// [Tin.Nguyen] - Lấy dữ liệu Loại Thiết Bị(Cat_Element) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatElementModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatElementModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ElementEntity>(id,ConstantSql.hrm_cat_sp_get_ElementById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatElementModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Loại Thiết Bị(Cat_Element) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatElementModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ElementEntity, CatElementModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Loại Thiết Bị(Cat_Element)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatElement")]
        public CatElementModel Post([Bind]CatElementModel model)
        {
            #region Validate
            string FieldInfoName = "Cat_Element";
            if (model.IsApplyGradeAll != null && model.IsApplyGradeAll == true)
            {
                FieldInfoName = "Cat_Element_ApplyAllGrade";
            }
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatElementModel>(model, FieldInfoName, ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            //model.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
            model.ElementType = CatElementType.Payroll.ToString();    
            if (model.TabType == CatElementType.Insurance.ToString())
            {
                model.ElementType = CatElementType.Insurance.ToString();    
            }
            if (model.IsApplyGradeAll == true)
            {
                model.GradePayrollID = null;
            }
            model.ElementName = model.ElementName.Replace("/t", "").Replace("/n", "").Trim();
            model.Formula = model.Formula.Replace("[+]", "+");
            return service.UpdateOrCreate<Cat_ElementEntity, CatElementModel>(model);
        }
	}

}