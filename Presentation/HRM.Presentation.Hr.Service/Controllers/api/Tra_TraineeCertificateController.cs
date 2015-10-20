using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Training.Models;
using HRM.Business.Training.Models;
using HRM.Business.Training.Domain;
using System.Collections.Generic;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Tra_TraineeCertificateController : ApiController
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
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_TraineeCertificateModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Tra_TraineeCertificateModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Tra_TraineeCertificateEntity>(id, ConstantSql.hrm_tra_sp_get_TraineeCertificateById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Tra_TraineeCertificateModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Ngân Hàng(Tra_Certificate) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_TraineeCertificateModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var traineeCertificateServices = new Tra_TraineeCertificateServices();
            string status = string.Empty;
            var result = new Tra_TraineeCertificateModel();
            Guid[] _TraineeIds = null;
            var Ids = id.Substring(7);
            if(Ids != null)
            {
                _TraineeIds = Ids.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }
            var objTraineeCertificate = new List<object>();
            objTraineeCertificate.Add(1);
            objTraineeCertificate.Add(int.MaxValue -1);
            var lstTraineeCertificate = traineeCertificateServices.GetData<Tra_TraineeCertificateEntity>(objTraineeCertificate, ConstantSql.hrm_tra_sp_get_TraineeCertificate,UserLogin, ref status).ToList();
            if (lstTraineeCertificate != null)
            {
                lstTraineeCertificate = lstTraineeCertificate.Where(s => _TraineeIds.Contains(s.TraineeID.Value)).ToList();
            }
            var lstTraineeCertificateIds = lstTraineeCertificate.Select(s => s.ID).ToList();
            var strIds = string.Join(",", lstTraineeCertificateIds.ToArray());
            if (!string.IsNullOrEmpty(strIds))
            {
                var  strRemoveIds = DeleteType.Remove.ToString() + "," + strIds;
                result = service.DeleteOrRemove<Tra_TraineeCertificateEntity, Tra_TraineeCertificateModel>(strRemoveIds);
                return result;
            }

            return result;
            
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Tra_Certificate)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Tra_TraineeCertificate")]
        public Tra_TraineeCertificateModel Post([Bind]Tra_TraineeCertificateModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_TraineeCertificateModel>(model, "Tra_TraineeCertificate", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            #region Check HV đã có Certificate hay chưa
            ActionService service = new ActionService(UserLogin);
            string status = string.Empty;
            var traineeCertificateServices = new Tra_TraineeCertificateServices();
            var obj = new List<object>();
            obj.Add(1);
            obj.Add(int.MaxValue -1);
            var lstTraineeCertifitacate = traineeCertificateServices.GetData<Tra_TraineeCertificateEntity>(obj, ConstantSql.hrm_tra_sp_get_TraineeCertificate,UserLogin, ref status).ToList();

            if(model.ID == Guid.Empty){
                var lstCertificate = lstTraineeCertifitacate.Where(s => s.TraineeID == model.TraineeID.Value).ToList();
                if (lstCertificate.Count == 0)
                {
                    return service.UpdateOrCreate<Tra_TraineeCertificateEntity, Tra_TraineeCertificateModel>(model);
                }
                model.ActionStatus = ConstantMessages.WarningProfileHaveCertificate.TranslateString();
                return model;
            }
            #endregion
           
            return service.UpdateOrCreate<Tra_TraineeCertificateEntity, Tra_TraineeCertificateModel>(model);
        }
    }
}
