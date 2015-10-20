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
using HRM.Business.Main.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_RelativesController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu Relatives(Hre_Relatives) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_RelativesModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_RelativesModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Hre_RelativesEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_RelativesById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_RelativesModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Relatives(Hre_Relatives) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_RelativesModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_RelativesEntity, Hre_RelativesModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Relatives(Hre_Relatives)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_Relatives")]
        public Hre_RelativesModel Post([Bind]Hre_RelativesModel model)
        {
            #region Validate
            string status = string.Empty;
            if (model.YearOfBirth != string.Empty && model.YearOfBirth.Length >= 10)
            {
                string[] ArrayYearOfBirth = model.YearOfBirth.Split('/').ToArray();
                ArrayYearOfBirth[2] = ArrayYearOfBirth[2].Substring(0, 4);
                if (ArrayYearOfBirth[0].Length < 2)
                {
                    ArrayYearOfBirth[0] = "0" + ArrayYearOfBirth[0];
                }
                if (ArrayYearOfBirth[1].Length < 2)
                {
                    ArrayYearOfBirth[1] = "0" + ArrayYearOfBirth[1];
                }
                string _tmp = ArrayYearOfBirth[1];
                ArrayYearOfBirth[1] = ArrayYearOfBirth[0];
                ArrayYearOfBirth[0] = _tmp;
                model.YearOfBirth = string.Join("/", ArrayYearOfBirth);

                //#region Kiểm tra tổi con nhỏ mà lớn hơn 18 tuổi thì không cho lưu

                //try
                //{
                //    DateTime bday = new DateTime(int.Parse(ArrayYearOfBirth[2]), int.Parse(ArrayYearOfBirth[1]), int.Parse(ArrayYearOfBirth[0]));
                //    DateTime today = DateTime.Today;
                //    int age = today.Year - int.Parse(ArrayYearOfBirth.LastOrDefault());
                //    if (bday > today.AddYears(-age))
                //    {
                //        age--;
                //    }
                //    if (age >= 18)
                //    {
                //        model.ActionStatus = "Con Nhỏ Không Thể Lớn Hơn 18 tuổi !";
                //        return model;
                //    }
                //}
                //catch
                //{

                //}
                //#endregion
            }
                
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_RelativesModel>(model, "Hre_Relatives", ref status);
            if (!checkValidate)
            {
                model.ActionStatus = status;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);

            #region thêm hoặc update người phụ thuộc từ người thân
            if (model.CheckAddDependant.HasValue && model.CheckAddDependant.Value==false)
            {
                Hre_DependantServices Services1 = new Hre_DependantServices();
                Services1.AddOrUpdateDependant(model.Copy<Hre_RelativesEntity>());
            }
            #endregion

            #region Son.Vo - 20150120 - gán cột EmpCodeRelativesName = mã nv + tên người thân
            var entityprofilebyRelative = service.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();
            string codeEmpByRelatives = entityprofilebyRelative != null ? entityprofilebyRelative.CodeEmp : string.Empty;
            model.EmpCodeRelativesName = codeEmpByRelatives + model.RelativeName; 
            #endregion

            var entity = service.GetData<Hre_RelativesEntity>(model.IDNo, ConstantSql.hrm_hr_sp_get_RelativesByIdNo, ref status);
   
             service.UpdateOrCreate<Hre_RelativesEntity, Hre_RelativesModel>(model);
            
            if (entity != null && entity.Count > 0)
            {
                model.ActionStatus += ",Số CMND đã tồn tại trong hệ thống";
            }
            return model;
        }
    }
}
