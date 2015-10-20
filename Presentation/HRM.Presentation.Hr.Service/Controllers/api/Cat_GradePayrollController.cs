using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Business.Category.Models;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_GradePayrollController : ApiController
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
        /// [Tin.Nguyen] - Lấy dữ liệu Quốc Gia(Cat_Country) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_GradePayrollModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_GradePayrollModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetById<Cat_GradeAttendanceEntity>(id, ref status);
            var entity = service.GetByIdUseStore<Cat_GradePayrollEntity>(id, ConstantSql.hrm_cat_sp_get_Cat_GradePayrollById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_GradePayrollModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái Quốc Gia(Cat_Country) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_GradePayrollModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_GradePayrollEntity, Cat_GradePayrollModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Quốc Gia(Cat_Country)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_GradeAttendance")]
        public Cat_GradePayrollModel Post([Bind]Cat_GradePayrollModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_GradePayrollModel>(model, "Cat_GradePayroll", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            #region Xử lý dấu cộng
            model.FormulaSalaryIns = model.FormulaSalaryIns.Replace("[+]", "+");
            #endregion
            return service.UpdateOrCreate<Cat_GradePayrollEntity, Cat_GradePayrollModel>(model);
        }
    }
}
