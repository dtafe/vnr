using System.Data.SqlClient;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using VnResource.Helper.Linq;
using VnResource.Helper.Data;
using VnResource.Helper.Utility;
using System.Reflection;
using System.Collections;
using HRM.Data.Entity;
using System.Data.Entity.Validation;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Hr.Models;
using System.Net.Mail;
using System.Net;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Category.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Finance.Models;
using HRM.Business.Payroll.Models;
using HRM.Business.Attendance.Models;

namespace HRM.Business.Finance.Domain
{
    public class Pur_MCAMService : BaseService
    {

        public void CheckBuyCar(string selectedIds, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;

                Guid[] Ids = null;
                if(!string.IsNullOrEmpty(selectedIds))
                {
                    Ids = selectedIds.Split(',').Select(s => Guid.Parse(s)).ToArray();

                }

                var objPurCam = new List<object>();
                objPurCam.AddRange(new object[8]);
                objPurCam[6] = 1;
                objPurCam[7] = int.MaxValue - 1;
                var lstPurCam = GetData<Pur_MCAMEntity>(objPurCam, ConstantSql.Hrm_CAT_SP_GET_PURMCAM,UserLogin,ref status).ToList();

            

                var objSupense = new List<object>();
                objSupense.AddRange(new object[13]);
                objSupense[11] = 1;
                objSupense[12] = int.MaxValue - 1;
                var lstSuspense = GetData<Hre_StopWorkingEntity>(objSupense, ConstantSql.hrm_hr_sp_get_Suspense, UserLogin, ref status).ToList();

                if (Ids != null)
                {
                    lstPurCam = lstPurCam.Where(s => Ids.Contains(s.ID)).ToList();
                }

                foreach (var item in lstPurCam)
                {
                    
                    var dateCheck = DateTime.Now;
                    if(item.DateRequest==null)
                    {
                        item.StatusCheck = EnumDropDown.StatusCheck.E_FAIL.ToString();
                        Edit(item);
                        continue;
                    }
                    var dateSub = item.DateRequest.Value.AddYears(-1);

                    #region KT thâm niên
                    var suspenseEntity = lstSuspense.Where(s =>s.ProfileID != null && item.ID == s.ProfileID.Value).FirstOrDefault();

                    if (suspenseEntity != null)
                    {
                        if (item.DateHire != null)
                        {
                            var dateSeniority = dateCheck.Subtract(item.DateHire.Value);

                            dateSeniority = dateCheck.Subtract(item.DateHire.Value);

                            if (suspenseEntity.DateStop != null)
                            {
                                var dateConvert = new TimeSpan(suspenseEntity.DateStop.Value.Ticks);
                                var seniority = Math.Floor((dateSeniority.Subtract(dateConvert).TotalDays / 30) / 12);
                                if (seniority < 1)
                                {
                                    item.StatusCheck = EnumDropDown.StatusCheck.E_FAIL.ToString();
                                    Edit(item);
                                    continue;
                                }
                            }
                            if(suspenseEntity.DateStop ==  null)
                            {
                                var seniority = Math.Floor((dateSeniority.TotalDays / 30) / 12);
                                if (seniority < 1)
                                {
                                    item.StatusCheck = EnumDropDown.StatusCheck.E_FAIL.ToString();
                                    Edit(item);
                                    continue;
                                }
                               
                            }
                        }
                    }
                    #endregion

                    #region KT Hop Dong
                    var contractEntity = GetData<Hre_ContractEntity>(Common.DotNetToOracle(item.ProfileID.Value.ToString()), ConstantSql.hrm_hr_sp_get_ContractsByProfileId, UserLogin, ref status).OrderByDescending(s => s.DateEnd).FirstOrDefault();

                    if(contractEntity != null && item.DateRequest != null && contractEntity.DateEnd != null)
                    {
                        var dateTemp = contractEntity.DateEnd.Value.Subtract(item.DateRequest.Value);
                        var monthSub = Math.Floor(dateTemp.TotalDays / 30) - item.DeadlinePayment != null ? item.DeadlinePayment : 0;
                        if (monthSub < 3)
                        {
                            item.StatusCheck = EnumDropDown.StatusCheck.E_FAIL.ToString();
                            Edit(item);
                            continue;
                        }
                       
                        
                    }
                    #endregion

                    #region KT BasicSalary
                    var basicSalaryEntity = GetData<Sal_BasicSalaryEntity>(item.ProfileID.Value.ToString(), ConstantSql.hrm_sal_sp_get_BasicSalaryByProfileIds, UserLogin, ref status).OrderByDescending(s => s.DateOfEffect).FirstOrDefault();
                    if (basicSalaryEntity != null && !string.IsNullOrEmpty(basicSalaryEntity.GrossAmount))
                    {
                      if(item.PaymentPerMonth != null)
                      {
                          if (item.PaymentPerMonth > (double.Parse(basicSalaryEntity.GrossAmount) / 3))
                          {
                              item.StatusCheck = EnumDropDown.StatusCheck.E_FAIL.ToString();
                              Edit(item);
                              continue;
                          }
                        
                       
                      }
                    }
                    #endregion

                    #region KT theo loại xe
                    if (item.ModelType == EnumDropDown.PurColor.E_MOTOR.ToString())
                    {
                        if(item.LiquidationDate != null)
                        {
                            var month = Math.Floor(item.DateRequest.Value.Subtract(item.LiquidationDate.Value).TotalDays/30);
                            if (month < 1)
                            {
                                item.StatusCheck = EnumDropDown.StatusCheck.E_FAIL.ToString();
                                Edit(item);
                                continue;
                            }
                          
                        }
                    }
                    if (item.ModelType == EnumDropDown.PurColor.E_CAR.ToString())
                    {
                        if (item.LiquidationDate != null)
                        {
                            var month = Math.Floor(item.DateRequest.Value.Subtract(item.LiquidationDate.Value).TotalDays / 30);
                            if (month < 3)
                            {
                                item.StatusCheck = EnumDropDown.StatusCheck.E_FAIL.ToString();
                                Edit(item);
                                continue;
                            }
                           
                         
                        }
                    }
                    #endregion

                    #region KT Theo Ngày Nghỉ
                    var objLeaveday = new List<object>();
                    objLeaveday.AddRange(new object[4]);
                    objLeaveday[0] = item.ProfileID;
                    objLeaveday[2] = 1;
                    objLeaveday[3] = int.MaxValue -1;
                    var leavedayEntity = GetData<Att_LeaveDayEntity>(objLeaveday, ConstantSql.hrm_att_sp_get_LeavedayByProIDandCutOID,UserLogin,ref status).Where(s => s.Status == WorkdayStatus.E_APPROVED.ToString()).ToList();

                    //loại D
                    var leavedayByTypeD = leavedayEntity.Where(s => s.LeaveDayTypeCode == "D" && s.DateStart >= dateSub && s.DateEnd <= item.DateRequest.Value).ToList();

                    if (leavedayByTypeD.Count >= 2)
                    {
                        item.StatusCheck = EnumDropDown.StatusCheck.E_FAIL.ToString();
                        Edit(item);
                        continue;

                    }
                 
                    // loại SU, SD, AL
                    var leavedayByTypeSUSDAL = leavedayEntity.Where(s => (s.LeaveDayTypeCode == "SU" || s.LeaveDayTypeCode == "SD" || s.LeaveDayTypeCode == "AL") && s.DateStart >= dateSub && s.DateEnd <= item.DateRequest.Value).ToList();
                    if (leavedayByTypeSUSDAL.Count > 20)
                    {
                        item.StatusCheck = EnumDropDown.StatusCheck.E_FAIL.ToString();
                        Edit(item);
                        continue;

                    }
                 

                    // loại DP
                    var leavedayByTypeDP = leavedayEntity.Where(s => s.LeaveDayTypeCode == "DP" && s.DateStart >= dateSub && s.DateEnd <= item.DateRequest.Value).ToList();
                    if (leavedayByTypeSUSDAL.Count > 7)
                    {
                        item.StatusCheck = EnumDropDown.StatusCheck.E_FAIL.ToString();
                        Edit(item);
                        continue;

                    }
                   
                    #endregion

                    #region KT theo Kỷ Luật
                    var objDiscipline = new List<object>();
                    objDiscipline.Add(item.ProfileID);
                    var lstDisciplineByProfile = GetData<Hre_DisciplineEntity>(item.ProfileID.Value, ConstantSql.hrm_hr_sp_get_DisciplineprofileId,UserLogin,ref status).Where(s => s.DateOfEffective <= item.DateRequest.Value && s.DateOfEffective > dateSub).ToList();
                    if (lstDisciplineByProfile.Count > 1)
                    {
                        item.StatusCheck = EnumDropDown.StatusCheck.E_FAIL.ToString();
                        Edit(item);
                        continue;
                    }

                    #endregion

                    item.StatusCheck = EnumDropDown.StatusCheck.E_PASS.ToString();
                    Edit(item);
                }

            }
        }


    }
}
