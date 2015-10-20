using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using System.Net.Http;
using System.Net;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using System.Web;
using HRM.Presentation.HrmSystem.Models;
using HRM.Business.HrmSystem.Models;
namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Rep_ControlController : ApiController
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
        /// Chú ý là mượn page để get list Master
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Rep_MasterModel> GetById(Guid id)
        {
            string status = string.Empty;
            var model = new List<Rep_MasterModel>();
            ActionService service = new ActionService(UserLogin);
            var listModel = new List<object>();
            listModel.AddRange(new object[3]);
            listModel[1] = 1;
            listModel[2] = Int32.MaxValue - 1;
            var entity = service.GetData<Rep_MasterModel>(listModel, ConstantSql.hrm_rep_sp_get_Master, ref status);
            return entity != null ? entity.ToList() : new List<Rep_MasterModel>();
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[System.Web.Mvc.HttpPost]
        //[System.Web.Mvc.RouteAttribute("api/Rep_Control")]
        //public Rep_ControlModel Post([Bind]Rep_ControlModel model)
        //{
        //    ActionService service = new ActionService(UserLogin);
        //    string status=string.Empty;
        //    var tt = service.GetByIdUseStore<Rep_MasterEntity>(model.MasterID, ConstantSql.hrm_rep_sp_get_MasterByID, ref status);


        //    return new Rep_ControlModel();
        //}

	}
}