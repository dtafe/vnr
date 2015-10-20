using System;
using HRM.Business.Attendance.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Insurance.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Data.BaseRepository;
using System.Collections.Generic;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities.Helper;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using System.Linq;
using HRM.Business.Attendance.Models;
using HRM.Business.Payroll.Domain;
using HRM.Business.Category.Models;
using System.Data;
using VnResource.Helper.Linq;
using HRM.Business.HrmSystem.Domain;

namespace HRM.Business.Insurance.Domain
{
    public class InsuranceServices : BaseService
    {
        #region Properties/variables
        public List<String> leaveDayInsuranceTypes
        {
            get
            {
                var lstLeaveDayTypes = typeof(HRM.Infrastructure.Utilities.EnumDropDown.InsuranceType).GetEnumNames();
                return lstLeaveDayTypes.ToList();
            }
        }

        #region Ngày Chốt Bảo Hiểm Báo Tăng LĐ [18->17 thang N]
        private int? _periodInsuranceDayPreMonth;
        /// <summary>Lấy Từ ngày chốt bảo hiểm [honda là ngày 18 tháng [N-1]]</summary>
        public int PeriodInsuranceDayPreMonth
        {
            get
            {
                if (!_periodInsuranceDayPreMonth.HasValue)
                {
                    var configService = new Sys_AttOvertimePermitConfigServices();
                    _periodInsuranceDayPreMonth = configService.GetConfigValue<int>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEYDAY);
                }
                if (_periodInsuranceDayPreMonth < 1 || _periodInsuranceDayPreMonth > 30)
                {
                    _periodInsuranceDayPreMonth = 16;
                }
                return _periodInsuranceDayPreMonth + 1 ?? 16;
            }
        }

        /// <summary>Lấy Đến ngày chốt bảo hiểm Báo Tăng LĐ [honda là ngày 17 tháng [N]]</summary>
        public int PeriodInsuranceDayCurrentMonth
        {
            get
            {
                return PeriodInsuranceDayPreMonth - 1;
            }
        } 
        #endregion

