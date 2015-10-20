using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;
using HRM.Presentation.Training.Models;
using HRM.Business.Training.Models;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Tra_CourseController : ApiController
    {
        #region UserLogin
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
        public Tra_CourseModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Tra_CourseModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Tra_CourseEntity>(id, ConstantSql.hrm_tra_sp_get_CourseById, ref status);
            if (entity!=null)
            {
                //lay ten phong ban cho tab giao vien
                if (entity.OrgListTrainerCode != null)
                {
                    var lstOrgStructureName = service.GetData<Cat_OrgStructureEntity>(entity.OrgListTrainerCode, ConstantSql.hrm_cat_sp_get_OrgByOrderNumbers, ref status);
                    if (lstOrgStructureName != null)
                    {
                        
                        foreach (var item in lstOrgStructureName)
	                    {
		                    entity.OrgStructureTrainerName +=item.OrgStructureName+",";
	                    }
                    }
                    
                }
                if (entity.OrgStructureTrainerName != null)
                {
                    entity.OrgStructureTrainerName = entity.OrgStructureTrainerName.Substring(0, entity.OrgStructureTrainerName.Length - 1);
                }
                //lay ten phong ban cho tab hoc vien
                if(entity.OrgListCode1 !=null )
                {
                    var lstOrgStructureNameStandards = service.GetData<Cat_OrgStructureEntity>(entity.OrgListCode1, ConstantSql.hrm_cat_sp_get_OrgByOrderNumbers, ref status);
                    if (lstOrgStructureNameStandards != null)
                    {

                        foreach (var item in lstOrgStructureNameStandards)
                        {
                            entity.OrgStructureName += item.OrgStructureName + ",";
                        }
                    }
                }
                if(entity.OrgStructureName != null)
                {
                    entity.OrgStructureName = entity.OrgStructureName.Substring(0, entity.OrgStructureName.Length - 1);
                }

                model = entity.CopyData<Tra_CourseModel>();
                if (model.IsComplex == false)
                {
                    model.IsNormal = true;
                }
                if(model.TypeHandleFomular == EnumDropDown.FormulaType.E_TYPEHANDLEFORMULA_AVERAGE.ToString())
                {
                    model.IsTypeHandleFomularAuto = true;
                }
                if(model.TypeHandleFomular == EnumDropDown.FormulaType.E_TYPEHANDLEFORMULA_NORMAL.ToString())
                {
                    model.IsTypeHandleFomular = true;
                }
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Ngân Hàng(Tra_Certificate) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_CourseModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Tra_CourseEntity, Tra_CourseModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Tra_Certificate)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Tra_Course")]
        public Tra_CourseModel Post([Bind]Tra_CourseModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_CourseModel>(model, "Tra_Course", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            model.Formula = model.Formula.Replace("[+]","+");
            return service.UpdateOrCreate<Tra_CourseEntity, Tra_CourseModel>(model);
        }
    }
}
