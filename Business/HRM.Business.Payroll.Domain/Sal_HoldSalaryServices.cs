using HRM.Business.Main.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Infrastructure.Utilities.Helper;
using System.Threading;
using HRM.Business.Insurance.Models;
using HRM.Data.Entity.Repositories;
using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using VnResource.Helper.Data;
using HRM.Business.HrmSystem.Domain;

namespace HRM.Business.Payroll.Domain
{
    public class Sal_HoldSalaryServices : BaseService
    {
        /// <summary>
        /// Lay DS luong hieu luc cua toan cong ty. Moi nhan vien chi co 1 muc luong trong danh sach.
        /// </summary>
        /// <param name="lstSalaryAll">get all Salary</param>
        /// <param name="monthEffect"></param>
        /// <returns></returns>


        public void Approved(List<Guid> selectedIds, string status, string userLoginName)
        {
            using (var context = new VnrHrmDataContext())
            {
                string statusMessage = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var repoHoldSalary = new Sal_HoldSalaryRepository(unitOfWork);
                var lstHoldSalary = repoHoldSalary.FindBy(s => selectedIds.Contains(s.ID)).ToList();
                var lstProfileIDs = lstHoldSalary.Select(s => s.ProfileID).ToList();
                var profileServices = new Hre_ProfileServices();
                var objProfile = new List<object>();
                objProfile.AddRange(new object[17]);
                objProfile[15] = 1;
                objProfile[16] = int.MaxValue - 1;
                var lstProfile = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileAll,userLoginName, ref statusMessage).ToList().Translate<Hre_Profile>();
                //var lstProfiles = repoProfile.FindBy(m => m.ID != null && lstProfileIDs.Contains(m.ID)).ToList();

                foreach (var holdSalary in lstHoldSalary)
                {
                    holdSalary.Status = status;
                    var profileByID = lstProfile.Where(s => s.ID == holdSalary.ProfileID.Value).FirstOrDefault();
                    profileByID.MonthHoldSal = holdSalary.MonthSalary;
                    profileByID.IsHoldSal = true;
                }
                repoHoldSalary.SaveChanges();
                repoProfile.SaveChanges();


            }
        }

