using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Web.Mvc;
using System;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_CandidateHistoryController : ApiController
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
        /// [Quoc.Do] - Lấy dữ liệu CandidateHistory(Hre_CandidateHistory) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_CandidateHistoryModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_CandidateHistoryModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Hre_CandidateHistoryEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_CandidateHistoryById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_CandidateHistoryModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Quoc.Do] - Xóa hoặc chuyển đổi trạng thái của CandidateHistory(Hre_CandidateHistory) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_CandidateHistoryModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_CandidateHistoryEntity, Hre_CandidateHistoryModel>(id);
            return result;
        }

        /// <summary>
        /// [Quoc.Do] - Xử lý thêm mới hoặc chỉnh sửa một CandidateHistory(Hre_CandidateHistory)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_CandidateHistory")]
        public Hre_CandidateHistoryModel Post([Bind]Hre_CandidateHistoryModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_CandidateHistoryModel>(model, "Hre_CandidateHistory", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            string status = string.Empty;
            var candidateHistoryservices = new Hre_CandidateHistoryServices();
            ActionService service = new ActionService(UserLogin);
            if(model.CandidateID != null && model.CandidateID != Guid.Empty)
            {
                var profileBycandidateID = service.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.CandidateID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileByCandidateID, ref status).FirstOrDefault();
                if (profileBycandidateID != null)
                {
                    var canhisbyprofile = service.GetData<Hre_CandidateHistoryEntity>(Common.DotNetToOracle(profileBycandidateID.ID.ToString()), ConstantSql.hrm_hr_sp_get_CandidateHistoryByProfileId, ref status).FirstOrDefault();
                    if (canhisbyprofile != null)
                    {
                        model.ID = canhisbyprofile.ID;
                        model.ProfileID = canhisbyprofile.ProfileID;
                    }
                }
            }

            if (model.ProfileID != null && model.ProfileID != Guid.Empty)
            {
                var profile = service.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();

                if (profile != null)
                {
                    if(profile.CandidateID != null)
                    {
                        var canhisbyprofile = service.GetData<Hre_CandidateHistoryEntity>(Common.DotNetToOracle(profile.CandidateID.ToString()), ConstantSql.hrm_hr_sp_get_CandidateHistoryByCandidateId, ref status).FirstOrDefault();
                        if (canhisbyprofile != null)
                        {
                            model.ID = canhisbyprofile.ID;
                            model.CandidateID = canhisbyprofile.CandidateID;
                        }
                    }
                }
            }

            return service.UpdateOrCreate<Hre_CandidateHistoryEntity, Hre_CandidateHistoryModel>(model);
        }

    }
}
