using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Payroll.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Attendance.Models;
using HRM.Business.Attendance.Models;
using HRM.Business.Attendance.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{

    public class Att_TimeSheetController : ApiController
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
        public Att_TimeSheetModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_TimeSheetModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_TimeSheetEntity>(id, ConstantSql.hrm_att_sp_get_TimeSheetById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_TimeSheetModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Att_TimeSheetModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_TimeSheetEntity, Att_TimeSheetModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_TimeSheet")]
        public Att_TimeSheetModel Post([Bind]Att_TimeSheetModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_TimeSheetModel>(model, "Att_TimeSheet", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            string status = string.Empty;
            var hrService = new Hre_ProfileServices();
            var timeSheetServices = new Att_TimeSheetServices();
            if (!string.IsNullOrEmpty(model.OrgStructureID))
            {
                List<Guid> listGuid = new List<Guid>();
                if (model.ProfileIDs != null)
                {
                    var listStr = model.ProfileIDs.Split(',');

                    if (listStr[0] != "")
                    {

                        foreach (var item in listStr)
                        {
                            listGuid.Add(Guid.Parse(item));
                        }
                    }
                }

                List<object> listObj = new List<object>();
                listObj.Add(model.OrgStructureID);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);
                var lstProfile = hrService.GetData<Hre_ProfileIdEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure,UserLogin, ref status).Select(s => s.ID).ToList();
                if (listGuid != null)
                {
                    lstProfile = lstProfile.Where(s => !listGuid.Contains(s)).ToList();
                }

                List<Att_TimeSheetEntity> lstTimeSheetEntity = new List<Att_TimeSheetEntity>();

                foreach (var item in lstProfile)
                {
                    Att_TimeSheetEntity timeSheetEntity = new Att_TimeSheetEntity
                    {
                        ProfileID = item,
                        DateCreate = model.DateCreate,
                        DateLock = model.DateLock,
                        RoleID = model.RoleID,
                        JobTypeID = model.JobTypeID,
                        ID = model.ID,
                        NoHour = model.NoHour,
                        Date = model.Date,
                        Sector = model.Sector,
                        Note = model.Note,
                        IsDelete = model.IsDelete,
                        UserUpdate = model.UserCreate,
                        UserCreate = model.UserCreate,
                    };
                    lstTimeSheetEntity.Add(timeSheetEntity);
                }
                model.ActionStatus = timeSheetServices.Add(lstTimeSheetEntity);
                return model;
            }
            if (model.ProfileIDs != null && model.ProfileIDs.IndexOf(',') > 0)
            {
                var listStr = model.ProfileIDs.Split(',');
                List<Guid> listGuid = new List<Guid>();
                if (listStr[0] != "")
                {
                    foreach (var item in listStr)
                    {
                        listGuid.Add(Guid.Parse(item));
                    }
                }
                List<Att_TimeSheetEntity> lstTimeSheetEntity = new List<Att_TimeSheetEntity>();
                foreach (var item in listGuid)
                {
                    Att_TimeSheetEntity TimeSheetEntity = new Att_TimeSheetEntity
                    {
                        ProfileID = item,
                        DateCreate = model.DateCreate,
                        DateLock = model.DateLock,
                        RoleID = model.RoleID,
                        JobTypeID = model.JobTypeID,
                        ID = model.ID,
                        NoHour = model.NoHour,
                        Date = model.Date,
                        Note = model.Note,
                        Sector = model.Sector,
                        IsDelete = model.IsDelete,
                        UserUpdate = model.UserCreate,
                        UserCreate = model.UserCreate,
                    };
                    lstTimeSheetEntity.Add(TimeSheetEntity);
                }
                model.ActionStatus = timeSheetServices.Add(lstTimeSheetEntity);
                return model;
            }

            return service.UpdateOrCreate<Att_TimeSheetEntity, Att_TimeSheetModel>(model);
        }

    }
}