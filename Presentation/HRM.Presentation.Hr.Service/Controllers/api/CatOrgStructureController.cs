using HRM.Presentation.Category.Models;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Web;
using HRM.Business.Category.Domain;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatOrgStructureController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu OrgStructure(Cat_OrgStructure) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatOrgStructureModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatOrgStructureModel();

            var orgServices = new Cat_OrgMoreInforServices();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_OrgStructureEntity>(id,ConstantSql.hrm_cat_sp_get_OrgStructureById ,ref status);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(id.ToString()));
            var orgInfoEntity = orgServices.GetData<Cat_OrgMoreInforEntity>(objs, ConstantSql.hrm_hr_sp_get_OrgMoreInfoByOrgID, UserLogin, ref status).FirstOrDefault();
            if (entity != null)
            {

                model = entity.CopyData<CatOrgStructureModel>();
                if (orgInfoEntity != null)
                {
                    model.ServicesType = orgInfoEntity.ServicesType;
                    model.BillingAddress = orgInfoEntity.BillingAddress;
                    model.ContractFrom = orgInfoEntity.ContractFrom;
                    model.ContractTo = orgInfoEntity.ContractTo;
                    model.BillingCompanyName = orgInfoEntity.BillingCompanyName;
                    model.TaxCode = orgInfoEntity.TaxCode;
                    model.DescriptionInfo = orgInfoEntity.Description;
                    model.DurationPay = orgInfoEntity.DurationPay;
                    model.RecipientInvoice = orgInfoEntity.RecipientInvoice;
                    model.TelePhone = orgInfoEntity.TelePhone;
                    model.CellPhone = orgInfoEntity.CellPhone;
                    model.EmailInfo = orgInfoEntity.Email;
                    model.OrgMoreInforID = (Guid?)orgInfoEntity.ID;
                }
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của OrgStructure(Cat_OrgStructure) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatOrgStructureModel DeleteOrRemove(string id)
        {
            //Xóa cache lưu lại của cây phòng ban
            HttpContext.Current.Cache.Remove("List_OrgStructureTreeView");
            HttpContext.Current.Cache.Remove("List_OrgStructureTreeViewSumProfile");

            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_OrgStructureEntity, CatOrgStructureModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một OrgStructure(Cat_OrgStructure)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatOrgStructure")]
        public CatOrgStructureModel Post([Bind]CatOrgStructureModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatOrgStructureModel>(model, "Cat_OrgStructure", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            //Xóa cache lưu lại của cây phòng ban
            HttpContext.Current.Cache.Remove("List_OrgStructureTreeView");
            HttpContext.Current.Cache.Remove("List_OrgStructureTreeViewSumProfile");

            #region Get max Order Number of Orgstructure
            if (model != null && model.ID == Guid.Empty)
            {
                Cat_OrgStructureServices orgService = new Cat_OrgStructureServices();
                model.OrderNumber = orgService.GetMaxOrgstructureOrder();
            }
            #endregion
            
            return service.UpdateOrCreate<Cat_OrgStructureEntity, CatOrgStructureModel>(model);
        }
    }
}