        public List<Sal_HoldSalaryEntity> ComputeHoldSalary(Guid? cutOffDurationID, Guid[] ProfileIDs, string orgStructureID, Guid TimeAnalyze, string userLoginName)
        {
            using (var context = new VnrHrmDataContext())
            {
                #region Get Data
                string status = string.Empty;
                List<Sal_HoldSalaryEntity> lstHoldSalaryEntity = new List<Sal_HoldSalaryEntity>();

                //ds kỳ công
                var cutOffServies = new Att_CutOffDurationServices();
                var lstObjCutOff = new List<object>();
                lstObjCutOff.Add(null);
                lstObjCutOff.Add(1);
                lstObjCutOff.Add(int.MaxValue - 1);
                var lstCutOff = cutOffServies.GetData<Att_CutOffDurationEntity>(Common.DotNetToOracle(cutOffDurationID.Value.ToString()), ConstantSql.hrm_att_sp_get_CutOffDurationById, userLoginName, ref status).FirstOrDefault();

                //ds nv
                var hrService = new Hre_ProfileServices();
                List<object> strOrgIDs = new List<object>();
                strOrgIDs.AddRange(new object[3]);
                strOrgIDs[0] = (object)orgStructureID;
                var lstProfile = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLoginName, ref status);

                if (ProfileIDs != null)
                {
                    lstProfile = lstProfile.Where(s => ProfileIDs.Contains(s.ID)).ToList();
                }

                //ds ngày nghỉ
                var leaveDayServices = new Att_LeavedayServices();
                var lstObjLeaveDay = new List<object>();
                lstObjLeaveDay.AddRange(new object[11]);
                lstObjLeaveDay[4] = lstCutOff.DateStart;
                lstObjLeaveDay[5] = lstCutOff.DateEnd;
                lstObjLeaveDay[9] = 1;
                lstObjLeaveDay[10] = int.MaxValue - 1;
                var lstLeaveDay = leaveDayServices.GetData<Att_LeaveDayEntity>(lstObjLeaveDay, ConstantSql.hrm_att_sp_get_Leaveday,userLoginName, ref status);

                //ds loại ngày nghỉ
                var leavedayTypeServices = new Cat_LeaveDayTypeServices();
                var lstObjLeavedayType = new List<object>();
                lstObjLeavedayType.Add(null);
                lstObjLeavedayType.Add(null);
                lstObjLeavedayType.Add(1);
                lstObjLeavedayType.Add(int.MaxValue - 1);
                var lstLeavedayType = leavedayTypeServices.GetData<Cat_LeaveDayTypeEntity>(lstObjLeavedayType, ConstantSql.hrm_cat_sp_get_LeaveDayType,userLoginName, ref status);

                lstObjLeavedayType = new List<object>();
                lstObjLeavedayType.Add(null);
                lstObjLeavedayType.Add(EnumDropDown.EntityType.E_CountAnalyzeHoldSalary.ToString());
                lstObjLeavedayType.Add(1);
                lstObjLeavedayType.Add(int.MaxValue - 1);
                var ListNameEntity = leavedayTypeServices.GetData<Cat_NameEntityEntity>(lstObjLeavedayType, ConstantSql.hrm_cat_sp_get_NameEntityByKPI,userLoginName, ref status);

                ////ds chế độ lương
                //var Sal_GradeServices = new Sal_GradeServices();
                //var lstObjSal_Grade = new List<object>();
                //lstObjSal_Grade.AddRange(new object[7]);
                //lstObjSal_Grade[3] = lstCutOff.DateStart;
                //lstObjSal_Grade[4] = lstCutOff.DateEnd;//sửa lại store lấy lên tất cả các grade mà ko có ngày kết thúc
                //lstObjSal_Grade[5] = 1;
                //lstObjSal_Grade[6] = int.MaxValue - 1;
                //var lstSal_Grade = Sal_GradeServices.GetData<Sal_GradeEntity>(lstObjSal_Grade, ConstantSql.hrm_sal_sp_get_Sal_Grade, ref status).ToList();

                Sys_AttOvertimePermitConfigServices Sys_Services = new Sys_AttOvertimePermitConfigServices();
                double DATECLOSE_SALARY = Sys_Services.GetConfigValue<double>(AppConfig.HRM_SAL_DATECLOSE_SALARY);

                foreach (var profile in lstProfile)
                {

                    Sal_HoldSalaryEntity entity = new Sal_HoldSalaryEntity();

                    entity.ProfileID = profile.ID;
                    entity.CodeEmp = profile.CodeEmp;
                    entity.ProfileName = profile.ProfileName;
                    entity.CodeAttendance = profile.CodeAttendance;
                    entity.EmployeeTypeName = profile.EmployeeTypeName;
                    entity.WorkPlaceName = profile.WorkPlaceName;
                    entity.IDNo = profile.IDNo;
                    entity.MonthSalary = lstCutOff.MonthYear;
                    entity.TimeAnalyzeID = TimeAnalyze;
                    entity.CatNameEntity = ListNameEntity.FirstOrDefault(m => m.ID == TimeAnalyze) != null ? ListNameEntity.FirstOrDefault(m => m.ID == TimeAnalyze).NameEntityName : "";
                    var lstLeavedayByProfileID = lstLeaveDay.Where(s => profile.ID == s.ProfileID).ToList();

                    #region NV Nghỉ việc trước ngày 18
                    if (profile.DateQuit != null)
                    {
                        if (profile.DateQuit.Value <= lstCutOff.DateEnd && profile.IsSettlement != true)
                        {
                            //if (profile.DateQuit.Value.Day <= 18)
                            //{
                            entity.Terminate = true;
                            lstHoldSalaryEntity.Add(entity);
                            continue;
                            //}
                        }
                    }
                    #endregion

                    #region kiểm tra nghỉ quá 12 ngày
                    if (lstLeavedayByProfileID.Count > 0)
                    {
                        var lstLeavedayTypeBy12 = lstLeavedayType.Where(m => m.Code == "SP" || m.Code == "SC" || m.Code == "DSP" || m.Code == "DL" || m.Code == "M").ToList();
                        var _tmp = lstLeavedayByProfileID.Where(m => !lstLeavedayTypeBy12.Any(t => t.ID == m.LeaveDayTypeID)).ToList();
                        double count = 0;
                        foreach (var i in _tmp)
                        {
                            count += i.LeaveDays != null ? (double)i.LeaveDays : 0;
                        }
                        if (count >= 12)
                        {
                            entity.DayLeave = (int)count;
                            lstHoldSalaryEntity.Add(entity);
                            continue;
                        }
                    }
                    #endregion

                    #region trường hợp đăng ký thai sản trước ngày 18
                    if (lstLeavedayByProfileID.Count > 0)
                    {
                        var lstLeavedayTypeBy12 = lstLeavedayType.Where(m => m.Code == "M").ToList();
                        var _tmp = lstLeavedayByProfileID.FirstOrDefault(m => lstLeavedayTypeBy12.Any(t => t.ID == m.LeaveDayTypeID) && m.DateStart.Day < 18);
                        if (_tmp != null)
                        {
                            entity.IsLeaveM = true;
                            lstHoldSalaryEntity.Add(entity);
                            continue;
                        }
                    }
                    #endregion

                    #region Nghỉ liên tục 3 ngày
                    if (lstLeavedayByProfileID.Count > 0)
                    {
                        var lstLeavedayTypeBy3 = lstLeavedayType.Where(m => m.Code == "D").ToList();
                        //lấy ngày nghỉ theo loại ngày nghỉ D
                        var lstLeavedayByProfileID3 = lstLeavedayByProfileID.Where(m => lstLeavedayTypeBy3.Any(t => t.ID == m.LeaveDayTypeID)).ToList();

                        lstLeavedayByProfileID3 = lstLeavedayByProfileID3.Where(m => m.DateEnd.Day <= DATECLOSE_SALARY).ToList();
                        double count = 0;
                        //còn 1 bug đó là khi đăng ký 3 dòng  liền kế ngày nhau
                        foreach (var i in lstLeavedayByProfileID3)
                        {
                            count = i.LeaveDays != null ? (double)i.LeaveDays : 0;
                            if (count >= 3)
                            {
                                entity.IsLeaveContinuous = true;
                                break;
                            }
                        }
                        if (entity.IsLeaveContinuous != null && entity.IsLeaveContinuous == true)
                        {
                            lstHoldSalaryEntity.Add(entity);
                            continue;
                        }
                    }

                    #endregion
                }
                return lstHoldSalaryEntity;
                #endregion



            }

        }

        public ResultsObject SaveChangeHoldSalary(List<Sal_HoldSalaryEntity> ListEntity)
        {
            try
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    var repoSal_HoldSalary = new CustomBaseRepository<Sal_HoldSalary>(unitOfWork);
                    List<Sal_HoldSalary> TotalHoldSalary = repoSal_HoldSalary.FindBy(m => m.IsDelete != true).ToList();

                    foreach (var i in ListEntity)
                    {
                        #region Kiểm tra nhân viên đã có dữ liệu giữ lương hay chưa

                        Sal_HoldSalary HoldSalaryByProfile = TotalHoldSalary.FirstOrDefault(m => m.ProfileID == i.ProfileID && m.MonthSalary != null && m.MonthSalary.Value.Day == i.MonthSalary.Value.Day && m.TimeAnalyzeID != null && m.TimeAnalyzeID == i.TimeAnalyzeID);
                        if (HoldSalaryByProfile != null)
                        {
                            if (HoldSalaryByProfile.Status == EnumDropDown.HoldSalaryStatus.E_APPROVED.ToString())
                            {
                                continue;
                            }
                            else
                            {
                                HoldSalaryByProfile.AmountSalary = i.AmountSalary;
                                HoldSalaryByProfile.IsLeaveContinuous = i.IsLeaveContinuous;
                                HoldSalaryByProfile.IsLeaveM = i.IsLeaveM;
                                HoldSalaryByProfile.Terminate = i.Terminate;
                                HoldSalaryByProfile.TimeAnalyzeID = i.TimeAnalyzeID;
                            }
                        }
                        else
                        {
                            Sal_HoldSalary HoldSalary = i.Copy<Sal_HoldSalary>();
                            repoSal_HoldSalary.Add(HoldSalary);
                        }
                        #endregion
                    }
                    unitOfWork.SaveChanges();
                    return new ResultsObject();
                }
            }
            catch (Exception ex)
            {
                return new ResultsObject() { Success = false, Messenger = ex.Message };
            }
        }

    }
}
