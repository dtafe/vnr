using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Category.Models;
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
    public class Att_OvertimeEditController : ApiController
    {
        public string status = string.Empty;
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
        public IEnumerable<Att_OvertimeModel> Get(Guid id)
        {
            var service = new Att_OvertimeServices();
            List<Att_OvertimeEntity> lst = new List<Att_OvertimeEntity>();
            List<object> objpara = new List<object>();
            objpara.AddRange(new object[3]);
            objpara[0] = id;
            objpara[1] = 1;
            objpara[2] = int.MaxValue - 1;
            var rs = service.GetData<Att_OvertimeEntity>(id, ConstantSql.hrm_att_sp_get_OvertimeByProfileId,UserLogin, ref status).FirstOrDefault();
            lst.Add(rs);
            return lst.Select(item => new Att_OvertimeModel
            {
                ID = item.ID,
                ProfileID = item.ProfileID,
                ProfileName = item.ProfileName,
                Status = item.Status,
                WorkDate = item.WorkDate,
                RegisterHours = item.RegisterHours,
                MethodPayment = item.MethodPayment,
                ReasonOT = item.ReasonOT,
                ShiftID = item.ShiftID,
                OvertimeTypeID = item.OvertimeTypeID,
                UserApproveID = item.UserApproveID,
                UserApproveID2 = item.UserApproveID2,
                ShiftName = item.ShiftName,
                OvertimeTypeName = item.OvertimeTypeName
            });
        }
        /// <summary>
        /// Xử lí eidt và add new truyền theo script
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_OvertimeEdit")]
        public Att_OvertimeUpdateModel Post([FromBody]Att_OvertimeUpdateModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_OvertimeUpdateModel>(model, "Att_Overtime", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            var AttOvertime = new Att_OvertimeEntityEdit
            {
                ID = model.ID,
                ProfileID = model.ProfileID,
                Status = model.Status,
                WorkDate = model.WorkDate,
                RegisterHours = model.RegisterHours,
                MethodPayment = model.MethodPayment,
                ReasonOT = model.ReasonOT,
                ShiftID = model.ShiftID,
                OvertimeTypeID = model.OvertimeTypeID != null ? model.OvertimeTypeID : Guid.Empty,
                DateUpdate = model.DateUpdate,
                UserApproveID = model.UserApproveID != null ? model.UserApproveID : Guid.Empty,
                UserApproveID2 = model.UserApproveID2,
                ActionStatus = model.ActionStatus
            };

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Att_OvertimeEntityEdit, Att_OvertimeUpdateModel>(model);
            //var service = new Att_OvertimeServices();
            //if (model.ID != Guid.Empty)
            //{
            //    AttOvertime.ID = model.ID;
            //    service.Edit(AttOvertime);
            //}
            //else
            //{
            //    service.Add(AttOvertime);
            //}
            //return model;
        }
    }
}