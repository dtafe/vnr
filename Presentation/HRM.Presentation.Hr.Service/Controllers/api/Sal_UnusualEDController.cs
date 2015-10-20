using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Payroll.Domain;
using HRM.Presentation.Payroll.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Payroll.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;
using HRM.Business.Hr.Domain;

namespace HRM.Presentation.Payroll.Service.Controllers.api
{
    public class Sal_UnusualEDController : ApiController
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
        //
        // GET: /Sal_UnusualED/
        public Sal_UnusualAllowanceModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_UnusualAllowanceModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_UnusualAllowanceEntity>(id, ConstantSql.hrm_sal_sp_get_UnusualEDById, ref status);
            if (entity != null)
            {
                model = entity.Copy<Sal_UnusualAllowanceModel>();
            }
            model.ActionStatus = status;
            return model;
        }
        public Sal_UnusualAllowanceModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_UnusualED")]
        public Sal_UnusualAllowanceModel Post([Bind]Sal_UnusualAllowanceModel model)
        {
            var hrService = new Hre_ProfileServices();
            var UnusualAllowanceServices = new Sal_UnusualAllowanceServices();
            string status = string.Empty;
            #region Validate
            string message = string.Empty;
            var checkValidate = false;
            //kiểm tra xem có phải là đang create ở trang chi tiết nv ko
            if (model.IsGeneralProfile != null && model.IsGeneralProfile == true)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualEDGeneralProfile", ref message);

                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
                ActionService service = new ActionService(UserLogin);
                return service.UpdateOrCreate<Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceModel>(model);
            }
            else
            {
                if (!string.IsNullOrEmpty(model.OrgStructureIDs))
                {
                    checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualEDOrg", ref message);
                }
                else
                {
                    checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualED", ref message);
                }
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }

                if (!string.IsNullOrEmpty(model.OrgStructureIDs))
                {
                    List<Guid> listGuid = new List<Guid>();
                    if (model.ProfileIDExclusion != null)
                    {
                        var listStr = model.ProfileIDExclusion.Split(',');

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
                    var lstProfile = hrService.GetData<Hre_ProfileIdEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure, userLogin, ref status).Select(s => s.ID).ToList();
                    if (listGuid != null)
                    {
                        lstProfile = lstProfile.Where(s => !listGuid.Contains(s)).ToList();
                    }

                    List<Sal_UnusualAllowanceEntity> lstUnusualAllowance = new List<Sal_UnusualAllowanceEntity>();
                    foreach (var i in lstProfile)
                    {
                        Sal_UnusualAllowanceEntity item = new Sal_UnusualAllowanceEntity();
                        item = model.Copy<Sal_UnusualAllowanceEntity>();
                        item.ProfileID = i;
                        lstUnusualAllowance.Add(item);
                    }
                    model.ActionStatus = UnusualAllowanceServices.Add(lstUnusualAllowance);
                    return model;
                }
                else
                {
                    string[] listProfile = model.ProfileIDs.Split(',');
                    List<Sal_UnusualAllowanceEntity> lstUnusualAllowance = new List<Sal_UnusualAllowanceEntity>();
                    foreach (var i in listProfile)
                    {
                        Sal_UnusualAllowanceEntity item = new Sal_UnusualAllowanceEntity();
                        item = model.Copy<Sal_UnusualAllowanceEntity>();
                        item.ProfileID = Guid.Parse(i);
                        lstUnusualAllowance.Add(item);
                    }
                    model.ActionStatus = UnusualAllowanceServices.Add(lstUnusualAllowance);
                    return model;
                }
            }
            #endregion

        }

	}
}