using System;
using HRM.Business.HrmSystem.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.HrmSystem.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System.Web.Mvc;
using HRM.Business.Main.Domain;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{

    public class Sys_AutoBackupController : ApiController
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
        /// [Tin.Nguyen] - Lấy dữ liệu Dân Tộc(Cat_ExportItem) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_AutoBackupModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sys_AutoBackupModel();
            ActionService service = new ActionService(UserLogin);
         
            var entity = service.GetById<Sys_AutoBackupEntity>(id, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Sys_AutoBackupModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Dân Tộc(Cat_ExportItem) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_AutoBackupModel DeleteOrRemove(string id)
        {
            //var spitId = id.Split(',');
            var model = new Sys_AutoBackupModel();
            //LibraryService.StartTaskScheduler(Common.ConvertToGuid(spitId[1]));
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sys_AutoBackupEntity, Sys_AutoBackupModel>(id);
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa Dân Tộc(Cat_ExportItem)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatExportItem")]
        public Sys_AutoBackupModel Post([Bind]Sys_AutoBackupModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sys_AutoBackupModel>(model, "Sys_AutoBackup", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }           
            #endregion
           
           
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sys_AutoBackupEntity, Sys_AutoBackupModel>(model);
        }

    }
}