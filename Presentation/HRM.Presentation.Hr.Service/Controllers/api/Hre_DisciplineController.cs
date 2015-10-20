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
using HRM.Business.Category.Models;
using HRM.Business.Category.Domain;
using HRM.Business.Main.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_DisciplineController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu Discipline(Hre_Discipline) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_DisciplineModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_DisciplineModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetByIdUseStore<Hre_DisciplineEntity>(id, ConstantSql.hrm_hr_sp_get_DisciplineById, ref status);
            var entity = service.GetData<Hre_DisciplineEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_DisciplineById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_DisciplineModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Discipline(Hre_Discipline) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_DisciplineModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_DisciplineEntity, Hre_DisciplineModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Discipline(Hre_Discipline)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_Discipline")]
        public Hre_DisciplineModel Post([Bind]Hre_DisciplineModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_DisciplineModel>(model, "Hre_Discipline", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion

            #region cột DepartmentName = phòng ban hiện tại của NV
            string status = string.Empty;
            var orgServices = new Cat_OrgStructureServices();
            var profileServices = new Hre_ProfileServices();
            var lstObjOrg = new List<object>();
            lstObjOrg.Add(null);
            lstObjOrg.Add(null);
            lstObjOrg.Add(null);
            lstObjOrg.Add(1);
            lstObjOrg.Add(int.MaxValue - 1);
            var lstOrg = orgServices.GetData<Cat_OrgStructureEntity>(lstObjOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, UserLogin, ref status).ToList();

            var orgTypeService = new Cat_OrgStructureTypeServices();
            var lstObjOrgType = new List<object>();
            lstObjOrgType.Add(null);
            lstObjOrgType.Add(null);
            lstObjOrgType.Add(1);
            lstObjOrgType.Add(int.MaxValue - 1);
            var lstOrgType = orgTypeService.GetData<Cat_OrgStructureTypeEntity>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, UserLogin, ref status).ToList();

            string orgstructureNameByProfile = string.Empty;
            if (model.ProfileID != null)
            {
                var profilebydis = profileServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref status).FirstOrDefault();
                if (profilebydis != null)
                {
                    Guid? orgId = profilebydis.OrgStructureID;
                    var org = lstOrg.FirstOrDefault(s => s.ID == profilebydis.OrgStructureID);
                    var department = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_DEPARTMENT, lstOrg, lstOrgType);
                    orgstructureNameByProfile = department != null ? department.OrgStructureName : string.Empty;
                }
            }
            model.DepartmentName = orgstructureNameByProfile; 
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_DisciplineEntity, Hre_DisciplineModel>(model);
        }
    }
}
