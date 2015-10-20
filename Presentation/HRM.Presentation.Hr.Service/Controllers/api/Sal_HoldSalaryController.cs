using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Payroll.Domain;
using HRM.Presentation.Payroll.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Payroll.Models;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Payroll.Service.Controllers.api
{
    public class Sal_HoldSalaryController : ApiController
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
        /// [Tho.Bui] - Lấy dữ liệu Quốc Gia(Cat_Country) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sal_BasicSalaryModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_BasicSalaryModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_BasicSalaryEntity>(id, ConstantSql.hrm_Sal_sp_get_BasicSalaryById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Sal_BasicSalaryModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tho.Bui] - Xóa hoặc chuyển đổi trạng thái Quốc Gia(Cat_Country) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sal_BasicSalaryModel DeleteOrRemove(string id)
        {
            
            if (id != string.Empty)
            {
                var listID = id.Split(',').ToList();
                foreach (var i in listID)
                {
                    Sal_BasicSalaryServices service = new Sal_BasicSalaryServices();
                    var result = service.Remove<Sal_BasicSalaryEntity>(Common.ConvertToGuid(i));
                }
            }
            return new Sal_BasicSalaryModel();
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Quốc Gia(Cat_Country)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_BasicSalary")]
        public Sal_HoldSalaryModel Post([Bind]Sal_HoldSalaryModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sal_HoldSalaryEntity, Sal_HoldSalaryModel>(model);
        }
    }
}
