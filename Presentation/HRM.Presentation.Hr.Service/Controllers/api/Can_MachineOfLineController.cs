using HRM.Business.Canteen.Domain;
using HRM.Business.Canteen.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Canteen.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Can_MachineOfLineController : ApiController
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
        /// [Tam.Le] - Lấy dữ liệu MachineOfLine theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_MachineOfLineModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Can_MachineOfLineModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Can_MachineOfLineEntity>(id, ConstantSql.hrm_can_sp_get_machineoflinebyid, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Can_MachineOfLineModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tam.Le] - Xóa hoặc chuyển đổi trạng thái của MachineOfLine sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_MachineOfLineModel DeleteOrRemove(Guid id)
        {

            var service = new Can_MachineOfLineServices();
            var isSuccess = service.Delete<Can_MachineOfLineEntity>(id);
            var result = new Can_MachineOfLineModel();
            return result;
        }

        /// <summary>
        /// [Tam.Le] - Xử lý thêm mới hoặc chỉnh sửa một MachineOfLine
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public Can_MachineOfLineModel Post([Bind]Can_MachineOfLineModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Can_MachineOfLineEntity, Can_MachineOfLineModel>(model);
        }
    
    }
}
