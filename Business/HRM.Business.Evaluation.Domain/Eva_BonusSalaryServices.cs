using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Business.Evaluation.Models;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;


namespace HRM.Business.Evaluation.Domain
{
    public class Eva_BonusSalaryServices : BaseService
    {
        public Eva_BonusSalaryEntity CalculateBonusSalary(Eva_BonusSalaryEntity model,string userLogin)
        {
            /*
            *  Goal(Tính lương thưởng)
            *  Steps :
            *      Step1  :  Lay tat ca chuc Danh trong phong ban,lay tat ca nv có chức danh
            *      Step2  :  Từ Chuc Danh , Tieu Chi , Loai Thuong,thang (lay từ mức, đến mức)
            *      Step2  :  Tự động insert xuống table Eva_PerformanceForDetail
            *      Step3  :  Tự động insert xuống table Eva_PerformanceEva &Eva_PerformanceEvaDetail 
            *                  - Tự Đánh Giá : orderNo =0 (Ngoai ra orderNo >0)
             
            */

            var status = string.Empty;

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork) (new UnitOfWork(context));
                var repoJobTitle = new Cat_JobTitleRepository(unitOfWork);
                var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
                var repoSaleBonus = new Eva_SaleBonusRepository(unitOfWork);
                var repoBonusSalary = new Eva_BonusSalaryRepository(unitOfWork);
                var repoSaleEvaluation = new Eva_SaleEvaluationRepository(unitOfWork);
                var repoProfile = new Hre_ProfileRepository(unitOfWork);

                //var orgId = Guid.Parse(model.OrgStructureID);
                //var a = repoOrg.FindBy(p => p.ID == orgId).Select(p=>p.OrderNumber).FirstOrDefault();
                //if (a != null)
                //{
                //    model.OrgStructureID = a.ToString();
                //}


