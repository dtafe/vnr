using HRM.Business.Payroll.Models;
using HRM.Business.Payroll.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Payroll.Models;
using HRM.Presentation.Hr.Models;
using HRM.Business.Category.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Business.Hr.Models;
using HRM.Business.Hr.Domain;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Sal_UnusualAllowanceController : ApiController
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sal_UnusualAllowanceModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_UnusualAllowanceModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_UnusualAllowanceEntity>(id, ConstantSql.hrm_sal_sp_get_UnusualAllowanceByIds, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Sal_UnusualAllowanceModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        public void Delete(Guid ID)
        {
            var service = new Sal_UnusualAllowanceServices();
            var result = service.Remove<Sal_UnusualAllowanceEntity>(ID);
        }

  
        /// <summary>
        /// Xử lý thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_UnusualAllowance")]
        public Sal_UnusualAllowanceModel Post([Bind]Sal_UnusualAllowanceModel model)
        {
            
            #region Validate
            string message = string.Empty;
            string status = string.Empty;
            ActionService service = new ActionService(UserLogin);
            var checkValidate=true;
            checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualAllowanceinfoNew", "Sal_UnusualAllowance", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            if(model != null && model.UnusualEDTypeID !=null)
            {
                var entity = service.GetByIdUseStore<Cat_UnusualAllowanceCfgEntity>(model.UnusualEDTypeID.Value, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfgId, ref status);
                if (entity != null && entity.Code != null)
                {
                    if (entity.Code.Replace(" ", "") == "DEATHOFEMPLOYEE" || entity.Code.Replace(" ", "") == "HOSPITALOFEMPLOYEEMORETHANONEMONTH")
                    {
                        checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualAllowanceinfoNew2", "Sal_UnusualAllowance", ref message);

                    }
                    else
                    {
                        checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualAllowanceinfoNew1", "Sal_UnusualAllowance", ref message);
                    }
                    if (!checkValidate)
                    {
                        model.ActionStatus = message;
                        return model;
                    }
                    if (entity.Code.Replace(" ", "") == "BIRTHOFACHILD" || entity.Code.Replace(" ", "") == "DEATHOFEMPLOYEECHILD" || entity.Code.Replace(" ", "") == "DEATHOFEMPLOYEESPOUSE" || entity.Code.Replace(" ", "") == "FUNERALOFPARENT")
                    {
                        checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualAllowanceinfo1And2", "Sal_UnusualAllowance", ref message);
                        if(checkValidate == false)
                        {
                            checkValidate = true;
                            //lay thong tin cua quan he
                            var objRelativesModel = service.GetData<Hre_RelativesModel>(Common.DotNetToOracle(model.RelativeID.ToString()), ConstantSql.hrm_hr_sp_get_RelativesById, ref status).FirstOrDefault();
                            var checkValidateRelatives = true;
                            if (objRelativesModel != null)
                            {
                                //lay all nguoi than cua nv do
                                Hre_RelativesServices hreServiveRelative = new Hre_RelativesServices();
                                var lstRelativeByProfileId = hreServiveRelative.GetRelativeByProfileId(model.ProfileID).ToList();
                                if (lstRelativeByProfileId.Count>0)
                                {
                                    //if (model.ID == Guid.Empty)
                                    //    objRelativesModel.ID = Guid.Empty;
                                    var lstDuplicateRelativeByProfileID=lstRelativeByProfileId.
                                        Where(s=>s.ProfileID==objRelativesModel.ProfileID && s.RelativeName==objRelativesModel.RelativeName && s.YearOfBirth==objRelativesModel.YearOfBirth).ToList();
                                    if(lstDuplicateRelativeByProfileID.Count>0)
                                        checkValidateRelatives=false;
                                }
                              //  checkValidateRelatives = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_RelativesModel>(objRelativesModel, "Hre_Relativesinfo1And2", "Hre_Relatives", ref message);
                            }
                            if (checkValidateRelatives == false)
                            {
                                model.ActionStatus = message;
                                return model;
                            }
                        }
                    }
                    else if (entity.Code.Replace(" ", "")=="HOSPITALOFEMPLOYEEMORETHANONEMONTH")
                    {
                        Sal_ReportService salService = new Sal_ReportService();
                        int _daysInMonth = System.DateTime.DaysInMonth(model.DateOccur.Value.Year, model.DateOccur.Value.Month);
                        DateTime _datestart = new DateTime(model.DateOccur.Value.Year, model.DateOccur.Value.Month, 1);
                        DateTime _dateend = new DateTime(model.DateOccur.Value.Year, model.DateOccur.Value.Month, _daysInMonth);
                        //List<object> paraUnusualAllowance = new List<object>();
                        //paraUnusualAllowance.AddRange(new object[7]);
                        //paraUnusualAllowance[2] = model.UnusualEDTypeID;
                        //paraUnusualAllowance[3] = _datestart;
                        //paraUnusualAllowance[4] = _dateend;
                        //paraUnusualAllowance[5] = 1;
                        //paraUnusualAllowance[6] = int.MaxValue;
                        Sal_UnusualAllowanceServices salServiceUnusualAllowance = new Sal_UnusualAllowanceServices();
                        //var lstUnusualAllowance = salServiceUnusualAllowance.GetData<Sal_UnusualAllowanceEntity>(paraUnusualAllowance, ConstantSql.hrm_sal_sp_get_UnusualAllowanceFilialWedding, ref status);
                        var lstUnusualAllowance = salServiceUnusualAllowance.GetUnusualAllowanceByUnusualEDTypeIDAndDateOccur(model.ProfileID,model.UnusualEDTypeID, _datestart, _dateend);
                        if (lstUnusualAllowance != null)
                        {
                            //lstUnusualAllowance = lstUnusualAllowance.Where(s => s.ProfileID == model.ProfileID).ToList();
                            if ((lstUnusualAllowance.Count > 0 && model.ID == Guid.Empty) || (lstUnusualAllowance.Count > 1 && model.ID != Guid.Empty))
                            {
                                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualAllowanceinfo3", "Sal_UnusualAllowance", ref message);
                                if (!checkValidate)
                                {
                                    model.ActionStatus = message;
                                    return model;
                                }
                            }
                        }
                    }
                    else if (entity.Code.Replace(" ", "") == "MARRIAGEOFEMPLOYEE" || entity.Code.Replace(" ", "") == "DEATHOFEMPLOYEE")
                    {
                        checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualAllowanceinfo5And6And7", "Sal_UnusualAllowance", ref message);
                    }
                }
             
            }
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
        
            return service.UpdateOrCreate<Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceModel>(model);
        }
    }
}