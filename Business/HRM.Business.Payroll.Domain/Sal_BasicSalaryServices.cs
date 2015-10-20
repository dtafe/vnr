using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Data.Entity.Repositories;
using HRM.Business.Attendance.Models;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Business.Category.Domain;
using HRM.Business.Hr.Domain;
using VnResource.Helper.Data;
using HRM.Business.Attendance.Domain;
using HRM.Infrastructure.Utilities.Helper;

namespace HRM.Business.Payroll.Domain
{
    public class Sal_BasicSalaryServices : BaseService
    {
        /// <summary>
        /// Lay DS luong hieu luc cua toan cong ty. Moi nhan vien chi co 1 muc luong trong danh sach.
        /// </summary>
        /// <param name="lstSalaryAll">get all Salary</param>
        /// <param name="monthEffect"></param>
        /// <returns></returns>
        public static List<Sal_BasicSalary> ListSalaryEffectAll(List<Sal_BasicSalary> lstSalaryAll, DateTime monthEffect)
        {
            List<Sal_BasicSalary> lstSalaryEffectAll = new List<Sal_BasicSalary>();
            return lstSalaryEffectAll;
        }

        public void AddDataForBasicSalary(string ProfileIDs, string BasicSalary, DateTime DateHire, Guid SalaryRankID,string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string status = string.Empty;
                var repoBasicSalary = new CustomBaseRepository<Sal_BasicSalary>(unitOfWork);
                List<Guid> lstProfileIDs = ProfileIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                List<Sal_BasicSalary> lstBasicSalary = new List<Sal_BasicSalary>();
                //chua co cach giai quyet nen hard code lay mac dinh la VND
                var repoCurrency = new CustomBaseRepository<Cat_Currency>(unitOfWork);
                var currency = repoCurrency.FindBy(s => s.IsDelete == null && s.CurrencyName == "VND").FirstOrDefault();
                //var repoSalaryRank = new CustomBaseRepository<Cat_SalaryRank>(unitOfWork);
                //var repoSalaryClass = new CustomBaseRepository<Cat_SalaryClass>(unitOfWork);
                //var salaryRank = repoSalaryRank.FindBy(s => s.IsDelete == null && s.ID == SalaryRankID).FirstOrDefault();
                //var salaryClass = repoSalaryClass.FindBy(s => s.IsDelete == null && s.ID == salaryRank.SalaryClassID).FirstOrDefault();
                var salaryRankServices = new Cat_SalaryRankServices();
                var lstObjRank = new List<object>();
                lstObjRank.Add(null);
                lstObjRank.Add(null);
                lstObjRank.Add(1);
                lstObjRank.Add(int.MaxValue -1);
                var lstRank = salaryRankServices.GetData<Cat_SalaryRankEntity>(lstObjRank, ConstantSql.hrm_cat_sp_get_SalaryRank, UserLogin, ref status).ToList().Translate<Cat_SalaryRank>();
                var rankEntity = lstRank.Where(s => s.ID == SalaryRankID).FirstOrDefault();

                var salaryClassServices = new Cat_SalaryClassServices();
                var lstObjClass = new List<object>();
                lstObjClass.Add(null);
                lstObjClass.Add(1);
                lstObjClass.Add(int.MaxValue -1);
                var salaryClass = salaryClassServices.GetData<Cat_SalaryClassEntity>(lstObjClass, ConstantSql.hrm_cat_sp_get_SalaryClass, UserLogin, ref status).ToList().Translate<Cat_SalaryClass>();
                
                var salaryClassEntity = salaryClass.Where(s => rankEntity.SalaryClassID == s.ID).FirstOrDefault();
               
                foreach (var item in lstProfileIDs)
                {
                    Sal_BasicSalary objBasicSalary = new Sal_BasicSalary();
                    objBasicSalary.ProfileID = item;
                    objBasicSalary.GrossAmount = BasicSalary;
                    objBasicSalary.Amount = BasicSalary.Encrypt(); 
                    objBasicSalary.DateOfEffect = DateHire;
                    objBasicSalary.RankRateID = SalaryRankID;
                    objBasicSalary.CurrencyID = currency.ID;
                    objBasicSalary.ClassRateID = salaryClassEntity == null ? Guid.Empty : salaryClassEntity.ID;
                    lstBasicSalary.Add(objBasicSalary);
                }

                repoBasicSalary.Add(lstBasicSalary);
                repoBasicSalary.SaveChanges();


            }
        }

