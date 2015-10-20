using HRM.Business.Recruitment.Models;
using HRM.Business.Recruitment.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Recruitment.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Rec_TagController : ApiController
    {
        /// <summary>
        /// [T Lấy dữ liệu Nhãn Hồ Sơ(Rec_Tag) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
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
        public Rec_TagModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_TagModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_TagEntity>(id, ConstantSql.hrm_rec_sp_get_TagById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Rec_TagModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        public void Delete(Guid ID)
        {
            var service = new Rec_TagServices();
            var result = service.Remove<Rec_TagEntity>(ID);
        }

        //public Rec_TagModel DeleteOrRemove(string id)
        //{
        //    ActionService service = new ActionService(UserLogin);
        //    var result = service.DeleteOrRemove<Rec_TagEntity, Rec_TagModel>(id);
        //    return result;
        //}

        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa nhãn hồ sơ (Rec_Tag)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_Tag")]
        public Rec_TagModel Post([Bind]Rec_TagModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_TagModel>(model, "Rec_Tag", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rec_TagEntity, Rec_TagModel>(model);
        }
    }
}