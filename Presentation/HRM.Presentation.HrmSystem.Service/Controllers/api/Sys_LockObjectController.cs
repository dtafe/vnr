using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_LockObjectController : ApiController
    {
        #region MyRegion
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    //var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    //userLogin = headerValues.FirstOrDefault();
                    userLogin = "son.vo";
                }
                return userLogin;
            }
        }
        #endregion
        //
        // GET: /Sys_LockObject/
        //// GET api/<controller>
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        public IEnumerable<Sys_LockObjectModel> Get()
        {
            string status = string.Empty;
            var service = new Sys_LockObjectServices();
            var list = service.GetAllUseEntity<Sys_LockObjectEntity>(ref status);

            return list.Translate<Sys_LockObjectModel>();

        }

        // GET api/<controller>/5
        public Sys_LockObjectModel Get(Guid id)
        {
            string status = string.Empty;
            var model = new Sys_LockObjectModel();
            ActionService service = new ActionService(UserLogin);
            var cutOfServices = new Att_CutOffDurationServices();
            var orgServices = new Cat_OrgStructureServices();
            var entity = service.GetByIdUseStore<Sys_LockObjectEntity>(id, ConstantSql.hrm_sys_sp_get_LockObjectByID, ref status);

            List<object> listModel = new List<object>();
            listModel.AddRange(new object[3]);
            listModel[1] = 1;
            listModel[2] = Int32.MaxValue - 1;
            List<Att_CutOffDurationEntity> listCutoffduration_All = cutOfServices.GetData<Att_CutOffDurationEntity>(listModel, ConstantSql.hrm_att_sp_get_CutOffDurations, UserLogin, ref status).ToList();
            
            var objOrg = new List<object>();
            objOrg.Add(null);
            objOrg.Add(null);
            objOrg.Add(null);
            objOrg.Add(1);
            objOrg.Add(int.MaxValue -1);
            var lstOrg = orgServices.GetData<Cat_OrgStructureEntity>(objOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, UserLogin, ref status).ToList();

            if (entity != null)
            {
                model = entity.CopyData<Sys_LockObjectModel>();

                if (model.PayrollGroups != null)
                {
                    model.lstPayrollGroupID = Common.GetListNumbersFromBinary(model.PayrollGroups);
                    model.PayrollGroupID = string.Join(",", model.lstPayrollGroupID);
                }
                if (model.OrgStructures != null)
                {
                    model.lstOrgStructureID = Common.GetListNumbersFromBinary(model.OrgStructures);
                    var lstOrgName =  lstOrg.Where(s => model.lstOrgStructureID.Contains(s.OrderNumber)).Select(s => s.OrgStructureName).ToList();
                    model.OrgStructureID = string.Join(",", model.lstOrgStructureID);
                    model.OrgStructureName = string.Join(",", lstOrgName);
                }
                var cutOfDurationEntity = listCutoffduration_All.Where(s => s.DateEnd == model.DateEnd && s.DateStart == model.DateStart).FirstOrDefault();
                if (cutOfDurationEntity != null)
                {
                    model.CutOffDurationID = cutOfDurationEntity.ID;
                    model.CutOffDurationName = cutOfDurationEntity.CutOffDurationName;
                }
            }
            model.ActionStatus = status;
            return model;
        }

        public Sys_LockObjectModel Put(Sys_LockObjectModel model)
        {
            var Sys_LockObject = model.CopyData<Sys_LockObjectEntity>();

            var service = new Sys_LockObjectServices();
            if (model.ID != Guid.Empty)
            {
                Sys_LockObject.ID = model.ID;
                service.Edit<Sys_LockObjectEntity>(Sys_LockObject);
            }
            else
            {
                service.Add<Sys_LockObjectEntity>(Sys_LockObject);
            }
            return model;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_LockObject")]
        public Sys_LockObjectModel Post(Sys_LockObjectModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sys_LockObjectModel>(model, "Sys_LockObject", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            model.Type = "E_LOCKOBJECT";
            if(!string.IsNullOrEmpty(model.OrgStructureID))
            {
                List<int> lstOrg = model.OrgStructureID.Split(',').Select(s => int.Parse(s)).Distinct().ToList();
                model.OrgStructures = Common.ListNumbersToBinary(lstOrg);
            }
           
            if (!string.IsNullOrEmpty(model.PayrollGroupID))
            {
                List<int> lstPayr = model.PayrollGroupID.Split(',').Select(s => int.Parse(s)).Distinct().ToList();
                model.PayrollGroups = Common.ListNumbersToBinary(lstPayr);
            }

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sys_LockObjectEntity, Sys_LockObjectModel>(model);
        }


        public Sys_LockObjectModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sys_LockObjectEntity, Sys_LockObjectModel>(id);
            return result;
        }
    }
}