        public void AddAdjustmentSuggestionForBasicSalary(List<Sal_AdjustmentSuggestionEntity> lstAdjust, DateTime dateOfEffect, string UserLogin) 
        {
            var salaryServices = new Sal_BasicSalaryServices();
            BaseService service = new BaseService();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);
            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting,UserLogin, ref status);
            string defaultcur = null;
            string valueConfig = AppConfig.HRM_HRE_CONFIG_DEFAULTCURRENCY.ToString();
            var currencyEntity = new Cat_CurrencyEntity();
            if (config != null)
            {
                defaultcur = config.Where(s => s.Name == valueConfig).Select(s => s.Value1).FirstOrDefault().ToString();
                List<object> listObjCurrency = new List<object>();
                listObjCurrency.Add(null);
                listObjCurrency.Add(defaultcur);
                listObjCurrency.Add(1);
                listObjCurrency.Add(Int32.MaxValue - 1);
                currencyEntity = service.GetData<Cat_CurrencyEntity>(listObjCurrency, ConstantSql.hrm_cat_sp_get_Currency,UserLogin, ref status).FirstOrDefault();
            }
            if (lstAdjust.Count > 0) 
            {
                var lstBasicSalary = new List<Sal_BasicSalaryEntity>();
                var lstBasicSalaryAdd = new List<Sal_BasicSalaryEntity>();
                lstBasicSalary = lstAdjust.Translate<Sal_BasicSalaryEntity>();
                foreach (var item in lstBasicSalary)
                {
                    item.ID = Guid.Empty;
                    item.CurrencyID = currencyEntity.ID;
                    item.DateOfEffect = dateOfEffect;
                    item.ClassRateID = item.ClassRateID;
                    item.RankRateID = item.RankRateID;
                    item.Status = EnumDropDown.Sal_BasicSalaryStatus.E_WATTING_APPROVED.ToString();
                    lstBasicSalaryAdd.Add(item);
                }
                salaryServices.Add(lstBasicSalaryAdd);
            }

        }

        //public void AddAdjustmentSuggestionForBasicSalary(string ProfileIDs, string BasicSalary, DateTime DateHire, Guid SalaryRankID, string orgStructureID)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var hrService = new Hre_ProfileServices();
        //        var salaryServices = new Sal_BasicSalaryServices();
        //        string status = string.Empty;
        //        BaseService service = new BaseService();
        //        string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
        //        List<object> lstOb = new List<object>();
        //        lstOb.Add(ConfigHre);
        //        lstOb.Add(null);
        //        lstOb.Add(null);

        //        var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, ref status);
        //        string defaultcur = null;
        //        string valueConfig = AppConfig.HRM_HRE_CONFIG_DEFAULTCURRENCY.ToString();
        //        var currencyEntity = new Cat_CurrencyEntity();
        //        if (config != null)
        //        {
        //            defaultcur = config.Where(s => s.Name == valueConfig).Select(s => s.Value1).FirstOrDefault().ToString();
        //            List<object> listObjCurrency = new List<object>();
        //            listObjCurrency.Add(null);
        //            listObjCurrency.Add(defaultcur);
        //            listObjCurrency.Add(1);
        //            listObjCurrency.Add(Int32.MaxValue - 1);
        //            currencyEntity = service.GetData<Cat_CurrencyEntity>(listObjCurrency, ConstantSql.hrm_cat_sp_get_Currency, ref status).FirstOrDefault();
        //        }

        //        var salaryRankServices = new Cat_SalaryRankServices();
        //        var lstObjRank = new List<object>();
        //        lstObjRank.Add(null);
        //        lstObjRank.Add(null);
        //        lstObjRank.Add(1);
        //        lstObjRank.Add(int.MaxValue - 1);
        //        var lstRank = salaryRankServices.GetData<Cat_SalaryRankEntity>(lstObjRank, ConstantSql.hrm_cat_sp_get_SalaryRank, ref status).ToList().Translate<Cat_SalaryRank>();
        //        var rankEntity = lstRank.Where(s => s.ID == SalaryRankID).FirstOrDefault();

        //        var salaryClassServices = new Cat_SalaryClassServices();
        //        var lstObjClass = new List<object>();
        //        lstObjClass.Add(null);
        //        lstObjClass.Add(1);
        //        lstObjClass.Add(int.MaxValue - 1);
        //        var salaryClass = salaryClassServices.GetData<Cat_SalaryClassEntity>(lstObjClass, ConstantSql.hrm_cat_sp_get_SalaryClass, ref status).ToList().Translate<Cat_SalaryClass>();
        //        var salaryClassEntity = salaryClass.Where(s => rankEntity.SalaryClassID == s.ID).FirstOrDefault();

        //        if (!string.IsNullOrEmpty(orgStructureID))
        //        {
        //            List<Guid> listGuid = new List<Guid>();
        //            if (ProfileIDs != null)
        //            {
        //                var listStr = ProfileIDs.Split(',');

        //                if (listStr[0] != "")
        //                {
        //                    foreach (var item in listStr)
        //                    {
        //                        listGuid.Add(Guid.Parse(item));
        //                    }
        //                }
        //            }

        //            List<object> listObj = new List<object>();
        //            listObj.Add(orgStructureID);
        //            listObj.Add(string.Empty);
        //            listObj.Add(string.Empty);
        //            var lstProfile = hrService.GetData<Hre_ProfileIdEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
        //            if (listGuid != null)
        //            {
        //                lstProfile = lstProfile.Where(s => !listGuid.Contains(s)).ToList();
        //            }

        //            List<Sal_BasicSalaryEntity> lstGradeEntity = new List<Sal_BasicSalaryEntity>();

        //            foreach (var item in lstProfile)
        //            {
        //                Sal_BasicSalaryEntity gradeEntity = new Sal_BasicSalaryEntity
        //                {
        //                    ProfileID = item,
        //                    RankRateID = SalaryRankID,
        //                    ClassRateID = salaryClassEntity == null ? Guid.Empty : salaryClassEntity.ID,
        //                    GrossAmount = BasicSalary,
        //                    DateOfEffect = DateHire,
        //                    CurrencyID = currencyEntity != null ? currencyEntity.ID : Guid.Empty,
        //                    Status = EnumDropDown.Sal_BasicSalaryStatus.E_WATTING_APPROVED.ToString()
        //                };
        //                //model.ActionStatus =  gradeServices.Add(gradeEntity);
        //                lstGradeEntity.Add(gradeEntity);
        //            }
        //            salaryServices.Add(lstGradeEntity);
        //           // return model;
        //        }
        //        if (ProfileIDs != null && ProfileIDs.IndexOf(',') > 0)
        //        {
        //            var listStr = ProfileIDs.Split(',');
        //            List<Guid> listGuid = new List<Guid>();
        //            if (listStr[0] != "")
        //            {
        //                foreach (var item in listStr)
        //                {
        //                    listGuid.Add(Guid.Parse(item));
        //                }
        //            }
        //            List<Sal_BasicSalaryEntity> lstGradeEntity = new List<Sal_BasicSalaryEntity>();
        //            foreach (var item in listGuid)
        //            {
        //                Sal_BasicSalaryEntity gradeEntity = new Sal_BasicSalaryEntity
        //                {
        //                    ProfileID = item,
        //                    RankRateID = SalaryRankID,
        //                    ClassRateID = salaryClassEntity == null ? Guid.Empty : salaryClassEntity.ID,
        //                    GrossAmount = BasicSalary,
        //                    DateOfEffect = DateHire,
        //                    CurrencyID = currencyEntity != null ? currencyEntity.ID : Guid.Empty,
        //                    Status = EnumDropDown.Sal_BasicSalaryStatus.E_WATTING_APPROVED.ToString()
        //                };
        //                //model.ActionStatus =  gradeServices.Add(gradeEntity);
        //                lstGradeEntity.Add(gradeEntity);
        //            }
        //            salaryServices.Add(lstGradeEntity);

        //        }
        //        else {
        //            Sal_BasicSalaryEntity entity = new Sal_BasicSalaryEntity
        //            {
        //                ProfileID = Common.ConvertToGuid(ProfileIDs),
        //                RankRateID = SalaryRankID,
        //                ClassRateID = salaryClassEntity == null ? Guid.Empty : salaryClassEntity.ID,
        //                GrossAmount = BasicSalary,
        //                DateOfEffect = DateHire,
        //                CurrencyID = currencyEntity != null ? currencyEntity.ID : Guid.Empty,
        //                Status = EnumDropDown.Sal_BasicSalaryStatus.E_WATTING_APPROVED.ToString()
        //            };

        //            salaryServices.Add(entity);

        //        }

            
        //    }
        //}

    }
}