        #region Chu Kỳ Bảo Hiểm [15->14 thang N]
        private static int? _periodInsuranceDayPreMonthDefault;
        /// <summary>Chu Kỳ BH mặc định [ngày 15 tháng trước] (Hoặc đọc từ cấu hình)</summary>
        public static int PeriodInsuranceDayPreMonthDefault
        {
            get
            {
                //if (!_periodInsuranceDayPreMonthDefault.HasValue)
                //{
                    var configService = new Sys_AttOvertimePermitConfigServices();
                    _periodInsuranceDayPreMonthDefault = configService.GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYDEFAULTFROM);
                    if (_periodInsuranceDayPreMonthDefault == null)
                    {
                        _periodInsuranceDayPreMonthDefault = 16;
                    }
                //}
                return _periodInsuranceDayPreMonthDefault.Value;
            }
        }

        private static int? _periodInsuranceDayCurrentMonthDefault;
        /// <summary>Chu Kỳ BH mặc định [ngày 14 tháng này] (Hoặc đọc từ cấu hình)</summary>
        public static int PeriodInsuranceDayCurrentMonthDefault
        {
            get
            {
                //if (!_periodInsuranceDayCurrentMonthDefault.HasValue)
                //{
                    var configService = new Sys_AttOvertimePermitConfigServices();
                    _periodInsuranceDayCurrentMonthDefault = configService.GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYDEFAULTTO);
                    if (_periodInsuranceDayCurrentMonthDefault == null)
                    {
                        _periodInsuranceDayCurrentMonthDefault = 15;
                    }
                //}
                return _periodInsuranceDayCurrentMonthDefault.Value;
            }
        }
        #endregion
        
        public bool HasLeaveGreater14day { get { return true; } }

        #endregion

        #region Phân tích Bảo Hiểm - Chuyển code từ bản 7 sql sang

        /// <summary> Hàm tính toán và save lại bảo hiểm của từng người theo tháng  </summary>
        /// <param name="orgs"></param>
        /// <param name="monthCheck">Vd: 01/12/2014</param>
        /// <param name="periodInsurance">Loại tạm thời , Loại Chính Thức</param>
        public void AnalyzeAndSaveInsuranceByMonth(string orgs, DateTime monthCheck, string periodInsurance, string codeEmp, List<Guid> socialInsPlaceIDs,string userLogin)
        {
            /*
            *  Goal(tính toán và save lại bảo hiểm của từng NV theo tháng và phòng ban (Tạo mới hoặc update Ins_ProfileInsuranceMonthly))
            *  Steps :
            *     - Step1  :  Lấy DS NV theo phòng ban và theo tháng được chọn
            *     - Step2  :  xử lý tính ra tháng bắt đầu join đóng BHXH,BHYT,BHTN
            *     - Step3  :  Set giá trị cho IsLeaveNonWorkday trong profile (cho nghỉ >=14 ngày)
            *     - Step4  :  kiểm tra xem có đóng BHXH,BHYT,BHTN
            *     - Step5  :  Tính lương và mức đóng của BHXH,BHYT,BHTN
            *     - Step6  :  Tạo mới hoặc cập nhật dữ liệu vào bảng Ins_ProfileInsuranceMonthly
            */


            //ngày 1 tháng N
            var beginMonth = new DateTime(monthCheck.Year, monthCheck.Month, 1);
            //ngày 31 tháng N
            DateTime endMonth = beginMonth.AddMonths(1).AddMinutes(-1);
            var step = 200;
            monthCheck = new DateTime(monthCheck.Year, monthCheck.Month, 1);
            var lstProfile = new List<Hre_ProfileEntity>();

            using (var context = new VnrHrmDataContext())
            {
                #region Khai báo
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoInsMonthly = new Ins_ProfileInsuranceMonthlyRepository(unitOfWork);
                var repoSalInsuranceSalary = new Sal_InsuranceSalaryRepository(unitOfWork);
                var repoAtt_WorkDay = new Att_WorkDayRepository(unitOfWork);
                var repoAtt_LeaveDay = new Att_LeavedayRepository(unitOfWork);
                var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var repoAtt_Roster = new Att_RosterRepository(unitOfWork);
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var repoIns_InsuranceRecord = new Ins_InsuranceRecordRepository(unitOfWork);
                var profileIDByCodeEmp = Guid.Empty;
                #endregion

                #region phân tích theo mã NV (từ đk tìm kiếm)
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    var profileByCodeEmp = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty, p => p.IsDelete == null && p.CodeEmp == codeEmp).FirstOrDefault();
                    if (profileByCodeEmp != null)
                    {
                        profileIDByCodeEmp = profileByCodeEmp.ID;
                    }
                    else
                    {
                        return;
                    }
                }
                #endregion

                //lay ds ngay nghi 
                var dayOffMonthChecks = unitOfWork.CreateQueryable<Cat_DayOff>(Guid.Empty, m => m.DateOff.Year == monthCheck.Year
                    && m.DateOff.Month == monthCheck.Month).ToList();

                #region lay hop dong theo phòng ban
                //List<object> contractPara = new List<object>();
                //contractPara.AddRange(new object[16]);
                //contractPara[0] = null;
                //contractPara[1] = null;
                //contractPara[2] = orgs;
                //contractPara[3] = null;
                //contractPara[4] = null;
                //contractPara[5] = null;
                //contractPara[6] = null;
                //contractPara[7] = null;
                //contractPara[8] = null;
                //contractPara[9] = null;
                //contractPara[10] = null;
                //contractPara[11] = null;
                //contractPara[12] = null;
                //contractPara[13] = null;
                //contractPara[14] = 1;
                //contractPara[15] = int.MaxValue;
                //if (profileIDByCodeEmp != Guid.Empty)
                //{
                //    contractPara[1] = codeEmp;
                //    contractPara[2] = null;
                //}
                //var contracts = GetData<Hre_ContractEntity>(contractPara, ConstantSql.hrm_hr_sp_get_Contract, ref status).Translate<Hre_Contract>().ToList();
                var contracts = new List<Hre_Contract>();

                #endregion

                #region Get Profiles by orgs
                var listObj = new List<object> { orgs, string.Empty, string.Empty };
                if (profileIDByCodeEmp != Guid.Empty)
                {
                    listObj[0] = null;
                    listObj[2] = codeEmp;
                }
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    lstProfile = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty, m => m.CodeEmp == codeEmp).ToList().Translate<Hre_ProfileEntity>();
                }
                else if (orgs != null)
                {
                    var lstOrderNumberORG = orgs.Split(',').Select(s => int.Parse(s)).Distinct().ToList();
                    var orgObj = unitOfWork.CreateQueryable<Cat_OrgStructure>(Guid.Empty).Select(p => p.ID).ToList();
                    var orgStructureIDs = unitOfWork.CreateQueryable<Cat_OrgStructure>(Guid.Empty, m => lstOrderNumberORG.Contains(m.OrderNumber)).Select(m => m.ID).ToList();
                    lstProfile = new List<Hre_ProfileEntity>();
                    foreach (var orgIds in orgStructureIDs.Chunk(step))
                    {
                        var profileByOrgs = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty, m => m.OrgStructureID.HasValue && orgIds.Contains(m.OrgStructureID.Value)).ToList().Translate<Hre_ProfileEntity>();
                        lstProfile.AddRange(profileByOrgs);
                    }
                }
                #endregion

                if (socialInsPlaceIDs != null)
                {
                    socialInsPlaceIDs = socialInsPlaceIDs.Where(p => p != null && p != Guid.Empty).ToList();
                    if (socialInsPlaceIDs != null && socialInsPlaceIDs.Any())
                    {
                        lstProfile = lstProfile.Where(p => socialInsPlaceIDs != null && socialInsPlaceIDs.Count > 0 && socialInsPlaceIDs.Contains(p.SocialInsPlaceID ?? Guid.Empty)).ToList();
                    }
                }



                #region lay ds inusuranceSalary by profile list
                //ds profileId theo phong ban
                List<Guid> lstProfileIDs = lstProfile.Select(m => m.ID).ToList();
                var lstInsuranceSalaryByProfile = new List<Sal_InsuranceSalary>();
                foreach (var profileIds in lstProfileIDs.Chunk(step))
                {
                    var lstInsuranceSalaryByProfileTemp = unitOfWork.CreateQueryable<Sal_InsuranceSalary>(Guid.Empty, m => m.InsuranceAmount != null && m.ProfileID != null
                        && m.DateEffect <= endMonth && profileIds.Contains(m.ProfileID.Value)).ToList();
                    lstInsuranceSalaryByProfile.AddRange(lstInsuranceSalaryByProfileTemp);
                    var contractTemp = unitOfWork.CreateQueryable<Hre_Contract>(Guid.Empty, m => profileIds.Contains(m.ProfileID)).ToList();
                    contracts.AddRange(contractTemp);
                }

                if (lstInsuranceSalaryByProfile.Any())
                {
                    lstProfileIDs = lstInsuranceSalaryByProfile.Select(p => p.ProfileID ?? Guid.Empty).ToList();
                    lstProfile = lstProfile.Where(p => lstProfileIDs.Contains(p.ID)).ToList();
                }
                #endregion

                #region ds NV đóng Bảo Hiểm theo phòng ban và tháng (Ins_ProfileInsuranceMonthly)
                var listInsMonthlyObj = new List<object> { orgs, monthCheck, null, 1, int.MaxValue - 1 };
                if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                {
                    listInsMonthlyObj.Add("id");
                }
                var lstProfileInsuranceMonthlyInDb = GetData<Ins_ProfileInsuranceMonthlyEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly,userLogin, ref status).Translate<Ins_ProfileInsuranceMonthly>();
                //   lstProfileInsuranceMonthlyInDb = lstProfileInsuranceMonthlyInDb.Where(m => m.PaybackID == null ).ToList();
                #endregion

                #region Tháng bắt đầu tham gia bảo hiểm
                SetMonthJoinInsuranceByProfile(lstProfile, lstInsuranceSalaryByProfile, endMonth);
                #endregion

                #region Kiem tra co dong bao hiem ko?
                //Kiểm tra xem co đóng BHXH,BHYT,BHTN
                SetIsHaveInsurnceByProfileByMonth(lstProfile, monthCheck, contracts,userLogin);
                if (lstProfile.Any())
                {
                    lstProfileIDs = lstProfile.Select(p => p.ID).ToList();
                }
                #endregion

                #region Nghỉ 14 ngày
                List<Hre_ProfileEntity> lstProfileEntities = new List<Hre_ProfileEntity>();
                List<string> lstLeaveInsuranceType = String.Join(",", leaveDayInsuranceTypes).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (HasLeaveGreater14day)
                {
                    string E_APPROVE = HDTJobStatus.E_APPROVE.ToString();
                    var lstProfilesIdChunks = lstProfileIDs;
                    var leaveDayTypeIDs = unitOfWork.CreateQueryable<Cat_LeaveDayType>(Guid.Empty, m => m.PaidRate == 0 || lstLeaveInsuranceType.Contains(m.InsuranceType)).Select(m => m.ID).ToList();
                    foreach (var profileIDs in lstProfilesIdChunks.Chunk(step))
                    {
                        var lstProfileEntitiesTemp = lstProfile.Where(p => profileIDs.Contains(p.ID)).ToList();
                        #region workday từ 1->31 tháng N theo các NV (không có quẹt thẻ)
                        var lstWorkday = unitOfWork.CreateQueryable<Att_Workday>(Guid.Empty, m => m.InTime1 == null && m.OutTime1 == null && m.WorkDate >= beginMonth
                            && m.WorkDate <= endMonth && profileIDs.Contains(m.ProfileID)).Select(m => new CustomWorkDayEntity() { ProfileID = m.ProfileID, InTime1 = m.InTime1, OutTime1 = m.OutTime1, WorkDate = m.WorkDate }).ToList();
                        #endregion
                        #region Leaveday (ngày nghỉ trong khoảng 1->31 tháng N) theo NV
                        string app = LeaveDayStatus.E_APPROVED.ToString();
                        var lstLeaveDay = unitOfWork.CreateQueryable<Att_LeaveDay>(Guid.Empty, m => m.Status == app && m.DateStart <= endMonth && m.DateEnd >= beginMonth && profileIDs.Contains(m.ProfileID))
                            .Select(m => new CustomLeavedayEntity() { ProfileID = m.ProfileID, DateStart = m.DateStart, DateEnd = m.DateEnd, LeaveDayTypeID = m.LeaveDayTypeID }).ToList();
                        #endregion
                        //paidrate = 0 :ko tra luong
                        var lstLeavedayTypeRateZero = unitOfWork.CreateQueryable<Cat_LeaveDayType>(Guid.Empty, p => p.PaidRate == 0).ToList();

                        #region lấy ds chứng từ bảo hiểm
                        var insRecords = unitOfWork.CreateQueryable<Ins_InsuranceRecord>(Guid.Empty, m =>
                            m.DateStart != null && m.DateEnd != null
                            && m.DateStart <= endMonth && m.DateEnd >= beginMonth && profileIDs.Contains(m.ProfileID))
                            .Select(m => new CustomInsuranceRecordEntity() { ProfileID = m.ProfileID, DateStart = m.DateStart.Value, DateEnd = m.DateEnd.Value, InsuranceType = m.InsuranceType }).ToList();
                        #endregion
                        var approved = RosterStatus.E_APPROVED.ToString();
                        var rosters = unitOfWork.CreateQueryable<Att_Roster>(Guid.Empty, p => p.Status == approved
                               && (p.DateStart <= endMonth && p.DateEnd >= beginMonth)
                               && profileIDs.Contains(p.ProfileID))
                               .Select(m => new CustomRosterEntity() { ProfileID = m.ProfileID, DateStart = m.DateStart, DateEnd = m.DateEnd, MonShiftID = m.MonShiftID, TueShiftID = m.TueShiftID, WedShiftID = m.WedShiftID, ThuShiftID = m.ThuShiftID, FriShiftID = m.FriShiftID, SatShiftID = m.SatShiftID, SunShiftID = m.SunShift2ID })
                               .ToList();

                        //kt co nghỉ hơn 14 ngay ko?
                        SetLeaveNonWorkdayByProfile(lstProfileEntitiesTemp, beginMonth, endMonth, lstWorkday, lstLeaveDay, lstLeavedayTypeRateZero, insRecords, rosters, dayOffMonthChecks);
                        //Hàm do Triet.Mai Hỗ trợ. tính ra số ngày  không làm HDTJOb
                        // "SC, SU, SM, SD, DP, D, M, AL, TSC,TAS"
                        lstLeaveDay = lstLeaveDay.Where(m => m.LeaveDayTypeID != null && leaveDayTypeIDs.Contains(m.LeaveDayTypeID.Value)).ToList();
                        var lstHDTJob = unitOfWork.CreateQueryable<Hre_HDTJob>(Guid.Empty, m => m.Status == E_APPROVE && m.DateFrom <= endMonth && m.DateTo >= beginMonth
                            && m.ProfileID.HasValue && profileIDs.Contains(m.ProfileID.Value)).ToList();
                        GetHDTJobDayByProfile(lstProfileEntitiesTemp, leaveDayTypeIDs, monthCheck, rosters, lstLeaveDay, lstHDTJob);

                        lstProfileEntities.AddRange(lstProfileEntitiesTemp);
                    }
                }
                lstProfile = lstProfileEntities;
                #endregion

                #region tính lương , mức đóng BHXH,BHYT,BHTN
                SetMoneyInsuranceByProfileByMonth(lstProfile, lstInsuranceSalaryByProfile, endMonth, orgs, contracts,userLogin);
                #endregion

                #region Save Change

                var lstProfileInsuranceMonthlyAddNew = new List<Ins_ProfileInsuranceMonthly>();
                var lstProfileInsuranceMonthlyModify = new List<Ins_ProfileInsuranceMonthly>();

                foreach (var profile in lstProfile)
                {
                    if (lstProfileInsuranceMonthlyInDb.Where(m => m.ProfileID == profile.ID && m.PaybackID != null).FirstOrDefault() != null)
                    {
                        continue;
                    }

                    Ins_ProfileInsuranceMonthly insuranceByProfile = lstProfileInsuranceMonthlyInDb
                        .FirstOrDefault(m => m.ProfileID == profile.ID && m.PaybackID == null);
                    if (insuranceByProfile != null) //Update cái dữ liệu cũ
                    {
                        insuranceByProfile.WorkPlaceID = profile.WorkPlaceID;
                        insuranceByProfile.SocialInsPlaceID = profile.SocialInsPlaceID;
                        insuranceByProfile.IsSocialInsurance = profile.IsHaveInsSocial;
                        insuranceByProfile.IsHealthInsurance = profile.IsHaveInsHealth;
                        insuranceByProfile.IsUnEmpInsurance = profile.IsHaveInsUnEmp;
                        insuranceByProfile.IsPregnant = profile.IsPregnant;
                        insuranceByProfile.MoneySocialInsurance = profile.MoneyInsuranceSocial;
                        insuranceByProfile.MoneyHealthInsurance = profile.MoneyInsuranceHealth;
                        insuranceByProfile.MoneyUnEmpInsurance = profile.MoneyInsuranceUnEmp;
                        insuranceByProfile.SalaryInsurance = profile.MoneyInsuranceTotal;
                        insuranceByProfile.SalaryHealthInsurance = profile.MoneyInsuranceHealthTotal;
                        insuranceByProfile.SalaryUnEmpInsurance = profile.MoneyInsuranceUnEmpTotal;
                        insuranceByProfile.IsDecreaseWorkingDays = profile.IsDecreaseWorkingDays;
                        insuranceByProfile.JobName = profile.JobName;
                        insuranceByProfile.AmountHDTIns = profile.AmountHDTIns;
                        insuranceByProfile.HDTGroupCode = profile.HDTJobGroupCode;
                        insuranceByProfile.TypeGetData = periodInsurance;
                        #region Tung.Ly set các tỉ lệ và mức đóng
                        insuranceByProfile.Allowance1 = profile.Allowance1;
                        insuranceByProfile.Allowance2 = profile.Allowance2;
                        insuranceByProfile.Allowance3 = profile.Allowance3;
                        insuranceByProfile.Allowance4 = profile.Allowance4;
                        insuranceByProfile.AmountChargeIns = profile.AmountChargeIns;
                        insuranceByProfile.SocialInsComRate = profile.SocialInsComRate;
                        insuranceByProfile.SocialInsComAmount = profile.SocialInsComAmount;
                        insuranceByProfile.SocialInsEmpRate = profile.SocialInsEmpRate;
                        insuranceByProfile.SocialInsEmpAmount = profile.SocialInsEmpAmount;
                        insuranceByProfile.HealthInsComRate = profile.HealthInsComRate;
                        insuranceByProfile.HealthInsComAmount = profile.HealthInsComAmount;
                        insuranceByProfile.HealthInsEmpRate = profile.HealthInsEmpRate;
                        insuranceByProfile.HealthInsEmpAmount = profile.HealthInsEmpAmount;
                        insuranceByProfile.UnemployComRate = profile.UnemployComRate;
                        insuranceByProfile.UnemployComAmount = profile.UnemployComAmount;
                        insuranceByProfile.UnemployEmpRate = profile.UnemployEmpRate;
                        insuranceByProfile.UnemployEmpAmount = profile.UnemployEmpAmount;
                        insuranceByProfile.IsPayback = false;
                        insuranceByProfile.MonthYearEffect = monthCheck;
                        #endregion
                        lstProfileInsuranceMonthlyModify.Add(insuranceByProfile);//add vao list (Modify)
                    }
                    else //Tạo mới
                    {
                        var newInsurance = new Ins_ProfileInsuranceMonthly
                        {
                            ProfileID = profile.ID,
                            WorkPlaceID = profile.WorkPlaceID,
                            SocialInsPlaceID = profile.SocialInsPlaceID,
                            IsSocialInsurance = profile.IsHaveInsSocial,
                            IsHealthInsurance = profile.IsHaveInsHealth,
                            IsUnEmpInsurance = profile.IsHaveInsUnEmp,
                            IsPregnant = profile.IsPregnant,
                            MoneySocialInsurance = profile.MoneyInsuranceSocial,
                            MoneyHealthInsurance = profile.MoneyInsuranceHealth,
                            MoneyUnEmpInsurance = profile.MoneyInsuranceUnEmp,
                            IsDecreaseWorkingDays = profile.IsDecreaseWorkingDays,
                            SalaryInsurance = profile.MoneyInsuranceTotal,
                            AmountHDTIns = profile.AmountHDTIns,
                            HDTGroupCode = profile.HDTJobGroupCode,
                            SalaryHealthInsurance = profile.MoneyInsuranceHealthTotal,
                            SalaryUnEmpInsurance = profile.MoneyInsuranceUnEmpTotal,
                            TypeGetData = periodInsurance,
                            MonthYear = monthCheck,
                            IsPayback = false,
                            MonthYearEffect = monthCheck,
                            #region Tung.Ly set các tỉ lệ và mức đóng
                            Allowance1 = profile.Allowance1,
                            Allowance2 = profile.Allowance2,
                            Allowance3 = profile.Allowance3,
                            Allowance4 = profile.Allowance4,
                            AmountChargeIns = profile.AmountChargeIns,
                            SocialInsComRate = profile.SocialInsComRate,
                            SocialInsComAmount = profile.SocialInsComAmount,
                            SocialInsEmpRate = profile.SocialInsEmpRate,
                            SocialInsEmpAmount = profile.SocialInsEmpAmount,
                            HealthInsComRate = profile.HealthInsComRate,
                            HealthInsComAmount = profile.HealthInsComAmount,
                            HealthInsEmpRate = profile.HealthInsEmpRate,
                            HealthInsEmpAmount = profile.HealthInsEmpAmount,
                            UnemployComRate = profile.UnemployComRate,
                            UnemployComAmount = profile.UnemployComAmount,
                            UnemployEmpRate = profile.UnemployEmpRate,
                            UnemployEmpAmount = profile.UnemployEmpAmount,
                            JobName = profile.JobName,
                            #endregion
                        };
                        lstProfileInsuranceMonthlyAddNew.Add(newInsurance);//add new
                    }
                }
                if (lstProfileInsuranceMonthlyAddNew.Any())
                {
                    repoInsMonthly.Add(lstProfileInsuranceMonthlyAddNew);
                }
                if (lstProfileInsuranceMonthlyModify.Any())
                {
                    repoInsMonthly.Edit(lstProfileInsuranceMonthlyModify);
                }
                repoInsMonthly.SaveChanges();
                #endregion
            }

        }

        /// <summary>
        /// Hàm tính ra số ngày không làm HDTJob
        /// </summary>
        /// <param name="lstProfile">Ds Nhân Viên</param>
        /// <param name="LeaveDayInsuranceType">các loại chứng từ bảo hiểm (Enum InsuranceType) </param>
        /// <param name="MonthCheck">tháng kiểm tra</param>
        public static void GetHDTJobDayByProfile(List<Hre_ProfileEntity> lstProfile, List<Guid> lstLeaveDayTypeIds, DateTime MonthCheck, List<CustomRosterEntity> lstRoster
            , List<CustomLeavedayEntity> lstLeaveDayHaveCode, List<Hre_HDTJob> lstHDTJob)
        {


            DateTime DateStart = new DateTime(MonthCheck.Year, MonthCheck.Month, 1);
            DateTime DateEnd = DateStart.AddMonths(1).AddSeconds(-1);
            string E_Four = EnumDropDown.HDTJobType.E_TYPE4.ToString();
            string E_Five = EnumDropDown.HDTJobType.E_TYPE5.ToString();
            string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();

            for (int i = 0; i < lstProfile.Count; i++)
            {
                var profile = lstProfile[i];
                if (profile.IsDecreaseWorkingDays == true)
                    continue;
                var lstHDTJobByProfile = lstHDTJob.Where(m => m.ProfileID == profile.ID).ToList();
                var lstLeaveDayHaveCodeByProfile = lstLeaveDayHaveCode.Where(m => m.ProfileID == profile.ID).ToList();
                var lstRosterByProfile = lstRoster.Where(m => m.ProfileID == profile.ID).ToList();
                int Numday_Non_HDTJob = 0;
                int Numday_HDTJob_4 = 0;
                int Numday_HDTJob_5 = 0;
                bool isHaveHDTJob = lstHDTJob.Any(m => m.ProfileID == profile.ID);
                if (isHaveHDTJob)
                {
                    for (DateTime dateCheck = DateStart; dateCheck <= DateEnd; dateCheck = dateCheck.AddDays(1))
                    {
                        bool isHaveRoster = false;
                        switch (dateCheck.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.MonShiftID != null))
                                {
                                    isHaveRoster = true;
                                }
                                break;
                            case DayOfWeek.Tuesday:
                                if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.TueShiftID != null))
                                {
                                    isHaveRoster = true;
                                }
                                break;
                            case DayOfWeek.Wednesday:
                                if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.WedShiftID != null))
                                {
                                    isHaveRoster = true;
                                }
                                break;
                            case DayOfWeek.Thursday:
                                if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.ThuShiftID != null))
                                {
                                    isHaveRoster = true;
                                }
                                break;
                            case DayOfWeek.Friday:
                                if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.FriShiftID != null))
                                {
                                    isHaveRoster = true;
                                }
                                break;
                            case DayOfWeek.Saturday:
                                if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.SatShiftID != null))
                                {
                                    isHaveRoster = true;
                                }
                                break;
                            case DayOfWeek.Sunday:
                                if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.SunShiftID != null))
                                {
                                    isHaveRoster = true;
                                }
                                break;
                        }
                        if (isHaveRoster)
                        {
                            //thỏa 2 điều kiên 1: nam ngoài cung hdt job Hoặc là có ngày nghỉ trong loại kia
                            if (lstLeaveDayHaveCodeByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck) || !lstHDTJobByProfile.Any(m => m.DateFrom <= dateCheck && m.DateTo >= dateCheck))
                            {
                                Numday_Non_HDTJob++;
                            }

                            if (lstHDTJobByProfile.Any(m => m.Type == E_Four && m.DateFrom <= dateCheck && m.DateTo >= dateCheck))
                            {
                                Numday_HDTJob_4++;
                            }
                            if (lstHDTJobByProfile.Any(m => m.Type == E_Five && m.DateFrom <= dateCheck && m.DateTo >= dateCheck))
                            {
                                Numday_HDTJob_5++;
                            }
                        }
                    }
                    profile.NumdayNonHDTJob = Numday_Non_HDTJob;
                    profile.NumdayHDTJobTypeIV = Numday_HDTJob_4;
                    profile.NumdayHDTJobTypeV = Numday_HDTJob_5;
                }
                else
                {
                    profile.NumdayNonHDTJob = int.MaxValue;
                    profile.NumdayHDTJobTypeIV = 0;
                    profile.NumdayHDTJobTypeV = 0;
                }

            }

        }


        /// <summary> hàm xử lý tính ra tháng bắt đầu join đóng BHXH,BHYT,BHTN </summary>
        /// <param name="lstProfile"></param>
        /// <param name="endMonth">Cuối tháng N</param>
        private void SetMonthJoinInsuranceByProfile(List<Hre_ProfileEntity> lstProfile, List<Sal_InsuranceSalary> lstSalInsByProfile, DateTime endMonth)
        {
            //Các Quy Tắc lấy Ngày Bắt Đầu Các Loại BH ở đây
            //Ngày 24/11/2014 hiện tại có một nguyên tắc lấy duy nhất là từ chế độ lương được cấu hình

            //lay lương đóng bao hiem voi thang kiem tra sau ngay ap dung cho tung nhan vien
            // kiểm tra lương đóng bảo hiểm có đóng bhxh ko? có => set MonthBeginInsSocial
            // kiểm tra lương đóng bảo hiểm có đóng BHYT ko? có => set MonthBeginInsHealth
            // kiểm tra lương đóng bảo hiểm có đóng BHTN ko? có => set MOnthBeginInsUnEmp

            foreach (var profile in lstProfile)
            {
                var salInsByProfile = lstSalInsByProfile.Where(m => m.ProfileID == profile.ID && m.DateEffect <= endMonth).ToList();
                var salInsByProfileSocical = salInsByProfile.Where(m => m.IsSocialIns == true).OrderBy(m => m.DateEffect).FirstOrDefault();
                if (salInsByProfileSocical != null)
                {
                    profile.MonthBeginInsSocial = salInsByProfileSocical.DateEffect;
                }
                var salInsByProfileHealth = salInsByProfile.Where(m => m.IsMedicalIns == true).OrderBy(m => m.DateEffect).FirstOrDefault();
                if (salInsByProfileHealth != null)
                {
                    profile.MonthBeginInsHealth = salInsByProfileHealth.DateEffect;
                }
                var salInsByProfileUnEmp = salInsByProfile.Where(m => m.IsUnimploymentIns == true).OrderBy(m => m.DateEffect).FirstOrDefault();
                if (salInsByProfileUnEmp != null)
                {
                    profile.MonthBeginInsUnEmp = salInsByProfileUnEmp.DateEffect;
                }
            }
        }

        /// <summary> hàm kiểm tra xem có đóng BHXH,BHYT,BHTN  </summary>
        /// <param name="lstProfile"></param>
        /// <param name="MonthCheck">Đầu tháng kiểm tra</param>
        private void SetIsHaveInsurnceByProfileByMonth(List<Hre_ProfileEntity> lstProfile, DateTime MonthCheck, List<Hre_Contract> contracts,string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoInsRecord = new Ins_InsuranceRecordRepository(unitOfWork);
                var repoCat_ContractType = new Cat_ContractTypeRepository(unitOfWork);
                var repoAtt_LeaveDay = new Att_LeavedayRepository(unitOfWork);
                string status = string.Empty;
                #region Thời gian bao hiểm tính từ 16 tháng đang kiểm tra đến 15 tháng sau
                List<Guid> lstProfileID = lstProfile.Select(m => m.ID).ToList();
                var dateStart = new DateTime(MonthCheck.Year, MonthCheck.Month, PeriodInsuranceDayPreMonthDefault );
                dateStart = dateStart.AddMonths(-1);
                var dateEnd = new DateTime(MonthCheck.Year, MonthCheck.Month, PeriodInsuranceDayCurrentMonthDefault );
                #endregion

                //sinh con , nuoi con nuoi
                string E_PREGNANCY_SUCKLE = InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString();

                var listInsuranceRecordObj = new List<object> { null, null, null, null, null, null, null, null, null, null, 1, 50000 };


                //lay ds chứng từ bảo hiểm
                var insRecords = GetData<Ins_InsuranceRecordEntity>(listInsuranceRecordObj, ConstantSql.hrm_ins_sp_get_InsuranceRecord,userLogin, ref status).Translate<Ins_InsuranceRecordEntity>();

                //lay chứng từ bảo hiểm thuộc loại [sinh con , nuoi con nuoi]
                var lstInsRecord = insRecords.Where(m => m.InsuranceType == E_PREGNANCY_SUCKLE
                                                       && m.DateStart <= dateEnd && m.DateEnd >= dateStart
                                                       && lstProfileID.Contains(m.ProfileID)
                                                       ).ToList().Translate<Ins_InsuranceRecord>();
                #region Lay Loai Hop Dong
                var hocViecContractType = EnumDropDown.TypeContract.E_APPRENTICESHIP.ToString();
                //loại hoc viec
                var contractTypes = unitOfWork.CreateQueryable<Cat_ContractType>(Guid.Empty, p => p.Type == hocViecContractType).ToList();
                #endregion

                #region Honda
                DateTime dateStartNewHONDA = new DateTime(MonthCheck.Year, MonthCheck.Month, PeriodInsuranceDayPreMonth);
                dateStartNewHONDA = dateStartNewHONDA.AddMonths(-1);
                DateTime dateEndNewHONDA = new DateTime(MonthCheck.Year, MonthCheck.Month, PeriodInsuranceDayCurrentMonth);
                #endregion

                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                var profileIds = lstProfile.Select(p => p.ID).ToList();
                var leaveDayTypeID = unitOfWork.CreateQueryable<Cat_LeaveDayType>(Guid.Empty, m => m.InsuranceType == E_PREGNANCY_SUCKLE).Select(m => m.ID).FirstOrDefault();
                var lstLeaveDayTypePreg = new List<Att_LeaveDay>();

                foreach (var lstProfileIds in profileIds.Chunk(200))
                {
                    var lstLeaveDayTypePregTemp = unitOfWork.CreateQueryable<Att_LeaveDay>(Guid.Empty, m => m.Status == E_APPROVED && m.DateStart <= dateEnd && m.DateEnd >= dateStart
                              && m.LeaveDayTypeID == leaveDayTypeID && lstProfileIds.Contains(m.ProfileID)).ToList();
                    lstLeaveDayTypePreg.AddRange(lstLeaveDayTypePregTemp);
                }

                foreach (var profile in lstProfile)
                {
                    if (profile.MonthBeginInsSocial == null || profile.MonthBeginInsSocial > dateEndNewHONDA || profile.IsDecreaseWorkingDays == true)
                    {
                        profile.IsHaveInsSocial = false;
                    }
                    else
                    {
                        profile.IsHaveInsSocial = true;
                    }

                    if (profile.MonthBeginInsHealth == null || profile.MonthBeginInsHealth > dateEndNewHONDA || profile.IsDecreaseWorkingDays == true)
                    {
                        profile.IsHaveInsHealth = false;
                    }
                    else
                    {
                        profile.IsHaveInsHealth = true;
                    }

                    if (profile.MonthBeginInsUnEmp == null || profile.MonthBeginInsUnEmp > dateEndNewHONDA || profile.IsDecreaseWorkingDays == true)
                    {
                        profile.IsHaveInsUnEmp = false;
                    }
                    else
                    {
                        profile.IsHaveInsUnEmp = true;
                    }

                    #region Thai San

                    #region Kiem tra co thai san hay khong
                    List<Att_LeaveDay> lstLeaveDayByProfile = lstLeaveDayTypePreg.Where(m => m.ProfileID == profile.ID).ToList();
                    lstInsRecord = lstInsRecord.Where(p => p.ProfileID == profile.ID).ToList();
                    if (HasPregnant(dateEnd, lstLeaveDayByProfile, lstInsRecord))
                    {
                        profile.IsHaveInsSocial = false;
                        profile.IsHaveInsUnEmp = false;
                        profile.IsHaveInsHealth = false;
                        profile.IsPregnant = true;
                    }
                    #endregion

                    #endregion

                    #region Nghi viec
                    if (profile.DateQuit.HasValue && profile.DateQuit.Value <= dateEnd)
                    {
                        profile.IsHaveInsSocial = false;
                        profile.IsHaveInsHealth = false;
                        profile.IsHaveInsUnEmp = false;
                    }
                    #endregion

                    #region Hợp đồng Học việc / Hợp đồng thử việc (không đóng BH)
                    if (contractTypes.Any())
                    {
                        //loai hop dong  học việc
                        var contractTypeIds = contractTypes.Select(p => p.ID).ToList();
                        //lay ngay bat dau hop dong mới nhất
                        var contractProfiles = contracts.Where(p => p.ProfileID == profile.ID && p.DateStart <= dateEndNewHONDA).OrderByDescending(p => p.DateStart).FirstOrDefault();
                        if (contractProfiles != null && contractTypeIds.Contains(contractProfiles.ContractTypeID))
                        {
                            profile.IsHaveInsSocial = false;
                            profile.IsHaveInsHealth = false;
                            profile.IsHaveInsUnEmp = false;
                        }
                    }
                    #endregion
                }
            }
        }

        /// <summary>Thai Sản</summary>
        /// <param name="MonthYear"></param>
        /// <param name="lstLeaveDayPregByProfile"></param>
        /// <param name="lstInsuranceRecordPregByProfile"></param>
        /// <returns></returns>
        public bool HasPregnant(DateTime MonthYear, List<Att_LeaveDay> lstLeaveDayPregByProfile, List<Ins_InsuranceRecord> lstInsuranceRecordPregByProfile)
        {
            DateTime MonthPresent = new DateTime(MonthYear.Year, MonthYear.Month,PeriodInsuranceDayCurrentMonthDefault);
            if (lstLeaveDayPregByProfile.Any(m => m.DateStart <= MonthPresent && m.DateEnd > MonthPresent) ||
                lstInsuranceRecordPregByProfile.Any(m => m.DateStart <= MonthPresent && m.DateEnd > MonthPresent))
            {
                return true;
            }
            return false;
        }



        /// <summary>Hàm tính lương và mức đóng của BHXH,BHYT,BHTN</summary>
        /// <param name="lstProfile"></param>
        /// <param name="lstSalInsByProfile"></param>
        /// <param name="endMonthCheck">Cuối tháng kiểm tra</param>
        /// <param name="orgs"></param>
        /// <param name="contracts"></param>
        private void SetMoneyInsuranceByProfileByMonth(List<Hre_ProfileEntity> lstProfile, List<Sal_InsuranceSalary> lstSalInsByProfile
            , DateTime endMonthCheck, string orgs, List<Hre_Contract> contracts,string userLogin)
        {
            /*
           *  Goal(Lấy tiền đóng bảo hiểm)
           *  Steps :
           *     - Step1    :  Lấy mức trần đóng bảo hiểm (theo ngày hiệu lực mới nhất)
           *     - Step2    :  Lấy lương tối thiểu  (theo ngày hiệu lực mới nhất)
           *     - Step3    :  Lấy tỉ lệ bảo hiểm với ngày áp dụng trước ngày kiểm tra
           *     - Step4    :  Lấy lương bảo hiểm của nv (bảng Sal_InsuranceSalary) với ngày ap dụng trước ngày ktra 
           *     - Step5    :  Cách lấy tiền bảo hiểm ([tienbaohiem])
           *     - Step5.1  :    If so tien(trong sal_insuranceSalary) >= mức trần -> [tienbaohiem] = [MucTran]
           *     - Step5.2  :    If so tien(trong sal_insuranceSalary) <= lương tối thiểu -> [tienbaohiem] = [LuongToiThieu]
           *     - Step6    :  [tienbaohiem]= [tienbaohiem]+ [PhuCapDieuChinh]+ [tiền HDTJob]
           */

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string status = string.Empty;
                var hdt4Amount = 0.0;
                var hdt5Amount = 0.0;
                List<Guid> lstProfileIDs = lstProfile.Select(m => m.ID).ToList();

                String typeInsurance = ExchangeRateType.E_RATE_SOCIALINSURANCE.ToString();
                var lstRateInsurance = unitOfWork.CreateQueryable<Cat_ExchangeRate>(Guid.Empty, m =>
                    m.Type == typeInsurance && m.MonthOfEffect <= endMonthCheck).OrderByDescending(al => al.MonthOfEffect).ToList();

                String statusInsCape = ValueEntityType.E_INSURANCE_CAPE_AMOUNT.ToString();
                String statusMinumunSal = ValueEntityType.E_MINIMUM_SALARY.ToString();
                var lstMaxMinSalary = unitOfWork.CreateQueryable<Cat_ValueEntity>(Guid.Empty, m => (m.IsDelete == null || m.IsDelete == false) && (m.Type == statusInsCape || m.Type == statusMinumunSal)).ToList();

                #region lay phu cap phat sinh

                List<object> unusualAllowancePara = new List<object>();
                unusualAllowancePara.AddRange(new object[9]);
                unusualAllowancePara[7] = 1;
                unusualAllowancePara[8] = Int32.MaxValue - 1;


                var lstUnusualAllowance = GetData<Sal_UnusualAllowanceEntity>(unusualAllowancePara, ConstantSql.hrm_sal_sp_get_UnusualED,userLogin, ref status).Translate<Sal_UnusualAllowance>()
                    .Where(m => (m.IsDelete == null || m.IsDelete == false) && lstProfileIDs.Contains(m.ProfileID))
                   .ToList();

                #endregion

                #region lay ds jobTitle
                var jobTitles = unitOfWork.CreateQueryable<Cat_JobTitle>(Guid.Empty, m => m.IsDelete == null).ToList();
                #endregion

                #region lay ds Cat_region
                var Regions = unitOfWork.CreateQueryable<Cat_Region>(Guid.Empty).ToList();

                #endregion

                var lstUnusualAllowanceCfg = unitOfWork.CreateQueryable<Cat_UnusualAllowanceCfg>(Guid.Empty).ToList();
                #region Lay số tiền HDT4 vã HDT5 theo timeline
                //Tại Cat_UnusualAllowanceCfg thiết lập Phụ cấp có mã: HDT4, HDT5
                //Tại Cat_UnAllowCfgAmount thiết lập Timeline cho mức HDT4 và HDT5 với mức hưởng tương ứng
                //Mong muốn: Lấy mức hưởng theo timeline để tính HDT Job cho Bảo hiểm

                const string HDT4 = "HDT4";
                const string HDT5 = "HDT5";
                var unAllowCfgAmount = unitOfWork.CreateQueryable<Cat_UnAllowCfgAmount>(Guid.Empty, p => p.Cat_UnusualAllowanceCfg != null
                    && (p.Cat_UnusualAllowanceCfg.Code == HDT4 || p.Cat_UnusualAllowanceCfg.Code == HDT5)
                    && (p.FromDate <= endMonthCheck && endMonthCheck <= p.ToDate)).ToList();
                hdt4Amount = unAllowCfgAmount.Where(p => p.Cat_UnusualAllowanceCfg != null && p.Cat_UnusualAllowanceCfg.Code == HDT4).Select(p => p.Amount ?? 0).FirstOrDefault();
                hdt5Amount = unAllowCfgAmount.Where(p => p.Cat_UnusualAllowanceCfg != null && p.Cat_UnusualAllowanceCfg.Code == HDT5).Select(p => p.Amount ?? 0).FirstOrDefault();

                #endregion

                //lấy mức trần bảo hiểm theo ngày hiệu lực
                List<Cat_ValueEntity> listMaxSalary =
                    lstMaxMinSalary.Where(val => val.Type == statusInsCape)
                        .OrderByDescending(pit => pit.DateOfEffect)
                        .ToList();
                //lấy lương tối thiểu theo ngày hiệu lực
                List<Cat_ValueEntity> listMinSalary = lstMaxMinSalary.Where(val => val.Type == statusMinumunSal)
                    .OrderByDescending(pit => pit.DateOfEffect).ToList();

                //lấy tỉ lệ bảo hiểm với ngày áp dụng trước ngày kiểm tra
                var lstInsRate = unitOfWork.CreateQueryable<Cat_RateInsurance>(Guid.Empty, m => m.ApplyFrom <= endMonthCheck).ToList();
                var orgTypes = unitOfWork.CreateQueryable<Cat_OrgStructureType>(Guid.Empty, s => s.OrgStructureTypeCode == OrgUnit.E_DEPARTMENT.ToString()).ToList();


                #region lay ds HDTJOb

                List<object> hdtJobPara = new List<object>();
                hdtJobPara.AddRange(new object[14]);
                hdtJobPara[0] = null;
                hdtJobPara[1] = null;
                hdtJobPara[2] = null;
                hdtJobPara[3] = null;
                hdtJobPara[4] = null;
                hdtJobPara[5] = orgs;
                hdtJobPara[6] = null;
                hdtJobPara[7] = null;
                hdtJobPara[8] = null;
                hdtJobPara[9] = null;
                hdtJobPara[10] = null;
                hdtJobPara[11] = null;
                hdtJobPara[12] = 1;
                hdtJobPara[13] = int.MaxValue - 1;

                var hreHDTJobs = GetData<Hre_HDTJobEntity>(hdtJobPara, ConstantSql.hrm_hr_sp_get_HDTJob,userLogin, ref status).Translate<Hre_HDTJob>().ToList();
                hreHDTJobs = hreHDTJobs.Where(p => p.DateTo.HasValue && p.DateTo.Value.Month == endMonthCheck.Month && p.DateTo.Value.Year == endMonthCheck.Year).ToList();
                //var hreHDTJobs = repoHreHDTJob.FindBy(p => p.IsDelete == null && lstProfileIDs.Contains(p.ProfileID ?? Guid.Empty)).ToList();

                #endregion

                #region Lay che do luong

                //lay ds sal_grade

                List<object> salGradePara = new List<object>();
                salGradePara.AddRange(new object[7]);
                salGradePara[0] = null;
                salGradePara[1] = null;
                salGradePara[2] = null;
                salGradePara[3] = null;
                salGradePara[4] = null;
                salGradePara[5] = 1;
                salGradePara[6] = 50000;

                var salGrade = GetData<Sal_Grade>(salGradePara, ConstantSql.hrm_sal_sp_get_Sal_Grade,userLogin, ref status)
               .Where(p => p.IsDelete == null && lstProfileIDs.Contains(p.ProfileID) && p.MonthStart.HasValue
               && p.MonthStart <= endMonthCheck).ToList();



                //var salGrade = repoSalGrade.FindBy(p => p.IsDelete == null && lstProfileIDs.Contains(p.ProfileID) && p.MonthStart.HasValue
                //    && p.MonthStart <= monthCheck).ToList();
                var catGradePayroll = new List<Cat_GradePayroll>();
                var cat_SalaryClasses = unitOfWork.CreateQueryable<Cat_SalaryClass>(Guid.Empty).ToList();
                var orgStructures = unitOfWork.CreateQueryable<Cat_OrgStructure>(Guid.Empty).ToList();
                var hdtJobGroups = unitOfWork.CreateQueryable<Cat_HDTJobGroup>(Guid.Empty).ToList();
                if (salGrade.Any())
                {
                    var catGradePayrollIds = salGrade.Select(p => p.GradePayrollID).ToList();
                    catGradePayroll = unitOfWork.CreateQueryable<Cat_GradePayroll>(Guid.Empty, p => catGradePayrollIds.Contains(p.ID)).ToList();
                }

                #endregion


                var cutOffDuration = new Att_CutOffDuration();
                if (hreHDTJobs.Any())
                {
                    cutOffDuration = unitOfWork.CreateQueryable<Att_CutOffDuration>(Guid.Empty, p => p.MonthYear.Year == endMonthCheck.Year && p.MonthYear.Month == endMonthCheck.Month).FirstOrDefault();
                    if (cutOffDuration == null)
                    {
                        cutOffDuration = new Att_CutOffDuration
                        {
                            DateStart = new DateTime(1970, 1, 1),
                            DateEnd = new DateTime(1970, 1, 1),
                            MonthYear = new DateTime(1970, 1, 1)
                        };
                    }
                }



                List<Cat_Element> listElementFormulaDB = unitOfWork.CreateQueryable<Cat_Element>(Guid.Empty).ToList();

                //tham giam bao hiem 
                var joinInsurance = false;
                foreach (var profile in lstProfile)
                {
                    if (profile.IsHaveInsSocial != true && profile.IsHaveInsHealth != true && profile.IsHaveInsUnEmp != true)
                    {
                        joinInsurance = false;
                        //   continue;
                    }
                    else
                    {
                        joinInsurance = true;
                    }
                    var salInsByProfile = lstSalInsByProfile.Where(m => m.ProfileID == profile.ID && m.DateEffect <= endMonthCheck).OrderByDescending(m => m.DateEffect).FirstOrDefault();
                    if (salInsByProfile == null)
                    {
                        salInsByProfile = new Sal_InsuranceSalary
                        {
                            InsuranceAmount = 0,
                        };
                    }

                    double salInsurance = GetInsuranceSalary(endMonthCheck, salInsByProfile, lstRateInsurance,
                        listMaxSalary, listMinSalary);

                    #region lấy tiền HDTJob theo profile và monthCheck
                    //Todo: lấy tiền HDTJob (sử dụng công thức)

                    //lay grade
                    var grade = salGrade.FirstOrDefault(p => p.ProfileID == profile.ID);
                    var gradePayroll = catGradePayroll.FirstOrDefault(p => grade != null && p.ID == grade.GradePayrollID);
                    var hdtJobMoney = 0.0;
                    if (gradePayroll != null && !string.IsNullOrEmpty(gradePayroll.FormulaSalaryIns))
                    {
                        Hre_HDTJob hreHdtJobProfile;
                        FormulaHelper.FormulaHelperModel result;
                        GetAllowanceInsuranceByFomular(salInsurance, endMonthCheck, hreHDTJobs, cutOffDuration, profile, gradePayroll, out hreHdtJobProfile, out result, lstUnusualAllowanceCfg, lstUnusualAllowance, listElementFormulaDB, hdt4Amount, hdt5Amount);
                        if (result != null && result.Value != null)
                        {
                            double.TryParse(result.Value.ToString(), out hdtJobMoney);
                        }

                        #region Generate tên công việc dựa vào tiền HDTJOb
                        FormulaHelper.FormulaHelperModel resultJobName;
                        var jobtitleName = string.Empty;
                        var jobTitleObj = jobTitles.Where(p => p.ID == profile.JobTitleID).FirstOrDefault();
                        if (jobTitleObj != null)
                        {
                            jobtitleName = jobTitleObj.JobTitleName;
                        }

                        string hdtJobGroupCode = string.Empty;
                        GetJobNameByFomular(hdtJobMoney, cat_SalaryClasses, hdtJobGroups, hreHdtJobProfile, profile, orgStructures, orgTypes, jobtitleName, gradePayroll, out resultJobName, listElementFormulaDB, out hdtJobGroupCode);
                        string jobName = string.Empty;
                        if (resultJobName != null && resultJobName.Value != null)
                        {
                            jobName = resultJobName.Value.ToString();
                        }
                        profile.JobName = jobName;
                        profile.HDTJobGroupCode = hdtJobGroupCode;
                        #endregion
                    }
                    #endregion
                    if (hdtJobMoney > 0)
                    {
                        profile.AmountHDTIns = hdtJobMoney;
                    }
                    profile.MoneyInsuranceTotal = (float)salInsurance + hdtJobMoney;
                    profile.MoneyInsuranceHealthTotal = (float)salInsurance + hdtJobMoney;
                    //profile.MoneyInsuranceUnEmpTotal = (float)salInsurance + hdtJobMoney;
                    var regionByProfile = Regions.Where(m => m.ID == profile.RegionID).FirstOrDefault();
                    if (regionByProfile != null && regionByProfile.MaxSalary != null)
                    {
                        //Todo: Code đang thiếu vì cái minimum của cái nơi đóng bảo hiểm Ps. Chờ Sơn build cái field cần thiết                      
                        profile.MoneyInsuranceUnEmpTotal = getMonneyBHTN(regionByProfile.MaxSalary.Value, salInsByProfile.InsuranceAmount ?? 0) + hdtJobMoney;
                    }
                    else
                    {
                        profile.MoneyInsuranceUnEmpTotal = (float)salInsurance + hdtJobMoney;
                    }


                    Cat_RateInsurance rateInsurance = GetInsuranceRate(endMonthCheck, lstInsRate);

                    #region [Tung.Ly  ] Lấy các phu cấp từ lương bảo hiểm (sal_insuranceSalary)

                    var contract = contracts.Where(p => p.ProfileID == profile.ID).OrderByDescending(p => p.DateUpdate).FirstOrDefault();
                    if (contract != null && joinInsurance == true)
                    {
                        profile.Allowance1 = contract.Allowance1 ?? 0;
                        profile.Allowance2 = contract.Allowance2 ?? 0;
                        profile.Allowance3 = contract.Allowance3 ?? 0;
                        profile.Allowance4 = contract.Allowance4 ?? 0;
                        profile.AmountChargeIns = profile.Allowance1 + profile.Allowance2 +
                            profile.Allowance3 + profile.Allowance4 + profile.MoneyInsuranceTotal;
                    }
                    #endregion

                    if (profile.IsHaveInsSocial == true)
                    {
                        //tong ti le BHXH do NSDLĐ và NLĐ đóng
                        double rate = (rateInsurance.SocialInsCompRate + rateInsurance.SocialInsEmpRate);
                        profile.MoneyInsuranceSocial = (float)(profile.MoneyInsuranceTotal * rate);

                        #region [Tung.Ly ]: ti le BHXH cty dong va ti le BHXH nv dong
                        //Ti lệ BHXH do cty đóng
                        profile.SocialInsComRate = (rateInsurance.SocialInsCompRate);
                        //Tỉ lệ BHXH do NV đóng
                        profile.SocialInsEmpRate = (rateInsurance.SocialInsEmpRate);
                        //Số tiền BHXH do cty đóng
                        profile.SocialInsComAmount = (double)(profile.MoneyInsuranceTotal * profile.SocialInsComRate);
                        //số tiền BHXH do NV đóng
                        profile.SocialInsEmpAmount = (double)(profile.MoneyInsuranceTotal * profile.SocialInsEmpRate);
                        #endregion

                    }
                    if (profile.IsHaveInsHealth == true)
                    {
                        //Tổng tỉ lệ BHYT do NSDLĐ và NLĐ đóng
                        double rate = (rateInsurance.HealthInsCompRate + rateInsurance.HealthInsEmpRate);
                        profile.MoneyInsuranceHealth = (float)(profile.MoneyInsuranceHealthTotal * rate);
                        #region [Tung.Ly ]: ti le BHYT cty dong va ti le BHXH nv dong
                        //Ti lệ BHYT do cty đóng
                        profile.HealthInsComRate = (rateInsurance.HealthInsCompRate);
                        //Tỉ lệ BHYT do NV đóng
                        profile.HealthInsEmpRate = (rateInsurance.HealthInsEmpRate);
                        //Số tiền BHYT do cty đóng
                        profile.HealthInsComAmount = (double)(profile.MoneyInsuranceHealthTotal * profile.HealthInsComRate);
                        //số tiền BHYT do NV đóng
                        profile.HealthInsEmpAmount = (double)(profile.MoneyInsuranceHealthTotal * profile.HealthInsEmpRate);
                        #endregion
                    }
                    if (profile.IsHaveInsUnEmp == true)
                    {
                        //Tổng tỉ lệ BHTN do NSDLĐ và NLĐ đóng
                        double rate = (rateInsurance.UnemployInsCompRate + rateInsurance.UnemployInsEmpRate);
                        profile.MoneyInsuranceUnEmp = (float)(profile.MoneyInsuranceUnEmpTotal * rate);
                        #region [Tung.Ly ]: ti le BHTN cty dong va ti le BHXH nv dong
                        //Ti lệ BHTN do cty đóng
                        profile.UnemployComRate = (rateInsurance.UnemployInsCompRate);
                        //Tỉ lệ BHTN do NV đóng
                        profile.UnemployEmpRate = (rateInsurance.UnemployInsEmpRate);
                        //Số tiền BHTN do cty đóng
                        profile.UnemployComAmount = (double)(profile.MoneyInsuranceUnEmpTotal * profile.UnemployComRate);
                        //số tiền BHTN do NV đóng
                        profile.UnemployEmpAmount = (double)(profile.MoneyInsuranceUnEmpTotal * profile.UnemployEmpRate);
                        #endregion
                    }
                }
            }

        }

        private Double GetInsuranceSalary(DateTime monthYear, List<Sal_BasicSalary> listInsBasicSalary, List<Cat_ExchangeRate> lstRateIns, List<Cat_ValueEntity> lstAmountMaxIns, List<Cat_ValueEntity> lstAmountMinIns)
        {
            Double _result = 0;
            DateTime date = DateTime.MinValue;
            Cat_Currency curIns = null;
            //Lay muc tran tai thoi diem monthYear => VND
            Double AmountMaxInsSuckle = GetMaxMinAmountOfMonth(monthYear, lstAmountMaxIns);
            Double AmountMinumumInsSuckle = GetMaxMinAmountOfMonth(monthYear, lstAmountMinIns);
            foreach (Sal_BasicSalary insBas in listInsBasicSalary)
            {
                //Lay ngay ap dung gan monthYear nhat
                if (insBas.DateOfEffect > date && insBas.DateOfEffect <= monthYear)
                {
                    date = insBas.DateOfEffect;
                    curIns = insBas.Cat_Currency1;
                    _result = insBas.InsuranceAmount;
                }
            }
            lstRateIns = lstRateIns.Where(rate => rate.MonthOfEffect <= monthYear).ToList();
            if (curIns != null && curIns.Code != CurrencyCode.VND.ToString() && lstRateIns.Count() > 0)
            {
                _result = ConvertExtractRateToVND(_result, curIns, lstRateIns);
            }

            if (_result >= AmountMaxInsSuckle)
                _result = AmountMaxInsSuckle;
            if (_result <= AmountMinumumInsSuckle && _result > 0)
                _result = AmountMinumumInsSuckle;

            return _result;
        }

        /// <summary> Lấy lương bảo hiểm </summary>
        /// <param name="monthYear"></param>
        /// <param name="InsuranceSalary"></param>
        /// <param name="lstRateIns"></param>
        /// <param name="lstInsuranceAmountCeiling">mức trần bảo hiểm theo ngày hiệu lực</param>
        /// <param name="lstInsAmountMinimum">lấy lương tối thiểu theo ngày hiệu lực</param>
        /// <returns></returns>
        private Double GetInsuranceSalary(DateTime monthYear, Sal_InsuranceSalary InsuranceSalary, List<Cat_ExchangeRate> lstRateIns, List<Cat_ValueEntity> lstInsuranceAmountCeiling, List<Cat_ValueEntity> lstInsAmountMinimum)
        {
            /*
            *  Goal(lay luong bao hiem )
            *  Steps :
            *      Step1  :  Lấy [LuongBaoHiem] (Sal_InsuranceSalary)
            *      Step2  :    Nếu [LuongBaoHiem] >= [MucTran] => lay [MucTran]
            *      Step3  :    Nếu [LuongBaoHiem] <= [LuongToiThieu] => lay [LuongToiThieu]
            */


            Double result = 0;
            //  DateTime date = DateTime.MinValue;
            Cat_Currency curIns = null;
            //Lay muc tran tai thoi diem monthYear => VND
            Double amountCeilingLasted = GetMaxMinAmountOfMonth(monthYear, lstInsuranceAmountCeiling);
            //Lay luong toi thieu tai thoi diem monthYear => VND
            Double amountMinimumSalary = GetMaxMinAmountOfMonth(monthYear, lstInsAmountMinimum);

            //    date = InsuranceSalary.DateEffect ?? monthYear;
            //tiền tệ
            curIns = InsuranceSalary.Cat_Currency;
            result = InsuranceSalary.InsuranceAmount ?? 0;

            lstRateIns = lstRateIns.Where(rate => rate.MonthOfEffect <= monthYear).ToList();
            if (curIns != null && curIns.Code != CurrencyCode.VND.ToString() && lstRateIns.Any())
            {
                result = ConvertExtractRateToVND(result, curIns, lstRateIns);
            }

            if (result >= amountCeilingLasted)
            {
                result = amountCeilingLasted;
            }
            if (result <= amountMinimumSalary && result > 0)
            {
                result = amountMinimumSalary;
            }

            return result;
        }

        private Double ConvertExtractRateToVND(Double moneyDest, Cat_Currency curDest, List<Cat_ExchangeRate> lstExchangeRate)
        {
            Double value = 0;
            String curVND = CurrencyCode.VND.ToString();
            var lstExchange = new List<Cat_ExchangeRate>();
            if (curVND == curDest.Code)
                return moneyDest;
            //Lay Nguyen Te : VND | Ngoai Te : USD
            lstExchange = lstExchangeRate.Where(cur => cur.CurrencyDestID == curDest.ID && cur.Cat_Currency.Code == curVND)
                                            .OrderByDescending(cu => cu.MonthOfEffect).ToList();
            if (lstExchange.Count <= 0)
            {
                //Neu lstExchange khong co thi lay nguoc lai Nguyen Te : USD | Ngoai te : VND
                lstExchange = lstExchangeRate.Where(cur => cur.CurrencyBaseID == curDest.ID && cur.Cat_Currency1.Code == curVND)
                                             .OrderByDescending(cu => cu.MonthOfEffect).ToList();

                if (lstExchange.Count <= 0)
                    throw new Exception("Currency " + curVND + " / " + curDest.Code + "not found ");

                //Xu ly voi truong hop Nguyen Te : USD | Ngoai te : VND
                Cat_ExchangeRate exRate = lstExchange[0];
                //Vi doi tu USD sang VND nen phai lay gia mua
                if (exRate.BuyingRate == 0)
                    throw new Exception("Exchange buying rate" + curDest.Code + " / " + curVND + "not found ");

                if (exRate.BuyingRate.HasValue)
                    value = moneyDest / exRate.BuyingRate.Value;
                return value;
            }

            //Xu ly voi truong hop Nguyen Te : VND | Ngoai te : USD
            Cat_ExchangeRate exRate1 = lstExchange[0];
            //Vi doi tu USD sang VND nen phai lay gia ban
            if (exRate1.SellingRate == 0)
                throw new Exception("Exchange selling rate" + curVND + " / " + curDest.Code + "not found ");

            value = moneyDest * exRate1.SellingRate;
            return value;
        }

        /// <summary> Lay rate dong BHXH thang gan nhat </summary>
        /// <param name="monthYear"></param>
        /// <param name="lstRateIns"></param>
        /// <returns></returns>
        private Cat_RateInsurance GetInsuranceRate(DateTime monthYear, List<Cat_RateInsurance> lstRateIns)
        {
            List<Cat_RateInsurance> lstInsRate = lstRateIns.Where(ins => ins.ApplyFrom <= monthYear).ToList();

            if (lstInsRate.Count > 0)
            {
                lstInsRate = lstInsRate.OrderByDescending(cfg => cfg.ApplyFrom).ToList();
                return lstInsRate[0];
            }
            return null;
        }

        public static int CalNumOfDayLeave(Guid profileId, DateTime DateStart, DateTime DateEnd, string TypeInsRecord, List<DateTime> lstDateOff, List<Att_WorkdayEntity> lstInOut, bool isWorkDayByInOut, bool isWorkDayByInOutMoreThan2Hour)
        {
            DateStart = DateStart.Date;
            DateEnd = DateEnd.Date.AddDays(1);
            int result = 0;

            int countInout = 0;
            int dayOff = 0;
            int sunday = 0;
            if (TypeInsRecord == InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString()
                || TypeInsRecord == InsuranceRecordType.E_PREGNANCY_PREVENTION.ToString()
                || TypeInsRecord == InsuranceRecordType.E_PREGNANCY_LOST.ToString()
                || TypeInsRecord == InsuranceRecordType.E_SICK_LONG.ToString())
            // những loại này thì tính liên ngày kể ca chủ nhật và ngày nghỉ lễ
            {
                dayOff = 0;
                sunday = 0;
            }
            #region Bao.Tran yêu cầu tính cả ngày chủ nhật và ngày nghỉ lễ (dành cho các loại dưỡng sức)
            else if (TypeInsRecord == InsuranceRecordType.E_RESTORATION_PREGNANCY.ToString()
                    || TypeInsRecord == InsuranceRecordType.E_RESTORATION_SICK.ToString()
                    || TypeInsRecord == InsuranceRecordType.E_RESTORATION_TNLD.ToString())
            {
                //Bao Trần yêu cầu các loại nghỉ dưỡng => tính cả chủ nhật , ca làm việc , ngày nghỉ lễ
                dayOff = 0;
                sunday = 0;
            }
            #endregion
            else //những loại khác thì phải tính trừ ra ngày nghỉ lễ và ngày chủ nhật
            {
                for (DateTime datecheck = DateStart; datecheck < DateEnd; datecheck = datecheck.AddDays(1))
                {
                    if (lstDateOff.Any(m => m == datecheck))
                        dayOff++;
                    if (datecheck.DayOfWeek == DayOfWeek.Sunday)
                        sunday++;
                }
            }
            if (isWorkDayByInOut)//tính có đi làm dựa theo inout
            {
                if (isWorkDayByInOutMoreThan2Hour)// Tính có đi làm nhưng phải trên 2 tiếng
                {
                    List<Att_WorkdayEntity> lstInOutCheck = lstInOut.Where(m => m.ProfileID == profileId && m.WorkDate >= DateStart && m.WorkDate < DateEnd).OrderBy(m => m.WorkDate).ToList();
                    foreach (var inout in lstInOutCheck)
                    {
                        double totalinout = 0;
                        if (inout.InTime1 != null && inout.OutTime1 != null)
                        {
                            totalinout += (inout.OutTime1.Value - inout.InTime1.Value).TotalHours;

                        }
                        if (inout.InTime2 != null && inout.OutTime2 != null)
                        {
                            totalinout += (inout.OutTime2.Value - inout.InTime2.Value).TotalHours;

                        }
                        if (inout.InTime3 != null && inout.OutTime3 != null)
                        {
                            totalinout += (inout.OutTime3.Value - inout.InTime3.Value).TotalHours;

                        }
                        if (inout.InTime4 != null && inout.OutTime4 != null)
                        {
                            totalinout += (inout.OutTime4.Value - inout.InTime4.Value).TotalHours;

                        }
                        if (totalinout > 2)
                        {
                            countInout++;
                        }
                    }

                }
                else // tính có đi làm chỉ cần có in và out
                {
                    countInout = lstInOut.Where(m => m.ProfileID == profileId && m.WorkDate >= DateStart && m.WorkDate < DateEnd).Select(m => m.WorkDate).Distinct().Count();
                }
            }
            else // tính không dựa theo inout
            {
                countInout = 0;
            }
            result = (int)(DateEnd - DateStart).TotalDays - countInout - dayOff - sunday;

            return result;

        }

        #region Cong Thuc

        /// <summary> Lấy JobName theo cong thức bên chế độ lương </summary>
        /// <param name="hdtJobMoney"></param>
        /// <param name="cat_SalaryClasses"></param>
        /// <param name="hdtJobGroups"></param>
        /// <param name="hreHdtJobProfile"></param>
        /// <param name="profile"></param>
        /// <param name="orgStructures"></param>
        /// <param name="orgTypes"></param>
        /// <param name="jobTitleName"></param>
        /// <param name="gradePayroll"></param>
        /// <param name="result"></param>
        /// <param name="listElementFormulaInDB"></param>
        private static void GetJobNameByFomular(double hdtJobMoney, List<Cat_SalaryClass> cat_SalaryClasses, List<Cat_HDTJobGroup> hdtJobGroups, Hre_HDTJob hreHdtJobProfile, Hre_ProfileEntity profile, List<Cat_OrgStructure> orgStructures, List<Cat_OrgStructureType> orgTypes, string jobTitleName, Cat_GradePayroll gradePayroll, out FormulaHelper.FormulaHelperModel result, List<Cat_Element> listElementFormulaInDB, out string hdtJobGroupCode)
        {
            /*
            *  Goal(Lấy JobName theo cong thức bên chế độ lương)
            *  Steps :
            *      Step1  :  Lấy giá trị của các phần tử 
            *      Step2  :  Thiết lập các phần tử (INS_JOBNAME_HDTJOBMONEY,INS_JOBNAME_NAMEOFRANK,INS_JOBNAME_HDTGROUPNAME,INS_JOBNAME_ORGSTRUCTURENAME,INS_JOBNAME_JOBTITLE)
            *      Step3  :  ParseFormula cho cot FormulaJobNameIns trong ds chế độ lương
            *      Step4  :  Note: rieng phần tử INS_JOBNAME_JOBTITLE  (nếu khac honda sẽ chỉ su dụng phần tử này)
            */

            var listElementFormula = new List<ElementFormula>();
            var item = new ElementFormula();

            #region Lấy giá trị của các phần tử
            var hdtGroupName = string.Empty;
            var nameOfRank = string.Empty;
            var orgName = string.Empty;

            #region Name of rank
            var salaryClass = cat_SalaryClasses.Where(p => profile != null && profile.SalaryClassID.HasValue && p.ID == profile.SalaryClassID).FirstOrDefault();
            if (salaryClass != null)
            {
                nameOfRank = salaryClass.AbilityTitleVNI + " ";
            }
            #endregion

            #region HdtJobGroup
            var hdtJobGroup = hdtJobGroups.Where(p => hreHdtJobProfile != null && p.ID == hreHdtJobProfile.HDTJobGroupID).FirstOrDefault();
            hdtJobGroupCode = string.Empty;
            if (hdtJobGroup != null)
            {
                hdtGroupName = hdtJobGroup.HDTJobGroupName;
                hdtJobGroupCode = hdtJobGroup.Code;
            }
            #endregion

            #region OrgStructure Name (tên tiếng Việt)
            var org = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_DEPARTMENT, orgStructures, orgTypes);

            if (org != null)
            {
                orgName = org.OrgStructureNameEN;
            }
            #endregion

            #endregion

            #region Thiết lập phần tử lấy jobName
            //lấy jobName
            string fomular = (new InsuranceServices()).FomularReplace(gradePayroll.FormulaJobNameIns, listElementFormulaInDB);
            gradePayroll.FormulaJobNameIns = fomular;
            if (gradePayroll.FormulaJobNameIns != null)
            {
                if (gradePayroll.FormulaJobNameIns.Contains(InsuranceElement.INS_JOBNAME_HDTJOBMONEY.ToString()))
                {
                    item = new ElementFormula(InsuranceElement.INS_JOBNAME_HDTJOBMONEY.ToString(), hdtJobMoney, 0);
                    listElementFormula.Add(item);
                }
                if (gradePayroll.FormulaJobNameIns.Contains(InsuranceElement.INS_JOBNAME_NAMEOFRANK.ToString()))
                {
                    item = new ElementFormula(InsuranceElement.INS_JOBNAME_NAMEOFRANK.ToString(), nameOfRank, 0);
                    listElementFormula.Add(item);
                }
                if (gradePayroll.FormulaJobNameIns.Contains(InsuranceElement.INS_JOBNAME_HDTGROUPNAME.ToString()))
                {
                    item = new ElementFormula(InsuranceElement.INS_JOBNAME_HDTGROUPNAME.ToString(), hdtGroupName, 0);
                    listElementFormula.Add(item);
                }
                if (gradePayroll.FormulaJobNameIns.Contains(InsuranceElement.INS_JOBNAME_ORGSTRUCTURENAME.ToString()))
                {
                    item = new ElementFormula(InsuranceElement.INS_JOBNAME_ORGSTRUCTURENAME.ToString(), orgName, 0);
                    listElementFormula.Add(item);
                }
                if (gradePayroll.FormulaJobNameIns.Contains(InsuranceElement.INS_JOBNAME_JOBTITLE.ToString()))
                {
                    item = new ElementFormula(InsuranceElement.INS_JOBNAME_JOBTITLE.ToString(), jobTitleName, 0);
                    listElementFormula.Add(item);
                }

                result = FormulaHelper.ParseFormula(fomular.Replace("[", "").Replace("]", ""), listElementFormula.Distinct().ToList());
            }
            else
            {
                result = new FormulaHelper.FormulaHelperModel();
                result.Value = string.Empty;
            }
            #endregion
        }

        private static void GetAllowanceInsuranceByFomular(double salInsurance, DateTime monthCheck, List<Hre_HDTJob> hreHDTJobs, Att_CutOffDuration cutOffDuration, Hre_ProfileEntity profile, Cat_GradePayroll gradePayroll, out Hre_HDTJob hreHdtJobProfile, out FormulaHelper.FormulaHelperModel result, List<Cat_UnusualAllowanceCfg> lstUnusualAllowanceCfg, List<Sal_UnusualAllowance> lstUnusualAllowance, List<Cat_Element> listElementFormulaInDB, double hdt4Amount, double hdt5Amount)
        {
            string fomular = (new InsuranceServices()).FomularReplace(gradePayroll.FormulaSalaryIns, listElementFormulaInDB);

            List<ElementFormula> listElementFormula = new List<ElementFormula>();
            hreHdtJobProfile = hreHDTJobs.Where(m => m.ProfileID == profile.ID && m.Status == HDTJobStatus.E_APPROVE.ToString()).OrderBy(m => m.DateFrom).FirstOrDefault();

            #region Công Loại IV
            var listHDTJob_Type4 = hreHDTJobs.Where(m => m.Status == HDTJobStatus.E_APPROVE.ToString() && m.Type == EnumDropDown.HDTJobType.E_TYPE4.ToString() && m.ProfileID == profile.ID).OrderBy(m => m.DateFrom).ToList();
            double TotalDayHDTJob4 = 0;
            if (listHDTJob_Type4 != null && listHDTJob_Type4.Count > 0)
            {
                TotalDayHDTJob4 = profile.NumdayHDTJobTypeIV ?? 0;
            }

            //Số ngày công làm HDT Job Loại 4 (tháng N)
            var item = new ElementFormula();
            item = new ElementFormula(PayrollElement.ATT_WORKDAY_HDTJOB_4.ToString(), TotalDayHDTJob4, 0);
            listElementFormula.Add(item);
            #endregion

            #region Công Loại V
            var listHDTJob_Type5 = hreHDTJobs.Where(m => m.Status == HDTJobStatus.E_APPROVE.ToString() && m.Type == EnumDropDown.HDTJobType.E_TYPE5.ToString() && m.ProfileID == profile.ID).OrderBy(m => m.DateFrom).ToList();
            double TotalDayHDTJob5 = 0;
            if (listHDTJob_Type5 != null && listHDTJob_Type5.Count > 0)
            {
                TotalDayHDTJob5 = profile.NumdayHDTJobTypeV ?? 0;
            }

            #region set hre_hdtjob có có ngày làm hdtjob lớn hơn
            if (TotalDayHDTJob4 >= TotalDayHDTJob5)
            {
                hreHdtJobProfile = listHDTJob_Type4.FirstOrDefault();
            }
            else
            {
                hreHdtJobProfile = listHDTJob_Type5.FirstOrDefault();
            }
            #endregion

            //Số ngày công làm HDT Job Loại 5 (tháng N)
            item = new ElementFormula(PayrollElement.ATT_WORKDAY_HDTJOB_5.ToString(), TotalDayHDTJob5, 0);
            listElementFormula.Add(item);
            #endregion


            gradePayroll.FormulaSalaryIns = fomular;
            if (hreHdtJobProfile != null)
            {
                if (hreHdtJobProfile.DateFrom.HasValue)
                {
                    item = new ElementFormula(PayrollElement.HR_START_DATE_HDTJOB.ToString(), hreHdtJobProfile.DateFrom.Value, 0);
                    listElementFormula.Add(item);
                }
                if (hreHdtJobProfile.DateTo.HasValue)
                {
                    item = new ElementFormula(PayrollElement.HR_END_DATE_HDTJOB.ToString(), hreHdtJobProfile.DateTo.Value, 0);
                    listElementFormula.Add(item);
                }
                if (hreHdtJobProfile.DateTo.HasValue)
                {
                    item = new ElementFormula(PayrollElement.ATT_CUTOFFDURATION_MONTH.ToString(), monthCheck, 0);
                    listElementFormula.Add(item);
                }
            }
            if (gradePayroll.FormulaSalaryIns.Contains(PayrollElement.INS_SALARY_INSURANCE_ROOT.ToString()))
            {
                item = new ElementFormula(PayrollElement.INS_SALARY_INSURANCE_ROOT.ToString(), salInsurance, 0);
                listElementFormula.Add(item);
            }

            if (gradePayroll.FormulaSalaryIns.Contains(InsuranceElement.INS_JOBNAME_NUMDAYNONHDTJOB.ToString()))
            {
                item = new ElementFormula(InsuranceElement.INS_JOBNAME_NUMDAYNONHDTJOB.ToString(), profile.NumdayNonHDTJob, 0);
                listElementFormula.Add(item);
            }

            if (gradePayroll.FormulaSalaryIns.Contains(InsuranceElement.INS_HDT4_TIMELINE.ToString()))
            {
                item = new ElementFormula(InsuranceElement.INS_HDT4_TIMELINE.ToString(), hdt4Amount, 0);
                listElementFormula.Add(item);
            }
            if (gradePayroll.FormulaSalaryIns.Contains(InsuranceElement.INS_HDT5_TIMELINE.ToString()))
            {
                item = new ElementFormula(InsuranceElement.INS_HDT5_TIMELINE.ToString(), hdt5Amount, 0);
                listElementFormula.Add(item);
            }

            foreach (var Allowance in lstUnusualAllowanceCfg)
            {
                var unusualAllowance = lstUnusualAllowance.FirstOrDefault(m => m.ProfileID == profile.ID && m.UnusualEDTypeID == Allowance.ID);
                if (gradePayroll.FormulaSalaryIns.Contains(Allowance.Code) && unusualAllowance != null)
                {
                    item = new ElementFormula(Allowance.Code, unusualAllowance.Amount, 0);
                    listElementFormula.Add(item);
                }
            }

            //tinh tien HDTJob

            result = FormulaHelper.ParseFormula(fomular.Replace("[", "").Replace("]", ""), listElementFormula.Distinct().ToList());
        }

        #endregion

        #region Ngày nghỉ lớn hơn 14 ngày

        /// <summary> Set giá trị cho IsLeaveNonWorkday trong profile (cho nghỉ >=14 ngày) </summary>
        /// <param name="lstProfile">ds profile</param>
        /// <param name="beginMonth">1 tháng N</param>
        /// <param name="endMonth">31 tháng N</param>
        /// <param name="lstWorkday">Ds workday không quet thẻ theo NV trong khoảng [16 tháng N-1] đến [15 tháng N]</param>
        /// <param name="lstLeaveDay">lấy ngày nghỉ trong khoảng [16 tháng N-1] đến [15 tháng N] theo NV</param>
        /// <param name="lstLeavedayTypeRateZero">Lấy DS loại ngày nghỉ không trả lương (PaidRate ==0)</param>
        private void SetLeaveNonWorkdayByProfile(List<Hre_ProfileEntity> lstProfile, DateTime beginMonth, DateTime endMonth, List<CustomWorkDayEntity> lstWorkday, List<CustomLeavedayEntity> lstLeaveDay
            , List<Cat_LeaveDayType> lstLeavedayTypeRateZero, List<CustomInsuranceRecordEntity> lstInsuranceRecord, List<CustomRosterEntity> lstRoster, List<Cat_DayOff> dayOffMonthChecks)
        {
            const int Numday = 14;//Lay trong config
            List<Guid> lstProfileID = lstProfile.Select(m => m.ID).ToList();
            var leaveCount = 0;
            for (int i = 0; i < lstProfile.Count; i++)
            {
                var profile = lstProfile[i];
                Guid profileID = profile.ID;
                DateTime? profileDateQuit = null;
                var lstWorkdayByProfile = lstWorkday.Where(m => m.ProfileID == profileID).ToList();
                var lstLeavedayByProfile = lstLeaveDay.Where(m => m.ProfileID == profileID).ToList();
                var lstRosterByProfile = lstRoster.Where(m => m.ProfileID == profileID).ToList();
                var lstInsuranceRecordByProfile = lstInsuranceRecord.Where(m => m.ProfileID == profileID).ToList();
                if (profile != null)
                {
                    profileDateQuit = profile.DateQuit;
                }
                if (IsLeaveGreater14day(profileID, beginMonth, endMonth, lstWorkdayByProfile, Numday, lstLeavedayByProfile, lstLeavedayTypeRateZero, lstInsuranceRecordByProfile, lstRosterByProfile, profileDateQuit, dayOffMonthChecks, out leaveCount))
                {
                    profile.IsDecreaseWorkingDays = true;

                    if (leaveCount >= 14)
                    {
                        profile.IsHaveInsSocial = false;
                        profile.IsHaveInsHealth = false;
                        profile.IsHaveInsUnEmp = false;
                    }
                }
            }
        }


        /// <summary>Kiểm tra nghỉ không lương 14 ngày</summary>
        /// <param name="profile">nhan vien</param>
        /// <param name="beginMonth">1 tháng N</param>
        /// <param name="endMonth">31 tháng N</param>
        /// <param name="lstWorkday">ds workday theo profile</param>
        /// <param name="dayAmountLeave">so ngay nghi khong luong duoc tinh (14 ngay)</param>
        /// <param name="lstLeaveDayByProfile">ds ngay nghi cua nhan vien (thang [N-1] va thang N)</param>
        /// <param name="lstLeavedayType">ds loai ngày nghỉ không trả lương</param>
        /// <param name="lstInsuranceRecord"></param>
        /// <param name="lstRoster"></param>
        /// <param name="countLeave"></param>
        /// <returns></returns>
        private bool IsLeaveGreater14day(Guid profile, DateTime beginMonth, DateTime endMonth, List<CustomWorkDayEntity> lstWorkday, int dayAmountLeave, List<CustomLeavedayEntity> lstLeaveDayByProfile, List<Cat_LeaveDayType> lstLeavedayType, List<CustomInsuranceRecordEntity> lstInsuranceRecord, List<CustomRosterEntity> lstRoster, DateTime? profileDateQuit, List<Cat_DayOff> dayOffMonthChecks, out int countLeave)
        {
            /*
            *  Goal(Kiểm tra nghỉ không lương 14 ngày)
            *  Steps :
            *     Step1    :  Duyệt ngày từ 1->31 tháng N
            *     Step2    :     Nếu NV Có Ca Làm Việc vào ngày hôm đó            
            *     Step3    :     Thì đếm số ngày nghỉ (từng ngày) theo điều kiện sau (đếm đến 14 ngày thì dừng) :
            *     Step3.1  :       1/ NV Có chứng từ (bất kỳ chứng từ nào cũng là nghỉ)
            *     Step3.2  :       2/ NV Không quẹt thẻ In/Out
            *     Step3.3  :       3/ NV Nghỉ không lương            
            */


            countLeave = 0;

            //paidrate = 0 :ko tra luong           
            List<Guid> lstLeaveTypePaidZero = lstLeavedayType.Where(m => leaveDayInsuranceTypes.Contains(m.InsuranceType) || (m.PaidRate == 0)).Select(m => m.ID).ToList();
            //ngày nghi NV co tra luong 
            var lstLeavedayPaidZeroBetween = lstLeaveDayByProfile.Where(m => m.LeaveDayTypeID == null || !lstLeaveTypePaidZero.Contains(m.LeaveDayTypeID.Value)).ToList();

            //lay ngay nghi viec cua nv 


            for (DateTime dateCheck = beginMonth; dateCheck <= endMonth; dateCheck = dateCheck.AddDays(1))
            {
                bool isHaveRoster = false;
                var roster = lstRoster.Where(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck).FirstOrDefault();
                if (roster != null)
                {
                    switch (dateCheck.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            if (roster.MonShiftID != null)
                                isHaveRoster = true;
                            break;
                        case DayOfWeek.Tuesday:
                            if (roster.TueShiftID != null)
                                isHaveRoster = true;
                            break;
                        case DayOfWeek.Wednesday:
                            if (roster.WedShiftID != null)
                                isHaveRoster = true;
                            break;
                        case DayOfWeek.Thursday:
                            if (roster.ThuShiftID != null)
                                isHaveRoster = true;
                            break;
                        case DayOfWeek.Friday:
                            if (roster.FriShiftID != null)
                                isHaveRoster = true;
                            break;
                        case DayOfWeek.Saturday:
                            if (roster.SatShiftID != null)
                                isHaveRoster = true;
                            break;
                        case DayOfWeek.Sunday:
                            if (roster.SunShiftID != null)
                                isHaveRoster = true;
                            break;
                    }
                }

                var leaveday = lstLeaveDayByProfile.Where(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck).FirstOrDefault();
                var record = lstInsuranceRecord.Where(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck).FirstOrDefault();
                //dieu kien workday != null khi  co ca hoac co inout
                if (isHaveRoster)
                {
                    var workday = lstWorkday.Where(m => m.WorkDate == dateCheck).FirstOrDefault();
                    if (record != null
                        || (workday != null && leaveday == null && workday.InTime1 == null && workday.OutTime1 == null)
                        || (leaveday != null && lstLeaveTypePaidZero.Any(m => m == leaveday.LeaveDayTypeID)))
                    {
                        countLeave++;
                    }
                }
                if (profileDateQuit.HasValue && dateCheck >= profileDateQuit.Value)
                {
                    //khi NV nghi viec => dem nhung ngay nv ko quyet the (ko tinh ngay nghi vao ngay nghi le)
                    if (dayOffMonthChecks.Any())
	                {
                        if (dayOffMonthChecks.FirstOrDefault(m=>m.DateOff == dateCheck) == null)
                        {
                             countLeave++;
                        }
	                }
                    else
                    {
                        countLeave++;
                    }
                }

                if (countLeave >= dayAmountLeave)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #endregion

        #region common fucntion
        private string FomularReplace(string Input, List<Cat_Element> listElementFormula)
        {
            if (String.IsNullOrEmpty(Input))
            {
                return string.Empty;
            }
            List<string> ListFormula = ParseFormulaToList(Input).Where(m => m.IndexOf('[') != -1 && m.IndexOf(']') != -1).ToList();
            foreach (var fomular in ListFormula)
            {
                string fo = fomular.Replace("[", "").Replace("]", "");
                if (listElementFormula.Any(m => m.ElementCode == fo)) // Nó chưa phải là hằng số hoặc enum
                {
                    string ElementConvert = listElementFormula.Where(m => m.ElementCode == fo).Select(m => m.Formula).FirstOrDefault();
                    ElementConvert = FomularReplace(ElementConvert, listElementFormula);
                    Input = Input.Replace(fomular, ElementConvert);
                }
            }
            return Input;
        }
        #endregion

        #region other

        /// <summary>      
        /// Tinh Thang tham gia bao hiem.
        /// </summary>
        /// <param name="rProfile"></param>
        /// <param name="MonthYear"></param>
        /// <returns></returns>
        public static DateTime GetMonthJoinInsurance(Hre_Profile pro, DateTime monthYear)
        {

            List<String> lstContractCodeSI = new List<String>();
            List<Sal_Grade> lstgrade = pro.Sal_Grade.Where(gr => gr.IsDelete == null || gr.IsDelete == false).ToList();
            List<Hre_Contract> lstContract = pro.Hre_Contract.Where(gr => gr.IsDelete == null || gr.IsDelete == false).ToList();

            if (pro.DateHire == null)
            {
                return DateTime.MinValue;
            }
            DateTime _dateHire = pro.DateHire.Value;
            if (_dateHire.Day > PeriodInsuranceDayCurrentMonthDefault)
                _dateHire = new DateTime(_dateHire.AddMonths(1).Year
                                        , _dateHire.AddMonths(1).Month
                                        , PeriodInsuranceDayCurrentMonthDefault);
            else
                _dateHire = new DateTime(_dateHire.Year
                                        , _dateHire.Month
                                        , PeriodInsuranceDayCurrentMonthDefault);

            DateTime monthJoinIns = new DateTime();
            if (pro.SocialInsDateReg.HasValue)
                monthJoinIns = pro.SocialInsDateReg.Value;
            else if (pro.DateEndProbation.HasValue)
                monthJoinIns = pro.DateEndProbation.Value;
            else
                monthJoinIns = _dateHire;

            for (DateTime _dx = _dateHire; _dx < monthYear;
                _dx = new DateTime(_dx.AddMonths(1).Year,
                                   _dx.AddMonths(1).Month,
                                   PeriodInsuranceDayCurrentMonthDefault)
                )
            {
                DateTime dateMaxGrade = DateTime.MinValue;
                Cat_GradePayroll gradeCfg = new Cat_GradePayroll();
                lstgrade = lstgrade.OrderBy(gra => gra.udMonthOfEffect).ToList();
                if (lstgrade.Count > 0)
                {
                    //Truong hop ngay hieu luc cua grade dau tien lon hon ngay vao lam
                    if (lstgrade[0].udMonthOfEffect.HasValue && lstgrade[0].udMonthOfEffect.Value > _dx)
                    {
                        //<RedundancyByResharper> //No Redundancy Trung.LE
                        dateMaxGrade = lstgrade[0].udMonthOfEffect.Value;
                        //</RedundancyByResharper>

                        gradeCfg = lstgrade[0].Cat_GradePayroll;
                    }
                    else
                    {
                        foreach (Sal_Grade gr in lstgrade)
                        {
                            if (gr.udMonthOfEffect > dateMaxGrade && gr.udMonthOfEffect <= _dx)
                            {
                                dateMaxGrade = gr.udMonthOfEffect.Value;
                                gradeCfg = gr.Cat_GradePayroll;
                            }
                        }
                    }
                }

                if (!String.IsNullOrEmpty(gradeCfg.SIContract))
                {
                    lstContractCodeSI.AddRange(gradeCfg.SIContract.Split(new char[1] { ',' }));
                    if (lstContract.Exists(ctr => ctr.DateStart <= _dx
                                      && (ctr.DateEnd == null || ctr.DateEnd > _dx)
                                      && lstContractCodeSI.Contains(ctr.Cat_ContractType.Code)))
                    {
                        monthJoinIns = _dx;
                        break;
                    }
                }
            }

            return monthJoinIns;

        }

        /// <summary>
        /// Tinh so tien luong tham gia BHXH, va so tien duoc huong doi voi tung record
        /// </summary>
        /// <param name="record">Chung tu</param>
        /// <param name="AmountMaxIns">Muc tran dong BHXH</param>
        /// <param name="AmountMinumumIns">Muc luong toi thieu</param>
        /// <param name="monthJoin">Thang tham gia BHXH</param>
        /// <param name="listBasicSalary">Danh sach luong co ban</param>
        /// <param name="listRateInsurance">Danh sach ty gia BHXH</param>
        /// <param name="moneyIns">Tien BHXH phai tra</param>
        /// <param name="SalaryIns">Muc luong tinh BHXH</param>
        public static void GetMoneyInsSalaryIns(Ins_InsuranceRecord record, List<Cat_ValueEntity> lstAmountMaxIns
                                        , List<Cat_ValueEntity> lstAmountMinIns, DateTime monthJoin
                                        , List<Sal_BasicSalary> listBasicSalary, List<Cat_ExchangeRate> listRateInsurance, List<Sys_AllSetting> appConfig
                                        , out Double moneyIns, out Double SalaryIns, bool isCheckGetBirthChildForRestorationPregnancy)
        {
            Hre_Profile pro = record.Hre_Profile;
            String status = record.InsuranceType.ToString();
            Double leaveDayInMonth = record.DayCount;
            moneyIns = 0;
            SalaryIns = 0;
            //BHXH quy dinh ngay cong chuan la 26
            Double _workdaySta = 26;

            //Pregnancy
            if (status == InsuranceRecordType.E_PREGNANCY_EXAMINE.ToString()
                || status == InsuranceRecordType.E_PREGNANCY_LOST.ToString()
                || status == InsuranceRecordType.E_PREGNANCY_PREVENTION.ToString()
                || status == InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString())
            {
                DateTime dateStart = record.DateStart != null ? record.DateStart.Value : record.RecordDate;
                DateTime dateEnd = record.DateEnd != null ? record.DateEnd.Value : record.RecordDate;
                Double AmountMaxIns = GetMaxMinAmountOfMonth(dateStart, lstAmountMaxIns);
                //Lay muc luong cua 6 thang lien ke
                List<Sal_BasicSalary> listBasicSalaryPro = listBasicSalary.Where(sl => sl.ProfileID == pro.ID).ToList();
                SalaryIns = GetAmountInsuranceSixMonth(pro, dateStart, monthJoin, AmountMaxIns, listBasicSalaryPro, listRateInsurance, status);

                if (SalaryIns > AmountMaxIns)
                {
                    SalaryIns = AmountMaxIns;
                }
                //Lay so tien BHXH tra
                moneyIns = (SalaryIns / _workdaySta) * leaveDayInMonth;
                if (status == InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString())
                {


                    //TODO: cấu hình động chỗ này
                    //TH1-FOV: ngày tính bắt đầu từ ngày nghỉ
                    //TH2-FGL: ngày tính bắt đầu từ ngày sinh con
                    //AppConfig:PREGNANCY_AVERAGE_SALARY_FROM_CHILD_BORN

                    //DateSuckle: ngay sinh con, bo frame se co cau hinh phu hop voi FGL
                    List<Sys_AllSetting> appcf = appConfig.Where(prop => prop.Name == AppConfig.E_COLLECT_SOCIAL_INSURANCE_CONFIGFGL.ToString()).ToList();
                    if (appcf.Count > 0)
                    {
                        if (appcf[0].Value3 != null && (Boolean.Parse(appcf[0].Value1.ToString()) == true))
                        {
                            if (record.DateSuckle.HasValue)
                                dateStart = record.DateSuckle.Value;
                        }
                    }
                    //Lay muc tran tai thoi diem nghi thai san or sinh con
                    Double AmountMaxInsSuckle = GetMaxMinAmountOfMonth(dateStart, lstAmountMaxIns);
                    Double AmountMinumumInsSuckle = GetMaxMinAmountOfMonth(dateStart, lstAmountMinIns);

                    SalaryIns = GetAmountInsuranceSixMonth(pro, dateStart, monthJoin, AmountMaxInsSuckle, listBasicSalaryPro, listRateInsurance, status);

                    if (SalaryIns > AmountMaxInsSuckle)
                    {
                        SalaryIns = AmountMaxInsSuckle;
                    }

                    //Neu loai sinh doi thi duoc cong them 4 thang luong toi thieu
                    Double countTypeSuckle = 2;
                    if (!String.IsNullOrEmpty(record.TypeSuckle))
                    {
                        if (record.TypeSuckle == TypeSuckle.E_SUCKLE_TWINS.ToString())
                            countTypeSuckle = 4;
                    }
                    //Quy dinh BHXH FG: cong nhan nghi 5 thang, nhan vien nghi 4 thang. Fuji : tat ca deu nghi 4 thang, sinh doi nghi 5 thang
                    //Neu so ngay nho hon 180 ngay thi tinh 6 thang. else tinh 7 thang.
                    //20130405 - Ap dung luat moi Neu DateEnd > '01/05/2013' thi ap dung thai san 6 thang else 4 thang.
                    DateTime dateValue = new DateTime(2013, 05, 01);
                    if (dateEnd >= dateValue)
                    {
                        if (record.DayCount <= 190)
                            moneyIns = SalaryIns * 6 + AmountMinumumInsSuckle * countTypeSuckle;
                        else
                            moneyIns = SalaryIns * 7 + AmountMinumumInsSuckle * countTypeSuckle;
                    }
                    else
                    {
                        if (record.DayCount < 150)
                            moneyIns = SalaryIns * 4 + AmountMinumumInsSuckle * countTypeSuckle;
                        else
                            moneyIns = SalaryIns * 5 + AmountMinumumInsSuckle * countTypeSuckle;
                    }

                }
            }
            else if (status == InsuranceRecordType.E_SICK_SHORT.ToString()
                        || status == InsuranceRecordType.E_SICK_LONG.ToString()
                        || status == InsuranceRecordType.E_SICK_CHILD.ToString())
            {

                //Lay muc luong cua thang lien ke
                //--Lay thang lien ke 
                DateTime dateSalary = new DateTime();
                dateSalary = new DateTime(record.DateStart.Value.AddMonths(-1).Year
                                    , record.DateStart.Value.AddMonths(-1).Month
                                    ,PeriodInsuranceDayCurrentMonthDefault);
                List<Sal_BasicSalary> lstbs = listBasicSalary.Where(sa => sa.DateOfEffect <= dateSalary
                                                                    && sa.ProfileID == pro.ID)
                                                             .OrderByDescending(sa => sa.DateOfEffect).ToList();

                Double AmountMaxIns = GetMaxMinAmountOfMonth(dateSalary, lstAmountMaxIns);
                if (lstbs.Count > 0)
                {
                    Sal_BasicSalary basicSalary = lstbs[0];
                    if (basicSalary != null)
                    {
                        Double amountIns = basicSalary.InsuranceAmount;
                        Cat_Currency curIns = basicSalary.Cat_Currency1;
                        if (curIns != null && curIns.Code != CurrencyCode.VND.ToString())
                            amountIns = Sal_PayrollLib.ConvertExtractRateToVND(amountIns, curIns, listRateInsurance);
                        SalaryIns = amountIns;
                    }
                }

                if (SalaryIns > AmountMaxIns)
                {
                    SalaryIns = AmountMaxIns;
                }
                //Lay so tien BHXH tra
                //--Bao cao om dau huong 75%
                moneyIns = (SalaryIns / _workdaySta) * leaveDayInMonth * 0.75;
            }
            else if (status == InsuranceRecordType.E_RESTORATION_PREGNANCY.ToString() ||
                    status == InsuranceRecordType.E_RESTORATION_TNLD.ToString() ||
                    status == InsuranceRecordType.E_RESTORATION_SICK.ToString())
            {
                //Khong tinh luong tham gia BHXH
                SalaryIns = 0;
                DateTime dateStart = record.DateStart.Value;

                if (isCheckGetBirthChildForRestorationPregnancy && status == InsuranceRecordType.E_RESTORATION_PREGNANCY.ToString() && record.DateSuckle != null)
                {
                    dateStart = record.DateSuckle.Value;
                }
                Double AmountMinumumIns = GetMaxMinAmountOfMonth(dateStart, lstAmountMinIns);
                //Lay so tien BHXH toi thieu tra
                //--Bao cao duong suc huong 25% muc luong toi thieu voi thoi diem hien tai
                if (status == InsuranceRecordType.E_RESTORATION_SICK.ToString() && record.TypeData == InsuranceRecordType.E_LEAVE_AT_SAME_PLACE.ToString())
                {
                    moneyIns = AmountMinumumIns * 0.4 * leaveDayInMonth;
                }
                else
                {
                    moneyIns = AmountMinumumIns * 0.25 * leaveDayInMonth;
                }
            }
        }


        /// <summary>
        /// Lấy mức luong toi thieu - MIN Hoac muc tran dong BHXH - MAX gan thang hieu luc nhat.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="lstAmountMaxIns"></param>
        /// <returns></returns>
        public static Double GetMaxMinAmountOfMonth(DateTime month, List<Cat_ValueEntity> lstValueMaxOrMin)
        {
            Double value = 0;
            DateTime maxDateTime = DateTime.MinValue;
            foreach (Cat_ValueEntity item in lstValueMaxOrMin)
            {
                if (item.DateOfEffect == null)
                    continue;
                if (item.DateOfEffect.Value > maxDateTime && item.DateOfEffect.Value <= month)
                {
                    value = item.Value != null ? item.Value.Value : 0;
                    maxDateTime = item.DateOfEffect.Value;
                }
            }
            return value;
        }

        /// <summary>      
        /// Lay luong trung binh dong BH 6 thang gan nhat
        /// </summary>
        /// <param name="monthYear">ngay 15</param>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static Double GetAmountInsuranceSixMonth(Hre_Profile pro, DateTime monthYear, DateTime monthJoin, Double AmountMaxIns, List<Sal_BasicSalary> lstSal, List<Cat_ExchangeRate> listRateInsurance, String typeRecord)
        {
            Double _amount = 0;
            DateTime monthYearIns = new DateTime();
            int month = 0;
            //Lay thang lien ke
            //Nghi trong thang nao tinh thang do ko tinh ngay 15
            monthYearIns = new DateTime(monthYear.AddMonths(-1).Year
                                            , monthYear.AddMonths(-1).Month
                                            , PeriodInsuranceDayCurrentMonthDefault);

            if (typeRecord == InsuranceRecordType.E_PREGNANCY_EXAMINE.ToString()
            || typeRecord == InsuranceRecordType.E_PREGNANCY_LOST.ToString()
            || typeRecord == InsuranceRecordType.E_PREGNANCY_PREVENTION.ToString())
            {
                //LamLe- Update luat moi neu sinh con 1-15 thi tinh 6 thang lien ke - Neu sinh con 16-cuối tháng tinh 6 thang bat dau tu thang sinh con.
                // cac loaij lien tuan den thai san thi ap dung ngay 15
                if (monthYear.Day >= PeriodInsuranceDayCurrentMonthDefault)
                    monthYearIns = new DateTime(monthYear.Year
                                                , monthYear.Month
                                                , PeriodInsuranceDayCurrentMonthDefault);
            }
            if (typeRecord == InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString())
            {
                //LamLe- Update luat moi neu sinh con 1-15 thi tinh 6 thang lien ke - Neu sinh con 16-cuối tháng tinh 6 thang bat dau tu thang sinh con.
                //Loai nghi sanh thi ap  dung ngay 16
                if (monthYear.Day > PeriodInsuranceDayCurrentMonthDefault)
                    monthYearIns = new DateTime(monthYear.Year
                                                , monthYear.Month
                                                , PeriodInsuranceDayCurrentMonthDefault);
            }

            //Kiem tra thoi gian dong bao hiem co lon hon 6 thang.
            DateTime monthSix = new DateTime(monthYearIns.AddMonths(-6).Year
                                            , monthYearIns.AddMonths(-6).Month
                                            , PeriodInsuranceDayCurrentMonthDefault);
            monthJoin = new DateTime(monthJoin.Year
                                     , monthJoin.Month
                                            , PeriodInsuranceDayCurrentMonthDefault);
            if (monthSix.Date.CompareTo(monthJoin.Date) < 0)
            {
                for (DateTime dx = monthJoin; dx <= monthYearIns; dx = new DateTime(dx.AddMonths(1).Year
                                                                           , dx.AddMonths(1).Month
                                                                           , PeriodInsuranceDayCurrentMonthDefault))
                {
                    month += 1;
                }
            }
            else
                month = 6;
            //lstSal = lstSal.Where(sal => sal.DateOfEffect > minMonthYear && ).ToList();
            lstSal = lstSal.Where(sal => sal.DateOfEffect <= monthYearIns).OrderByDescending(pit => pit.DateOfEffect).ToList();
            for (int i = 0; i < month; i++)
            {
                DateTime dateMonth = new DateTime(monthYearIns.AddMonths(-i).Year
                                            , monthYearIns.AddMonths(-i).Month
                                            , PeriodInsuranceDayCurrentMonthDefault);
                DateTime dateMaxSal = DateTime.MinValue;
                Sal_BasicSalary salMonth = new Sal_BasicSalary();
                foreach (Sal_BasicSalary sal in lstSal)
                {
                    if (sal.DateOfEffect > dateMaxSal && sal.DateOfEffect <= dateMonth)
                    {
                        dateMaxSal = sal.DateOfEffect;
                        salMonth = sal;
                    }
                }
                Double amountIns = salMonth.InsuranceAmount;
                Cat_Currency curIns = salMonth.Cat_Currency1;
                if (curIns != null && curIns.Code != CurrencyCode.VND.ToString())
                    amountIns = Sal_PayrollLib.ConvertExtractRateToVND(amountIns, curIns, listRateInsurance);

                if (amountIns > AmountMaxIns)
                {
                    _amount += AmountMaxIns;
                }
                else
                {
                    _amount += amountIns;
                }

            }
            return _amount / month;

        }

        public static Double GetCountLeaveRecord(Ins_InsuranceRecord record)
        {
            Double leaveInMonth = record.DayCount;
            if (record.Status == InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString())
            {
                DateTime dateEnd = record.DateEnd != null ? record.DateEnd.Value : record.RecordDate;
                DateTime dateStart = record.DateStart != null ? record.DateStart.Value : record.RecordDate;
                DateTime dateValue = new DateTime(2013, 05, 01);
                if (dateStart >= dateValue)
                {
                    //Truong hop thai san neu nho hon 150 ngay lam tron thanh 4 thang 120 ngay
                    if (leaveInMonth <= 190)
                        leaveInMonth = 180;
                    else
                        leaveInMonth = 210;
                }
                else
                {
                    //Truong hop thai san neu nho hon 150 ngay lam tron thanh 4 thang 120 ngay
                    if (leaveInMonth < 150)
                        leaveInMonth = 120;
                    else
                        leaveInMonth = 150;
                }

            }
            return leaveInMonth;
        }

        /// <summary>
        /// Lấy mức luong toi thieu - MIN Hoac muc tran dong BHXH - MAX gan thang hieu luc nhat.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="lstAmountMaxIns"></param>
        /// <returns></returns>
        public static void GetMaxMinAmountOfMonth(DateTime month, String type, List<Cat_ValueEntity> lstValueMaxMin, out DateTime dateEffect, out double _salary)
        {
            String statusMinumunSal = ValueEntityType.E_MINIMUM_SALARY.ToString();
            List<Cat_ValueEntity> lstValueMaxOrMin = lstValueMaxMin.Where(val => val.Type == type).OrderByDescending(pit => pit.DateOfEffect).ToList();
            Double value = 0;
            DateTime maxDateTime = DateTime.MinValue;
            foreach (Cat_ValueEntity item in lstValueMaxOrMin)
            {
                if (item.DateOfEffect == null)
                    continue;
                if (item.DateOfEffect.Value > maxDateTime && item.DateOfEffect.Value <= month)
                {
                    value = item.Value != null ? item.Value.Value : 0;
                    maxDateTime = item.DateOfEffect.Value;
                }
            }
            _salary = value;
            dateEffect = maxDateTime;
        }

        public static double getMonneyBHTN(double MoneyMaxByRegion, double MoneyBHYT)
        {
            if (MoneyBHYT > MoneyMaxByRegion)
                return MoneyMaxByRegion;
            return MoneyBHYT;
        }


        #region HDTJob
        /// <summary>
        /// Hàm tính ra số ngày không làm HDTJob
        /// </summary>
        /// <param name="lstProfile">Ds Nhân Viên</param>
        /// <param name="LeaveDayTypeCode">các mã ngày nghỉ không tính HDJob </param>
        /// <param name="MonthCheck">tháng kiểm tra</param>
        public static void GetHDTJobDayByProfile(List<Hre_ProfileEntity> lstProfile, string LeaveDayTypeCode, DateTime MonthCheck)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHre_HDTJob = new Hre_HDTJobRepository(unitOfWork);
                var repoAtt_LeaveDay = new Att_LeavedayRepository(unitOfWork);
                var repoAtt_Roster = new Att_RosterRepository(unitOfWork);


                DateTime DateStart = new DateTime(MonthCheck.Year, MonthCheck.Month, 1);
                DateTime DateEnd = DateStart.AddMonths(1).AddSeconds(-1);
                List<string> lstLeaveTypeCode = LeaveDayTypeCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //=======Get DB
                string E_APPROVE = HDTJobStatus.E_APPROVE.ToString();
                string E_Four = EnumDropDown.HDTJobType.E_TYPE4.ToString();
                string E_Five = EnumDropDown.HDTJobType.E_TYPE5.ToString();
                var lstHDTJob = repoHre_HDTJob.FindBy(m => m.IsDelete == null && m.Status == E_APPROVE && m.DateFrom <= DateEnd && m.DateTo >= DateStart).Select(m => new { m.ProfileID, m.DateFrom, m.DateTo, m.Type }).ToList();

                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                //=======Get DB
                var lstLeaveDayHaveCode = repoAtt_LeaveDay.FindBy(m => m.IsDelete == null && m.Status == E_APPROVE && m.DateStart <= DateEnd && m.DateEnd >= DateStart && m.LeaveDayTypeID != null
                    && lstLeaveTypeCode.Contains(m.Cat_LeaveDayType.Code)).Select(m => new { m.ProfileID, m.DateStart, m.DateEnd }).ToList();

                //=======Get DB
                var lstRoster = repoAtt_Roster.FindBy(m => m.IsDelete == null && m.Status == E_APPROVED && m.DateStart <= DateEnd && m.DateEnd >= DateStart).Select(m => new { m.ProfileID, m.DateStart, m.DateEnd, m.MonShiftID, m.TueShiftID, m.WedShiftID, m.ThuShiftID, m.FriShiftID, m.SatShiftID, m.SunShiftID });
                foreach (var profile in lstProfile)
                {
                    var lstHDTJobByProfile = lstHDTJob.Where(m => m.ProfileID == profile.ID).ToList();
                    var lstLeaveDayHaveCodeByProfile = lstLeaveDayHaveCode.Where(m => m.ProfileID == profile.ID).ToList();
                    var lstRosterByProfile = lstRoster.Where(m => m.ProfileID == profile.ID).ToList();
                    int Numday_Non_HDTJob = 0;
                    int Numday_HDTJob_4 = 0;
                    int Numday_HDTJob_5 = 0;

                    bool isHaveHDTJob = lstHDTJob.Any(m => m.ProfileID == profile.ID);

                    if (isHaveHDTJob)
                    {
                        for (DateTime dateCheck = DateStart; dateCheck <= DateEnd; dateCheck = dateCheck.AddDays(1))
                        {
                            bool isHaveRoster = false;
                            switch (dateCheck.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.MonShiftID != null))
                                    {
                                        isHaveRoster = true;
                                    }
                                    break;
                                case DayOfWeek.Tuesday:
                                    if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.TueShiftID != null))
                                    {
                                        isHaveRoster = true;
                                    }
                                    break;
                                case DayOfWeek.Wednesday:
                                    if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.WedShiftID != null))
                                    {
                                        isHaveRoster = true;
                                    }
                                    break;
                                case DayOfWeek.Thursday:
                                    if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.ThuShiftID != null))
                                    {
                                        isHaveRoster = true;
                                    }
                                    break;
                                case DayOfWeek.Friday:
                                    if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.FriShiftID != null))
                                    {
                                        isHaveRoster = true;
                                    }
                                    break;
                                case DayOfWeek.Saturday:
                                    if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.SatShiftID != null))
                                    {
                                        isHaveRoster = true;
                                    }
                                    break;
                                case DayOfWeek.Sunday:
                                    if (lstRosterByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.SunShiftID != null))
                                    {
                                        isHaveRoster = true;
                                    }
                                    break;
                            }
                            if (isHaveRoster)
                            {
                                //thỏa 2 điều kiên 1: nam ngoài cung hdt job Hoặc là có ngày nghỉ trong loại kia
                                if (lstLeaveDayHaveCodeByProfile.Any(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck) || !lstHDTJobByProfile.Any(m => m.DateFrom <= dateCheck && m.DateTo >= dateCheck))
                                {
                                    Numday_Non_HDTJob++;
                                }

                                if (lstHDTJobByProfile.Any(m => m.Type == E_Four && m.DateFrom <= dateCheck && m.DateTo >= dateCheck))
                                {
                                    Numday_HDTJob_4++;
                                }
                                if (lstHDTJobByProfile.Any(m => m.Type == E_Five && m.DateFrom <= dateCheck && m.DateTo >= dateCheck))
                                {
                                    Numday_HDTJob_5++;
                                }
                            }
                        }
                        profile.NumdayNonHDTJob = Numday_Non_HDTJob;
                        profile.NumdayHDTJobTypeIV = Numday_HDTJob_4;
                        profile.NumdayHDTJobTypeV = Numday_HDTJob_5;
                    }
                    else
                    {
                        profile.NumdayNonHDTJob = int.MaxValue;
                        profile.NumdayHDTJobTypeIV = 0;
                        profile.NumdayHDTJobTypeV = 0;
                    }

                }
            }//close using
        }

        #endregion

        #region Quyet Toan Bao Hiem
        /// <summary>
        /// Quyết Toán Bảo Hiểm
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public bool InsuranceSettlementSelected(List<Guid> selectedIds)
        {
            var isSuccess = false;
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHreProfile = new Hre_ProfileRepository(unitOfWork);

                var profiles = repoHreProfile.FindBy(p => selectedIds.Contains(p.ID)).ToList();
                var lstProfileModify = new List<Hre_Profile>();

                foreach (var profile in profiles)
                {
                    if (profile.IsSettlement == null || profile.IsSettlement.Value == false)
                    {
                        profile.IsSettlement = true;
                        if (profile.Settlement.HasValue)
                        {
                            profile.Settlement++;
                        }
                        else
                        {
                            profile.Settlement = 1;
                        }
                        profile.MonnthSettlement = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        lstProfileModify.Add(profile);
                    }
                }
                if (lstProfileModify.Any())
                {
                    repoHreProfile.Edit(lstProfileModify);
                    var resultStatus = repoHreProfile.SaveChanges();
                    if (resultStatus == DataErrorCode.Success)
                    {
                        isSuccess = true;
                    }
                }
            }
            return isSuccess;
        }

        public bool InsuranceReceiveSocialInsSelected(List<Guid> selectedIds)
        {
            var isSuccess = false;
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHreProfile = new Hre_ProfileRepository(unitOfWork);

                var profiles = repoHreProfile.FindBy(p => selectedIds.Contains(p.ID)).ToList();
                var lstProfileModify = new List<Hre_Profile>();

                foreach (var profile in profiles)
                {
                    if (profile.IsSettlement.HasValue && profile.IsSettlement.Value == true)
                    {
                        profile.ReceiveSocialIns = true;
                        lstProfileModify.Add(profile);
                    }
                }
                if (lstProfileModify.Any())
                {
                    repoHreProfile.Edit(lstProfileModify);
                    var resultStatus = repoHreProfile.SaveChanges();
                    if (resultStatus == DataErrorCode.Success)
                    {
                        isSuccess = true;
                    }
                }
            }
            return isSuccess;
        }
        #endregion

        #endregion

        #region code BC
        public DataTable BC_INSURANCETOTAL(DateTime DateStart, DateTime DateEnd, List<Hre_Profile> lstProfiles)
        {

            lstProfiles = lstProfiles.Where(m => m.RegionID != null).ToList();
            List<Guid> lstProfileID = lstProfiles.Select(m => m.ID).ToList();
            //todo: Cần phải where thêm điều kiện là lấy 1 trong 2 loại  (có một loại là chính và một loại là tạm - 2 loại này dùng để ra báo cáo chênh lệch)
            //Ps. Hàm bên dưới thiếu điều kiện where đó.
            List<Ins_ProfileInsuranceMonthly> lstProfileInsuranceMonthly = new List<Ins_ProfileInsuranceMonthly>().Where(m => m.ProfileID != null && m.MonthYear >= DateStart && m.MonthYear <= DateEnd).ToList();
            lstProfileInsuranceMonthly = lstProfileInsuranceMonthly.Where(m => lstProfileID.Contains(m.ProfileID.Value)).ToList();
            List<Guid> RegionIDs = lstProfiles.Where(m => m.RegionID != null).Select(m => m.RegionID.Value).Distinct().ToList();
            List<Cat_Region> lstRegion = new List<Cat_Region>().Where(m => RegionIDs.Contains(m.ID)).ToList();
            //Sắp xếp thứ tự của region
            List<OrderRegion> lstRegionOrder = new List<OrderRegion>();
            foreach (var item in lstRegion)
            {
                double NumPerson = lstProfiles.Where(m => m.RegionID == item.ID).Count();
                lstRegionOrder.Add(new OrderRegion() { ID = item.ID, RegionName = item.RegionName, Num = NumPerson });
            }
            lstRegionOrder = lstRegionOrder.OrderByDescending(m => m.Num).ToList();


            DataTable dt = new DataTable();

            int stt = 0;
            for (DateTime DateCheck = DateStart; DateCheck < DateEnd; DateCheck = DateCheck.AddMonths(1))
            {
                stt++;
                DataRow dr = dt.NewRow();
                var lstProfileInsuranceMonthly_ByMonth = lstProfileInsuranceMonthly.Where(m => m.MonthYear == DateCheck).ToList();
                dr["Stt"] = stt;
                dr["Name"] = DateCheck.ToString("dd/MM/yyyy");
                double TotalEmp = lstProfileInsuranceMonthly_ByMonth.Select(m => m.ProfileID).Distinct().ToList().Count;
                dr["TotalEmp"] = TotalEmp;
                dr["BHXH"] = lstProfileInsuranceMonthly_ByMonth.Sum(m => m.SocialInsComAmount + m.SocialInsEmpAmount);
                dr["BHYT"] = lstProfileInsuranceMonthly_ByMonth.Sum(m => m.HealthInsComAmount + m.HealthInsEmpAmount);
                dr["BHTN"] = lstProfileInsuranceMonthly_ByMonth.Sum(m => m.UnemployComAmount + m.UnemployEmpAmount);
                dt.Rows.Add(dr);
                foreach (var item in lstRegionOrder)
                {
                    List<Guid> lstProfileIDsByRegion = lstProfiles.Where(m => m.RegionID == item.ID).Select(m => m.ID).Distinct().ToList();
                    var lstProfileInsuranceMonthly_ByMonth_ByProfile = lstProfileInsuranceMonthly_ByMonth.Where(m => m.ProfileID != null && lstProfileIDsByRegion.Contains(m.ProfileID.Value)).ToList();
                    dr["Name"] = item.RegionName;
                    dr["TotalEmp"] = lstProfileInsuranceMonthly_ByMonth_ByProfile.Select(m => m.ProfileID).Distinct().ToList().Count;
                    dr["BHXH"] = lstProfileInsuranceMonthly_ByMonth_ByProfile.Sum(m => m.SocialInsComAmount + m.SocialInsEmpAmount);
                    dr["BHYT"] = lstProfileInsuranceMonthly_ByMonth_ByProfile.Sum(m => m.HealthInsComAmount + m.HealthInsEmpAmount);
                    dr["BHTN"] = lstProfileInsuranceMonthly_ByMonth_ByProfile.Sum(m => m.UnemployComAmount + m.UnemployEmpAmount);
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }


        /// <summary>
        /// hàm tính ra có thai sản ở tháng cần kiểm tra hay không
        /// </summary>
        /// <param name="MonthYear">tháng cần kiểm tra</param>
        /// <param name="lstLeaveDayPregByProfile">danh sách nghỉ loại thai sản và của nhân viên đang kiếm</param>
        /// <param name="lstInsuranceRecordByProfile">danh sách chứng từ thai sản của nhân viên đang tìm kiếm</param>
        /// <returns></returns>
        public class OrderRegion
        {
            public Guid ID { get; set; }
            public string RegionName { get; set; }
            public double Num { get; set; }
        }

        #endregion

        #region Truy Lĩnh Bảo Hiểm
        public List<Ins_TypeD02MultiEntity> GetTypeD02TypeNameList(string typeCode)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var typeD02s = unitOfWork.CreateQueryable<Ins_TypeD02>(Guid.Empty, m => (typeCode == "" || typeCode == null) || m.TypeCode == typeCode).Select(m => new Ins_TypeD02MultiEntity
                {
                    //ID =m.ID,
                    Code = m.TypeCode,
                    Name = m.TypeName
                }).Distinct().ToList();
                var result = new List<Ins_TypeD02MultiEntity>();
                foreach (var item in typeD02s)
                {
                    if (!result.Any(m=>m.Code==item.Code.TrimAll()))
                    {
                        var typeD02 = new Ins_TypeD02MultiEntity {
                            Code = item.Code.Trim(),
                            Name=item.Name.Trim()
                        };
                        result.Add(typeD02);
                    }
                }
                return result;
            }
        }

        public List<Ins_TypeD02MultiEntity> GetTypeD02StatusNameList(string typeCode)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var typeD02s = unitOfWork.CreateQueryable<Ins_TypeD02>(Guid.Empty, m => (typeCode == "" || typeCode == null) || m.TypeCode == typeCode).Select(m => new Ins_TypeD02MultiEntity
                {
                    //ID = m.ID,
                    Code = m.StatusCode,
                    Name = m.StatusName
                }).Distinct().ToList();
                var result = new List<Ins_TypeD02MultiEntity>();
                foreach (var item in typeD02s)
                {
                    if (!result.Any(m => m.Code == item.Code.TrimAll()))
                    {
                        var typeD02 = new Ins_TypeD02MultiEntity
                        {
                            Code = item.Code.Trim(),
                            Name = item.Name.Trim()
                        };
                        result.Add(typeD02);
                    }
                }
                return result;
            }
        }

        public List<Ins_TypeD02MultiEntity> GetTypeD02CommentList(string statusCode)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var typeD02s = unitOfWork.CreateQueryable<Ins_TypeD02>(Guid.Empty, m => (statusCode == "" || statusCode == null) || m.StatusCode == statusCode).Select(m => new Ins_TypeD02MultiEntity
                {
                    ID = m.ID,
                    Code = m.CommentCode,
                    Name = m.Comment
                }).Distinct().ToList();
                return typeD02s;
            }
        }

        public void CalculateInsurancePayBack(List<Guid> payBackIds, DateTime monthYear,string userLogin)
        {
            /*
            *  Goal(tính toán Truy Lĩnh Bảo Hiểm - Lưu bảng Ins_ProfileInsuranceMonthly)
            *  Steps :
            *      Step1  :  lấy ds InsuranceSalaryPayback
            *      Step2  :  Duyệt InsuranceSalaryPayback 
            *      Step3  :  Duyệt fromMonthEffect đến ToMonthEffect (để insert vào Ins_ProfileMonthly tương ứng với những tháng đó)
            *      Step3  :  Lưu vào DB
            */


            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoInsMonthly = new Ins_ProfileInsuranceMonthlyRepository(unitOfWork);
                var repoInsReportD02 = new Ins_ReportD02Repository(unitOfWork);
                var repoInsReportD02Item = new Ins_ReportD02ItemRepository(unitOfWork);
                var repoInsuranceSalaryPayback = new Ins_InsuranceSalaryPaybackRepository(unitOfWork);
                string status = string.Empty;
                DateTime endDayOfMonthYear = new DateTime(monthYear.Year, monthYear.Month, DateTime.DaysInMonth(monthYear.Year, monthYear.Month));
                var reportD02 = new Ins_ReportD02();
                List<Ins_ProfileInsuranceMonthly> lstProfileMonthly = new List<Ins_ProfileInsuranceMonthly>();
                List<Ins_ProfileInsuranceMonthly> lstProfileMonthlyEditing = new List<Ins_ProfileInsuranceMonthly>();
                List<Ins_ReportD02Item> lstD02Item = new List<Ins_ReportD02Item>();
                List<Ins_ReportD02Item> lstD02ItemEditing = new List<Ins_ReportD02Item>();
                var insPayBacks = unitOfWork.CreateQueryable<Ins_InsuranceSalaryPayback>(Guid.Empty, m => payBackIds.Contains(m.ID)).ToList();
                var insTypeD02Ids = insPayBacks.Select(m => m.TypeID).ToList();
                var insTypeD02s = unitOfWork.CreateQueryable<Ins_TypeD02>(Guid.Empty, m => insTypeD02Ids.Contains(m.ID)).ToList();
                //lấy tỉ lệ bảo hiểm với ngày áp dụng trước ngày kiểm tra
                var lstInsRate = unitOfWork.CreateQueryable<Cat_RateInsurance>(Guid.Empty, m => m.ApplyFrom <= endDayOfMonthYear).ToList();
                Cat_RateInsurance rateInsurance = GetInsuranceRate(endDayOfMonthYear, lstInsRate);
                var insMonthYearFrom = monthYear.AddMonths(-1);
                insMonthYearFrom = new DateTime(insMonthYearFrom.Year, insMonthYearFrom.Month,PeriodInsuranceDayPreMonthDefault);
                var insMonthYearTo = new DateTime(monthYear.Year, monthYear.Month,PeriodInsuranceDayCurrentMonthDefault );

                var beginMonthYear = insPayBacks.Min(m => m.FromMonthEffect);
                var endMonthYear = insPayBacks.Max(m => m.ToMonthEffect);

                #region ds NV đóng Bảo Hiểm theo thang
                var listInsMonthlyObj = new List<object> { null, beginMonthYear, endMonthYear, null, null, null, null, null, 1, int.MaxValue - 1 };
                if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                {
                    listInsMonthlyObj.Add("id");
                }
                var lstProfileInsuranceMonthlyInDb = GetData<Ins_ProfileInsuranceMonthlyEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthlyFromTo,userLogin, ref status).Translate<Ins_ProfileInsuranceMonthly>(); ;
                var profileIds = insPayBacks.Select(m => m.ProfileID).ToList();
                var hreProfiles = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty, m => m.SocialInsPlaceID.HasValue && profileIds.Contains(m.ID)).Select(m => new { ID = m.ID, SocialInsPlaceID = m.SocialInsPlaceID.Value }).ToList();

                if (profileIds.Any())
                {
                    lstProfileInsuranceMonthlyInDb = lstProfileInsuranceMonthlyInDb.Where(m => profileIds.Contains(m.ProfileID)).ToList();
                }

                #endregion

                foreach (var payBack in insPayBacks)
                {
                    if (payBack.FromMonthEffect.HasValue && payBack.ToMonthEffect.HasValue)
                    {
                        payBack.IsCallPayBack = true;
                        //duyet tung nv
                        var profileID = payBack.ProfileID;

                        #region xoa truoc khi tinh toan truy lĩnh
                        var profMonthlyDelete = unitOfWork.CreateQueryable<Ins_ProfileInsuranceMonthly>(Guid.Empty, m => m.ProfileID == profileID
                            && m.MonthYear == payBack.MonthYear && (m.IsPayback == null || (m.IsPayback.HasValue && m.IsPayback.Value ==false)) ).FirstOrDefault();
                        if (profMonthlyDelete != null)
                        {
                            unitOfWork.RemoveObject(profMonthlyDelete);
                            unitOfWork.SaveChanges();
                        }

                        #endregion

                        #region lấy nơi đóng bảo hiểm của NV
                        var prof = hreProfiles.Where(m => m.ID == profileID).FirstOrDefault();
                        Guid? socialInsPlaceID = null;
                        if (prof != null)
                        {
                            socialInsPlaceID = prof.SocialInsPlaceID;
                        }
                        #endregion

                        for (var i = payBack.FromMonthEffect.Value; i <= payBack.ToMonthEffect.Value; i = i.AddMonths(1))
                        {
                            #region Ins_ProfileInsuranceMonthly
                            #region edit
                            var monthlyExist = lstProfileInsuranceMonthlyInDb.Where(m => m.ProfileID == profileID && m.MonthYearEffect == i && m.IsPayback == true).FirstOrDefault();
                            if (monthlyExist != null)
                            {
                                monthlyExist.IsSocialInsurance = payBack.IsSocialIns;
                                monthlyExist.IsHealthInsurance = payBack.IsMedicalIns;
                                monthlyExist.IsUnEmpInsurance = payBack.IsUnemploymentIns;
                                monthlyExist.MonthYear = payBack.MonthYear;
                                monthlyExist.MonthYearEffect = i;
                                monthlyExist.Allowance1 = 0;
                                monthlyExist.Allowance2 = 0;
                                monthlyExist.Allowance3 = 0;
                                monthlyExist.Allowance4 = 0;
                                monthlyExist.AllowanceAdditional = 0;

                                monthlyExist.JobName = payBack.JobtitleName;
                                monthlyExist.AmountHDTIns = payBack.AmoutHDTInsPayBack - payBack.AmoutHDTIns;
                                monthlyExist.PaybackID = payBack.ID;
                                monthlyExist.SalaryInsurance = payBack.InsSalaryAdjust;
                                monthlyExist.SalaryHealthInsurance = payBack.InsSalaryAdjust;
                                monthlyExist.SalaryUnEmpInsurance = payBack.InsSalaryAdjust;
                                monthlyExist.SocialInsPlaceID = socialInsPlaceID;
                                if (rateInsurance != null)
                                {
                                    monthlyExist.SocialInsComRate = rateInsurance.SocialInsCompRate;
                                    monthlyExist.SocialInsEmpRate = rateInsurance.SocialInsEmpRate;
                                    monthlyExist.HealthInsComRate = rateInsurance.HealthInsCompRate;
                                    monthlyExist.HealthInsEmpRate = rateInsurance.HealthInsEmpRate;
                                    monthlyExist.UnemployComRate = rateInsurance.UnemployInsCompRate;
                                    monthlyExist.UnemployEmpRate = rateInsurance.UnemployInsEmpRate;
                                    var moneyInsuranceTotal = payBack.InsSalaryAdjust ?? 0 + payBack.AmoutHDTIns ?? 0;
                                    #region Set tỉ lệ và tiền đóng BHXH,BHYT,BHTN

                                    if (monthlyExist.IsSocialInsurance == true)
                                    {
                                        //tong ti le BHXH do NSDLĐ và NLĐ đóng
                                        double rate = (rateInsurance.SocialInsCompRate + rateInsurance.SocialInsEmpRate);
                                        monthlyExist.MoneySocialInsurance = (float)(moneyInsuranceTotal * rate);

                                        #region [Tung.Ly ]: ti le BHXH cty dong va ti le BHXH nv dong
                                        //Số tiền BHXH do cty đóng
                                        monthlyExist.SocialInsComAmount = (double)(moneyInsuranceTotal * monthlyExist.SocialInsComRate);
                                        //số tiền BHXH do NV đóng
                                        monthlyExist.SocialInsEmpAmount = (double)(moneyInsuranceTotal * monthlyExist.SocialInsEmpRate);
                                        #endregion
                                    }
                                    if (monthlyExist.IsHealthInsurance == true)
                                    {
                                        //tong ti le BHYT do NSDLĐ và NLĐ đóng
                                        double rate = (rateInsurance.HealthInsCompRate + rateInsurance.HealthInsEmpRate);
                                        monthlyExist.MoneyHealthInsurance = (float)(moneyInsuranceTotal * rate);

                                        #region [Tung.Ly ]: ti le BHXH cty dong va ti le BHXH nv dong
                                        //Số tiền BHYT do cty đóng
                                        monthlyExist.HealthInsComAmount = (double)(moneyInsuranceTotal * monthlyExist.HealthInsComRate);
                                        //số tiền BHYT do NV đóng
                                        monthlyExist.HealthInsEmpAmount = (double)(moneyInsuranceTotal * monthlyExist.HealthInsEmpRate);
                                        #endregion
                                    }
                                    if (monthlyExist.IsUnEmpInsurance == true)
                                    {
                                        //tong ti le BHTN do NSDLĐ và NLĐ đóng
                                        double rate = (rateInsurance.UnemployInsCompRate + rateInsurance.UnemployInsEmpRate);
                                        monthlyExist.MoneyUnEmpInsurance = (float)(moneyInsuranceTotal * rate);

                                        #region [Tung.Ly ]: ti le BHTN cty dong va ti le BHXH nv dong
                                        //Số tiền BHTN do cty đóng
                                        monthlyExist.UnemployComAmount = (double)(moneyInsuranceTotal * monthlyExist.UnemployComRate);
                                        //số tiền BHTN do NV đóng
                                        monthlyExist.UnemployEmpAmount = (double)(moneyInsuranceTotal * monthlyExist.UnemployEmpRate);
                                        #endregion
                                    }
                                    monthlyExist.AmountChargeIns = monthlyExist.Allowance1 + monthlyExist.Allowance2 +
                                        monthlyExist.Allowance3 + monthlyExist.Allowance4 + moneyInsuranceTotal;
                                    #endregion
                                }
                                lstProfileMonthlyEditing.Add(monthlyExist);
                            }
                            #endregion

                            #region them moi
                            else
                            {
                                Ins_ProfileInsuranceMonthly proMonthly = new Ins_ProfileInsuranceMonthly();
                                proMonthly.MonthYear = payBack.MonthYear;
                                proMonthly.ProfileID = payBack.ProfileID;
                                proMonthly.MonthYearEffect = i;
                                proMonthly.IsPayback = true;
                                proMonthly.IsSocialInsurance = payBack.IsSocialIns;
                                proMonthly.IsHealthInsurance = payBack.IsMedicalIns;
                                proMonthly.IsUnEmpInsurance = payBack.IsUnemploymentIns;
                                proMonthly.SalaryInsurance = payBack.InsSalaryAdjust;
                                proMonthly.SalaryHealthInsurance = payBack.InsSalaryAdjust;
                                proMonthly.SalaryUnEmpInsurance = payBack.InsSalaryAdjust;
                                proMonthly.JobName = payBack.JobtitleName;
                                proMonthly.AmountHDTIns = payBack.AmoutHDTInsPayBack - payBack.AmoutHDTIns;
                                proMonthly.PaybackID = payBack.ID;
                                proMonthly.SocialInsPlaceID = socialInsPlaceID;
                                if (rateInsurance != null)
                                {
                                    proMonthly.SocialInsComRate = rateInsurance.SocialInsCompRate;
                                    proMonthly.SocialInsEmpRate = rateInsurance.SocialInsEmpRate;
                                    proMonthly.HealthInsComRate = rateInsurance.HealthInsCompRate;
                                    proMonthly.HealthInsEmpRate = rateInsurance.HealthInsEmpRate;
                                    proMonthly.UnemployComRate = rateInsurance.UnemployInsCompRate;
                                    proMonthly.UnemployEmpRate = rateInsurance.UnemployInsEmpRate;

                                    var moneyInsuranceTotal = (payBack.InsSalaryAdjust ?? 0) + (payBack.AmoutHDTIns ?? 0);

                                    #region Set tỉ lệ và tiền đóng BHXH,BHYT,BHTN

                                    if (proMonthly.IsSocialInsurance == true)
                                    {
                                        //tong ti le BHXH do NSDLĐ và NLĐ đóng
                                        double rate = (rateInsurance.SocialInsCompRate + rateInsurance.SocialInsEmpRate);
                                        proMonthly.MoneySocialInsurance = (float)(moneyInsuranceTotal * rate);

                                        #region [Tung.Ly ]: ti le BHXH cty dong va ti le BHXH nv dong
                                        //Số tiền BHXH do cty đóng
                                        proMonthly.SocialInsComAmount = (double)(moneyInsuranceTotal * proMonthly.SocialInsComRate);
                                        //số tiền BHXH do NV đóng
                                        proMonthly.SocialInsEmpAmount = (double)(moneyInsuranceTotal * proMonthly.SocialInsEmpRate);
                                        #endregion
                                    }
                                    if (proMonthly.IsHealthInsurance == true)
                                    {
                                        //tong ti le BHYT do NSDLĐ và NLĐ đóng
                                        double rate = (rateInsurance.HealthInsCompRate + rateInsurance.HealthInsEmpRate);
                                        proMonthly.MoneyHealthInsurance = (float)(moneyInsuranceTotal * rate);

                                        #region [Tung.Ly ]: ti le BHXH cty dong va ti le BHXH nv dong
                                        //Số tiền BHYT do cty đóng
                                        proMonthly.HealthInsComAmount = (double)(moneyInsuranceTotal * proMonthly.HealthInsComRate);
                                        //số tiền BHYT do NV đóng
                                        proMonthly.HealthInsEmpAmount = (double)(moneyInsuranceTotal * proMonthly.HealthInsEmpRate);
                                        #endregion
                                    }
                                    if (proMonthly.IsUnEmpInsurance == true)
                                    {
                                        //tong ti le BHTN do NSDLĐ và NLĐ đóng
                                        double rate = (rateInsurance.UnemployInsCompRate + rateInsurance.UnemployInsEmpRate);
                                        proMonthly.MoneyUnEmpInsurance = (float)(moneyInsuranceTotal * rate);

                                        #region [Tung.Ly ]: ti le BHTN cty dong va ti le BHXH nv dong
                                        //Số tiền BHTN do cty đóng
                                        proMonthly.UnemployComAmount = (double)(moneyInsuranceTotal * proMonthly.UnemployComRate);
                                        //số tiền BHTN do NV đóng
                                        proMonthly.UnemployEmpAmount = (double)(moneyInsuranceTotal * proMonthly.UnemployEmpRate);
                                        #endregion
                                    }
                                    proMonthly.AmountChargeIns = proMonthly.Allowance1 + proMonthly.Allowance2 +
                                        proMonthly.Allowance3 + proMonthly.Allowance4 + moneyInsuranceTotal;
                                    #endregion

                                }
                                proMonthly.ID = Guid.NewGuid();
                                lstProfileMonthly.Add(proMonthly);
                            }
                            #endregion
                            #endregion
                        }

                        #region Ins_ReportD02
                        #region d02
                        var periodD02 = EnumDropDown.GetEnumDescription<PeriodInsurance>(PeriodInsurance.GETTEMP);
                        var cutOfDuration = "Kỳ " + periodD02 + " - " + payBack.MonthYear.Value.ToString(ConstantFormat.HRM_Format_MonthYear);
                        var reportD02Existed = unitOfWork.CreateQueryable<Ins_ReportD02>(p => p.DateMonth.HasValue
                           && p.DateMonth.Value.Month == payBack.MonthYear.Value.Month && p.DateMonth.Value.Year == payBack.MonthYear.Value.Year
                           && p.ReportD02Name == cutOfDuration).FirstOrDefault();
                        var reportD02ID = Guid.Empty;
                                               
                        #endregion

                        if (reportD02Existed == null)
                        {
                            //insert D02Report
                            reportD02 = new Ins_ReportD02
                            {
                                DateReport = DateTime.Now,
                                DateStart = insMonthYearFrom,
                                DateEnd = insMonthYearTo,
                                DateMonth = payBack.MonthYear,
                                ReportD02Name = cutOfDuration,
                                MaxSalary = 0,
                                MinSalary = 0,
                                SociaInsCountPro = 0,
                                SociaInsTotalSalary = 0,
                                HealthInsCountPro = 0,
                                HealthInsTotalSalary = 0,
                                UnEmpInsCountPro = 0,
                                Type = PeriodInsurance.GETTEMP.ToString(),
                                UnEmpInsTotalSalary = 0
                            };
                            repoInsReportD02.Add(reportD02);
                            var statusCode = repoInsReportD02.SaveChanges();
                            reportD02ID = reportD02.ID;
                            reportD02Existed = unitOfWork.CreateQueryable<Ins_ReportD02>(Guid.Empty, m => m.ID == reportD02ID).FirstOrDefault();
                        }

                        if (reportD02Existed != null)
                        {
                            var insTypeD02 = insTypeD02s.Where(m => m.ID == payBack.TypeID).FirstOrDefault();
                            var reportD02ItemExisted = unitOfWork.CreateQueryable<Ins_ReportD02Item>(m => m.ReportD02ID == reportD02Existed.ID && m.PayBackID == payBack.ID).FirstOrDefault();
                            var monthFrom = payBack.MonthYear.Value.AddMonths(-1);
                            monthFrom = new DateTime(monthFrom.Year, monthFrom.Month,PeriodInsuranceDayPreMonthDefault );
                            var monthTo = new DateTime(payBack.MonthYear.Value.Year, payBack.MonthYear.Value.Month, PeriodInsuranceDayCurrentMonthDefault);

                            #region delete d02 truoc khi tinh truy lĩnh     
                            #region xoa D02Item truoc khi tinh toan truy lĩnh
                            if (reportD02Existed != null && reportD02Existed.ID != Guid.Empty)
                            {
                                var reportD02ItemDelete = unitOfWork.CreateQueryable<Ins_ReportD02Item>(Guid.Empty, m => m.ReportD02ID == reportD02Existed.ID && m.ProfileID == profileID && m.IsPayBack == null).ToList();
                                if (reportD02ItemDelete != null)
                                {
                                    unitOfWork.RemoveObject(typeof(Ins_ReportD02Item), reportD02ItemDelete.ToArray());
                                    unitOfWork.SaveChanges();
                                }
                            }
                            #endregion


                            if (reportD02ItemExisted != null)
                            {
                                #region Edit
                                reportD02ItemExisted.ProfileID = profileID;
                                reportD02ItemExisted.ReportD02ID = reportD02Existed.ID;
                                reportD02ItemExisted.OldBasicSalary = payBack.InsSalary;
                                reportD02ItemExisted.NewBasicSalary = payBack.InsSalaryAdjust;
                                reportD02ItemExisted.SocialInsPlaceID = socialInsPlaceID;
                                reportD02ItemExisted.IsPayBack = true;
                                reportD02ItemExisted.PayBackID = payBack.ID;
                                reportD02ItemExisted.MonthFrom = payBack.FromMonthEffect;
                                reportD02ItemExisted.MonthTo = payBack.FromMonthEffect;
                                reportD02ItemExisted.RateHealthIns = payBack.HealthInsComRate + payBack.HealthInsEmpRate;
                                reportD02ItemExisted.RateSocialIns = payBack.SocialInsComRate + payBack.SocialInsEmpRate;
                                reportD02ItemExisted.RateUnEmpIns = payBack.UnemployComRate + payBack.UnemployEmpRate;
                                reportD02ItemExisted.JobName = payBack.JobtitleName;
                                if (insTypeD02 != null)
                                {
                                    reportD02ItemExisted.Status = insTypeD02.CommentCode;
                                    reportD02ItemExisted.Type = insTypeD02.TypeCode;
                                    reportD02ItemExisted.Comment = payBack.Comment;
                                }
                                if (!lstD02ItemEditing.Any(m => m.ID == reportD02ItemExisted.ID))
                                {
                                    lstD02ItemEditing.Add(reportD02ItemExisted);
                                }

                                #endregion
                            }
                            else
                            {
                                #region Add new
                                var d02Item = new Ins_ReportD02Item();
                                d02Item.ProfileID = profileID;
                                d02Item.ReportD02ID = reportD02Existed.ID;
                                d02Item.OldBasicSalary = payBack.InsSalary;
                                d02Item.NewBasicSalary = payBack.InsSalaryAdjust;
                                d02Item.MonthFrom = payBack.FromMonthEffect;
                                d02Item.MonthTo = payBack.FromMonthEffect;
                                d02Item.IsPayBack = true;
                                d02Item.PayBackID = payBack.ID;
                                d02Item.JobName = payBack.JobtitleName;
                                d02Item.SocialInsPlaceID = socialInsPlaceID;
                                d02Item.RateHealthIns = payBack.HealthInsComRate + payBack.HealthInsEmpRate;
                                d02Item.RateSocialIns = payBack.SocialInsComRate + payBack.SocialInsEmpRate;
                                d02Item.RateUnEmpIns = payBack.UnemployComRate + payBack.UnemployEmpRate;
                                if (insTypeD02 != null)
                                {
                                    d02Item.Status = insTypeD02.CommentCode;
                                    d02Item.Type = insTypeD02.TypeCode;
                                    d02Item.Comment = payBack.Comment;
                                }
                                lstD02Item.Add(d02Item);
                                #endregion
                            }
                        }
                        #endregion
                        #endregion
                    }

                    #region Save Change
                    if (lstProfileMonthly.Any())
                    {
                        repoInsMonthly.Add(lstProfileMonthly);
                    }
                    if (lstProfileMonthlyEditing.Any())
                    {
                        repoInsMonthly.Edit(lstProfileMonthlyEditing);
                    }
                    if (lstD02Item.Any())
                    {
                        repoInsReportD02Item.Add(lstD02Item);
                    }
                    if (lstD02ItemEditing.Any())
                    {
                        repoInsReportD02Item.Edit(lstD02ItemEditing);
                    }
                    repoInsMonthly.SaveChanges();
                    #endregion
                }
            }
        }

        public Ins_ProfileInsuranceMonthlyEntity GetProfileMonthly(Guid profileId, DateTime fromMonthEffect)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var result = unitOfWork.CreateQueryable<Ins_ProfileInsuranceMonthly>(Guid.Empty, m => m.ProfileID == profileId && m.MonthYear == fromMonthEffect).Select(m => new Ins_ProfileInsuranceMonthlyEntity
                {
                    AmountHDTIns = m.AmountHDTIns,
                    SalaryInsurance = m.SalaryInsurance,
                    JobName = m.JobName,
                    IsSocialInsurance = m.IsSocialInsurance,
                    IsHealthInsurance = m.IsHealthInsurance,
                    IsUnEmpInsurance = m.IsUnEmpInsurance,
                    SocialInsPlaceID= m.SocialInsPlaceID
                }).FirstOrDefault();
                return result;
            }
        }

        public void DeleteInsurancePayBack(string payBackIDs)
        {
            /*
            *  Goal(Xoá PayBack và xoa profileMonthly,insReportD02Item)
            *  Steps :
            *      Step1  :  chuyển payBackIDs thành List<Guid>
            *      Step2  :  xoá Ins_profileMonthly
            *      Step3  :  xoa Ins_ReportD02Item
            */


            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                #region Xoá truy lĩnh bảo hiểm
                if (!string.IsNullOrEmpty(payBackIDs))
                {
                    #region convert payBackIDs thành List<Guid>
                    List<Guid> lstPayBackID = new List<Guid>();
                    var arrPayBacks = payBackIDs.Split(',');
                    for (int i = 0; i < arrPayBacks.Length; i++)
                    {
                        var payBackID = Guid.Empty;
                        Guid.TryParse(arrPayBacks[i], out payBackID);
                        lstPayBackID.Add(payBackID);
                    }
                    #endregion

                    if (lstPayBackID.Any())
                    {
                        var payBacks = unitOfWork.CreateQueryable<Ins_InsuranceSalaryPayback>(Guid.Empty, m => lstPayBackID.Contains(m.ID)).ToList();
                        var d02Items = unitOfWork.CreateQueryable<Ins_ReportD02Item>(Guid.Empty, m => m.PayBackID.HasValue && lstPayBackID.Contains(m.PayBackID.Value)).ToList();
                        var profileMonthlys = unitOfWork.CreateQueryable<Ins_ProfileInsuranceMonthly>(Guid.Empty, m => m.PaybackID.HasValue && lstPayBackID.Contains(m.PaybackID.Value)).ToList();
                        foreach (var item in payBacks)
                        {
                            item.IsDelete = true;
                        }
                        foreach (var d02Item in d02Items)
                        {
                            d02Item.IsDelete = true;
                        }
                        foreach (var profileMonthly in profileMonthlys)
                        {
                            profileMonthly.IsDelete = true;
                        }
                        unitOfWork.SaveChanges(Guid.Empty);
                    }
                }
                #endregion
            }
        }

        #endregion

    }
}
