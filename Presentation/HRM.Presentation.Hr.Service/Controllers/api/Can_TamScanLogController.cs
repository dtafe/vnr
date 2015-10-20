using HRM.Business.Canteen.Domain;

using HRM.Presentation.Canteen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Canteen.Domain;
using HRM.Business.Canteen.Models;
using HRM.Presentation.Service;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Can_TamScanLogController : ApiController
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
        string status = string.Empty;
        public Can_TamScanLogModel Get(Guid id)
        {
            string status = string.Empty;
            var model = new Can_TamScanLogModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Can_TamScanLogEntity>(id, ConstantSql.hrm_can_sp_get_tamscanlogbyid, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Can_TamScanLogModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// Xử lí eidt và add new truyền theo script
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>     
        public Can_TamScanLogModel Post(Can_TamScanLogModel model)
        {

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Can_TamScanLogEntity, Can_TamScanLogModel>(model);
        }
      
        public Can_TamScanLogModel Delete(Guid id)
        {
            var service = new Can_TamScanLogServices();
            service.Delete<Can_TamScanLogEntity>(id);
            var result = new Can_TamScanLogModel();
            return result;
        }
    }
}
