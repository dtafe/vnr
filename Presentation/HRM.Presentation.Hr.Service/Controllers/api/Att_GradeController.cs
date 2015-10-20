using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
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

    public class Att_GradeController : ApiController
    {
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
        /// <summary>
        /// [Hieu.Van] - Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_GradeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_GradeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_GradeEntity>(id, ConstantSql.hrm_att_sp_get_Att_GradeById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_GradeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Hieu.Van] - Xóa hoặc chuyển đổi trạng thái sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_GradeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_GradeEntity, Att_GradeModel>(id);
            return result;
        }

        /// <summary>
        /// [Hieu.Van] - Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_Grade")]
        public Att_GradeModel Post([Bind]Att_GradeModel model)
        {
            #region Validate
            string message = string.Empty;
            bool checkValidate = false;
            if(model.ProfileID!=Guid.Empty && model.ProfileIDs==null)
            {
                model.ProfileIDs = model.ProfileID.ToString();
            }
            if (model.IsProfileNotGrade == true)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_GradeModel>(model, "Att_ProfileNotGradeInfo", "Att_Grade", ref message);
            }
            else
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_GradeModel>(model, "Att_Grade", ref message);
            }
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            var baseService = new BaseService();
            ActionService service = new ActionService(UserLogin);
            if (model.ID != Guid.Empty)
            {
                if (string.IsNullOrEmpty(model.OrgStructureID) && string.IsNullOrEmpty(model.ProfileIDs))
                {
                    message = string.Format(ConstantMessages.FieldNotAllowNull.TranslateString(), ("ProfileID").TranslateString());
                    model.ActionStatus = message;
                    return model;
                }
            }
            #endregion
            List<Guid> lstProfileIDs = new List<Guid>();
            string status = string.Empty;
            //xu ly chon nhan vien hay chon phong ban
            if (!string.IsNullOrEmpty(model.ProfileIDs))
            {
                List<Guid> lstMulti = new List<Guid>();
                var lst = model.ProfileIDs.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    lstMulti.Add(_Id);
                }
                lstProfileIDs = lstMulti;
            }
            else
            {
                //if (!string.IsNullOrEmpty(model.OrgStructureID))
                //{
                    if (model.OrgStructureID == "")
                    {
                        model.OrgStructureID = null;
                    }
                    List<object> lstObj = new List<object>();
                    lstObj.Add(model.OrgStructureID);
                    lstObj.Add(null);
                    lstObj.Add(null);
                    List<Hre_ProfileEntity> lstOrg = baseService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();
                    lstProfileIDs = lstOrg.Select(d => d.ID).ToList();
                    //neu co loai tru
                    if(!string.IsNullOrEmpty(model.ProfileIDsExclude) && lstProfileIDs.Count>0)
                    {
                        List<Guid> lstProfileIDsExclude = new List<Guid>();
                        var lst = model.ProfileIDsExclude.Split(',');
                        foreach (var item in lst)
                        {
                            Guid _Id = new Guid(item);
                            lstProfileIDsExclude.Add(_Id);
                        }
                        lstProfileIDs = lstProfileIDs.Where(d => !lstProfileIDsExclude.Contains(d)).ToList();
                    }
                //}
            }
            if (lstProfileIDs.Count > 0)
            {
                foreach (Guid item in lstProfileIDs)
                {
                    Att_GradeModel modelSave = model.CopyData<Att_GradeModel>();
                    modelSave.ProfileID = item;
                    modelSave = service.UpdateOrCreate<Att_GradeEntity, Att_GradeModel>(modelSave);
                    if (modelSave.ActionStatus != NotificationType.Success.ToString())
                        return modelSave;
                }
                model.SetPropertyValue(Constant.ActionStatus, NotificationType.Success.ToString());
                return model;
            }
            else
                return service.UpdateOrCreate<Att_GradeEntity, Att_GradeModel>(model);

            //return service.UpdateOrCreate<Att_GradeEntity, Att_GradeModel>(model);
        }

    }
}