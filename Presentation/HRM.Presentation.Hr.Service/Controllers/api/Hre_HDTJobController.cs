using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Web.Mvc;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_HDTJobController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu HDTJob(Hre_HDTJob) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_HDTJobModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_HDTJobModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetByIdUseStore<Hre_HDTJobEntity>(id, ConstantSql.hrm_hr_sp_get_HDTJobById, ref status);
            var entity = service.GetData<Hre_HDTJobEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_HDTJobById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_HDTJobModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của HDTJob(Hre_HDTJob) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_HDTJobModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_HDTJobEntity, Hre_HDTJobModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một HDTJob(Hre_HDTJob)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_HDTJob")]
        public Hre_HDTJobModel Post([Bind]Hre_HDTJobModel model)
        {
            bool warning = false;
            string status = string.Empty;
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_HDTJobModel>(model, "Hre_HDTJob", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            #region Kiểm tra quyền duyệt trong quá khi khi tạo HDT trong quá khứ

            if (model.DateFrom < DateTime.Now.Date)
            {
                List<object> lstobjUA = new List<object>();
                lstobjUA.Add(Common.DotNetToOracle(model.UserLoginID));
                lstobjUA.Add(ApproveType.E_HDTJOB_PAST.ToString());
                lstobjUA.Add(1);
                lstobjUA.Add(Int32.MaxValue - 1);
                var checkPermission = service.GetData<Sys_UserApproveEntity>(lstobjUA, ConstantSql.hrm_sys_sp_get_UserApprove, ref status);
                if (checkPermission == null || checkPermission.Count == 0)
                {
                    model.ActionStatus = ConstantDisplay.HRM_NotHavePermissionToCreatePastHDT.TranslateString();
                    return model;
                }
            }

            #endregion
            var baseService = new BaseService();
            var result = baseService.GetData<Hre_HDTJobEntity>(Common.DotNetToOracle(model.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_HDTJobsByProfileId, UserLogin, ref status).ToList();
            string ActionStatus = null;

            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    #region Validate theo task 0050045
                    if (((model.ID != item.ID) && (item.DateFrom != null && item.DateTo != null && item.DateFrom <= model.DateFrom && model.DateFrom <= item.DateTo)
                                    || (model.DateFrom <= item.DateFrom && item.DateFrom <= model.DateTo && model.DateFrom != item.DateFrom)) && (item.StatusOut != HDTJobStatus.E_APPROVE.ToString()))
                    {
                        ActionStatus = ConstantDisplay.HRM_ProfileNotOutOfOldHDT.TranslateString();
                    }
                    if (((model.ID != item.ID) && (item.DateFrom != null && item.DateTo != null && item.DateFrom <= model.DateFrom && model.DateFrom <= item.DateTo)
                                   || (model.DateFrom <= item.DateFrom && item.DateFrom <= model.DateTo && model.DateFrom != item.DateFrom)) && (item.StatusOut == HDTJobStatus.E_APPROVE.ToString()))
                    {
                        ActionStatus = ConstantDisplay.HRM_DuplidateTimeHDT.TranslateString();
                    } 
                    #endregion

                    #region Validate theo task 0050038
                    if (item.StatusOut != HDTJobStatus.E_APPROVE.ToString())
                    {
                        ActionStatus = ConstantDisplay.HRM_StatusOldNotApproved.TranslateString();
                    } 
                    #endregion
                }

            }
            if (ActionStatus != null)
            {
                model.ActionStatus = ActionStatus;
                return model;
            }

            var HDT = service.UpdateOrCreate<Hre_HDTJobEntity, Hre_HDTJobModel>(model);
            return HDT;
        }
    }
}
