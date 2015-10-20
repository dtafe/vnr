using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Hr.Domain;
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
    public class Att_AllowLimitOvertimeController : ApiController
    {
        #region MyRegion
        ////
        //// GET: /Att_AllowLimitOvertime

        //public IEnumerable<Att_AllowLimitOvertimeModel> Get()
        //{
        //   ListQueryModel model = new ListQueryModel();
        //    var service = new Att_AllowLimitOvertimeServices();
        //    var list = service.GetAllowLimitOvertimes(model);

        //    return list.Select(item => new Att_AllowLimitOvertimeModel
        //    {
        //        Id = item.Id,
        //        ProfileID = item.ProfileID,
        //        ProfileName = item.ProfileName,
        //        DateStart = item.DateStart,
        //        DateEnd = item.DateEnd,
        //        LevelAllowLimit = item.LevelAllowLimit,
        //        Type = item.Type,

        //    });
        //}

        //// GET api/<controller>/5
        //public Att_AllowLimitOvertimeModel Get(int id)
        //{
        //    var service = new Att_AllowLimitOvertimeServices();
        //    var result = service.GetAllowLimitOvertimeById(id);
        //    var AttAllowLimitOvertime = new Att_AllowLimitOvertimeModel
        //    {
        //        Id = result.Id,
        //        ProfileID = result.ProfileID,
        //        ProfileName = result.ProfileName,
        //        DateStart = result.DateStart,
        //        DateEnd = result.DateEnd,
        //        LevelAllowLimit = result.LevelAllowLimit,
        //        Type = result.Type
        //    };
        //    return AttAllowLimitOvertime;
        //}

        //public Att_AllowLimitOvertimeModel Put(Att_AllowLimitOvertimeModel model)
        //{
        //    var AttAllowLimitOvertime = new Att_AllowLimitOvertime
        //    {
        //        Id = model.Id,
        //        ProfileID = model.ProfileID,
        //        DateStart = model.DateStart,
        //        DateEnd = model.DateEnd,
        //        LevelAllowLimit = model.LevelAllowLimit,
        //        Type = model.Type
        //    };
        //    var service = new Att_AllowLimitOvertimeServices();
        //    if (model.Id != 0)
        //    {
        //        AttAllowLimitOvertime.Id = model.Id;
        //        service.Edit(AttAllowLimitOvertime);
        //    }
        //    else
        //    {
        //        service.Add(AttAllowLimitOvertime);
        //    }

        //    return model;
        //}

        //public Att_AllowLimitOvertimeModel Post([FromBody]Att_AllowLimitOvertimeModel allowLimitOvertime)
        //{
        //    var model = new Att_AllowLimitOvertime
        //    {
        //        Id = allowLimitOvertime.Id,
        //        ProfileID = allowLimitOvertime.ProfileID,
        //        DateStart = allowLimitOvertime.DateStart,
        //        DateEnd = allowLimitOvertime.DateEnd,
        //        LevelAllowLimit = allowLimitOvertime.LevelAllowLimit,
        //        Type = allowLimitOvertime.Type
        //    };
        //    var service = new Att_AllowLimitOvertimeServices();
        //    if (allowLimitOvertime.Id != 0)
        //    {
        //        model.Id = allowLimitOvertime.Id;
        //        service.Edit(model);
        //    }
        //    else
        //    {
        //        service.Add(model);
        //        allowLimitOvertime.Id = model.Id;
        //    }

        //    return allowLimitOvertime;
        //}

        //// DELETE api/<controller>/5
        //public Att_AllowLimitOvertimeModel Delete(int id)
        //{
        //    var service = new Att_AllowLimitOvertimeServices();
        //    service.Delete(id);
        //    var result = new Att_AllowLimitOvertimeModel();
        //    return result;
        //}
        #endregion
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
        public Att_AllowLimitOvertimeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_AllowLimitOvertimeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_AllowLimitOvertimeEntity>(id, ConstantSql.hrm_att_sp_get_AllowLimitOvertimebyid ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_AllowLimitOvertimeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Hieu.Van] - Xóa hoặc chuyển đổi trạng thái sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_AllowLimitOvertimeModel DeleteOrRemove(Guid id)
        {
            //ActionService service = new ActionService(UserLogin);
            //var result = service.DeleteOrRemove<Att_AllowLimitOvertimeEntity, Att_AllowLimitOvertimeModel>(id);
            //return result;
            Att_AllowLimitOvertimeServices service = new Att_AllowLimitOvertimeServices();
            var result = service.Remove<Att_AllowLimitOvertimeEntity>(id);
            //return result;
            return new Att_AllowLimitOvertimeModel();
        }

        /// <summary>
        /// [Hieu.Van] - Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_AllowLimitOvertime")]
        public Att_AllowLimitOvertimeModel Post([Bind]Att_AllowLimitOvertimeModel model)
        {
            #region
            string message = string.Empty;
            ActionService service = new ActionService(UserLogin);
            var baseService = new BaseService();
            string Status=string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_AllowLimitOvertimeModel>(model, "Att_AllowLimitOvertime", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            #region kiểm tra trùng dữ liệu
            List<object> listpara = new List<object>();
            listpara.Add(null);
            listpara.Add(null);
            listpara.Add(null);
            listpara.Add(null);
            listpara.Add(null);
            listpara.Add(1);
            listpara.Add(10000000);


            var result = baseService.GetData<Att_AllowLimitOvertimeEntity>(listpara, ConstantSql.hrm_att_sp_get_AllowLimitOvertime, UserLogin, ref Status);
            if (result != null && result.Count > 0)
            {
                if (model.ID == Guid.Empty)
               {
                   if (result.Where(x => x.ProfileID == model.ProfileID && x.DateStart <= model.DateEnd && x.DateEnd >= model.DateStart).Count()!=0)
                   {

                       message = result.Where(s=>s.ProfileID==model.ProfileID).FirstOrDefault().ProfileName +", ";
                       model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + message + ConstantMessages.FieldDuplicate);
                               return model;
 
                   }
               }
                else
                {
                    var objAllowLimitOvertime = service.GetByIdUseStore<Att_AllowLimitOvertimeEntity>(model.ID, ConstantSql.hrm_att_sp_get_AllowLimitOvertimebyid, ref Status);
                   // if (AllowLimitOvertime != null &&( AllowLimitOvertime.DateStart != model.DateStart && AllowLimitOvertime.DateEnd!=model.)
                    result = result.Where(x => x.ID != objAllowLimitOvertime.ID).ToList();
                    if (result.Count > 0)
                    {
                        if (result.Where(x => x.ProfileID == objAllowLimitOvertime.ProfileID && x.DateStart <= model.DateEnd && x.DateEnd >= model.DateStart).Count() != 0)
                        {
                            message = result.Where(s => s.ProfileID == model.ProfileID).FirstOrDefault().ProfileName + ", ";
                            model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + message + ConstantMessages.FieldDuplicate);
                            return model;

                        }
                    }
                }

            }
            #endregion
            return service.UpdateOrCreate<Att_AllowLimitOvertimeEntity, Att_AllowLimitOvertimeModel>(model);
        }
	}
}