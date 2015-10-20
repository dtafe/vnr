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
using HRM.Business.Main.Domain;

namespace HRM.Presentation.Payroll.Service.Controllers.api
{
    public class Sal_BasicSalaryController : ApiController
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
        //public Sal_BasicSalaryModel DeleteOrRemove(string id)
        //{
        //    if (id != string.Empty)
        //    {
        //        var listID = id.Split(',').ToList();
        //        foreach (var i in listID)
        //        {
        //            Sal_BasicSalaryServices service = new Sal_BasicSalaryServices();
        //            var result = service.Remove<Sal_BasicSalaryEntity>(Common.ConvertToGuid(i));
        //        }
        //    }
        //    return new Sal_BasicSalaryModel();
        //}

        public Sal_BasicSalaryModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_BasicSalaryEntity, Sal_BasicSalaryModel>(id);
            return result;
        }



        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Quốc Gia(Cat_Country)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_BasicSalary")]
        public Sal_BasicSalaryModel Post([Bind]Sal_BasicSalaryModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_BasicSalaryModel>(model, "Sal_BasicSalary", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            model.Amount = model.GrossAmount.Encrypt();
            return service.UpdateOrCreate<Sal_BasicSalaryEntity, Sal_BasicSalaryModel>(model);
        }


        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_BasicSalary")]
        public Sal_BasicSalaryModel Put([Bind]Sal_BasicSalaryModel model)
        {            
            if (!string.IsNullOrEmpty(model.IDs))
            {
                try
                {
                    model.IDs = Common.DotNetToOracle(model.IDs);
                    BaseService service = new BaseService();
                    string statusMessages = string.Empty;
                    List<object> lstObj = new List<object>();
                    lstObj.Add(model.IDs);
                    lstObj.Add(model.Status);
                    service.UpdateData<Sal_BasicSalaryModel>(lstObj, ConstantSql.hrm_sal_sp_get_BasicSalary_UpdateStatus, ref statusMessages);
                    model.ActionStatus = statusMessages;
                }
                catch (Exception)
                {
                    
                    throw;
                }
                
            }
            return model;

        }
    }
}