                List<object> listObj = new List<object>();
                listObj.Add(model.OrgStructureID);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);
                var profilebyOrgs = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status);
                //var profilebyOrgs = repoProfile.FindBy(x => x.OrgStructureID == Guid.Parse(model.OrgStructureID)).ToList();
                var profiles = profilebyOrgs.Where(p => p.JobTitleID != null).ToList();
                var jobTitleIds = profilebyOrgs.Where(p => p.JobTitleID != null).Select(p=>p.JobTitleID).Distinct().ToList();
                var jobTitle = repoJobTitle.FindBy(p => jobTitleIds.Contains(p.ID)).ToList();

                //lay tu muc den muc
                var month =  DateTime.Now;
                 // lấy danh sách thưởng doanh số dựa vào loại doanh số và chức danh và loại 
                var lstsalebonus = repoSaleBonus.FindBy(x => jobTitleIds.Contains(x.JobTittleID.Value) && x.Type == model.Type && x.SalesTypeID==model.SalesTypeID&&x.IsDelete==null).ToList();
                // lấy danh sách doanh số dựa vào loại doanh số và chức danh và loại 
                if (model.Month != null)
                {
                    month = model.Month.Value;
                }
                List<Eva_BonusSalary> lstEva_BonusSalaryEntity = new List<Eva_BonusSalary>();
                if (profiles == null || profiles.Count == 0)
                {
                    return null;
 
                }
                var lstBonusSalaryEdit = repoBonusSalary.FindBy(x => x.SalesTypeID == model.SalesTypeID && x.Type == model.Type && x.IsDelete == null).ToList();
                var lstSaleEvaluation = repoSaleEvaluation.FindBy(x=> x.SalesTypeID == model.SalesTypeID && x.IsDelete == null).ToList();
                if (model.Type == EvaBonusType.E_Month.ToString())
                {
                    
                    foreach (var item in profiles)
                    {
                        var lstBonusSalaryEdit2 = lstBonusSalaryEdit.Where(x => x.ProfileID == item.ID&& x.Month.Value.Month == model.Month.Value.Month).ToList();
                        var objSaleEvaluation = lstSaleEvaluation.Where(x => x.ProfileID == item.ID && x.Year.Value.Month == model.Month.Value.Month).FirstOrDefault();
                        if (lstBonusSalaryEdit2 != null && lstBonusSalaryEdit2.Count != 0)
                        {
                            var objBonusSalaryEdit = lstBonusSalaryEdit2.FirstOrDefault();

                            if (objSaleEvaluation != null)
                            {
                                var SaleBonus = lstsalebonus.Where(x => x.FromPercent <= objSaleEvaluation.ResultPercent && x.ToPercent >= objSaleEvaluation.ResultPercent && x.JobTittleID == item.JobTitleID).ToList();
                                if (SaleBonus != null && SaleBonus.Count != 0)
                                {
                                    objBonusSalaryEdit.Bonus = float.Parse(SaleBonus.FirstOrDefault().Amount.ToString());
                                    repoBonusSalary.SaveChanges();

                                }
                            }
                        }
                        else
                        {

                            var objBonusSalary = new Eva_BonusSalary();
                            // objBonusSalary = model;
                            objBonusSalary.SalesTypeID = model.SalesTypeID;
                            objBonusSalary.ProfileID = item.ID;
                            objBonusSalary.Type = model.Type;
                            objBonusSalary.Month = month;
                            objBonusSalary.Bonus = 0;
                            if (objSaleEvaluation != null)
                            {
                                var SaleBonus = lstsalebonus.Where(x => x.FromPercent <= objSaleEvaluation.ResultPercent && x.ToPercent >= objSaleEvaluation.ResultPercent && x.JobTittleID == item.JobTitleID).ToList();
                                if (SaleBonus != null && SaleBonus.Count != 0)
                                {
                                    objBonusSalary.Bonus = float.Parse(SaleBonus.FirstOrDefault().Amount.ToString());

                                }
                            }
                            lstEva_BonusSalaryEntity.Add(objBonusSalary);
                        }
                        

                    }
                    repoBonusSalary.Add(lstEva_BonusSalaryEntity);

                    repoBonusSalary.SaveChanges();

                }
                if (model.Type == EvaBonusType.E_Quarter.ToString())
                {
                  //int quarter = //((month.Month-1)/ 3) + 1;Quarter
                    int quarter = (int)Enum.Parse(typeof(Quarter), model.Quarter);     

                    DateTime firstDayOfQuarter = new DateTime(month.Year, (quarter - 1) * 3 + 1, 1);
                    DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
                      
                    foreach (var item in profiles)
                    {
                        var objSaleEvaluation = lstSaleEvaluation.Where(x => x.ProfileID == item.ID && x.Year <= lastDayOfQuarter && x.Year >= firstDayOfQuarter).ToList();
                       // var lstBonusSalaryEdit = repoBonusSalary.FindBy(x => x.ProfileID == item.ID && x.SalesTypeID == model.SalesTypeID && x.Type == model.Type && x.Month.Value.Month == model.Month.Value.Month).ToList();
                        var lsttBonusSalaryEdit2 = lstBonusSalaryEdit.Where(x => x.ProfileID == item.ID && x.Month <= lastDayOfQuarter && x.Month >= firstDayOfQuarter).ToList();
                        if (lsttBonusSalaryEdit2 != null && lsttBonusSalaryEdit2.Count != 0)
                        {
                            var objBonusSalaryEdit = lsttBonusSalaryEdit2.FirstOrDefault();
                            if (objSaleEvaluation != null && objSaleEvaluation.Count != 0)
                            {
                                var AveragePercent = objSaleEvaluation.Sum(x => x.ResultPercent) / 3;
                                var SaleBonus = lstsalebonus.Where(x => x.FromPercent <= AveragePercent && x.ToPercent >= AveragePercent && x.JobTittleID == item.JobTitleID).ToList();
                                if (SaleBonus != null && SaleBonus.Count != 0)
                                {
                                    objBonusSalaryEdit.Bonus = float.Parse(SaleBonus.FirstOrDefault().Amount.ToString());
                                    repoBonusSalary.SaveChanges();

                                }

                            }
                        }
                        else
                        {

                            var objBonusSalary = new Eva_BonusSalary();
                            //objBonusSalary = model;
                            objBonusSalary.SalesTypeID = model.SalesTypeID;
                            //   objBonusSalary.Quarter = quarter.ToString();
                            objBonusSalary.ProfileID = item.ID;
                            objBonusSalary.Type = model.Type;
                            objBonusSalary.Month = month;
                            objBonusSalary.Bonus = 0;
                            
                            if (objSaleEvaluation != null && objSaleEvaluation.Count != 0)
                            {
                                var AveragePercent = objSaleEvaluation.Sum(x => x.ResultPercent) / 3;
                                var SaleBonus = lstsalebonus.Where(x => x.FromPercent <= AveragePercent && x.ToPercent >= AveragePercent && x.JobTittleID == item.JobTitleID).ToList();
                                if (SaleBonus != null && SaleBonus.Count != 0)
                                {
                                    objBonusSalary.Bonus = float.Parse(SaleBonus.FirstOrDefault().Amount.ToString());

                                }

                            }
                            lstEva_BonusSalaryEntity.Add(objBonusSalary);

                        }
                    }
                    repoBonusSalary.Add(lstEva_BonusSalaryEntity);
                    repoBonusSalary.SaveChanges();
 
                }
            }
            return model;
        }

    }
}
