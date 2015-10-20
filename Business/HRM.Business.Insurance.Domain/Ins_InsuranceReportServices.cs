
using System.ComponentModel;
using System.Reflection;
using HRM.Business.Category.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using System;
using System.Linq;

using System.Data;
using HRM.Business.Attendance.Domain;
using HRM.Business.Payroll.Domain;
using HRM.Business.Insurance.Models;
using VnResource.Helper.Data;
using VnResource.Helper.Setting;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using System.Collections;
using System.Collections.Generic;
using VnResource.Helper.Linq;
using HRM.Business.HrmSystem.Domain;

namespace HRM.Business.Insurance.Domain
{
    public class Ins_InsuranceReportServices : BaseService
    {
        #region Properties
        public const string MONTHYEAR = "MMM-yyyy";
        /// <summary> 15 tháng N-1 </summary>
       // public  int D02DayFrom = InsuranceServices.PeriodInsuranceDayPreMonthDefault ;//15 thang N-1
        /// <summary> 14 tháng N </summary>
       // public const int D02DayTo = InsuranceServices.PeriodInsuranceDayCurrentMonthDefault ;// 14 thang N

        /// <summary>Tỉ Lệ BHXH : 0.24</summary>
        public const double RateSocial = 0.24;
        /// <summary>Ti Lệ BHYT: 0.045</summary>
        public const double RateHealth = 0.045;
        /// <summary>Tỉ lệ BHTN : 0.02</summary>
        public const double RateUnEmp = 0.02;

        private int? _periodInsuranceDayPreMonth;
        /// <summary>Lấy ngày chốt bảo hiểm [honda là ngày 18]</summary>
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

        /// <summary>Lấy ngày chốt bảo hiểm [honda là ngày 17]</summary>
        public int PeriodInsuranceDayCurrentMonth
        {
            get
            {
                return PeriodInsuranceDayPreMonth - 1;
            }
        }

        #endregion

        #region Mẫu Báo Cáo D02TS

        /// <summary> Compute D02 và xuat bao cao D02 </summary>
        /// <param name="all"></param>
        /// <param name="increase"></param>
        /// <param name="descrease"></param>
        /// <param name="dtMonthYear"></param>
        /// <param name="orgIds"></param>
        /// <returns></returns>
        public List<Ins_InsuranceReportD02Entity> LoadData(bool? all, bool? increase, bool? descrease, DateTime? dtMonthYear, string orgIds, string searchNoteType, string searchStatus, string codeEmp, List<Guid> socialInsPlaceIDs, string userLogin)
        {
            var monthYear = dtMonthYear ?? DateTime.Now;
            DateTime dateFrom = monthYear.AddMonths(-1);
            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, InsuranceServices.PeriodInsuranceDayPreMonthDefault);
            DateTime dateTo = new DateTime(monthYear.Year, monthYear.Month, InsuranceServices.PeriodInsuranceDayCurrentMonthDefault);
            var status = string.Empty;
            var allCheck = all ?? false;
            var inCreaseCheck = increase ?? false;
            var descreaseCheck = descrease ?? false;

            DateTime dateFromNewHONDA = new DateTime(dateFrom.Year, dateFrom.Month, PeriodInsuranceDayPreMonth);//18
            DateTime dateToNewHONDA = new DateTime(dateTo.Year, dateTo.Month, PeriodInsuranceDayCurrentMonth);//17
            //
            int? sociaInsCountPro = 0;
            double? sociaInsTotalSalary = 0;
            int? healthInsCountPro = 0;
            double? healthInsTotalSalary = 0;
            int? unEmpInsCountPro = 0;
            double? unEmpInsTotalSalary = 0;
            double? maxSalary = long.MinValue + 1;
            double? minSalary = long.MaxValue - 1;
            //

            //lay ds Ins_ProfileInsuranceMonthly
            //List<object> listInsMonthlyObj = new List<object>();
            //listInsMonthlyObj.Add(orgs);
            //listInsMonthlyObj.Add(monthYear);
            //var profileInsMonthlys = GetData<Hre_ProfileEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, ref status);
            Hashtable htbData = new Hashtable();
            var d02Reports = new List<Ins_InsuranceReportD02Entity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var profileInsMonthly = new Ins_ProfileInsuranceMonthlyRepository(unitOfWork);
                var orgStructureRepo = new Cat_OrgStructureRepository(unitOfWork);
                var insInsuranceRecord = new Ins_InsuranceRecordRepository(unitOfWork);
                var hre_ContractRepo = new Hre_ContractRepository(unitOfWork);
                var cat_ContractTypeRepo = new Cat_ContractTypeRepository(unitOfWork);
                var att_LeaveDayRepo = new Att_LeavedayRepository(unitOfWork);

                #region Lấy DS Ins_ProfileInsuranceMonthly (lấy tất cả)
                var listInsMonthlyObj = new List<object> { orgIds, null, null, 1, int.MaxValue - 1 };
                if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                {
                    listInsMonthlyObj.Add("id");
                }
                var lstProfileInsuranceMonthlyInDb = GetData<Ins_ProfileInsuranceMonthlyEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, userLogin, ref status);
                #endregion

                //get profiles trong Ins_ProfileInsuranceMonthly theo thang kiểm tra
                var lstProfile = lstProfileInsuranceMonthlyInDb.Where(s => (s.IsDelete == null || s.IsDelete.Value==false) && s.MonthYear.HasValue
                    && (s.MonthYear.Value.Year == dateFrom.Year || s.MonthYear.Value.Year == dateTo.Year)
                    && (s.MonthYear.Value.Month == dateFrom.Month || s.MonthYear.Value.Month == dateTo.Month)
                    && s.MonthYearEffect.HasValue && s.MonthYear == s.MonthYearEffect).ToList().Translate<Ins_ProfileInsuranceMonthly>();

                #region lay ins_ProfileInsuranceMonthly de tinh thai san 1 nam lien ke
                //Lấy Ds Ins_ProfileInsuranceMonthly 12 tháng trước so với tháng kiểm tra (de tinh thai san 1 nam lien ke)
                List<Ins_ProfileInsuranceMonthlyEntity> lstProfilePregnant = null;
                #endregion


                //var lstProfile = profileInsMonthly.FindBy(s => s.IsDelete == null && s.MonthYear.HasValue
                //    && (s.MonthYear.Value.Year == dateFrom.Year || s.MonthYear.Value.Year == dateTo.Year)
                //    && (s.MonthYear.Value.Month == dateFrom.Month || s.MonthYear.Value.Month == dateTo.Month)).ToList();

                //get InsuranceRecord (Chứng từ bảo hiểm)
                var lstInsuranceRecord = insInsuranceRecord.GetAll().Where(p => p.IsDelete == null).ToList();// lay tat ca chứng từ bảo hiểm (sau nay sửa lai sau)


                #region Lay ds Phong Ban
                var orgs = GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).ToList();
                List<Cat_OrgStructureEntity> orgStructures = orgs.Translate<Cat_OrgStructureEntity>();
                #endregion

                #region Lấy profile theo phong ban
                List<object> listObj = new List<object>();
                listObj.Add(orgIds);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);
                var profilebyOrgs = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status);
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    profilebyOrgs = profilebyOrgs.Where(p => p.CodeEmp == codeEmp).ToList();
                }
                else if (profilebyOrgs != null)
                {
                    profilebyOrgs = profilebyOrgs.Where(p => p.DateHire != null
                                && (p.DateQuit == null || p.DateQuit > dateFrom)
                                && p.DateHire <= monthYear).ToList();
                }


                var profileIds = profilebyOrgs.Select(p => p.ID).ToList();
                #endregion

                #region  lay ds hop dong đã ký so với tháng chọn (Loại HD có đóng BH)
                var contractTypeObjs = cat_ContractTypeRepo.FindBy(p => p.IsDelete == null).ToList();
                //Ds loại HĐ đóng BHXH
                var contractSocialTypeIDObjs = contractTypeObjs.Where(p => p.IsSocialInsurance.HasValue && p.IsSocialInsurance.Value).ToList();
                //Ds loại HĐ đóng BHTN
                var contractUnEmpTypeIDObjs = contractTypeObjs.Where(p => p.IsUnEmployInsurance.HasValue && p.IsUnEmployInsurance.Value).ToList();
                var contracts = new List<Hre_Contract>();
                var contractSocials = new List<Hre_Contract>();
                var contractUnEmps = new List<Hre_Contract>();
                if (contractSocialTypeIDObjs.Any())
                {
                    var contractSocialTypeIds = contractSocialTypeIDObjs.Select(p => p.ID).ToList();
                    var contractUnEmpTypeIds = contractUnEmpTypeIDObjs.Select(p => p.ID).ToList();
                    //Ds Hop Đồng của cac NV
                    contracts = hre_ContractRepo.FindBy(p => p.IsDelete == null && profileIds.Contains(p.ProfileID)).OrderBy(p => p.DateStart).ToList();
                    //Ds Hop đồng thuộc loại đóng BHXH
                    contractSocials = contracts.Where(p => contractSocialTypeIds.Contains(p.ContractTypeID)).OrderBy(p => p.DateStart).ToList();
                    //Ds Hop đồng thuộc loại đóng BHTN
                    contractUnEmps = contracts.Where(p => contractUnEmpTypeIds.Contains(p.ContractTypeID)).OrderBy(p => p.DateStart).ToList();
                }

                #endregion

                #region lấy Leaveday
                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                string E_PREGNANCY_SUCKLE = InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString();

                var lstLeaveday = att_LeaveDayRepo.FindBy(m => m.IsDelete == null && m.Status == E_APPROVED && m.DateStart <= dateTo && m.DateEnd >= dateFrom && m.Cat_LeaveDayType != null
                    && m.Cat_LeaveDayType.InsuranceType == E_PREGNANCY_SUCKLE && profileIds.Contains(m.ProfileID)).ToList();
                #endregion

                List<object> salInsuranceSalaryParams = new List<object>();
                salInsuranceSalaryParams.Add(null);
                salInsuranceSalaryParams.Add(null);
                salInsuranceSalaryParams.Add(orgIds);
                salInsuranceSalaryParams.Add(null);
                salInsuranceSalaryParams.Add(null);
                salInsuranceSalaryParams.Add(null);
                salInsuranceSalaryParams.Add(null);
                salInsuranceSalaryParams.Add(1);
                salInsuranceSalaryParams.Add(Int32.MaxValue - 1);
                var dateEnd = new DateTime(dateTo.Year, dateTo.Month, DateTime.DaysInMonth(dateTo.Year, dateTo.Month));
                var lstInsuranceSalaryByProfile = GetData<Sal_InsuranceSalary>(salInsuranceSalaryParams, ConstantSql.hrm_sal_sp_get_InsuranceSalary, userLogin, ref status)
                    .Where(p => p.DateEffect <= dateEnd && profileIds.Contains(p.ProfileID ?? Guid.Empty) && p.InsuranceAmount.HasValue).ToList();

                if (socialInsPlaceIDs != null)
                {
                    socialInsPlaceIDs = socialInsPlaceIDs.Where(p => p != null && p != Guid.Empty).ToList();
                    if (socialInsPlaceIDs != null && socialInsPlaceIDs.Any())
                    {
                        lstProfile = lstProfile.Where(p => socialInsPlaceIDs != null && socialInsPlaceIDs.Count > 0 && socialInsPlaceIDs.Contains(p.SocialInsPlaceID ?? Guid.Empty)).ToList();
                    }
                }



                foreach (var profile in profilebyOrgs)
                {
                    //lay các chung tu bao hiem cua nv
                    List<Ins_InsuranceRecord> lstInsRecordProfile = lstInsuranceRecord.Where(rec => rec.ProfileID == profile.ID).ToList();
                    var lstleavePregByProfile = lstLeaveday.Where(rec => rec.ProfileID == profile.ID).ToList();
                    var lstInsRecordPregByProfile = lstInsRecordProfile.Where(m => m.InsuranceType == E_PREGNANCY_SUCKLE).ToList();


                    var profileMonthlys = lstProfile.Where(p => p.ProfileID == profile.ID).OrderBy(p => p.MonthYear).ToList();



                    var inFirst = profileMonthlys.FirstOrDefault(p => p.MonthYear.HasValue && p.MonthYear.Value.Month == dateFrom.Month);
                    var inNow = profileMonthlys.LastOrDefault(p => p.MonthYear.HasValue && p.MonthYear.Value.Month == dateTo.Month);

                    if (inFirst == null)
                    {
                        inFirst = new Ins_ProfileInsuranceMonthly
                        {
                            IsHealthInsurance = false,
                            IsSocialInsurance = false,
                            IsUnEmpInsurance = false
                        };
                    }

                    if (inNow != null)
                    {
                        profile.JobName = inNow.JobName;
                        #region set Phu cap
                        if (inNow != null)
                        {
                            profile.Allowance1 = inNow.Allowance1;
                            profile.Allowance2 = inNow.Allowance2;
                            profile.Allowance3 = inNow.Allowance3;
                            profile.Allowance4 = inNow.Allowance4;
                        }
                        #endregion

                        #region Thống kê số lượng người đóng BHXH,BHYT,BHTN và tổng lương BHXH,BHTN,BHTN
                        if (inNow.IsSocialInsurance.HasValue && inNow.IsSocialInsurance.Value)
                        {
                            sociaInsCountPro++;
                            sociaInsTotalSalary += inNow.MoneySocialInsurance;
                        }
                        if (inNow.IsHealthInsurance.HasValue && inNow.IsHealthInsurance.Value)
                        {
                            healthInsCountPro++;
                            healthInsTotalSalary += inNow.MoneyHealthInsurance;
                        }
                        if (inNow.IsUnEmpInsurance.HasValue && inNow.IsUnEmpInsurance.Value)
                        {
                            unEmpInsCountPro++;
                            unEmpInsTotalSalary += inNow.MoneyUnEmpInsurance;
                        }
                        if (inNow.SalaryInsurance.HasValue && inNow.SalaryInsurance.Value > maxSalary)
                        {
                            maxSalary = inNow.SalaryInsurance.Value;
                        }
                        if (inNow.SalaryInsurance.HasValue && inNow.SalaryInsurance.Value < minSalary)
                        {
                            minSalary = inNow.SalaryInsurance.Value;
                        }
                        #endregion
                        
                        #region tang

                        if (IsIncreaseInsurance(inFirst, inNow) && (inCreaseCheck == true || allCheck == true))
                        {
                            String strPre = InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString();
                            List<Ins_InsuranceRecord> lstInsPre = lstInsRecordProfile.Where(ins => ins.InsuranceType == strPre && ins.DateEnd > dateFrom && ins.DateEnd < dateTo).ToList();
                            String strSickLong = InsuranceRecordType.E_SICK_LONG.ToString();
                            String strSickShort = InsuranceRecordType.E_SICK_SHORT.ToString();
                            List<Ins_InsuranceRecord> lstInsSick = lstInsRecordProfile.Where(ins => ins.InsuranceType == strSickLong || ins.InsuranceType == strSickShort && ins.DayCount > 30
                                && ins.DateEnd > dateFrom && ins.DateEnd < dateTo).ToList();

                            #region Tăng LĐ
                            //lay ds hop dong cua NV
                            var contractProfile = contractSocials.Where(p => p.ProfileID == profile.ID).OrderBy(p => p.DateStart).FirstOrDefault();
                            Hre_Contract contract = null;
                            //lay hd moi nhat cua nv nằm trong khoảng 16-[N-1] đến 15-[N]
                            if (contractProfile != null && dateFromNewHONDA <= contractProfile.DateStart && contractProfile.DateStart <= dateToNewHONDA)
                            {
                                contract = contractProfile;
                            }

                            #region Tăng do Nghỉ 14 ngày đi làm lại

                            //Kiểm tra Tháng N-1 có ký HD ?    
                            //tang moi lao dong (Tháng N-1 ký HD nhưng chưa đóng BH do nghỉ hơn 14 ngày , Tháng N đóng bảo hiểm => thuoc loại tăng mới LĐ
                            if (inFirst.IsDecreaseWorkingDays.HasValue && inFirst.IsDecreaseWorkingDays.Value)
                            {
                                var contractDatefrom = new DateTime(dateFrom.Year, dateFrom.Month, 1);
                                var contractDateTo = contractDatefrom.AddMonths(1).AddDays(-1);
                                if (contractProfile != null && contractDatefrom <= contractProfile.DateStart && contractProfile.DateStart <= contractDateTo)
                                {
                                    //ký hợp đồng tháng trước
                                    SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_TANG.ToString(), TypeInsuranceD02TS.E_TANG_LD.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "I.1");
                                }
                                else
                                {
                                    //tang do nghỉ 14 ngày đi làm lại
                                    SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_TANG.ToString(), TypeInsuranceD02TS.E_TANG_LEAVE_14WORKINGDAYS.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "I.1");
                                }
                            }
                            #endregion

                            else if (contract != null)
                            {
                                //lay hd moi nhat cua nv nằm trong khoảng 16-[N-1] đến 15-[N]=>tăng lao động
                                SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_TANG.ToString(), TypeInsuranceD02TS.E_TANG_LD.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "I.1");
                            }

                            #endregion

                            #region Tăng Thai Sản
                            else if (HasPregnant(dateTo.AddMonths(-1), lstleavePregByProfile, lstInsRecordPregByProfile))
                            {
                                SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_TANG.ToString(), TypeInsuranceD02TS.E_TANG_TS.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "I.1");
                            }
                            #endregion

                            #region Tăng Bệnh
                            else if (lstInsSick.Count() > 0 /* && allCheck == false*/)
                            {
                                SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_TANG.ToString(), TypeInsuranceD02TS.E_TANG_BENH.ToString(), inFirst.MoneySocialInsurance, inNow.MoneySocialInsurance, monthYear, orgStructures, "I.1");
                            }
                            #endregion

                            #region Honda - Tăng Đóng BHTN
                            else if ((!inFirst.IsUnEmpInsurance.HasValue || (inFirst.IsUnEmpInsurance.HasValue &&
                              !inFirst.IsUnEmpInsurance.Value))
                              && inNow.IsUnEmpInsurance.HasValue && inNow.IsUnEmpInsurance.Value)
                            {
                                var contractUnEmpProfile = contractUnEmps.Where(p => p.ProfileID == profile.ID).OrderBy(p => p.DateStart).FirstOrDefault();
                                if (contractUnEmpProfile != null && dateFrom <= contractUnEmpProfile.DateStart && contractUnEmpProfile.DateStart <= dateTo)
                                {
                                    SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_TANG.ToString(), TypeInsuranceD02TS.E_TANG_BHTN.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "I.2");
                                }
                            }
                            #endregion

                            #region Tăng BHYT
                            else if ((!inFirst.IsHealthInsurance.HasValue || (inFirst.IsHealthInsurance.HasValue &&
                               !inFirst.IsHealthInsurance.Value))
                               && inNow.IsHealthInsurance.HasValue && inNow.IsHealthInsurance.Value)
                            {
                                SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_TANG.ToString(), TypeInsuranceD02TS.E_TANG_BHYT.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "I.1");
                            }
                            #endregion

                        }
                        #endregion

                        #region giam
                        if (IsDecreaseInsurance(inFirst, inNow) && (descreaseCheck == true || allCheck == true))
                        {
                            String strPre = InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString();
                            List<Ins_InsuranceRecord> lstInsPre = lstInsRecordProfile.Where(ins => ins.InsuranceType == strPre && ins.DateEnd > dateFrom && ins.DateStart < dateTo).ToList();
                            String strSickLong = InsuranceRecordType.E_SICK_LONG.ToString();
                            String strSickShort = InsuranceRecordType.E_SICK_SHORT.ToString();
                            List<Ins_InsuranceRecord> lstInsSick = lstInsRecordProfile.Where(ins => ins.InsuranceType == strSickLong && ins.InsuranceType == strSickShort
                                                                                                   && ins.DayCount > 30
                                                                                                   && ins.DateEnd > dateFrom && ins.DateEnd < dateTo).ToList();
                            if (inFirst.IsDecreaseWorkingDays == null)
                            {
                                inFirst.IsDecreaseWorkingDays = false;
                            }
                            if (inNow.IsDecreaseWorkingDays == null)
                            {
                                inNow.IsDecreaseWorkingDays = false;
                            }
                            #region giam

                            //neu NV nghỉ từ 16 tháng [N-1] đến 15 tháng N => Tạm Hoãn hoặc nghỉ luôn
                            if (profile.DateQuit.HasValue && dateFrom <= profile.DateQuit && profile.DateQuit <= dateTo)
                            {
                                #region Set trạng thái trả thẻ
                                var statusGiam_BHYT = string.Empty;
                                if (profile.ReceiveHealthIns.HasValue && profile.ReceiveHealthIns.Value)
                                {
                                    // Giam LD tra the 
                                    statusGiam_BHYT = TypeInsuranceD02TS.E_GIAM_LD_BHYT.ToString();
                                }
                                else if ((!profile.ReceiveHealthIns.HasValue || profile.ReceiveHealthIns == false))
                                {
                                    // Giảm Lao Động Không Trả Thẻ
                                    statusGiam_BHYT = TypeInsuranceD02TS.E_GIAM_LD_NOT_BHYT.ToString();
                                }
                                else
                                {
                                    // Giảm LĐ 
                                    statusGiam_BHYT = TypeInsuranceD02TS.E_GIAM_LD.ToString();
                                }
                                //HonDa
                                statusGiam_BHYT = TypeInsuranceD02TS.E_GIAM_LD_BHYT.ToString();
                                #endregion

                                if (profile.StopWorkType == EnumDropDown.StopWorkType.E_SUSPENSE.ToString())
                                {
                                    //tạm hoãn
                                    SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_QUIT_SUSPENSE.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II.1");
                                }
                                else if (!string.IsNullOrEmpty(statusGiam_BHYT))
                                {
                                    #region Nghỉ Việc Sau Khi Nghỉ Sinh Con
                                    if (HasPregnant(dateTo.AddMonths(-1), lstleavePregByProfile, lstInsRecordPregByProfile))
                                    {
                                        SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_TS_QUIT.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II.1");
                                    }
                                    else
                                    {
                                        SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), statusGiam_BHYT, inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II.1");
                                    }
                                    #endregion
                                }
                            }

                            #region Giam Thai San
                            else if (HasPregnant(dateTo, lstleavePregByProfile, lstInsRecordPregByProfile) 
                                && (!profile.DateQuit.HasValue || profile.DateQuit.Value > dateTo))
                            {
                                if (lstProfilePregnant == null)
                                {
                                    //chỉ lay 1 lần
                                    #region lay ins_ProfileInsuranceMonthly de tinh thai san 1 nam lien ke
                                    var dtPreDateCheck = dateFrom.AddYears(-1);
                                    //Lấy Ds Ins_ProfileInsuranceMonthly 12 tháng trước so với tháng kiểm tra (de tinh thai san 1 nam lien ke)
                                    lstProfilePregnant = lstProfileInsuranceMonthlyInDb.Where(s => s.MonthYear.HasValue
                                        && (dtPreDateCheck <= s.MonthYear.Value && s.MonthYear <= dateFrom)).ToList();
                                    #endregion
                                }

                                // Nhân viên đóng bảo hiểm đủ 6 tháng trong vòng 12 tháng liền kề
                                var insCount = lstProfilePregnant.Where(p => p.ProfileID == profile.ID && p.IsSocialInsurance.HasValue && p.IsSocialInsurance.Value).Count();
                                if (insCount >= 6)
                                {
                                    SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_TS.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II.1");
                                }
                                else
                                {
                                    SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_LEAVE_14WORKINGDAYS.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II.1");
                                }
                            }

                            #endregion

                            #region Giam benh
                            else if (lstInsSick.Count() > 0 /*&& allCheck == false*/)
                            {
                                SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_BENH.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II.1");
                            }
                            #endregion

                            #region giam do Nghỉ 14 ngày đi làm lại

                            // tháng N là false (hoac null) => Giảm do nghỉ >= 14 ngày                            
                            else if (inNow.IsDecreaseWorkingDays.HasValue && inNow.IsDecreaseWorkingDays.Value)
                            {
                                SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_LEAVE_14WORKINGDAYS.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II.1");
                            }
                            #endregion

                            #endregion
                        }//ket thuc giảm
                        #endregion

                        #region Thay doi luong

                        else if (inFirst != null && inFirst.SalaryInsurance != inNow.SalaryInsurance)
                        {
                            //-------Giam luong
                            if (inFirst != null && inFirst.SalaryInsurance > inNow.SalaryInsurance)
                            {
                                #region giảm lương và thay đổi chức danh nghề
                                if (inFirst.JobName != inNow.JobName)
                                {
                                    //giảm lương và thay đổi chức danh nghề
                                    SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM_LUONG_CHANGEJOBNAME.ToString(), TypeInsuranceD02TS.E_GIAM_LUONG_CHANGEJOBNAME.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "IV.");
                                }
                                #endregion
                                #region Giảm Lương
                                else
                                {
                                    //giảm lương
                                    SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_LUONG.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "IV.");
                                }
                                #endregion
                            }
                            //------------Tang luong-----------
                            else if (inFirst != null && inFirst.SalaryInsurance < inNow.SalaryInsurance)
                            {
                                #region tăng lương và thay đổi chức danh nghề
                                if (inFirst.JobName != inNow.JobName)
                                {
                                    //tăng lương và thay đổi chức danh nghề
                                    SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_TANG_LUONG_CHANGEJOBNAME.ToString(), TypeInsuranceD02TS.E_TANG_LUONG_CHANGEJOBNAME.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "III.");
                                }
                                #endregion
                                #region tăng lương
                                else
                                {
                                    //tăng lương 
                                    SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_TANG.ToString(), TypeInsuranceD02TS.E_TANG_LUONG.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "III.");
                                }
                                #endregion
                            }
                        }
                        #endregion

                        #region Thay đổi chức danh nghề (không đổi lương)
                        //Thay đổi chức danh nghề (không đổi lương)
                        if (inFirst != null && inNow != null && inFirst.SalaryInsurance == inNow.SalaryInsurance && inFirst.JobName != inNow.JobName)
                        {
                            SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_CHANGEJOBNAME.ToString(), TypeInsuranceD02TS.E_CHANGEJOBNAME.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "V.");
                        }
                        #endregion

                        #region Trường Hợp đặc biệt (Honda)

                        #region Tháng N-1 nghỉ Thai Sản và tháng N nghỉ luon =>Nghỉ luôn sau thai sản
                        if (inFirst != null && inFirst.IsPregnant.HasValue && inFirst.IsPregnant.Value
                            && inNow != null && (inNow.IsPregnant == null || inNow.IsPregnant.Value == false)
                            && profile.DateQuit.HasValue && dateFrom <= profile.DateQuit && profile.DateQuit <= dateTo)                       
                        {
                            //>Nghỉ luôn sau thai sản
                            SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_TS_QUIT.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II.1");
                        }
                        #region Tháng N-1 nghỉ Thai Sản và tháng N nghỉ hơn 14 ngày => Nghỉ 14 ngày sau thai sản
                        else if (inNow != null && inNow.IsDecreaseWorkingDays.HasValue && inNow.IsDecreaseWorkingDays.Value
                            && HasPregnant(dateTo.AddMonths(-1), lstleavePregByProfile, lstInsRecordPregByProfile)
                            && (inNow.IsPregnant == null || inNow.IsPregnant.Value == false))
                        {
                            //Giảm do nghỉ hơn 14 ngày sau thai sản
                            SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_PREGNANT_14WORKINGDAYS.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II.1");
                        }
                        #endregion
                        #endregion

                        #region tháng N-1 giảm 14 ngày và tháng này nghỉ việc
                        if (profile != null && inFirst != null && inNow != null
                            && !HasPregnant(dateTo.AddMonths(-1), lstleavePregByProfile, lstInsRecordPregByProfile)
                            && inFirst.IsDecreaseWorkingDays.HasValue && inFirst.IsDecreaseWorkingDays.Value
                            && profile.DateQuit.HasValue && dateFrom <= profile.DateQuit && profile.DateQuit <= dateTo)
                        {
                            //Nghỉ việc mà trước đó nghỉ >=14 ngày
                            SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_LEAVE_PREMONTH_14WORKINGDAYS.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II.1");
                        }
                        #endregion

                      
                        
                        #region Nghiệp Vụ Chuyển Nơi Đóng BH (Honda)
                        if (inFirst != null && inNow != null 
                            && (inFirst.SocialInsPlaceID.HasValue && inFirst.SocialInsPlaceID.Value != Guid.Empty) 
                            && (inNow.SocialInsPlaceID.HasValue && inNow.SocialInsPlaceID.Value != Guid.Empty)
                            && (inFirst.SocialInsPlaceID.Value != inNow.SocialInsPlaceID.Value))
                        {
                            //thang truoc giam do chuyen noi dong bao hiem
                            profile.JobName = string.Copy(inFirst.JobName);
                            SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_TANG.ToString(), TypeInsuranceD02TS.E_TANG_LD_CHANGE_INSPLACE.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "I.1");
                            //thang check tang do chuyen noi dong bao hiem
                            profile.JobName = string.Copy(inNow.JobName);
                            SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_LD_CHANGE_INSPLACE.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II.1");
                        }
                        #endregion

                        #endregion

                    }
                }//ket thuc foreach profile
            }

            //duyet hashtable => danh sach nv theo loai bao hiem (tangLd,tangBHXH,.....)
            var result = new List<Ins_InsuranceReportD02Entity>();
            var tang = TypeInsuranceD02TS.E_TANG.ToString();
            var giam = TypeInsuranceD02TS.E_GIAM.ToString();
            var tangLD = TypeInsuranceD02TS.E_GIAM_LD.ToString();
            foreach (DictionaryEntry entry in htbData)
            {
                var lst = new List<Ins_InsuranceReportD02Entity>();
                lst = (List<Ins_InsuranceReportD02Entity>)entry.Value;
                var d02Entity = lst.FirstOrDefault(p => p.OrderGroup != null);
                var orderGroup = string.Empty;
                if (d02Entity != null)
                {
                    orderGroup = d02Entity.OrderGroup;
                }

                var proName = entry.Key.ToString();
                if (!string.IsNullOrEmpty(proName))
                {
                    proName = GetDescription(proName);
                }
                var title = new Ins_InsuranceReportD02Entity()
                {
                    ProfileID = Guid.Empty,
                    ProfileName = proName,
                    OrderGroup = orderGroup,
                    CodeEmp = orderGroup,
                    BOLD = true.ToString()
                };

                lst.Insert(0, title);
                if (lst.Any(p => p.Status1 == tang) && !result.Any(p => p.Status == tangLD) && !result.Any(p => p.ProfileName == "Tăng"))
                {
                    lst.Insert(0, new Ins_InsuranceReportD02Entity { ProfileID = Guid.Empty, ProfileName = "Tăng", CodeEmp = "I", OrderGroup = "I" });
                }
                if (lst.Any(p => p.Status1 == giam) && !result.Any(p => p.ProfileName == "Giảm"))
                {
                    lst.Insert(0, new Ins_InsuranceReportD02Entity { ProfileID = Guid.Empty, ProfileName = "Giảm", CodeEmp = "II", OrderGroup = "II" });
                }
                //if (lst.Any(p => p.Status == TypeInsuranceD02TS.E_GIAM_LD.ToString()) && !result.Any(p => p.Status1 == giam) && !result.Any(p => p.ProfileName == "Giảm Lao Động"))
                //{
                //    lst.Insert(0, new Ins_InsuranceReportD02Entity { ProfileName = "Giảm Lao Động", CodeEmp = "II", OrderGroup = "II" });
                //}

                result.AddRange(lst);
            }

            result = result.OrderBy(p => p.OrderGroup).ToList();

            #region compute D02

            var periodIns = EnumDropDown.GetEnumDescription<PeriodInsurance>(PeriodInsurance.GETTEMP);
            var d02 = new Ins_ReportD02Entity
            {
                DateReport = DateTime.Now,
                SociaInsCountPro = sociaInsCountPro,
                SociaInsTotalSalary = sociaInsTotalSalary,
                HealthInsCountPro = healthInsCountPro,
                HealthInsTotalSalary = healthInsTotalSalary,
                UnEmpInsCountPro = unEmpInsCountPro,
                UnEmpInsTotalSalary = unEmpInsTotalSalary,
                MaxSalary = maxSalary,
                MinSalary = minSalary,
                Type = PeriodInsurance.GETTEMP.ToString()
            };
            //luu D02
            ComputeD02(result, d02, monthYear, dateFrom, dateTo, periodIns);
            #endregion

            return result;
        }

        /// <summary> Tìm kiếm bc D02TS (Hien tai chua dùng toi method này) </summary>
        /// <param name="all"></param>
        /// <param name="increase"></param>
        /// <param name="descrease"></param>
        /// <param name="dtMonthYear"></param>
        /// <param name="orgIds"></param>
        /// <returns></returns>
        public List<Ins_InsuranceReportD02Entity> SearchD02TS(bool? all, bool? increase, bool? descrease, DateTime? dtMonthYear, string orgIds, string searchNoteType, string searchStatus, string codeEmp, List<Guid> socialInsPlaceIDs, string userLogin)
        {
            var periodIns = EnumDropDown.GetEnumDescription<PeriodInsurance>(PeriodInsurance.GETTEMP);
            List<Ins_InsuranceReportD02Entity> lstInsuranceReportD02 = new List<Ins_InsuranceReportD02Entity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoReportD02 = new Ins_ReportD02Repository(unitOfWork);
                var repoReportD02Item = new Ins_ReportD02ItemRepository(unitOfWork);
                var repoProfileInsMonthly = new Ins_ProfileInsuranceMonthlyRepository(unitOfWork);
                string status = string.Empty;
                var monthYear = dtMonthYear ?? DateTime.Now;
                DateTime dateFrom = monthYear.AddMonths(-1);
                dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, InsuranceServices.PeriodInsuranceDayPreMonthDefault);
                DateTime dateTo = new DateTime(monthYear.Year, monthYear.Month, InsuranceServices.PeriodInsuranceDayCurrentMonthDefault);

                #region Lấy DS Ins_ProfileInsuranceMonthly (lấy tất cả)
                var listInsMonthlyObj = new List<object> { orgIds, monthYear, null, 1, int.MaxValue - 1 };
                if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                {
                    listInsMonthlyObj.Add("id");
                }
                var lstProfileInsuranceMonthlyInDb = GetData<Ins_ProfileInsuranceMonthlyEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, userLogin, ref status);
                #endregion

                var lstProfileInsMonthly = lstProfileInsuranceMonthlyInDb.Where(s => s.MonthYear.HasValue
                   && (s.MonthYear.Value.Year == dateFrom.Year || s.MonthYear.Value.Year == dateTo.Year)
                   && (s.MonthYear.Value.Month == dateFrom.Month || s.MonthYear.Value.Month == dateTo.Month)).ToList();

                //tự sinh ra kỳ
                var cutOfDuration = "Kỳ " + periodIns + " - " + monthYear.ToString(ConstantFormat.HRM_Format_MonthYear);

                #region Get D02
                List<object> listD02Obj = new List<object>();
                listD02Obj.Add(null);
                listD02Obj.Add(dtMonthYear);
                listD02Obj.Add(cutOfDuration);
                var d02 = GetData<Ins_ReportD02>(listD02Obj, ConstantSql.hrm_ins_sp_get_D02, userLogin, ref status).FirstOrDefault();
                #endregion

                if (d02 != null)
                {
                    List<object> listD02ItemObj = new List<object>();
                    listD02ItemObj.Add(d02.ID);
                    listD02ItemObj.Add(codeEmp);
                    var d02Item = GetData<Ins_ReportD02ItemEntity>(listD02ItemObj, ConstantSql.hrm_ins_sp_get_D02ItemByD02ID, userLogin, ref status);

                    if (socialInsPlaceIDs != null && socialInsPlaceIDs.Any())
                    {
                        socialInsPlaceIDs = socialInsPlaceIDs.Where(p => p != null && p != Guid.Empty).ToList();
                        if (socialInsPlaceIDs.Any())
                        {
                            d02Item = d02Item.Where(p => socialInsPlaceIDs.Contains(p.SocialInsPlaceID ?? Guid.Empty)).ToList();
                        }
                    }


                    var monthYear1 = new DateTime(monthYear.Year, monthYear.Month, 1);
                    var isHonda = true;
                    foreach (var insReportD02Item in d02Item)
                    {
                        var profileMonthlys = lstProfileInsMonthly.Where(p => p.ProfileID == insReportD02Item.ProfileID && p.MonthYear == monthYear1).FirstOrDefault();
                        var d02Status = insReportD02Item.Status;
                        if (profileMonthlys != null)
                        {
                            #region Honda

                            if (isHonda)
                            {
                                if (insReportD02Item.Status == TypeInsuranceD02TS.E_TANG_TS.ToString() || insReportD02Item.Status == TypeInsuranceD02TS.E_TANG_LEAVE_14WORKINGDAYS.ToString()
                                    || insReportD02Item.Status == TypeInsuranceD02TS.E_TANG_BENH.ToString() || insReportD02Item.Status == TypeInsuranceD02TS.E_TANG_BHYT.ToString()
                                    || insReportD02Item.Status == TypeInsuranceD02TS.E_TANG_LEAVE_14WORKINGDAYS.ToString()
                                    || insReportD02Item.Status == TypeInsuranceD02TS.E_TANG_LD_CHANGE_INSPLACE.ToString())
                                {
                                    d02Status = TypeInsuranceD02TS.E_TANG_LD.ToString();
                                }
                                if (insReportD02Item.Status == TypeInsuranceD02TS.E_GIAM_TS_QUIT.ToString() || insReportD02Item.Status == TypeInsuranceD02TS.E_GIAM_TS.ToString()
                                    || insReportD02Item.Status == TypeInsuranceD02TS.E_GIAM_BENH.ToString() || insReportD02Item.Status == TypeInsuranceD02TS.E_GIAM_LD_BHYT.ToString()
                                    || insReportD02Item.Status == TypeInsuranceD02TS.E_GIAM_LD_NOT_BHYT.ToString()
                                    || insReportD02Item.Status == TypeInsuranceD02TS.E_GIAM_LEAVE_PREMONTH_14WORKINGDAYS.ToString()
                                    || insReportD02Item.Status == TypeInsuranceD02TS.E_GIAM_LEAVE_14WORKINGDAYS.ToString()
                                    || insReportD02Item.Status == TypeInsuranceD02TS.E_GIAM_QUIT_SUSPENSE.ToString()
                                    || insReportD02Item.Status == TypeInsuranceD02TS.E_GIAM_PREGNANT_14WORKINGDAYS.ToString()
                                    || insReportD02Item.Status == TypeInsuranceD02TS.E_GIAM_LD_CHANGE_INSPLACE.ToString())
                                {
                                    d02Status = TypeInsuranceD02TS.E_GIAM_LD.ToString();
                                }
                                if (insReportD02Item.Status == TypeInsuranceD02TS.E_GIAM_LUONG_CHANGEJOBNAME.ToString())
                                {
                                    d02Status = TypeInsuranceD02TS.E_GIAM_LUONG.ToString();
                                }

                                if (insReportD02Item.Status == TypeInsuranceD02TS.E_TANG_LUONG_CHANGEJOBNAME.ToString())
                                {
                                    d02Status = TypeInsuranceD02TS.E_TANG_LUONG.ToString();
                                }
                            }
                            #endregion

                            var insuranceReportD02 = new Ins_InsuranceReportD02Entity
                            {
                                Comment = insReportD02Item.Comment,
                                ProfileID = insReportD02Item.ProfileID,
                                Status = d02Status,
                                Status1 = insReportD02Item.Type,
                                STT = insReportD02Item.ItemOrder.HasValue ? insReportD02Item.ItemOrder.Value.ToString() : null,
                                ItemOrder = insReportD02Item.ItemOrder ?? 0,
                                RateBHXH = insReportD02Item.RateSocialIns,
                                RateBHYT = insReportD02Item.RateHealthIns,
                                RateBHTN = insReportD02Item.RateUnEmpIns,
                                NewBasicSalary = insReportD02Item.NewBasicSalary,
                                NotCardHealth = insReportD02Item.NotCardHealth.HasValue ? insReportD02Item.NotCardHealth.Value.ToString() : "false",
                                ProfileName = insReportD02Item.ProfileName,
                                CodeEmp = insReportD02Item.CodeEmp,
                                SocialInsNo = insReportD02Item.SocialInsNo,
                                JobName = insReportD02Item.JobName,
                                Allowance1 = profileMonthlys.Allowance1,
                                Allowance2 = profileMonthlys.Allowance2,
                                Allowance3 = profileMonthlys.Allowance3,
                                Allowance4 = profileMonthlys.Allowance4,
                                AllowanceAdditional = profileMonthlys.AllowanceAdditional,
                                FromMonth = insReportD02Item.MonthTo,
                                ToMonth = insReportD02Item.MonthTo,
                                PayBackID = insReportD02Item.PayBackID,
                                IsPayBack = insReportD02Item.IsPayBack
                            };
                            lstInsuranceReportD02.Add(insuranceReportD02);
                        }
                    }
                }
            }

            lstInsuranceReportD02 = lstInsuranceReportD02.OrderBy(p => p.Status).ToList();
            var tang = TypeInsuranceD02TS.E_TANG.ToString();
            var giam = TypeInsuranceD02TS.E_GIAM.ToString();
            List<Ins_InsuranceReportD02Entity> lst = new List<Ins_InsuranceReportD02Entity>();

            var statuses = new List<string> { 
                TypeInsuranceD02TS.E_TANG.ToString(), 
                TypeInsuranceD02TS.E_TANG_LD.ToString(), 
                TypeInsuranceD02TS.E_TANG_BHTN.ToString(), 
                 TypeInsuranceD02TS.E_GIAM.ToString(), 
                TypeInsuranceD02TS.E_GIAM_LD.ToString(), 
                TypeInsuranceD02TS.E_TANG_LUONG.ToString(), 
                TypeInsuranceD02TS.E_GIAM_LUONG.ToString(), 
                TypeInsuranceD02TS.E_Dieu_Chinh.ToString(),            
                TypeInsuranceD02TS.E_DieuChinhTang.ToString(),            
                TypeInsuranceD02TS.E_DieuChinhGiam.ToString(),            
            };
            foreach (var item in statuses)
            {
                var lstInsuranceByStatus = lstInsuranceReportD02.Where(p => p.Status == item).OrderBy(p => p.ItemOrder).ToList();

                if (lstInsuranceByStatus.Any())
                {
                    Ins_InsuranceReportD02Entity ins_InsuranceReportD02 = new Ins_InsuranceReportD02Entity
                    {
                        ProfileName = GetDescription(item),
                        CodeEmp = GetOrderStatus(item)
                    };

                    lstInsuranceByStatus = lstInsuranceByStatus.OrderBy(p => p.Comment).ToList();
                    int stt = 1;
                    foreach (var insuranceStatus in lstInsuranceByStatus)
                    {
                        insuranceStatus.STT = stt.ToString();
                        stt++;
                    }
                    lstInsuranceByStatus.Insert(0, ins_InsuranceReportD02);
                    lst.AddRange(lstInsuranceByStatus);
                }
                else
                {
                    Ins_InsuranceReportD02Entity ins_InsuranceReportItemD02 = new Ins_InsuranceReportD02Entity
                    {
                        ProfileName = GetDescription(item),
                        CodeEmp = GetOrderStatus(item)
                    };
                    lst.Add(ins_InsuranceReportItemD02);
                }
            }

            #region filter searchNoteType

            if (searchNoteType != "All")
            {
                var statusSearch = searchNoteType;
                searchNoteType = GetDescription(searchNoteType);
                if (statusSearch == TypeInsuranceD02TS.E_DieuChinhTang.ToString() ||
                    statusSearch == TypeInsuranceD02TS.E_DieuChinhGiam.ToString() ||
                    statusSearch == TypeInsuranceD02TS.E_Dieu_Chinh.ToString()
                    )
                {
                    lst = lst.Where(p => (p.Comment == null || p.Comment == "") || p.Status == statusSearch).ToList();
                }
                else
                {
                    lst = lst.Where(p => (p.Comment == null || p.Comment == "") || p.Comment == searchNoteType).ToList();
                }
            }
            else
                    {
                var lstSearch = new List<string>();
                if (searchStatus == TypeInsuranceD02TS.E_TANG.ToString())
                {
                    lstSearch = new List<string> {
                     GetDescription(TypeInsuranceD02TS.E_TANG_LD.ToString()),
                     GetDescription(TypeInsuranceD02TS.E_TANG_TS.ToString()),
                     //GetDescription(TypeInsuranceD02TS.E_TANG_BENH.ToString()),
                     //GetDescription(TypeInsuranceD02TS.E_TANG_BHYT.ToString()),
                     GetDescription(TypeInsuranceD02TS.E_TANG_BHTN.ToString()),
                     GetDescription(TypeInsuranceD02TS.E_TANG_LEAVE_14WORKINGDAYS.ToString() )                   
                    };
                }
                if (searchStatus == TypeInsuranceD02TS.E_GIAM.ToString())
                {
                    lstSearch = new List<string> {
                    GetDescription(TypeInsuranceD02TS.E_GIAM_LD_BHYT.ToString()),
                    GetDescription(TypeInsuranceD02TS.E_GIAM_QUIT_SUSPENSE.ToString()),
                    GetDescription(TypeInsuranceD02TS.E_GIAM_TS.ToString()),
                    GetDescription(TypeInsuranceD02TS.E_GIAM_LEAVE_14WORKINGDAYS.ToString()),
                    GetDescription(TypeInsuranceD02TS.E_GIAM_LEAVE_PREMONTH_14WORKINGDAYS.ToString()),                    
                    //GetDescription(TypeInsuranceD02TS.E_GIAM_BENH.ToString())
                    };
                }
                if (searchStatus == TypeInsuranceD02TS.E_GIAM_LUONG.ToString())
                {
                    lstSearch = new List<string> {
                     GetDescription( TypeInsuranceD02TS.E_GIAM_LUONG.ToString()),
                      GetDescription(TypeInsuranceD02TS.E_GIAM_LUONG_CHANGEJOBNAME.ToString())
                    };
                }
                if (searchStatus == TypeInsuranceD02TS.E_TANG_LUONG.ToString())
                {
                    lstSearch = new List<string> {
                     GetDescription(TypeInsuranceD02TS.E_TANG_LUONG.ToString()),
                     GetDescription(TypeInsuranceD02TS.E_TANG_LUONG_CHANGEJOBNAME.ToString())
                    };
                }
                if (searchStatus == TypeInsuranceD02TS.E_CHANGEJOBNAME.ToString())
                {
                    lstSearch = new List<string> { 
                         GetDescription(TypeInsuranceD02TS.E_CHANGEJOBNAME.ToString()),
                    };
                }
                if (searchStatus == TypeInsuranceD02TS.E_Dieu_Chinh.ToString())
                {
                    lstSearch = new List<string> { 
                         TypeInsuranceD02TS.E_Dieu_Chinh.ToString(),
                         GetDescription(TypeInsuranceD02TS.E_DieuChinhGiam.ToString()),
                         GetDescription(TypeInsuranceD02TS.E_DieuChinhTang.ToString()),
                    };
                }
                if (lstSearch.Any())
                {
                    lst = lst.Where(p => (p.Comment == null || p.Comment == "") || lstSearch.Contains(p.Comment)).ToList();
                }

            }

            #endregion

            return lst;
        }

        #region Lấy thứ tự cột codeEmp cho bc D02TS - HonDa
        private string GetOrderStatus(string typeInsuranceD02TS)
        {
            string result = string.Empty;
            Dictionary<string, string> dicOrderStatus = new Dictionary<string, string>();
            dicOrderStatus.Add(TypeInsuranceD02TS.E_TANG.ToString(), "I");
            dicOrderStatus.Add(TypeInsuranceD02TS.E_TANG_LD.ToString(), "I.1");
            dicOrderStatus.Add(TypeInsuranceD02TS.E_TANG_BHTN.ToString(), "I.2");
            dicOrderStatus.Add(TypeInsuranceD02TS.E_GIAM.ToString(), "II");
            dicOrderStatus.Add(TypeInsuranceD02TS.E_GIAM_LD.ToString(), "II.1");
            dicOrderStatus.Add(TypeInsuranceD02TS.E_TANG_LUONG.ToString(), "III");
            dicOrderStatus.Add(TypeInsuranceD02TS.E_GIAM_LUONG.ToString(), "IV");
            dicOrderStatus.Add(TypeInsuranceD02TS.E_Dieu_Chinh.ToString(), "V");
            dicOrderStatus.Add(TypeInsuranceD02TS.E_DieuChinhTang.ToString(), "V.1");
            dicOrderStatus.Add(TypeInsuranceD02TS.E_DieuChinhGiam.ToString(), "V.2");

            if (dicOrderStatus.ContainsKey(typeInsuranceD02TS))
            {
                result = dicOrderStatus[typeInsuranceD02TS];
            }
            return result;
        }
        #endregion

        /// <summary> Lưu D02 </summary>
        public void ComputeD02(List<Ins_InsuranceReportD02Entity> lstD02, Ins_ReportD02Entity d02, DateTime dtMonthYear, DateTime dtStart, DateTime dtEnd, string periodD02)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoReportD02 = new Ins_ReportD02Repository(unitOfWork);
                var repoReportD02Item = new Ins_ReportD02ItemRepository(unitOfWork);
                var reportD02 = new Ins_ReportD02();
                var reportD02ID = Guid.Empty;
                var count = 0;
                count = repoReportD02.FindBy(p => p.IsDelete == null && p.DateMonth == dtMonthYear).Count();
                count++;

                //test
                count = 1;//test xoa sau

                //tự sinh ra kỳ
                var cutOfDuration = "Kỳ " + periodD02 + " - " + dtMonthYear.ToString(ConstantFormat.HRM_Format_MonthYear);
                var reportD02Existed = repoReportD02.FindBy(p => p.IsDelete == null && p.DateMonth.HasValue
                    && p.DateMonth.Value.Month == dtMonthYear.Month && p.DateMonth.Value.Year == dtMonthYear.Year
                    && p.ReportD02Name == cutOfDuration).FirstOrDefault();
                var d02ItemsExist = new List<Ins_ReportD02Item>();
                var d02ItemsModify = new List<Ins_ReportD02Item>();
                var d02ItemsAdding = new List<Ins_ReportD02Item>();
                #region Chỉnh Sửa
                if (reportD02Existed != null)
                {
                    var d02ID = reportD02Existed.ID;
                    reportD02ID = reportD02Existed.ID;

                    reportD02Existed.SociaInsCountPro = d02.SociaInsCountPro;
                    reportD02Existed.SociaInsTotalSalary = d02.SociaInsTotalSalary;
                    reportD02Existed.HealthInsCountPro = d02.HealthInsCountPro;
                    reportD02Existed.HealthInsTotalSalary = d02.HealthInsTotalSalary;
                    reportD02Existed.UnEmpInsCountPro = d02.UnEmpInsCountPro;
                    reportD02Existed.UnEmpInsTotalSalary = d02.UnEmpInsTotalSalary;
                    reportD02Existed.MaxSalary = d02.MaxSalary;
                    reportD02Existed.MinSalary = d02.MinSalary;
                    reportD02Existed.Type = d02.Type;
                    repoReportD02.Edit(reportD02Existed);
                    repoReportD02.SaveChanges();

                    var profileIds = lstD02.Select(m => m.ProfileID).ToList();
                    d02ItemsExist = unitOfWork.CreateQueryable<Ins_ReportD02Item>(Guid.Empty, p => profileIds.Contains(p.ProfileID) && p.ReportD02ID == d02ID && (p.IsPayBack == null || (p.IsPayBack != null && p.IsPayBack == false))).ToList();
                    if (d02ItemsExist.Any())
                    {
                        unitOfWork.RemoveObject(typeof(Ins_ReportD02Item), d02ItemsExist.ToArray());
                        unitOfWork.SaveChanges();
                    }
                    d02ItemsExist = repoReportD02Item.FindBy(p => p.IsDelete == null && p.ReportD02ID == d02ID && (p.IsPayBack == null || (p.IsPayBack != null && p.IsPayBack == false))).ToList();

                }
                #endregion

                #region Tạo mới
                if (lstD02.Count > 0 && reportD02Existed == null)
                {
                    //insert D02Report
                    reportD02 = new Ins_ReportD02
                    {
                        DateReport = DateTime.Now,
                        DateStart = dtStart,
                        DateEnd = dtEnd,
                        DateMonth = dtMonthYear,
                        ReportD02Name = cutOfDuration,
                        MaxSalary = d02.MaxSalary,
                        MinSalary = d02.MinSalary,
                        SociaInsCountPro = d02.SociaInsCountPro,
                        SociaInsTotalSalary = d02.SociaInsTotalSalary,
                        HealthInsCountPro = d02.HealthInsCountPro,
                        HealthInsTotalSalary = d02.HealthInsTotalSalary,
                        UnEmpInsCountPro = d02.UnEmpInsCountPro,
                        Type = d02.Type,
                        UnEmpInsTotalSalary = d02.UnEmpInsTotalSalary
                    };
                    repoReportD02.Add(reportD02);
                    repoReportD02.SaveChanges();
                    reportD02ID = reportD02.ID;
                }
                #endregion

                //get d02Item
                var d02ItemPayBack = unitOfWork.CreateQueryable<Ins_ReportD02Item>(Guid.Empty, m => m.ReportD02ID == reportD02ID && m.PayBackID != null).ToList();

                foreach (var insInsuranceReportD02Entity in lstD02)
                {
                    if (!insInsuranceReportD02Entity.ProfileID.HasValue || (insInsuranceReportD02Entity.ProfileID.HasValue && insInsuranceReportD02Entity.ProfileID.Value == Guid.Empty))
                    {
                        continue;
                    }

                    if (d02ItemPayBack.Where(m => m.ProfileID == insInsuranceReportD02Entity.ProfileID && m.PayBackID != null).FirstOrDefault() != null)
                    {
                        continue;
                    }

                    var notCardHeath = false;
                    bool.TryParse(insInsuranceReportD02Entity.NotCardHealth, out notCardHeath);
                    #region lay so thu tu
                    var itemOrder = 0;
                    var strItemOrder = string.Empty;
                    if (!string.IsNullOrEmpty(insInsuranceReportD02Entity.STT))
                    {
                        strItemOrder = insInsuranceReportD02Entity.STT.ToString();
                    }
                    int.TryParse(strItemOrder, out itemOrder);
                    #endregion
                    var reportD02Item = new Ins_ReportD02Item()
                    {
                        Comment = insInsuranceReportD02Entity.Comment,
                        ReportD02ID = reportD02ID,
                        ProfileID = insInsuranceReportD02Entity.ProfileID,
                        Type = insInsuranceReportD02Entity.Status1,
                        Status = insInsuranceReportD02Entity.Status,
                        RateHealthIns = insInsuranceReportD02Entity.RateBHYT,
                        RateSocialIns = insInsuranceReportD02Entity.RateBHXH,
                        RateUnEmpIns = insInsuranceReportD02Entity.RateBHTN,
                        NewBasicSalary = insInsuranceReportD02Entity.NewBasicSalary,
                        JobName = insInsuranceReportD02Entity.JobName,
                        Allowance1 = insInsuranceReportD02Entity.Allowance1,
                        Allowance2 = insInsuranceReportD02Entity.Allowance2,
                        Allowance3 = insInsuranceReportD02Entity.Allowance3,
                        AllowanceAdditional = insInsuranceReportD02Entity.AllowanceAdditional,
                        SocialInsPlaceID = insInsuranceReportD02Entity.SocialInsPlaceID,
                        NotCardHealth = notCardHeath,
                        ItemOrder = itemOrder,
                        MonthFrom = dtStart,
                        MonthTo = dtEnd
                    };

                    var d02ItemExisted = d02ItemsExist.Where(p => p.ProfileID == insInsuranceReportD02Entity.ProfileID && p.Status == insInsuranceReportD02Entity.Status).FirstOrDefault();
                    if (d02ItemExisted != null)
                    {
                        if (d02ItemExisted.Type != insInsuranceReportD02Entity.Status1
                            || d02ItemExisted.RateHealthIns != insInsuranceReportD02Entity.RateBHYT
                            || d02ItemExisted.RateSocialIns != insInsuranceReportD02Entity.RateBHXH
                            || d02ItemExisted.RateUnEmpIns != insInsuranceReportD02Entity.RateBHTN
                            || d02ItemExisted.NewBasicSalary != insInsuranceReportD02Entity.NewBasicSalary
                            || d02ItemExisted.NotCardHealth != notCardHeath
                            || d02ItemExisted.Comment != insInsuranceReportD02Entity.Comment
                            || d02ItemExisted.JobName != insInsuranceReportD02Entity.JobName
                            || d02ItemExisted.SocialInsPlaceID != insInsuranceReportD02Entity.SocialInsPlaceID)
                        {
                            d02ItemExisted.Type = insInsuranceReportD02Entity.Status1;
                            d02ItemExisted.RateHealthIns = insInsuranceReportD02Entity.RateBHYT;
                            d02ItemExisted.RateSocialIns = insInsuranceReportD02Entity.RateBHXH;
                            d02ItemExisted.RateUnEmpIns = insInsuranceReportD02Entity.RateBHTN;
                            d02ItemExisted.NewBasicSalary = insInsuranceReportD02Entity.NewBasicSalary;
                            d02ItemExisted.NotCardHealth = notCardHeath;
                            d02ItemExisted.Status = insInsuranceReportD02Entity.Status;
                            d02ItemExisted.Type = insInsuranceReportD02Entity.Status1;
                            d02ItemExisted.ItemOrder = itemOrder;
                            d02ItemExisted.MonthFrom = dtStart;
                            d02ItemExisted.MonthTo = dtEnd;
                            d02ItemExisted.JobName = insInsuranceReportD02Entity.JobName;
                            d02ItemExisted.Allowance1 = insInsuranceReportD02Entity.Allowance1;
                            d02ItemExisted.Allowance2 = insInsuranceReportD02Entity.Allowance2;
                            d02ItemExisted.Allowance3 = insInsuranceReportD02Entity.Allowance3;
                            d02ItemExisted.AllowanceAdditional = insInsuranceReportD02Entity.AllowanceAdditional;
                            d02ItemExisted.SocialInsPlaceID = insInsuranceReportD02Entity.SocialInsPlaceID;
                            d02ItemExisted.Comment = insInsuranceReportD02Entity.Comment;
                            d02ItemsModify.Add(d02ItemExisted);
                        }
                    }
                    else
                    {
                        d02ItemsAdding.Add(reportD02Item);
                    }
                }

                if (d02ItemsModify.Any())
                {
                    repoReportD02Item.Edit(d02ItemsModify);
                }
                if (d02ItemsAdding.Any())
                {
                    repoReportD02Item.Add(d02ItemsAdding);
                }

                if (d02ItemsModify.Any() || d02ItemsAdding.Any())
                {
                    //save change
                    repoReportD02Item.SaveChanges();
                }
            }
        }

        /// <summary> Set Data vào hashtable theo trạng thái </summary>
        /// <param name="hsData"></param>
        /// <param name="profile"></param>
        /// <param name="d02Reports"></param>
        /// <param name="_status">Trạng thái Group (Tăng , Giảm)</param>
        /// <param name="_status1">Trạng thai (tăng lao động,giảm lao động , giảm thai sản....)</param>
        /// <param name="_firstSal">Mức đóng tháng trước</param>
        /// <param name="_currentSal">Mức đóng tháng này</param>
        /// <param name="monthYear"></param>
        /// <param name="orgStructures"></param>
        /// <param name="orderGroup"></param>
        /// <returns></returns>
        private Hashtable SetHashtableData(Hashtable hsData, Hre_ProfileEntity profile, ref List<Ins_InsuranceReportD02Entity> d02Reports, string _status, String _status1, double? _firstSal, double? _currentSal, DateTime monthYear, List<Cat_OrgStructureEntity> orgStructures, string orderGroup)
        {
            bool isHOnda = true;
            _firstSal = _firstSal ?? 0;
            _currentSal = _currentSal ?? 0;
            var rptD02 = SetReportD02(profile, _status1, _firstSal.Value, _currentSal.Value, monthYear, orgStructures, _status, orderGroup);

            #region Xử Lý Gôm Nhóm Trạng Thái D02 - Dành cho Honda
            //honda
            //set lai status1 
            if (isHOnda)
            {
                if (_status1 == TypeInsuranceD02TS.E_TANG_TS.ToString() || _status1 == TypeInsuranceD02TS.E_TANG_LEAVE_14WORKINGDAYS.ToString()
                    || _status1 == TypeInsuranceD02TS.E_TANG_BENH.ToString() || _status1 == TypeInsuranceD02TS.E_TANG_BHYT.ToString())
                {
                    _status1 = TypeInsuranceD02TS.E_TANG_LD.ToString();
                }
                if (_status1 == TypeInsuranceD02TS.E_GIAM_TS_QUIT.ToString() || _status1 == TypeInsuranceD02TS.E_GIAM_TS.ToString()
                    || _status1 == TypeInsuranceD02TS.E_GIAM_BENH.ToString() || _status1 == TypeInsuranceD02TS.E_GIAM_LD_BHYT.ToString()
                    || _status1 == TypeInsuranceD02TS.E_GIAM_LD_NOT_BHYT.ToString()
                    || _status1 == TypeInsuranceD02TS.E_GIAM_LEAVE_PREMONTH_14WORKINGDAYS.ToString()
                    || _status1 == TypeInsuranceD02TS.E_GIAM_LEAVE_14WORKINGDAYS.ToString()
                    || _status1 == TypeInsuranceD02TS.E_GIAM_PREGNANT_14WORKINGDAYS.ToString()
                    || _status1 == TypeInsuranceD02TS.E_GIAM_QUIT_SUSPENSE.ToString()
                    )
                {
                    _status1 = TypeInsuranceD02TS.E_GIAM_LD.ToString();
                }
                if (_status1 == TypeInsuranceD02TS.E_GIAM_LUONG_CHANGEJOBNAME.ToString())
                {
                    _status1 = TypeInsuranceD02TS.E_GIAM_LUONG.ToString();
                }

                if (_status1 == TypeInsuranceD02TS.E_TANG_LUONG_CHANGEJOBNAME.ToString())
                {
                    _status1 = TypeInsuranceD02TS.E_TANG_LUONG.ToString();
                }
            }

            #endregion

            if (!hsData.ContainsKey(_status1))
            {
                d02Reports = new List<Ins_InsuranceReportD02Entity>();
                rptD02.STT = "1";
                d02Reports.Add(rptD02);
                hsData.Add(_status1, d02Reports);
            }
            else
            {
                var lstInsurances = (List<Ins_InsuranceReportD02Entity>)hsData[_status1];
                rptD02.STT = (lstInsurances.Count + 1).ToString();
                if (!lstInsurances.Any(m => m.Status == rptD02.Status && m.ProfileID == rptD02.ProfileID))
                {
                    lstInsurances.Add(rptD02);
                    hsData[_status1] = lstInsurances;
                }
            }
            return hsData;
        }

        /// <summary> set giá trị BC D02 </summary>
        /// <param name="profile"></param>
        /// <param name="status"></param>
        /// <param name="firstSal"></param>
        /// <param name="currentSal"></param>
        /// <param name="MonthYear"></param>
        /// <param name="orgStructures"></param>
        /// <param name="status1"></param>
        /// <param name="orderGroup">Số Thứ tự theo tăng , giảm (I,II,I.1)</param>
        /// <returns></returns>
        private Ins_InsuranceReportD02Entity SetReportD02(Hre_ProfileEntity profile, string status, double firstSal, double currentSal, DateTime MonthYear, List<Cat_OrgStructureEntity> orgStructures, string status1, string orderGroup)
        {
            var ins_d02 = new Ins_InsuranceReportD02Entity()
            {
                CodeEmp = profile.CodeEmp,
                Comment = GetDescription(status),
                ProfileName = profile.ProfileName,
                SocialInsNo = profile.SocialInsNo,
                OrderGroup = orderGroup,
                Status1 = status1,
                Status = status,
                JobTitle = profile.JobTitleName,
                Position = profile.PositionName,
                ProfileID = profile.ID,
                Gender = profile.Gender,
                DateOfBirth = profile.DateOfBirth.HasValue ? profile.DateOfBirth.Value.ToString(ConstantFormat.HRM_Format_DayMonthYear) : string.Empty,
                HireDate = profile.DateHire ?? DateTime.Now,
                EndProbationDate = profile.DateEndProbation ?? DateTime.Now,
                EndDate = profile.DateEnd ?? DateTime.Now,
                RateBHXH = RateSocial,
                RateBHYT = RateHealth,
                RateBHTN = RateUnEmp,
                WorkPlaceID = profile.WorkPlaceID,
                SocialInsPlaceID = profile.SocialInsPlaceID,
                Allowance1 = profile.Allowance1,
                Allowance2 = profile.Allowance2,
                Allowance3 = profile.Allowance3,
                Allowance4 = profile.Allowance4,
                AllowanceAdditional = profile.AllowanceAdditional,
                JobName = profile.JobName
            };

            if (string.IsNullOrEmpty(ins_d02.Position))
            {
                ins_d02.Position = ins_d02.JobTitle;
            }

            if (profile.OrgStructureID.HasValue)
            {
                ins_d02.CodeParentOrgLevel = GetCodeOrgStructure(orgStructures, profile.OrgStructureID ?? Guid.Empty);
            }

            if (status == TypeInsuranceD02TS.E_TANG_LUONG.ToString())
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = currentSal;
            }
            else if (status == TypeInsuranceD02TS.E_TANG_LUONG_CHANGEJOBNAME.ToString())
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = currentSal;
            }
            else if (status == TypeInsuranceD02TS.E_GIAM_LUONG.ToString()) //Ho tro masan xuat trang thai tru co luong ngay 12/08/2013
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = currentSal;
            }
            else if (status == TypeInsuranceD02TS.E_GIAM_LUONG_CHANGEJOBNAME.ToString())
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = currentSal;
            }
            else if (status == TypeInsuranceD02TS.E_GIAM_LEAVE_14WORKINGDAYS.ToString())
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = firstSal;
            }
            else if (status == TypeInsuranceD02TS.E_TANG_LD.ToString() || status == TypeInsuranceD02TS.E_TANG_BENH.ToString() || status == TypeInsuranceD02TS.E_TANG_TS.ToString() || status == TypeInsuranceD02TS.E_TANG_LEAVE_14WORKINGDAYS.ToString())
            {
                ins_d02.OldBasicSalary = 0;
                ins_d02.NewBasicSalary = currentSal;
            }

            else if (status == TypeInsuranceD02TS.E_GIAM_BHYT.ToString() || status == TypeInsuranceD02TS.E_TANG_BHYT.ToString())
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = currentSal;
                ins_d02.RateBHXH = 0;
                ins_d02.RateBHYT = RateHealth;
                ins_d02.RateBHTN = 0;
            }
            else if (status == TypeInsuranceD02TS.E_GIAM_BHTN.ToString() || status == TypeInsuranceD02TS.E_TANG_BHTN.ToString())
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = currentSal;
                ins_d02.RateBHXH = 0;
                ins_d02.RateBHYT = 0;
                ins_d02.RateBHTN = RateUnEmp;
            }
            else if (status == TypeInsuranceD02TS.E_GIAM_LD.ToString() || status == TypeInsuranceD02TS.E_GIAM_LD_BHYT.ToString())
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = firstSal;
            }
            else if (status == TypeInsuranceD02TS.E_GIAM_TS.ToString() || status == TypeInsuranceD02TS.E_GIAM_TS_QUIT.ToString())
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = firstSal;
            }
            else if (status == TypeInsuranceD02TS.E_GIAM_LD_NOT_BHYT.ToString() || status == TypeInsuranceD02TS.E_GIAM_LD_BHYT.ToString()
                || status == TypeInsuranceD02TS.E_GIAM_LD_BHYT_KOKIPTHOI.ToString() || status == TypeInsuranceD02TS.E_GIAM_QUIT_SUSPENSE.ToString()) //Ho tro masan xuat trang thai tru co luong ngay 12/08/2013
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = firstSal;
            }
            else if (status == TypeInsuranceD02TS.E_CHANGEJOBNAME.ToString())
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = currentSal;
            }
            else if (status == TypeInsuranceD02TS.E_GIAM_LD.ToString() || status == TypeInsuranceD02TS.E_GIAM_LD_BHYT.ToString()
                || status == TypeInsuranceD02TS.E_GIAM_LD_BHYT_KOKIPTHOI.ToString() || status == TypeInsuranceD02TS.E_GIAM_LD_NOT_BHYT.ToString())
            {
                ins_d02.OldBasicSalary = firstSal;
                ins_d02.NewBasicSalary = firstSal;
            }

            ins_d02.MonthNow = new DateTime(MonthYear.Year, MonthYear.Month, 1);
            ins_d02.TitleMonthNow = "Số:..........Tháng " + MonthYear.Month + " Năm " + MonthYear.Year;
            ins_d02.FromMonth = new DateTime(MonthYear.AddMonths(-1).Year, MonthYear.AddMonths(-1).Month, 1);
            ins_d02.ToMonth = new DateTime(MonthYear.Year, MonthYear.Month, 1);
            ins_d02.Status = status;
            //dr["CodeParentOrgLevel"] = HRService.GetCodeInListOrgParent(pro.OrgStructureID ?? Guid.Empty, lstStrucAll, 1);

            return ins_d02;

        }


        public static string GetDescription(string en)
        {
            try
            {
                TypeInsuranceD02TS enumTypeInsuranceD02TS = (TypeInsuranceD02TS)Enum.Parse(typeof(TypeInsuranceD02TS), en);

                Type type = enumTypeInsuranceD02TS.GetType();

                MemberInfo[] memInfo = type.GetMember(en);

                if (memInfo != null && memInfo.Length > 0)
                {
                    object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attrs != null && attrs.Length > 0)
                    {
                        return ((DescriptionAttribute)attrs[0]).Description;
                    }
                }
            }
            catch (Exception)
            { }
            return en;
        }

        public string GetCodeOrgStructure(List<Cat_OrgStructureEntity> ListOrg, Guid CurrentID)
        {

            string result = string.Empty;
            var model = ListOrg.Where(m => m.ID == CurrentID).FirstOrDefault();
            if (model != null)
            {
                var parent = ListOrg.Single(m => m.ID == model.ParentID);
                if (parent != null)
                {
                    result = parent.OrgStructureName;
                }
            }
            return result;
        }

        public static Boolean IsIncreaseInsurance(Ins_ProfileInsuranceMonthly insPreMonth
                                          , Ins_ProfileInsuranceMonthly insNow)
        {
            if (insPreMonth == null && insNow.IsSocialInsurance.HasValue && insNow.IsSocialInsurance.Value)
            {
                return true;
            }

            if (insPreMonth != null && insNow != null && insNow.IsSocialInsurance != null && (insPreMonth.IsSocialInsurance != null
                && (!insPreMonth.IsSocialInsurance.Value && insNow.IsSocialInsurance.Value)))
            {
                return true;
            }
            //Co them truong hop tang BHTN
            if (insPreMonth != null && insNow != null && insNow.IsUnEmpInsurance != null && (insPreMonth.IsUnEmpInsurance != null
                && (!insPreMonth.IsUnEmpInsurance.Value && insNow.IsUnEmpInsurance.Value)))
            {
                return true;
            }
            return false;
        }

        public static Boolean IsDecreaseInsurance(Ins_ProfileInsuranceMonthly insPreMonth, Ins_ProfileInsuranceMonthly insNow)
        {
            if (insPreMonth == null)
            {
                return false;
            }
            if (insPreMonth != null && insNow != null && insNow.IsSocialInsurance != null && (insPreMonth.IsSocialInsurance != null
                && (insPreMonth.IsSocialInsurance.Value && !insNow.IsSocialInsurance.Value)))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region D02TS Tail
        public List<Ins_InsuranceReportD02TailEntity> ReportD02Tail(string orgs, DateTime monthCheck, string userLogin)
        {
            string status = string.Empty;
            DateTime preMonth = monthCheck.AddMonths(-1);
            var d02Tails = new List<Ins_InsuranceReportD02TailEntity>();

            #region Lấy DS Ins_ProfileInsuranceMonthly tháng trước (so với tháng kiểm tra)
            var listInsMonthlyObjPre = new List<object> { orgs, preMonth, null, 1, int.MaxValue - 1 };
            if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
            {
                listInsMonthlyObjPre.Add("id");
            }
            var lstProfileInsuranceMonthlyInDbPre = GetData<Ins_ProfileInsuranceMonthly>(listInsMonthlyObjPre, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, userLogin, ref status).Translate<Ins_ProfileInsuranceMonthly>();
            #endregion

            #region Lấy DS Ins_ProfileInsuranceMonthly tháng N (so với tháng kiểm tra)
            var listInsMonthlyObj = new List<object> { orgs, monthCheck, null, 1, int.MaxValue - 1 };
            if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
            {
                listInsMonthlyObjPre.Add("id");
            }
            var lstProfileInsuranceMonthlyInDb = GetData<Ins_ProfileInsuranceMonthly>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, userLogin, ref status).Translate<Ins_ProfileInsuranceMonthly>();
            #endregion

            #region test

            var E_TANG_LD = TypeInsuranceD02TS.E_TANG_LD.ToString();
            //giam
            var E_GIAM_LD = TypeInsuranceD02TS.E_GIAM_LD.ToString();
            var E_TANG_BHYT = TypeInsuranceD02TS.E_TANG_BHYT.ToString();
            var E_GIAM_BHYT = TypeInsuranceD02TS.E_GIAM_BHYT.ToString();
            var E_GIAM_LD_BHYT = TypeInsuranceD02TS.E_GIAM_LD_BHYT.ToString();
            var E_GIAM_LD_BHYT_KOKIPTHOI = TypeInsuranceD02TS.E_GIAM_LD_BHYT_KOKIPTHOI.ToString();
            var E_GIAM_LD_NOT_BHYT = TypeInsuranceD02TS.E_GIAM_LD_NOT_BHYT.ToString();
            var E_TANG_LUONG = TypeInsuranceD02TS.E_TANG_LUONG.ToString();
            var E_TANG_LUONG_CHANGEJOBNAME = TypeInsuranceD02TS.E_TANG_LUONG_CHANGEJOBNAME.ToString();
            var E_GIAM_LUONG = TypeInsuranceD02TS.E_GIAM_LUONG.ToString();
            var E_GIAM_LUONG_CHANGEJOBNAME = TypeInsuranceD02TS.E_GIAM_LUONG_CHANGEJOBNAME.ToString();

            //giam BHTN
            var E_GIAM_BHTN = TypeInsuranceD02TS.E_GIAM_BHTN.ToString();
            var E_TANG_BHTN = TypeInsuranceD02TS.E_TANG_BHTN.ToString();

            int tangBHXH = 0;
            int giamBHXH = 0;
            int tangBHYT = 0;
            int giamBHYT = 0;
            int tangBHTN = 0;
            int giamBHTN = 0;

            double tangLuongBHXH = 0;
            double giamLuongBHXH = 0;
            double tangLuongBHYT = 0;
            double giamLuongBHYT = 0;
            double tangLuongBHTN = 0;
            double giamLuongBHTN = 0;
            #endregion

            #region duyet profiles
            var profileIds = lstProfileInsuranceMonthlyInDb.Select(p => p.ProfileID).ToList();
            foreach (var profileId in profileIds)
            {
                var profileMonthlys = lstProfileInsuranceMonthlyInDb.Where(p => p.ProfileID == profileId).OrderBy(p => p.MonthYear).ToList();
                var profileMonthlyPrevious = lstProfileInsuranceMonthlyInDbPre.Where(p => p.ProfileID == profileId).OrderBy(p => p.MonthYear).ToList();


                var inFirst = profileMonthlyPrevious.FirstOrDefault();
                var inNow = profileMonthlys.FirstOrDefault();

                if (inFirst == null)
                {
                    inFirst = new Ins_ProfileInsuranceMonthly
                    {
                        IsHealthInsurance = false,
                        IsSocialInsurance = false,
                        IsUnEmpInsurance = false
                    };
                }

                if (inNow != null)
                {
                    #region tang

                    if (inFirst != null && inNow != null
                        && (inFirst.IsSocialInsurance.HasValue && !inFirst.IsSocialInsurance.Value)
                        && (inNow.IsSocialInsurance.HasValue && inNow.IsSocialInsurance.Value))
                    {
                        tangBHXH++;
                        tangLuongBHXH += inNow.SalaryInsurance ?? 0;
                    }
                    if (inFirst != null && inNow != null
                        && (inFirst.IsHealthInsurance.HasValue && !inFirst.IsHealthInsurance.Value)
                        && (inNow.IsHealthInsurance.HasValue && inNow.IsHealthInsurance.Value))
                    {
                        tangBHYT++;
                        tangLuongBHYT += inNow.SalaryInsurance ?? 0;
                    }
                    if (inFirst != null && inNow != null
                        && (inFirst.IsUnEmpInsurance.HasValue && !inFirst.IsUnEmpInsurance.Value)
                        && (inNow.IsUnEmpInsurance.HasValue && inNow.IsUnEmpInsurance.Value))
                    {
                        tangBHTN++;
                        tangLuongBHTN += inNow.SalaryInsurance ?? 0;
                    }

                    #endregion
                    #region Giam

                    if (inFirst != null && inNow != null
                        && (inFirst.IsSocialInsurance.HasValue && inFirst.IsSocialInsurance.Value)
                        && (inNow.IsSocialInsurance.HasValue && !inNow.IsSocialInsurance.Value))
                    {
                        giamBHXH++;
                        giamLuongBHXH += inNow.SalaryInsurance ?? 0;
                    }

                    if (inFirst != null && inNow != null
                        && (inFirst.IsHealthInsurance.HasValue && inFirst.IsHealthInsurance.Value)
                        && (inNow.IsHealthInsurance.HasValue && !inNow.IsHealthInsurance.Value))
                    {
                        giamBHYT++;
                        giamLuongBHYT += inNow.SalaryInsurance ?? 0;
                    }

                    #endregion
                }
            }
            #endregion

            #region xử lý
            Ins_InsuranceReportD02TailEntity row1 = new Ins_InsuranceReportD02TailEntity();
            Ins_InsuranceReportD02TailEntity row2 = new Ins_InsuranceReportD02TailEntity();

            #region Row1
            //thang N-1
            var prePeriodInsHealthAmount = lstProfileInsuranceMonthlyInDbPre.Where(p => p.IsHealthInsurance.HasValue && p.IsHealthInsurance.Value && p.IsDelete == null).Count();
            var prePeriodInsSocialAmount = lstProfileInsuranceMonthlyInDbPre.Where(p => p.IsSocialInsurance.HasValue && p.IsSocialInsurance.Value && p.IsDelete == null).Count();
            var prePeriodInsUnEmpAmount = lstProfileInsuranceMonthlyInDbPre.Where(p => p.IsUnEmpInsurance.HasValue && p.IsUnEmpInsurance.Value && p.IsDelete == null).Count();

            row1 = new Ins_InsuranceReportD02TailEntity
            {
                STT = 1,
                Name = " Số lao động",
                InsSocialIncreaseAmount = tangBHXH,
                InsSocialDecreaseAmount = giamBHXH,
                InsHealthIncreaseAmount = tangBHYT,
                InsHealthDecreaseAmount = giamBHYT,
                InsUnEmpIncreaseAmount = tangBHTN,
                InsUnEmpDecreaseAmount = giamBHTN,
                PrePeriodInsHealthAmount = prePeriodInsHealthAmount,
                PrePeriodInsSocialAmount = prePeriodInsSocialAmount,
                PrePeriodInsUnEmpAmount = prePeriodInsUnEmpAmount,
            };

            d02Tails.Add(row1);
            #endregion

            #region row2
            //Thang N-1
            var salaryInsurancePre = lstProfileInsuranceMonthlyInDbPre.Where(p => p.IsSocialInsurance.HasValue && p.IsSocialInsurance.Value && p.SalaryInsurance.HasValue && p.IsDelete == null).Sum(p => p.SalaryInsurance ?? 0);
            var salaryHealthInsurancePre = lstProfileInsuranceMonthlyInDbPre.Where(p => p.IsHealthInsurance.HasValue && p.IsHealthInsurance.Value && p.SalaryHealthInsurance.HasValue && p.IsDelete == null).Sum(p => p.SalaryHealthInsurance ?? 0);
            var salaryUnEmpInsurancePre = lstProfileInsuranceMonthlyInDbPre.Where(p => p.IsUnEmpInsurance.HasValue && p.IsUnEmpInsurance.Value && p.SalaryUnEmpInsurance.HasValue && p.IsDelete == null).Sum(p => p.SalaryUnEmpInsurance ?? 0);

            row2 = new Ins_InsuranceReportD02TailEntity
            {
                STT = 2,
                Name = "Quỹ Lương",
                InsSocialIncreaseAmount = tangLuongBHXH,
                InsSocialDecreaseAmount = giamLuongBHXH,
                InsHealthIncreaseAmount = tangLuongBHYT,
                InsHealthDecreaseAmount = giamLuongBHYT,
                InsUnEmpIncreaseAmount = tangLuongBHTN,
                InsUnEmpDecreaseAmount = giamLuongBHTN,
                PrePeriodInsHealthAmount = salaryHealthInsurancePre,
                PrePeriodInsSocialAmount = salaryInsurancePre,
                PrePeriodInsUnEmpAmount = salaryUnEmpInsurancePre,
            };
            #endregion

            d02Tails.Add(row2);

            #endregion

            return d02Tails;
        }
        #endregion

        #region Mẫu Báo Cáo D03TS

        public List<Ins_InsuranceReportD03Entity> LoadDataD03(bool? all, bool? increase, bool? descrease, DateTime? dtMonthYear, string orgIds, string searchNoteType, string searchStatus, string codeEmp, List<Guid> workPlaceIDs, string userLogin)
        {

            var monthYear = dtMonthYear ?? DateTime.Now;
            DateTime dateFrom = monthYear.AddMonths(-1);
            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, InsuranceServices.PeriodInsuranceDayPreMonthDefault);
            DateTime dateTo = new DateTime(monthYear.Year, monthYear.Month, InsuranceServices.PeriodInsuranceDayCurrentMonthDefault);
            var status = string.Empty;
            var allCheck = all ?? false;
            var inCreaseCheck = increase ?? false;
            var descreaseCheck = descrease ?? false;


            //lay ds Ins_ProfileInsuranceMonthly
            //List<object> listInsMonthlyObj = new List<object>();
            //listInsMonthlyObj.Add(orgs);
            //listInsMonthlyObj.Add(monthYear);
            //var profileInsMonthlys = GetData<Hre_ProfileEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, ref status);
            Hashtable htbData = new Hashtable();
            var d02Reports = new List<Ins_InsuranceReportD02Entity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var profileInsMonthly = new Ins_ProfileInsuranceMonthlyRepository(unitOfWork);
                var orgStructureRepo = new Cat_OrgStructureRepository(unitOfWork);
                var insInsuranceRecord = new Ins_InsuranceRecordRepository(unitOfWork);
                var hre_ContractRepo = new Hre_ContractRepository(unitOfWork);
                var cat_ContractTypeRepo = new Cat_ContractTypeRepository(unitOfWork);
                var att_LeaveDayRepo = new Att_LeavedayRepository(unitOfWork);

                #region Lấy DS Ins_ProfileInsuranceMonthly (lấy tất cả)
                var listInsMonthlyObj = new List<object> { orgIds, null, null, 1, int.MaxValue - 1 };
                if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                {
                    listInsMonthlyObj.Add("id");
                }
                var lstProfileInsuranceMonthlyInDb = GetData<Ins_ProfileInsuranceMonthlyEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, userLogin, ref status);
                #endregion

                //get profiles trong Ins_ProfileInsuranceMonthly theo thang kiểm tra
                var lstProfile = lstProfileInsuranceMonthlyInDb.Where(s => s.MonthYear.HasValue
                    && (s.MonthYear.Value.Year == dateFrom.Year || s.MonthYear.Value.Year == dateTo.Year)
                    && (s.MonthYear.Value.Month == dateFrom.Month || s.MonthYear.Value.Month == dateTo.Month))
                    .ToList().Translate<Ins_ProfileInsuranceMonthly>();

                #region lay ins_ProfileInsuranceMonthly de tinh thai san 1 nam lien ke
                var dtPreDateCheck = dateFrom.AddYears(-1);
                //Lấy Ds Ins_ProfileInsuranceMonthly 12 tháng trước so với tháng kiểm tra (de tinh thai san 1 nam lien ke)
                var lstProfilePregnant = lstProfileInsuranceMonthlyInDb.Where(s => s.MonthYear.HasValue
                    && (dtPreDateCheck <= s.MonthYear.Value && s.MonthYear <= dateFrom)).ToList();
                #endregion


                //var lstProfile = profileInsMonthly.FindBy(s => s.IsDelete == null && s.MonthYear.HasValue
                //    && (s.MonthYear.Value.Year == dateFrom.Year || s.MonthYear.Value.Year == dateTo.Year)
                //    && (s.MonthYear.Value.Month == dateFrom.Month || s.MonthYear.Value.Month == dateTo.Month)).ToList();

                //get InsuranceRecord (Chứng từ bảo hiểm)
                //var lstInsuranceRecord = insInsuranceRecord.GetAll().Where(p => p.IsDelete == null).ToList();// lay tat ca chứng từ bảo hiểm (sau nay sửa lai sau)


                #region Lay ds Phong Ban
                var orgs = GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).ToList();
                List<Cat_OrgStructureEntity> orgStructures = orgs.Translate<Cat_OrgStructureEntity>();
                // var orgs = orgStructureRepo.GetAll();
                //  var orgIds = orgs.Select(p => p.ID).ToList();
                #endregion

                #region Lấy profile theo phong ban
                List<object> listObj = new List<object>();
                listObj.Add(orgIds);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);
                var profilebyOrgs = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status);
                if (profilebyOrgs != null)
                {
                    profilebyOrgs = profilebyOrgs.Where(p => p.DateHire != null
                                && (p.DateQuit == null || p.DateQuit > dateFrom)
                                && p.DateHire < monthYear).ToList();
                }
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    profilebyOrgs = profilebyOrgs.Where(p => p.CodeEmp == codeEmp).ToList();
                }

                var profileIds = profilebyOrgs.Select(p => p.ID).ToList();
                #endregion

                #region  lay ds hop dong đã ký so với tháng chọn (Loại HD có đóng BH)
                //var contractTypeObjs = cat_ContractTypeRepo.FindBy(p => p.IsDelete == null).ToList();
                ////Ds loại HĐ đóng BHXH
                //var contractSocialTypeIDObjs = contractTypeObjs.Where(p => p.IsSocialInsurance.HasValue && p.IsSocialInsurance.Value).ToList();
                ////Ds loại HĐ đóng BHTN
                //var contractUnEmpTypeIDObjs = contractTypeObjs.Where(p => p.IsUnEmployInsurance.HasValue && p.IsUnEmployInsurance.Value).ToList();
                //var contracts = new List<Hre_Contract>();
                //var contractSocials = new List<Hre_Contract>();
                //var contractUnEmps = new List<Hre_Contract>();
                //if (contractSocialTypeIDObjs.Any())
                //{
                //    var contractSocialTypeIds = contractSocialTypeIDObjs.Select(p => p.ID).ToList();
                //    var contractUnEmpTypeIds = contractUnEmpTypeIDObjs.Select(p => p.ID).ToList();
                //    //Ds Hop Đồng của cac NV
                //    contracts = hre_ContractRepo.FindBy(p => p.IsDelete == null && profileIds.Contains(p.ProfileID)).OrderBy(p => p.DateStart).ToList();
                //    //Ds Hop đồng thuộc loại đóng BHXH
                //    contractSocials = contracts.Where(p => contractSocialTypeIds.Contains(p.ContractTypeID)).OrderBy(p => p.DateStart).ToList();
                //    //Ds Hop đồng thuộc loại đóng BHTN
                //    contractUnEmps = contracts.Where(p => contractUnEmpTypeIds.Contains(p.ContractTypeID)).OrderBy(p => p.DateStart).ToList();
                //}

                #endregion

                #region lấy Leaveday
                //string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                //string E_PREGNANCY_SUCKLE = InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString();

                //var lstLeaveday = att_LeaveDayRepo.FindBy(m => m.IsDelete == null && m.Status == E_APPROVED && m.DateStart <= dateTo && m.DateEnd >= dateFrom && m.Cat_LeaveDayType != null
                //    && m.Cat_LeaveDayType.InsuranceType == E_PREGNANCY_SUCKLE && profileIds.Contains(m.ProfileID)).ToList();
                #endregion

                //List<object> salInsuranceSalaryParams = new List<object>();
                //salInsuranceSalaryParams.Add(null);
                //salInsuranceSalaryParams.Add(null);
                //salInsuranceSalaryParams.Add(orgIds);
                //salInsuranceSalaryParams.Add(null);
                //salInsuranceSalaryParams.Add(null);
                //salInsuranceSalaryParams.Add(null);
                //salInsuranceSalaryParams.Add(null);
                //salInsuranceSalaryParams.Add(1);
                //salInsuranceSalaryParams.Add(50000);
                //var lstInsuranceSalaryByProfile = GetData<Sal_InsuranceSalary>(salInsuranceSalaryParams, ConstantSql.hrm_sal_sp_get_InsuranceSalary, ref status)
                //    .Where(p => p.DateEffect <= dateTo && profileIds.Contains(p.ProfileID ?? Guid.Empty) && p.InsuranceAmount.HasValue).ToList();


                if (workPlaceIDs != null && workPlaceIDs.Any())
                {
                    workPlaceIDs = workPlaceIDs.Where(p => p != null && p != Guid.Empty).ToList();
                    if (workPlaceIDs.Any())
                    {
                        lstProfile = lstProfile.Where(p => workPlaceIDs != null && workPlaceIDs.Count > 0 && workPlaceIDs.Contains(p.WorkPlaceID ?? Guid.Empty)).ToList();
                    }
                }

                // workPlaceID = workPlaceID.Where(p => p != null).ToList();
                foreach (var profile in profilebyOrgs)
                {
                    //lay các chung tu bao hiem cua nv
                    //    List<Ins_InsuranceRecord> lstInsRecordProfile = lstInsuranceRecord.Where(rec => rec.ProfileID == profile.ID).ToList();
                    //    var lstleavePregByProfile = lstLeaveday.Where(rec => rec.ProfileID == profile.ID).ToList();
                    //   var lstInsRecordPregByProfile = lstInsRecordProfile.Where(m => m.InsuranceType == E_PREGNANCY_SUCKLE).ToList();


                    var profileMonthlys = lstProfile.Where(p => p.ProfileID == profile.ID).OrderBy(p => p.MonthYear).ToList();



                    var inFirst = profileMonthlys.FirstOrDefault(p => p.MonthYear.HasValue && p.MonthYear.Value.Month == dateFrom.Month);
                    var inNow = profileMonthlys.LastOrDefault(p => p.MonthYear.HasValue && p.MonthYear.Value.Month == dateTo.Month);

                    if (inFirst == null)
                    {
                        inFirst = new Ins_ProfileInsuranceMonthly
                        {
                            IsHealthInsurance = false,
                            IsSocialInsurance = false,
                            IsUnEmpInsurance = false
                        };
                    }

                    if (inNow != null)
                    {
                        profile.JobName = inNow.JobName;
                        #region set Phu cap
                        if (inNow != null)
                        {
                            profile.Allowance1 = inNow.Allowance1;
                            profile.Allowance2 = inNow.Allowance2;
                            profile.Allowance3 = inNow.Allowance3;
                            profile.Allowance4 = inNow.Allowance4;
                        }
                        #endregion

                        #region tang

                        if (IsIncreaseInsurance(inFirst, inNow) && (inCreaseCheck == true || allCheck == true))
                        {

                            #region Tăng BHYT
                            if ((!inFirst.IsHealthInsurance.HasValue || (inFirst.IsHealthInsurance.HasValue &&
                               !inFirst.IsHealthInsurance.Value))
                               && inNow.IsHealthInsurance.HasValue && inNow.IsHealthInsurance.Value)
                            {
                                SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_TANG.ToString(), TypeInsuranceD02TS.E_TANG_BHYT.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "I");
                            }
                            #endregion

                        }
                        #endregion

                        #region giam
                        if (IsDecreaseInsurance(inFirst, inNow) && (descreaseCheck == true || allCheck == true))
                        {
                            #region giam
                            if ((inFirst.IsHealthInsurance.HasValue && inFirst.IsHealthInsurance.Value)
                            && !inNow.IsHealthInsurance.HasValue || (inNow.IsHealthInsurance.HasValue && !inNow.IsHealthInsurance.Value))
                            {
                                SetHashtableData(htbData, profile, ref d02Reports, TypeInsuranceD02TS.E_GIAM.ToString(), TypeInsuranceD02TS.E_GIAM_BHYT.ToString(), inFirst.SalaryInsurance, inNow.SalaryInsurance, monthYear, orgStructures, "II");
                            }
                            #endregion
                        }
                        #endregion

                    }
                }
            }

            //duyet hashtable => danh sach nv theo loai bao hiem (tangLd,tangBHXH,.....)
            var result = new List<Ins_InsuranceReportD02Entity>();
            var tang = TypeInsuranceD02TS.E_TANG.ToString();
            var giam = TypeInsuranceD02TS.E_GIAM.ToString();
            var tangLD = TypeInsuranceD02TS.E_GIAM_LD.ToString();
            foreach (DictionaryEntry entry in htbData)
            {
                var lst = new List<Ins_InsuranceReportD02Entity>();
                lst = (List<Ins_InsuranceReportD02Entity>)entry.Value;
                // lst = lst.OrderBy(p => p.STT).ToList();
                var d02Entity = lst.FirstOrDefault(p => p.OrderGroup != null);
                var orderGroup = string.Empty;
                if (d02Entity != null)
                {
                    orderGroup = d02Entity.OrderGroup;
                }

                var proName = entry.Key.ToString();
                if (!string.IsNullOrEmpty(proName))
                {
                    proName = GetDescription(proName);
                }
                var title = new Ins_InsuranceReportD02Entity()
                {
                    ProfileID = Guid.Empty,
                    ProfileName = proName,
                    OrderGroup = orderGroup,
                    CodeEmp = orderGroup,
                    BOLD = true.ToString()
                };

                lst.Insert(0, title);
                if (lst.Any(p => p.Status1 == tang) && !result.Any(p => p.Status == tangLD) && !result.Any(p => p.ProfileName == "Tăng"))
                {
                    lst.Insert(0, new Ins_InsuranceReportD02Entity { ProfileID = Guid.Empty, ProfileName = "Tăng", CodeEmp = "I", OrderGroup = "I" });
                }
                if (lst.Any(p => p.Status1 == giam) && !result.Any(p => p.ProfileName == "Giảm"))
                {
                    lst.Insert(0, new Ins_InsuranceReportD02Entity { ProfileID = Guid.Empty, ProfileName = "Giảm", CodeEmp = "II", OrderGroup = "II" });
                }
                //if (lst.Any(p => p.Status == TypeInsuranceD02TS.E_GIAM_LD.ToString()) && !result.Any(p => p.Status1 == giam) && !result.Any(p => p.ProfileName == "Giảm Lao Động"))
                //{
                //    lst.Insert(0, new Ins_InsuranceReportD02Entity { ProfileName = "Giảm Lao Động", CodeEmp = "II", OrderGroup = "II" });
                //}
                result.AddRange(lst);

            }

            result = result.Where(p => p.ProfileName != "Tăng Lao Động").ToList();
            result = result.Where(p => p.ProfileName != "Giảm BHYT").ToList();
            //var result = LoadData(all, increase, descrease, dtMonthYear, orgIds, searchNoteType, searchStatus, codeEmp, workPlaceIDs);
            //var E_TANG_BHYT = TypeInsuranceD02TS.E_TANG_BHYT.ToString();
            //var E_GIAM_BHYT = TypeInsuranceD02TS.E_GIAM_BHYT.ToString();
            //result = result.Where(p => p.Status == E_TANG_BHYT || p.Status == E_GIAM_BHYT).ToList();
            var listD03 = new List<Ins_InsuranceReportD03Entity>();
            foreach (var item in result)
            {
                var d03 = item.Copy<Ins_InsuranceReportD03Entity>();
                listD03.Add(d03);
            }
            return listD03.OrderBy(p => p.OrderGroup).ToList();
        }

        #endregion

        #region C70A
        DataTable getSchema_ReportC70A(string co, DateTime ms, DateTime ds, DateTime de)
        {
            DataTable tblData = new DataTable("Ins_C70aReportEntity");
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.Stt, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.CodeEmp, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.ProfileName, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.Birthday, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.SocialInsNo, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.SalaryInsurance, typeof(Double));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.MonthJoinInsurance, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.LeaveInMonth, typeof(int));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.LeaveInYear, typeof(int));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.Money, typeof(Double));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.StatusGroup, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.DateStart, typeof(DateTime));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.DateEnd, typeof(DateTime));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.Notes, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.Notes1, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.FemaleBirthYear, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.MaleBirthYear, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.Status, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.GroupName, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.BeginSocialInsIssueDate, typeof(String));
            tblData.Columns.Add(Ins_C70aReportEntity.FieldNames.CodeParentOrgLevel, typeof(String));

            DataColumn _ds = new DataColumn(Ins_C70aReportEntity.FieldNames.SDateStart);
            _ds.DataType = typeof(DateTime);
            _ds.DefaultValue = ds;
            tblData.Columns.Add(_ds);

            DataColumn _de = new DataColumn(Ins_C70aReportEntity.FieldNames.SDateEnd);
            _de.DataType = typeof(DateTime);
            _de.DefaultValue = de;
            tblData.Columns.Add(_de);

            DataColumn _ms = new DataColumn(Ins_C70aReportEntity.FieldNames.SMonth);
            _ms.DataType = typeof(DateTime);
            _ms.DefaultValue = ms;
            tblData.Columns.Add(_ms);

            DataColumn _co = new DataColumn(Ins_C70aReportEntity.FieldNames.CutOffDurationName);
            _co.DataType = typeof(String);
            _co.DefaultValue = co;
            tblData.Columns.Add(_co);
            return tblData;
        }

        String _strTOTAL = "CỘNG";
        String _strSUMTOTAL = "TỔNG CỘNG";

        int totalEmployees = 0;
        int totalFemales = 0;
        int totalEmployeesOrg = 0;
        int totalFemalesOrg = 0;
        Double totalSalaryInMonth = 0;
        Double totalSalaryInMonthOrg = 0;
        List<Cat_ValueEntity> _lstMinSalary = new List<Cat_ValueEntity>();
        public List<Cat_ValueEntity> ListMinSalary
        {
            get
            {
                return _lstMinSalary;
            }
            set
            {
                _lstMinSalary = value;
            }
        }

        List<Cat_ValueEntity> _lstMaxSalary = new List<Cat_ValueEntity>();
        public List<Cat_ValueEntity> ListMaxSalary
        {
            get
            {
                return _lstMaxSalary;
            }
            set
            {
                _lstMaxSalary = value;
            }
        }
        Double totalSalaryInsurance = 0;
        Double totalMoney = 0;
        Double totalMoneyNotRound = 0;
        Double totalLeaveInYear = 0;
        Double totalLeaveInMonth = 0;

        Double _TotalSalaryIns = 0;
        Double _TotalMoney = 0;
        Double _TotalMoneyNotRound = 0;
        Double _TotalLeaveInYear = 0;
        Double _TotalLeaveInMonth = 0;
        int Stt = 0;
        bool ckb_ShowAll = false;


        public DataTable GetReportC70A(DateTime DateMonth, DateTime DateFrom, DateTime DateTo, string strOrgStructureON, string CutOffName, bool isCreateTemplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tblData = getSchema_ReportC70A(CutOffName, DateMonth, DateFrom, DateTo);
                if (isCreateTemplate)
                {
                    return tblData;
                }
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_Position = new CustomBaseRepository<Cat_Position>(unitOfWork);
                var repoCat_ValueEntity = new CustomBaseRepository<Cat_ValueEntity>(unitOfWork);
                var repoCat_ExchangeRate = new CustomBaseRepository<Cat_ExchangeRate>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);

                var repoIns_InsuranceRecord = new CustomBaseRepository<Ins_InsuranceRecord>(unitOfWork);
                var repoSys_AllSetting = new CustomBaseRepository<Sys_AllSetting>(unitOfWork);

                var repoSal_BasicSalary = new CustomBaseRepository<Sal_BasicSalary>(unitOfWork);
                var repoSal_Grade = new CustomBaseRepository<Sal_Grade>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);

                //List<object> lstParam = new List<object>();
                //lstParam.AddRange(new object[3]);
                //lstParam[0] = (object)strOrgStructureON;
                //var dataProfile = GetData<Hre_ProfileEntity>(lstParam, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

                var dataProfile = repoHre_Profile.FindBy(s => s.IsDelete == null).ToList();

                #region get data

                var lstStrucAll = repoCat_OrgStructure.FindBy(m => m.ID != null).ToList();
                List<Cat_OrgStructure> lstStruc;

                if (strOrgStructureON != null)
                {
                    var lstOrderNumberORG = strOrgStructureON.Split(',').Select(s => int.Parse(s)).Distinct().ToList();
                    lstStruc = lstStrucAll.Where(m => lstOrderNumberORG.Contains(m.OrderNumber)).ToList();
                }
                else
                {
                    lstStruc = lstStrucAll;
                }
                List<Guid> lstStrucId = lstStruc.Select(prop => prop.ID).ToList();

                //Lay muc luong toi thieu
                String statusMinumunSal = EnumDropDown.ValueEntityType.E_MINIMUM_SALARY.ToString();
                ListMinSalary = repoCat_ValueEntity.FindBy(val => val.Type == statusMinumunSal).ToList();

                //Lay muc tran dong BHXH
                String statusInsCape = EnumDropDown.ValueEntityType.E_INSURANCE_CAPE_AMOUNT.ToString();
                ListMaxSalary = repoCat_ValueEntity.FindBy(val => val.Type == statusInsCape).OrderByDescending(pit => pit.DateOfEffect).ToList();

                DateTime MonthYear = new DateTime(DateMonth.Year
                                                     , DateFrom.Month
                                                     , InsuranceServices.PeriodInsuranceDayCurrentMonthDefault);
                DateTime _from = DateFrom;
                DateTime _to = DateTo;
                Att_AttendanceServices.GetSalaryMonthRange(MonthYear, out _from, out _to);
                //InsuranceRecordType.E_SICK_SHORT Bản Thân Ốm Ngắn Ngày
                //InsuranceRecordType.E_SICK_LONG Bản Thân Ốm Dài Ngày
                //InsuranceRecordType.E_SICK_CHILD Con Ốm

                string e_SICK_SHORT = InsuranceRecordType.E_SICK_SHORT.ToString();

                string e_SICK_LONG = InsuranceRecordType.E_SICK_LONG.ToString();

                string e_SICK_CHILD = InsuranceRecordType.E_SICK_CHILD.ToString();

                string e_Approve = LeaveDayStatus.E_APPROVED.ToString();

                string e_FullShift = LeaveDayDurationType.E_FULLSHIFT.ToString();

                //Lay DS BHXH trong ca nam (de khong phai xuong database nhieu lan trong truong hop LeaveInYear) 
                //Lay record nam ngoai truong hop nghi nam truoc sang nam sau lam chung tu thi luy ke phai cua nam ngoai.
                DateTime yearOld = DateFrom.AddYears(-1);
                //string[] strIncludePro = new string[] { ClassNames.Hre_Profile, ClassNames.Hre_Profile + "." + ClassNames.Cat_OrgStructure };
                List<Ins_InsuranceRecord> list_InsYear = repoIns_InsuranceRecord.FindBy(ir => ir.IsDelete == null && (ir.RecordDate.Year == MonthYear.Year || ir.RecordDate.Year == yearOld.Year)).ToList();

                //Lay DS BHXH trong Ky

                List<Ins_InsuranceRecord> list_InsMonth = list_InsYear.Where(ir => ir.RecordDate <= DateTo && ir.RecordDate >= DateFrom).ToList();

                //List<Ins_LeaveDayIns> list_Ins = EntityService.GetEntityList<Ins_LeaveDayIns>(EntityService.GuidMainContext, LoginUserID.Value, ir => ir.DateStart.Month == MonthYear.Year
                //    && ir.Status == e_Approve);

                //List<Ins_InsuranceSalary> listInsSalary = EntityService.GetAllEntities<Ins_InsuranceSalary>(EntityService.GuidMainContext, LoginUserID.Value);
                String typeInsurance = ExchangeRateType.E_RATE_SOCIALINSURANCE.ToString();
                List<Cat_ExchangeRate> lstRateInsurance = repoCat_ExchangeRate.FindBy(al => al.Type == typeInsurance && al.MonthOfEffect <= DateMonth).OrderByDescending(al => al.MonthOfEffect).ToList();


                string waitStatus = ProfileStatusSyn.E_WAITING.ToString();
                var listAllProfiles = dataProfile.Where(pro => pro.StatusSyn != waitStatus && (pro.DateQuit == null || pro.DateQuit.Value > MonthYear)
                                                                                                                              && pro.DateHire < MonthYear);
                List<Guid> listGuidAllPro = listAllProfiles.Select(pro => pro.ID).ToList();
                //string[] strInclude = new string[] { Sal_BasicSalary.FieldNames.Cat_Currency, Sal_BasicSalary.FieldNames.Cat_Currency1 };
                List<Sal_BasicSalary> listBasicSalary = repoSal_BasicSalary.FindBy(bs => listGuidAllPro.Contains(bs.ProfileID)).ToList();

                List<Sal_Grade> lstAllGrades = repoSal_Grade.FindBy(s => s.ID != null).ToList();

                List<Ins_InsuranceRecord> list_SHORT = list_InsMonth.Where(ir => ir.InsuranceType == e_SICK_SHORT).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();

                List<Ins_InsuranceRecord> list_LONG = list_InsMonth.Where(ir => ir.InsuranceType == e_SICK_LONG).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();

                List<Ins_InsuranceRecord> list_CHILD = list_InsMonth.Where(ir => ir.InsuranceType == e_SICK_CHILD).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();


                List<Guid> PositionIsWorkingHard = repoCat_Position.FindBy(m => m.Code == "Worker" || m.Code == "Supervisor").Select(m => m.ID).ToList();

                List<Cat_DayOff> lstDayOff = repoCat_DayOff.FindBy(s => s.IsDelete == null).ToList();

                var lstProfileOrg = listAllProfiles.Where(pro => pro.OrgStructureID != null && lstStrucId.Contains(pro.OrgStructureID.Value)).ToList();

                List<Sys_AllSetting> lstAppConfig = repoSys_AllSetting.FindBy(s => s.IsDelete == null).ToList();

                totalEmployees = listAllProfiles.Count();
                totalEmployeesOrg = lstProfileOrg.Count();

                string gender = EnumDropDown.Sexual.E_FEMALE.ToString();
                totalFemales = listAllProfiles.Count();
                totalFemalesOrg = lstProfileOrg.Where(pro => pro.Gender == gender).Count();


                List<Sal_BasicSalary> lstSalPro = Sal_BasicSalaryServices.ListSalaryEffectAll(listBasicSalary, MonthYear);

                totalSalaryInMonth = lstSalPro.Sum(sl => sl.InsuranceAmount);
                List<Guid> lstGuidProOrg = lstProfileOrg.Select(pro => pro.ID).ToList();
                totalSalaryInMonthOrg = lstSalPro.Where(sal => lstGuidProOrg.Contains(sal.ProfileID)).Count();



                String _STATUS = String.Empty;
                //duyet qua danh sach profile nghi short leave
                String A_SICK_GROUP = InsuranceRecordType.A_SICK_GROUP.ToString();

                totalSalaryInsurance = 0;
                totalMoney = 0;
                totalMoneyNotRound = 0;
                totalLeaveInYear = 0;
                totalLeaveInMonth = 0;
                if (ckb_ShowAll && list_SHORT.Count <= 0)
                    tblData = FillRowNull(tblData, e_SICK_SHORT, A_SICK_GROUP);
                #endregion

                #region Fill tung loai Orderby bang tay

                #region SICK
                if (list_SHORT.Count > 0 || list_LONG.Count > 0 || list_CHILD.Count > 0)
                {
                    tblData = fillRowHeaderGroupType(tblData, A_SICK_GROUP);
                }

                #region list_SHORT
                if (list_SHORT.Count > 0)
                {
                    tblData = fillRowHeaderType(tblData, e_SICK_SHORT, A_SICK_GROUP);
                }
                foreach (Ins_InsuranceRecord item in list_SHORT)
                {
                    if (item.Hre_Profile != null && item.Hre_Profile.Cat_OrgStructure != null && !lstStrucId.Contains(item.Hre_Profile.Cat_OrgStructure.ID))
                    {
                        continue;
                    }
                    tblData = FillRow(item.Hre_Profile, item, tblData
                        , _from, _to, MonthYear, listBasicSalary
                        , list_InsYear, lstRateInsurance, e_SICK_SHORT, lstDayOff, PositionIsWorkingHard, A_SICK_GROUP, lstStrucAll, lstAppConfig);
                }
                if (totalSalaryInsurance != 0)
                {
                    tblData = FillRowTotal(tblData, _strTOTAL, totalSalaryInsurance, totalMoney, totalMoneyNotRound
                                            , totalLeaveInYear, totalLeaveInMonth, e_SICK_SHORT, A_SICK_GROUP);
                    tblData = FillRowNull(tblData, e_SICK_SHORT, A_SICK_GROUP);
                    _STATUS = e_SICK_SHORT;
                }
                SetDataToPriVariables();
                #endregion

                #region list_LONG
                if (list_LONG.Count > 0)
                {
                    tblData = fillRowHeaderType(tblData, e_SICK_LONG, A_SICK_GROUP);
                }
                foreach (Ins_InsuranceRecord item in list_LONG)
                {
                    if (item.Hre_Profile != null && item.Hre_Profile.Cat_OrgStructure != null && !lstStrucId.Contains(item.Hre_Profile.Cat_OrgStructure.ID))
                    {
                        continue;
                    }
                    tblData = FillRow(item.Hre_Profile, item, tblData
                        , _from, _to, MonthYear, listBasicSalary
                        , list_InsYear, lstRateInsurance, e_SICK_LONG, lstDayOff, PositionIsWorkingHard, A_SICK_GROUP, lstStrucAll, lstAppConfig);
                }
                if (totalSalaryInsurance != 0)
                {
                    tblData = FillRowTotal(tblData, _strTOTAL, totalSalaryInsurance, totalMoney, totalMoneyNotRound
                                         , totalLeaveInYear, totalLeaveInMonth, e_SICK_LONG, A_SICK_GROUP);
                    tblData = FillRowNull(tblData, e_SICK_LONG, A_SICK_GROUP);
                    _STATUS = e_SICK_LONG;
                }
                SetDataToPriVariables();
                #endregion

                #region list_CHILD
                if (list_CHILD.Count > 0)
                {
                    tblData = fillRowHeaderType(tblData, e_SICK_CHILD, A_SICK_GROUP);
                }
                foreach (Ins_InsuranceRecord item in list_CHILD)
                {
                    if (item.Hre_Profile != null && item.Hre_Profile.Cat_OrgStructure != null && !lstStrucId.Contains(item.Hre_Profile.Cat_OrgStructure.ID))
                    {
                        continue;
                    }
                    tblData = FillRow(item.Hre_Profile, item, tblData
                       , _from, _to, MonthYear, listBasicSalary
                       , list_InsYear, lstRateInsurance, e_SICK_CHILD, lstDayOff, PositionIsWorkingHard, A_SICK_GROUP, lstStrucAll, lstAppConfig);
                }
                if (totalSalaryInsurance != 0)
                {
                    tblData = FillRowTotal(tblData, _strTOTAL, totalSalaryInsurance, totalMoney, totalMoneyNotRound
                                         , totalLeaveInYear, totalLeaveInMonth, e_SICK_CHILD, A_SICK_GROUP);
                    tblData = FillRowNull(tblData, e_SICK_CHILD, A_SICK_GROUP);

                    _STATUS = e_SICK_CHILD;
                }

                SetDataToPriVariables();
                #endregion

                #endregion

                #region ThaiPhu
                String B_PREGNANCY_GROUP = InsuranceRecordType.B_PREGNANCY_GROUP.ToString();
                string E_PREGNANCY_EXAMINE = InsuranceRecordType.E_PREGNANCY_EXAMINE.ToString();
                string E_PREGNANCY_LOST = InsuranceRecordType.E_PREGNANCY_LOST.ToString();
                string E_PREGNANCY_SUCKLE = InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString();
                string E_PREGNANCY_PREVENTION = InsuranceRecordType.E_PREGNANCY_PREVENTION.ToString();
                #region du lieu gia thai phu
                List<Ins_InsuranceRecord> list_EXAMINE = list_InsMonth.Where(ir => ir.InsuranceType == E_PREGNANCY_EXAMINE).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();
                List<Ins_InsuranceRecord> list_LOST = list_InsMonth.Where(ir => ir.InsuranceType == E_PREGNANCY_LOST).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();
                List<Ins_InsuranceRecord> list_SUCKLE = list_InsMonth.Where(ir => ir.InsuranceType == E_PREGNANCY_SUCKLE).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();
                List<Ins_InsuranceRecord> list_PREVENTION = list_InsMonth.Where(ir => ir.InsuranceType == E_PREGNANCY_PREVENTION).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();
                #endregion

                if (list_EXAMINE.Count > 0 || list_LOST.Count > 0 || list_SUCKLE.Count > 0 || list_PREVENTION.Count > 0)
                {
                    tblData = fillRowHeaderGroupType(tblData, B_PREGNANCY_GROUP);
                }

                #region list_EXAMINE
                if (list_EXAMINE.Count > 0)
                {
                    tblData = fillRowHeaderType(tblData, E_PREGNANCY_EXAMINE, B_PREGNANCY_GROUP);
                }
                foreach (Ins_InsuranceRecord item in list_EXAMINE)
                {
                    if (item.Hre_Profile != null && item.Hre_Profile.Cat_OrgStructure != null && !lstStrucId.Contains(item.Hre_Profile.Cat_OrgStructure.ID))
                    {
                        continue;
                    }
                    tblData = FillRow(item.Hre_Profile, item, tblData
                        , _from, _to, MonthYear, listBasicSalary
                        , list_InsYear, lstRateInsurance, E_PREGNANCY_EXAMINE, lstDayOff, PositionIsWorkingHard, B_PREGNANCY_GROUP, lstStrucAll, lstAppConfig);
                }

                if (totalSalaryInsurance != 0)
                {
                    tblData = FillRowTotal(tblData, _strTOTAL, totalSalaryInsurance, totalMoney, totalMoneyNotRound
                                            , totalLeaveInYear, totalLeaveInMonth, E_PREGNANCY_EXAMINE, B_PREGNANCY_GROUP);
                    tblData = FillRowNull(tblData, E_PREGNANCY_EXAMINE, B_PREGNANCY_GROUP);
                    _STATUS = e_SICK_SHORT;
                }

                SetDataToPriVariables();
                #endregion

                #region list_LOST
                if (list_LOST.Count > 0)
                {
                    tblData = fillRowHeaderType(tblData, E_PREGNANCY_LOST, B_PREGNANCY_GROUP);
                }
                foreach (Ins_InsuranceRecord item in list_LOST)
                {
                    if (item.Hre_Profile != null && item.Hre_Profile.Cat_OrgStructure != null && !lstStrucId.Contains(item.Hre_Profile.Cat_OrgStructure.ID))
                    {
                        continue;
                    }
                    tblData = FillRow(item.Hre_Profile, item, tblData
                        , _from, _to, MonthYear, listBasicSalary
                        , list_InsYear, lstRateInsurance, E_PREGNANCY_LOST, lstDayOff, PositionIsWorkingHard, B_PREGNANCY_GROUP, lstStrucAll, lstAppConfig);
                }
                if (totalSalaryInsurance != 0)
                {
                    tblData = FillRowTotal(tblData, _strTOTAL, totalSalaryInsurance, totalMoney, totalMoneyNotRound
                                            , totalLeaveInYear, totalLeaveInMonth, E_PREGNANCY_LOST, B_PREGNANCY_GROUP);
                    tblData = FillRowNull(tblData, E_PREGNANCY_LOST, B_PREGNANCY_GROUP);
                    _STATUS = e_SICK_SHORT;
                }

                SetDataToPriVariables();
                #endregion

                #region list_SUCKLE
                if (list_SUCKLE.Count > 0)
                {
                    tblData = fillRowHeaderType(tblData, E_PREGNANCY_SUCKLE, B_PREGNANCY_GROUP);
                }
                foreach (Ins_InsuranceRecord item in list_SUCKLE)
                {
                    if (item.Hre_Profile != null && item.Hre_Profile.Cat_OrgStructure != null && !lstStrucId.Contains(item.Hre_Profile.Cat_OrgStructure.ID))
                    {
                        continue;
                    }
                    tblData = FillRow(item.Hre_Profile, item, tblData
                        , _from, _to, MonthYear, listBasicSalary
                        , list_InsYear, lstRateInsurance, E_PREGNANCY_SUCKLE, lstDayOff, PositionIsWorkingHard, B_PREGNANCY_GROUP, lstStrucAll, lstAppConfig);
                }
                if (totalSalaryInsurance != 0)
                {
                    tblData = FillRowTotal(tblData, _strTOTAL, totalSalaryInsurance, totalMoney, totalMoneyNotRound
                                            , totalLeaveInYear, totalLeaveInMonth, E_PREGNANCY_SUCKLE, B_PREGNANCY_GROUP);
                    tblData = FillRowNull(tblData, E_PREGNANCY_SUCKLE, B_PREGNANCY_GROUP);
                    _STATUS = e_SICK_SHORT;
                }

                SetDataToPriVariables();
                #endregion

                #region list_PREVENTION
                if (list_PREVENTION.Count > 0)
                {
                    tblData = fillRowHeaderType(tblData, E_PREGNANCY_PREVENTION, B_PREGNANCY_GROUP);
                }
                foreach (Ins_InsuranceRecord item in list_PREVENTION)
                {
                    if (item.Hre_Profile != null && item.Hre_Profile.Cat_OrgStructure != null && !lstStrucId.Contains(item.Hre_Profile.Cat_OrgStructure.ID))
                    {
                        continue;
                    }
                    tblData = FillRow(item.Hre_Profile, item, tblData
                        , _from, _to, MonthYear, listBasicSalary
                        , list_InsYear, lstRateInsurance, E_PREGNANCY_PREVENTION, lstDayOff, PositionIsWorkingHard, B_PREGNANCY_GROUP, lstStrucAll, lstAppConfig);
                }
                if (totalSalaryInsurance != 0)
                {
                    tblData = FillRowTotal(tblData, _strTOTAL, totalSalaryInsurance, totalMoney, totalMoneyNotRound
                                            , totalLeaveInYear, totalLeaveInMonth, E_PREGNANCY_PREVENTION, B_PREGNANCY_GROUP);
                    tblData = FillRowNull(tblData, E_PREGNANCY_PREVENTION, B_PREGNANCY_GROUP);
                    _STATUS = e_SICK_SHORT;
                }

                SetDataToPriVariables();
                #endregion

                #endregion

                #region Dưỡng Sức Ốm Đau

                String C_RESTORATION_SICK_GROUP = InsuranceRecordType.C_RESTORATION_SICK_GROUP.ToString();
                string E_RESTORATION_SICK = InsuranceRecordType.E_RESTORATION_SICK.ToString();
                string AT_HOME = InsuranceRecordType.E_LEAVE_AT_HOME.ToString();
                string AT_SAME_PLACE = InsuranceRecordType.E_LEAVE_AT_SAME_PLACE.ToString();

                #region du lieu
                List<Ins_InsuranceRecord> list_RESTORATION_SICK = list_InsMonth.Where(ir => ir.InsuranceType == E_RESTORATION_SICK).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();
                List<Ins_InsuranceRecord> list_RESTORATION_SICK_AT_HOME = list_InsMonth.Where(ir => ir.InsuranceType == E_RESTORATION_SICK && ir.TypeData == AT_HOME).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();
                List<Ins_InsuranceRecord> list_RESTORATION_SICK_AT_SAME_PLACE = list_InsMonth.Where(ir => ir.InsuranceType == E_RESTORATION_SICK && ir.TypeData == AT_SAME_PLACE).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();
                #endregion

                if (list_RESTORATION_SICK.Count > 0)
                {
                    tblData = fillRowHeaderGroupType(tblData, C_RESTORATION_SICK_GROUP);
                }
                //=========================================

                #region Nhóm Tại Gia

                if (list_RESTORATION_SICK_AT_HOME.Count > 0)
                {
                    tblData = fillRowHeaderType(tblData, AT_HOME, C_RESTORATION_SICK_GROUP);
                }
                foreach (Ins_InsuranceRecord item in list_RESTORATION_SICK_AT_HOME)
                {
                    if (item.Hre_Profile != null && item.Hre_Profile.Cat_OrgStructure != null && !lstStrucId.Contains(item.Hre_Profile.Cat_OrgStructure.ID))
                    {
                        continue;
                    }
                    tblData = FillRow(item.Hre_Profile, item, tblData
                        , _from, _to, MonthYear, listBasicSalary
                        , list_InsYear, lstRateInsurance, AT_HOME, lstDayOff, PositionIsWorkingHard, C_RESTORATION_SICK_GROUP, lstStrucAll, lstAppConfig);
                }
                if (totalSalaryInsurance != 0)
                {
                    tblData = FillRowTotal(tblData, _strTOTAL, totalSalaryInsurance, totalMoney, totalMoneyNotRound
                                            , totalLeaveInYear, totalLeaveInMonth, AT_HOME, C_RESTORATION_SICK_GROUP);
                    tblData = FillRowNull(tblData, AT_HOME, C_RESTORATION_SICK_GROUP);
                    _STATUS = e_SICK_SHORT;
                }

                SetDataToPriVariables();

                #endregion

                #region Nhóm Tại Tập Trung

                if (list_RESTORATION_SICK_AT_SAME_PLACE.Count > 0)
                {
                    tblData = fillRowHeaderType(tblData, AT_SAME_PLACE, C_RESTORATION_SICK_GROUP);
                }
                foreach (Ins_InsuranceRecord item in list_RESTORATION_SICK_AT_SAME_PLACE)
                {
                    if (item.Hre_Profile != null && item.Hre_Profile.Cat_OrgStructure != null && !lstStrucId.Contains(item.Hre_Profile.Cat_OrgStructure.ID))
                    {
                        continue;
                    }
                    tblData = FillRow(item.Hre_Profile, item, tblData
                        , _from, _to, MonthYear, listBasicSalary
                        , list_InsYear, lstRateInsurance, AT_SAME_PLACE, lstDayOff, PositionIsWorkingHard, C_RESTORATION_SICK_GROUP, lstStrucAll, lstAppConfig);
                }
                if (totalSalaryInsurance != 0)
                {
                    tblData = FillRowTotal(tblData, _strTOTAL, totalSalaryInsurance, totalMoney, totalMoneyNotRound
                                            , totalLeaveInYear, totalLeaveInMonth, AT_SAME_PLACE, C_RESTORATION_SICK_GROUP);
                    tblData = FillRowNull(tblData, AT_SAME_PLACE, C_RESTORATION_SICK_GROUP);
                    _STATUS = e_SICK_SHORT;
                }

                SetDataToPriVariables();

                #endregion

                #endregion

                #region Dưỡng Sức Thai Sản Restoration
                String D_RESTORATION_PREGNANCY_GROUP = InsuranceRecordType.D_RESTORATION_PREGNANCY_GROUP.ToString();
                string E_RESTORATION_PREGNANCY = InsuranceRecordType.E_RESTORATION_PREGNANCY.ToString();
                List<Ins_InsuranceRecord> list_RESTORATION_PREGNANCY = list_InsMonth.Where(ir => ir.InsuranceType == E_RESTORATION_PREGNANCY).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();

                if (list_RESTORATION_PREGNANCY.Count > 0)
                {
                    tblData = fillRowHeaderGroupType(tblData, D_RESTORATION_PREGNANCY_GROUP);
                }
                //=======================================
                if (list_RESTORATION_PREGNANCY.Count > 0)
                {
                    tblData = fillRowHeaderType(tblData, E_RESTORATION_PREGNANCY, D_RESTORATION_PREGNANCY_GROUP);
                }
                foreach (Ins_InsuranceRecord item in list_RESTORATION_PREGNANCY)
                {
                    if (item.Hre_Profile != null && item.Hre_Profile.Cat_OrgStructure != null && !lstStrucId.Contains(item.Hre_Profile.Cat_OrgStructure.ID))
                    {
                        continue;
                    }
                    tblData = FillRow(item.Hre_Profile, item, tblData
                        , _from, _to, MonthYear, listBasicSalary
                        , list_InsYear, lstRateInsurance, E_RESTORATION_PREGNANCY, lstDayOff, PositionIsWorkingHard, D_RESTORATION_PREGNANCY_GROUP, lstStrucAll, lstAppConfig);
                }
                if (totalSalaryInsurance != 0)
                {
                    tblData = FillRowTotal(tblData, _strTOTAL, totalSalaryInsurance, totalMoney, totalMoneyNotRound
                                            , totalLeaveInYear, totalLeaveInMonth, E_RESTORATION_PREGNANCY, D_RESTORATION_PREGNANCY_GROUP);
                    tblData = FillRowNull(tblData, E_RESTORATION_PREGNANCY, D_RESTORATION_PREGNANCY_GROUP);
                    _STATUS = e_SICK_SHORT;
                }

                SetDataToPriVariables();
                //=======================================

                #endregion

                #region Dưỡng Sức Sau Tai Nạn Lao Động. Bệnh Nghề Nghiệp
                String E_RESTORATION_TNLD_GROUP = InsuranceRecordType.E_RESTORATION_TNLD_GROUP.ToString();
                string E_RESTORATION_TNLD = InsuranceRecordType.E_RESTORATION_TNLD.ToString();
                List<Ins_InsuranceRecord> list_RESTORATION_TNLD = list_InsMonth.Where(ir => ir.InsuranceType == E_RESTORATION_TNLD).OrderBy(ir => ir.DateStart).OrderBy(ir => ir.Hre_Profile.CodeEmp).ToList();

                if (list_RESTORATION_TNLD.Count > 0)
                {
                    tblData = fillRowHeaderGroupType(tblData, E_RESTORATION_TNLD_GROUP);
                }

                //=======================================
                if (list_RESTORATION_TNLD.Count > 0)
                {
                    tblData = fillRowHeaderType(tblData, E_RESTORATION_TNLD, E_RESTORATION_TNLD_GROUP);
                }
                foreach (Ins_InsuranceRecord item in list_RESTORATION_TNLD)
                {
                    if (item.Hre_Profile != null && item.Hre_Profile.Cat_OrgStructure != null && !lstStrucId.Contains(item.Hre_Profile.Cat_OrgStructure.ID))
                    {
                        continue;
                    }
                    tblData = FillRow(item.Hre_Profile, item, tblData
                        , _from, _to, MonthYear, listBasicSalary
                        , list_InsYear, lstRateInsurance, E_RESTORATION_TNLD, lstDayOff, PositionIsWorkingHard, E_RESTORATION_TNLD_GROUP, lstStrucAll, lstAppConfig);
                }
                if (totalSalaryInsurance != 0)
                {
                    tblData = FillRowTotal(tblData, _strTOTAL, totalSalaryInsurance, totalMoney, totalMoneyNotRound
                                            , totalLeaveInYear, totalLeaveInMonth, E_RESTORATION_TNLD, E_RESTORATION_TNLD_GROUP);
                    tblData = FillRowNull(tblData, E_RESTORATION_TNLD, E_RESTORATION_TNLD_GROUP);
                    _STATUS = e_SICK_SHORT;
                }

                SetDataToPriVariables();
                //=======================================
                #endregion

                #endregion

                //ListControl1.DataSource = tblData;
                //ViewState.Add("Data", tblData);
                return tblData;
            }
        }

        private DataTable fillRowHeaderGroupType(DataTable tblData, string GroupName)
        {
            DataRow row = tblData.NewRow();
            row[Ins_C70aReportEntity.FieldNames.CodeEmp] = GroupName;
            row[Ins_C70aReportEntity.FieldNames.GroupName] = GroupName;
            tblData.Rows.Add(row);
            return tblData;
        }

        private DataTable fillRowHeaderType(DataTable tblData, string status, string GroupName)
        {
            DataRow row = tblData.NewRow();
            //row["CodeEmp"] = LanguageManager.GetString(status).ToUpper();
            //row["GroupName"] = LanguageManager.GetString(GroupName);
            row[Ins_C70aReportEntity.FieldNames.CodeEmp] = status.ToUpper();
            row[Ins_C70aReportEntity.FieldNames.Status] = status.ToUpper();
            row[Ins_C70aReportEntity.FieldNames.GroupName] = GroupName;
            tblData.Rows.Add(row);
            return tblData;
        }

        private DataTable FillRowNull(DataTable tblData, string status, string GroupName)
        {
            DataRow row = tblData.NewRow();
            //row["Status"] = LanguageManager.GetString(status);
            //row["GroupName"] = LanguageManager.GetString(GroupName);
            row[Ins_C70aReportEntity.FieldNames.Status] = status;
            row[Ins_C70aReportEntity.FieldNames.GroupName] = GroupName;
            tblData.Rows.Add(row);
            return tblData;
        }

        private DataRow fillRowBasic(DataRow dr, DateTime monthYear, Hre_Profile profile, Ins_InsuranceRecord record, List<Ins_InsuranceRecord> lstRecordByProfile, List<Cat_DayOff> lstDayOff)
        {
            if (profile == null)
            {
                return dr;
            }
            Stt++;
            dr[Ins_C70aReportEntity.FieldNames.Stt] = Stt;
            dr[Ins_C70aReportEntity.FieldNames.CodeEmp] = profile.CodeEmp;
            dr[Ins_C70aReportEntity.FieldNames.ProfileName] = ConvertStringToNamePerson(profile.ProfileName);
            if (profile.DateOfBirth != null)
            {
                if (profile.Gender == EnumDropDown.Sexual.E_FEMALE.ToString())
                {
                    dr["FemaleBirthYear"] = profile.DateOfBirth.Value.Year;
                }
                else
                {
                    dr["MaleBirthYear"] = profile.DateOfBirth.Value.Year;
                }
            }
            dr["SocialInsNo"] = profile.SocialInsNo;
            if (record.DateStart != null)
            {
                dr["DateStart"] = record.DateStart.Value;
            }
            if (record.DateEnd != null)
            {
                dr["DateEnd"] = record.DateEnd.Value;
            }
            //if (record.DateStart != null && record.DateEnd != null)
            //{
            //    dr["SumStartEnd"] = (record.DateEnd.Value - record.DateStart.Value).TotalDays;
            //    totalLeaveInMonth += (record.DateEnd.Value - record.DateStart.Value).TotalDays;
            //}

            //DateTime beginYear = new DateTime(monthYear.Year-1,12,15); //bat dau tu ngay 15 theo bao hiem. Can phai hoi lai chi Oanh van de tren
            //lstDayOff = lstDayOff.Where(m=>m.DateOff > beginYear).ToList();
            //if (record.DateStart != null && record.DateEnd != null)
            //{
            //    List<Ins_InsuranceRecord> lstRecordLeaveInYear = lstRecordByProfile.Where(m => m.ProfileID == profile.ID && m.InsuranceType == record.InsuranceType
            //        && m.DateEnd != null && m.DateEnd.Value > beginYear
            //        && m.DateStart != null && m.DateStart.Value <= record.DateStart).ToList();
            //    dr["SumStartEndInYear"] = (record.DateEnd.Value - record.DateStart.Value).TotalDays + getNumDayLeaveInYear(lstRecordLeaveInYear, lstDayOff);
            //    totalLeaveInYear = (record.DateEnd.Value - record.DateStart.Value).TotalDays + getNumDayLeaveInYear(lstRecordLeaveInYear, lstDayOff);
            //}
            return dr;
        }

        private System.Data.DataTable FillRow(Hre_Profile Profile, Ins_InsuranceRecord record
          , System.Data.DataTable tblData, DateTime _from, DateTime _to, DateTime monthYear
          , List<Sal_BasicSalary> listBasicSalary, List<Ins_InsuranceRecord> list_Year, List<Cat_ExchangeRate> lstRateInsurance
          , string status, List<Cat_DayOff> lstDayOff, List<Guid> PositionIsWorkingHard, String GroupName, List<Cat_OrgStructure> lstOrgAll, List<Sys_AllSetting> lstAppConfig)
        {

            //List<Sys_AllSetting> lstAppConfig = EntityService.GetAllEntities<Sys_AllSetting>(EntityService.GuidMainContext, LoginUserID.Value);
            if (record.DayCount <= 0)
                return tblData;
            Guid proID = Profile.ID;
            //HRService hrser = new HRService();
            DataRow row = tblData.NewRow();
            row = fillRowBasic(row, monthYear, Profile, record, list_Year.Where(m => m.ProfileID == Profile.ID).ToList(), lstDayOff);
            DateTime _monthJoin = InsuranceServices.GetMonthJoinInsurance(Profile, monthYear);
            #region loai SICK
            if (status == InsuranceRecordType.E_SICK_SHORT.ToString() ||
                status == InsuranceRecordType.E_SICK_LONG.ToString() ||
                status == InsuranceRecordType.E_SICK_CHILD.ToString())
            {
                //Tính cột  thời gian đóng bảo hiểm xã hội
                if (record.DateStart != null)
                {


                    DateTime dateStartInsurance = new DateTime(record.DateStart.Value.AddMonths(-1).Year, record.DateStart.Value.AddMonths(-1).Month, InsuranceServices.PeriodInsuranceDayCurrentMonthDefault);
                    int month = 0;
                    if (Profile.UnEmpInsCountMonthOld != null)
                    {
                        month = Profile.UnEmpInsCountMonthOld ?? 0;
                    }
                    for (DateTime dateCheck = _monthJoin; dateCheck <= dateStartInsurance; dateCheck = dateCheck.AddMonths(1))
                    {
                        month++;
                    }
                    int year = month / 12;
                    month = month % 12;
                    string yearTemp = year.ToString();
                    yearTemp = yearTemp.Length == 1 ? "0" + yearTemp : yearTemp;
                    string monthTemp = month.ToString();
                    monthTemp = monthTemp.Length == 1 ? "0" + monthTemp : monthTemp;
                    row[Ins_C70aReportEntity.FieldNames.BeginSocialInsIssueDate] = yearTemp + "-" + monthTemp;
                }


                //Tình trạng của nhân viên 
                //Logic Tất cả các chức danh trừ nhân viên thì là tình trang "Điều kiện NN-ĐH"còn nhân viên thì là "Điều Kiện BT"
                if (status == InsuranceRecordType.E_SICK_CHILD.ToString()) // Nếu loại con óm thì không cần ghi
                {
                    row[Ins_C70aReportEntity.FieldNames.Notes] = string.Empty;
                    row[Ins_C70aReportEntity.FieldNames.Notes1] = record.DateSuckle != null ? string.Format("{0:dd/MM/yyyy}", record.DateSuckle.Value) : string.Empty;
                }
                else
                {
                    if (PositionIsWorkingHard.Contains(Profile.PositionID ?? Guid.Empty))
                    {
                        row[Ins_C70aReportEntity.FieldNames.Notes] = "Điều kiện NN-ĐH";
                    }
                    else
                    {
                        row[Ins_C70aReportEntity.FieldNames.Notes] = "Điều Kiện BT";
                    }
                }
            }
            #endregion
            #region loai Kham thai
            if (status == InsuranceRecordType.E_PREGNANCY_EXAMINE.ToString() ||
                status == InsuranceRecordType.E_PREGNANCY_LOST.ToString() ||
                status == InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString() ||
                status == InsuranceRecordType.E_PREGNANCY_PREVENTION.ToString())
            {
                //Tính cột  thời gian đóng bảo hiểm xã hội
                if (record.DateStart != null)
                {

                    DateTime dateStartInsurance = new DateTime(record.DateStart.Value.AddMonths(-1).Year, record.DateStart.Value.AddMonths(-1).Month, InsuranceServices.PeriodInsuranceDayCurrentMonthDefault);
                    int month = 0;
                    for (DateTime dateCheck = _monthJoin; dateCheck <= dateStartInsurance; dateCheck = dateCheck.AddMonths(1))
                    {
                        month++;
                    }
                    month = month > 12 ? 12 : month;
                    string monthTemp = month.ToString();
                    monthTemp = monthTemp.Length == 1 ? "0" + monthTemp : monthTemp;
                    row[Ins_C70aReportEntity.FieldNames.BeginSocialInsIssueDate] = "0" + "-" + monthTemp;

                    if (status == InsuranceRecordType.E_PREGNANCY_LOST.ToString())
                    {
                        row[Ins_C70aReportEntity.FieldNames.Notes] = record.Comment;
                    }
                    if (status == InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString())
                    {
                        //row["Notes"] = LanguageManager.GetString(record.TypeSuckle);
                        row[Ins_C70aReportEntity.FieldNames.Notes] = record.TypeSuckle;
                        row[Ins_C70aReportEntity.FieldNames.Notes1] = record.DateSuckle != null ? string.Format("{0:dd/MM/yyyy}", record.DateSuckle.Value) : string.Empty;
                    }
                }
            }
            #endregion
            #region loai Duong Suc
            if (status == InsuranceRecordType.E_RESTORATION_SICK.ToString() ||
                status == InsuranceRecordType.E_RESTORATION_TNLD.ToString() ||
                status == InsuranceRecordType.E_RESTORATION_PREGNANCY.ToString())
            {
                //Tính cột  thời gian đóng bảo hiểm xã hội
                //row["Notes"] = LanguageManager.GetString(record.TypeSuckle);
                row[Ins_C70aReportEntity.FieldNames.Notes] = record.TypeSuckle;
                row[Ins_C70aReportEntity.FieldNames.Notes1] = record.DateStartWorking != null ? string.Format("{0:dd/MM/yyyy}", record.DateStartWorking.Value) : string.Empty;
            }
            #endregion
            Double salaryIns = 0;
            Double moneyIns = 0;
            InsuranceServices.GetMoneyInsSalaryIns(record, ListMaxSalary, ListMinSalary, _monthJoin, listBasicSalary, lstRateInsurance, lstAppConfig, out moneyIns, out salaryIns, false);
            //Tien BHXH tra
            // Double roundMoney = Common.GetRoundMoney(moneyIns, 2);
            //totalMoney += roundMoney;
            row[Ins_C70aReportEntity.FieldNames.Money] = moneyIns;
            totalMoney += moneyIns;

            totalMoneyNotRound += moneyIns;
            // row["MoneyNotRound"] = moneyIns;

            //Luong tham gia BHXH
            totalSalaryInsurance += salaryIns;
            row[Ins_C70aReportEntity.FieldNames.SalaryInsurance] = salaryIns;

            if (status == InsuranceRecordType.E_LEAVE_AT_SAME_PLACE.ToString() ||
                status == InsuranceRecordType.E_LEAVE_AT_HOME.ToString() ||
                status == InsuranceRecordType.E_RESTORATION_TNLD.ToString() ||
                status == InsuranceRecordType.E_RESTORATION_PREGNANCY.ToString())//Nếu là phần dưỡng sưc thì thể hiện 25%
            {

                totalSalaryInsurance = 0.25;
                row[Ins_C70aReportEntity.FieldNames.SalaryInsurance] = 0.25;

                if (status == InsuranceRecordType.E_LEAVE_AT_SAME_PLACE.ToString())
                {
                    totalSalaryInsurance = 0.4;
                    row[Ins_C70aReportEntity.FieldNames.SalaryInsurance] = 0.4;
                }
            }




            //Lay nam nghi viec cua record de tinh luy ke. Neu nghi tu nam ngoai thi luy ke cac ngay nghi tu nam ngoai

            List<Ins_InsuranceRecord> lstInSursInYear = list_Year.Where(ir => ir.ProfileID == proID
                                                                        && ir.DateStart.Value.Year == record.DateStart.Value.Year
                                                                        && ir.DateEnd <= record.DateEnd
                                                                        && ir.InsuranceType == status).ToList();

            //Trường hợp con Ốm thì con nào phải đúng tuyến cho con đó.
            if (record.InsuranceType == InsuranceRecordType.E_SICK_CHILD.ToString())
            {
                lstInSursInYear = lstInSursInYear.Where(m => m.ChildSickID == record.ChildSickID).ToList();
            }

            Double leaveInYear = 0;
            foreach (Ins_InsuranceRecord it in lstInSursInYear)
            {
                leaveInYear += InsuranceServices.GetCountLeaveRecord(it);
            }
            totalLeaveInYear += leaveInYear;
            row[Ins_C70aReportEntity.FieldNames.LeaveInYear] = leaveInYear;
            Double leaveInMonth = InsuranceServices.GetCountLeaveRecord(record);
            totalLeaveInMonth += leaveInMonth;
            row[Ins_C70aReportEntity.FieldNames.LeaveInMonth] = leaveInMonth;
            if (record.DateStart != null)
            {
                row[Ins_C70aReportEntity.FieldNames.DateStart] = record.DateStart.Value;
            }
            if (record.DateEnd != null)
            {
                row[Ins_C70aReportEntity.FieldNames.DateEnd] = record.DateEnd.Value;
            }
            //row["Status"] = LanguageManager.GetString(status);
            //row["GroupName"] = LanguageManager.GetString(GroupName);
            row[Ins_C70aReportEntity.FieldNames.Status] = status;
            row[Ins_C70aReportEntity.FieldNames.GroupName] = GroupName;
            row[Ins_C70aReportEntity.FieldNames.CodeParentOrgLevel] = Hre_ProfileServices.GetCodeInListOrgParent(Profile.OrgStructureID ?? Guid.Empty, lstOrgAll, 1);
            tblData.Rows.Add(row);
            return tblData;
        }

        private System.Data.DataTable FillRowTotal(System.Data.DataTable tblData, String str,
                                                   Double totalSalaryIns, Double totalMoney, Double totalMoneyNotRound,
                                                   Double totalLeaveInYear,
                                                   Double totalLeaveInMonth,
                                                   String _status, string GroupName)
        {
            DataRow row = tblData.NewRow();
            row[Ins_C70aReportEntity.FieldNames.CodeEmp] = String.Empty;
            row[Ins_C70aReportEntity.FieldNames.ProfileName] = str;
            row[Ins_C70aReportEntity.FieldNames.Birthday] = String.Empty;
            row[Ins_C70aReportEntity.FieldNames.SocialInsNo] = String.Empty;
            if (!ckb_ShowAll)
            {
                row[Ins_C70aReportEntity.FieldNames.SalaryInsurance] = totalSalaryIns;
                row[Ins_C70aReportEntity.FieldNames.MonthJoinInsurance] = String.Empty;
                row[Ins_C70aReportEntity.FieldNames.LeaveInMonth] = totalLeaveInMonth;
                row[Ins_C70aReportEntity.FieldNames.LeaveInYear] = totalLeaveInYear;
            }
            row[Ins_C70aReportEntity.FieldNames.Money] = totalMoney;
            //  row["MoneyNotRound"] = totalMoneyNotRound;
            //row["DateStart"] = String.Empty;
            //row["DateEnd"] = String.Empty;
            //row["Status"] = LanguageManager.GetString(_status);
            //row["GroupName"] = LanguageManager.GetString(GroupName);
            row[Ins_C70aReportEntity.FieldNames.Status] = _status;
            row[Ins_C70aReportEntity.FieldNames.GroupName] = GroupName;
            tblData.Rows.Add(row);
            return tblData;
        }

        private string ConvertStringToNamePerson(string Input)
        {
            string result = string.Empty;
            List<string> LstNames = Input.Split(' ').ToList();
            foreach (var item in LstNames)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                try
                {
                    string FirstLetter = item.Substring(0, 1);
                    string LastLetter = item.Substring(1, item.Length - 1);
                    FirstLetter = FirstLetter.ToUpper();
                    LastLetter = LastLetter.ToLower();
                    result += FirstLetter + LastLetter + " ";
                }
                catch
                {
                    continue;
                }

            }
            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        private void SetDataToPriVariables()
        {
            _TotalSalaryIns += totalSalaryInsurance;
            _TotalMoney += totalMoney;
            _TotalMoneyNotRound += totalMoneyNotRound;
            _TotalLeaveInYear += totalLeaveInYear;
            _TotalLeaveInMonth += totalLeaveInMonth;

            //duyet qua danh sach profile nghi Long leave
            totalSalaryInsurance = 0;
            totalMoney = 0;
            totalMoneyNotRound = 0;
            totalLeaveInYear = 0;
            totalLeaveInMonth = 0;

            Stt = 0;
        }

        #endregion

        #region  Báo cáo theo dõi mức tiền đóng bảo hiểm hàng tháng xem theo khoảng thời gian (Honda)
        public DataTable GetReportNotHaveSocialSchema(DateTime monthYearFrom, DateTime monthYearTo)
        {
            DataTable tb = new DataTable("Ins_ReportInsuranceTrackingMonthly");

            tb.Columns.Add(new DataColumn(ConstantDisplay.HRM_HR_Profile_DateHire, typeof(DateTime)));
            tb.Columns.Add(new DataColumn(ConstantDisplay.HRM_HR_Profile_CodeEmp));
            tb.Columns.Add(new DataColumn(ConstantDisplay.HRM_HR_Profile_ProfileName));
            tb.Columns.Add(new DataColumn(ConstantDisplay.HRM_HR_Profile_SocialInsNo));
            tb.Columns.Add(new DataColumn(ConstantDisplay.HRM_HR_Profile_SalaryClassID));

            for (var i = monthYearFrom; i <= monthYearTo; i = i.AddMonths(1))
            {
                tb.Columns.Add(new DataColumn(i.ToString(MONTHYEAR)));
            }
            return tb;
        }

        public DataTable Ins_ReportInsuranceTrackingMonthlyLoadData(string orgIds, DateTime? dtDateFrom, DateTime? dtDateTo, bool? isProfileQuit, string codeEmp, List<Guid> socialInsPlaceIDs, string userLogin)
        {
            var DtInsuranceTrackingMonthly = new DataTable();
            var dateFrom = dtDateFrom.HasValue ? dtDateFrom.Value : new DateTime(1996, 1, 1);
            var dateTo = dtDateTo.HasValue ? dtDateTo.Value : DateTime.Now;

            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoInsMonthly = new Ins_ProfileInsuranceMonthlyRepository(unitOfWork);
                var repoCat_SalaryClass = new Cat_SalaryClassRepository(unitOfWork);
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);

                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);


                //Lay DS Ma Luong
                var salaryClass = repoCat_SalaryClass.FindBy(p => p.IsDelete == null).ToList();

                #region ds NV đóng Bảo Hiểm theo phòng ban và tháng
                var listInsMonthlyObj = new List<object> { orgIds, dateFrom, dateTo, null, null, null, null, null, 1, int.MaxValue - 1 };
                if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                {
                    listInsMonthlyObj.Add("id");
                }
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    listInsMonthlyObj[0] = null;
                    listInsMonthlyObj[3] = codeEmp;
                }
                var lstProfileInsuranceMonthlyInDb = GetData<Ins_ProfileInsuranceMonthlyEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthlyFromTo, userLogin, ref status).Translate<Ins_ProfileInsuranceMonthly>();

                if (!lstProfileInsuranceMonthlyInDb.Any())
                {
                    return new DataTable();
                }
                List<Guid> lstProfileIDs = lstProfileInsuranceMonthlyInDb.Select(p => p.ProfileID ?? Guid.Empty).Distinct().ToList();
                #endregion


                #region Get Profiles by orgs

                #region DS Profile
                int orgOrderNumber = 0;
                var lstOrgOrderNumber = orgIds.Split(',').Where(p => int.TryParse(p, out orgOrderNumber)).Select(p => orgOrderNumber).ToList();
                lstOrgOrderNumber = lstOrgOrderNumber.Where(p => p != 0).ToList();
                var catOrgIDs = repoCat_OrgStructure.FindBy(p => p.IsDelete == null && lstOrgOrderNumber.Contains(p.OrderNumber)).Select(p => p.ID).ToList();
                var lstProfile = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty, p => catOrgIDs.Contains(p.OrgStructureID ?? Guid.Empty)).Select(p => new
                {
                    p.ProfileName,
                    p.ID,
                    p.DateHire,
                    p.CodeEmp,
                    p.SocialInsNo,
                    p.DateQuit,
                    p.SalaryClassID,
                }).ToList();
                #endregion

                lstProfile = lstProfile.Where(p => lstProfileIDs.Contains(p.ID)).ToList();
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    lstProfile = lstProfile.Where(p => p.CodeEmp == codeEmp).ToList();
                }
                if (isProfileQuit.HasValue && isProfileQuit.Value)
                {
                    //Nv Nghỉ Việc
                    lstProfile = lstProfile.Where(p => p.DateQuit.HasValue).ToList();
                }
                else
                {
                    //NV đang làm việc
                    lstProfile = lstProfile.Where(p => p.DateQuit == null).ToList();
                }

                #endregion

                //ds profileId theo phong ban
                lstProfileIDs = lstProfile.Select(m => m.ID).ToList();

                #region thiet lap column
                DtInsuranceTrackingMonthly = GetReportNotHaveSocialSchema(dateFrom, dateTo);
                #endregion

                if (socialInsPlaceIDs != null && socialInsPlaceIDs.Any())
                {
                    socialInsPlaceIDs = socialInsPlaceIDs.Where(p => p != null && p != Guid.Empty).ToList();
                    lstProfileInsuranceMonthlyInDb = lstProfileInsuranceMonthlyInDb.Where(p => socialInsPlaceIDs != null && socialInsPlaceIDs.Count > 0 && socialInsPlaceIDs.Contains(p.SocialInsPlaceID ?? Guid.Empty)).ToList();
                }
                DataRow Row;
                try
                {
                    List<Guid> lstProfileIDTotal = lstProfile.Select(m => m.ID).ToList();

                    foreach (var lstProfileID in lstProfileIDTotal.Chunk(500))
                    {
                        lstProfile = lstProfile.Where(p => lstProfileID.Contains(p.ID)).ToList();
                        lstProfileInsuranceMonthlyInDb = lstProfileInsuranceMonthlyInDb.Where(p => p.ProfileID.HasValue && lstProfileID.Contains(p.ProfileID.Value)).ToList();

                        foreach (var profile in lstProfile)
                        {
                            var insuranceByProfiles = lstProfileInsuranceMonthlyInDb
                              .Where(m => m.ProfileID == profile.ID).ToList();

                            if (!insuranceByProfiles.Any())
                            {
                                continue;
                            }
                            Row = DtInsuranceTrackingMonthly.NewRow();

                            for (var i = dateFrom; i <= dateTo; i = i.AddMonths(1))
                            {
                                var lst = insuranceByProfiles.Where(m => m.MonthYearEffect.HasValue && m.MonthYearEffect.Value.Year == i.Year
                              && m.MonthYearEffect.Value.Month == i.Month).OrderByDescending(m => m.MonthYearEffect).ToList();
                                var insuranceByProfile = lst.FirstOrDefault();
                                if (insuranceByProfile == null)
                                {
                                    continue;
                                }

                                if (insuranceByProfile.MonthYearEffect.HasValue && insuranceByProfile.MonthYearEffect.Value == i)
                                {
                                    var totalInsurancePerProfile = lst.Sum(m=>m.SalaryInsurance);

                                    if (insuranceByProfile.IsPregnant != null && insuranceByProfile.IsPregnant.Value)
                                    {
                                        Row[i.ToString(MONTHYEAR)] = ConstantDisplay.HRM_Ins_ReportEmpHardJob_PregnantLeave.TranslateString();
                                    }
                                    if (insuranceByProfile.IsDecreaseWorkingDays.HasValue && insuranceByProfile.IsDecreaseWorkingDays.Value
                                        && (insuranceByProfile.IsPregnant == null || !insuranceByProfile.IsPregnant.Value))
                                    {
                                        Row[i.ToString(MONTHYEAR)] = ConstantDisplay.HRM_Ins_ReportEmpHardJob_DecreaseWorkingDays.TranslateString();
                                    }

                                    else if (totalInsurancePerProfile > 0)
                                    {
                                        if (profile.DateQuit.HasValue && profile.DateQuit.Value <= i)
                                        {
                                            Row[i.ToString(MONTHYEAR)] = DBNull.Value;
                                        }
                                        else
                                        {
                                            Row[i.ToString(MONTHYEAR)] = String.Format("{0:#,###,###.##}", totalInsurancePerProfile);
                                        }
                                    }

                                }
                            }

                            Row[ConstantDisplay.HRM_HR_Profile_DateHire] = profile.DateHire;
                            Row[ConstantDisplay.HRM_HR_Profile_CodeEmp] = profile.CodeEmp;
                            Row[ConstantDisplay.HRM_HR_Profile_ProfileName] = profile.ProfileName;
                            Row[ConstantDisplay.HRM_HR_Profile_SocialInsNo] = profile.SocialInsNo;

                            #region lấy mã lương theo nhan vien
                            var salaryClassProfileObj = salaryClass.Where(p => p.ID == profile.SalaryClassID).FirstOrDefault();
                            if (salaryClassProfileObj != null)
                            {
                                Row[ConstantDisplay.HRM_HR_Profile_SalaryClassID] = salaryClassProfileObj.Code;
                            }
                            #endregion

                            //them 1 row
                            DtInsuranceTrackingMonthly.Rows.Add(Row);
                        }
                    }
                }
                catch
                {
                    return null;
                }

            }
            var configs = new Dictionary<string, Dictionary<string, object>>();
            var config = new Dictionary<string, object>();
            config.Add("width", 80);
            configs.Add(ConstantDisplay.HRM_HR_Profile_DateHire.TranslateString(), config);
            return DtInsuranceTrackingMonthly.ConfigTable(configs);
        }
        #endregion

        #region BC Tham Gia Bảo Hiểm Tháng
        public List<Ins_ReportJoinInsuranceMonthlyEntity> ReportJoinInsuranceMonthly(string orgs, DateTime monthCheck, string codeEmp, List<Guid> socialInsPlaceIDs, string userLogin)
        {
            string status = string.Empty;
            int stt = 1;
            var listorgstructureTypesObj = new List<object> { null, null, 1, int.MaxValue - 1 };
            if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
            {
                listorgstructureTypesObj.Add("id");
            }
            var orgstructureTypes = GetData<Cat_OrgStructureType>(listorgstructureTypesObj, ConstantSql.hrm_cat_sp_get_OrgStructureType, userLogin, ref status).ToList();
            var orgstructures = GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).ToList();

            var listObj = new List<object> { orgs, string.Empty, string.Empty };
            var lstProfile = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).Translate<Hre_ProfileEntity>();


            //#region trich nop thang N-1
            //var listInsPreMonthlyObj = new List<object> { orgs, new DateTime(monthCheck.Year, monthCheck.Month - 1, 1), 1, 50000 };
            //if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
            //{
            //    listInsPreMonthlyObj.Add("id");
            //}
            //var lstProfileInsurancePreMonthlyInDb = GetData<Ins_ReportJoinInsuranceMonthlyEntity>(listInsPreMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, ref status);
            //lstProfileInsurancePreMonthlyInDb = lstProfileInsurancePreMonthlyInDb.Where(p => p.IsSocialInsurance.HasValue && p.IsSocialInsurance.Value).ToList();
            //#endregion

            var listInsMonthlyObj = new List<object> { orgs, monthCheck, null, 1, 50000 };
            if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
            {
                listInsMonthlyObj.Add("id");
            }
            var lstProfileInsuranceMonthlyInDb = GetData<Ins_ReportJoinInsuranceMonthlyEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, userLogin, ref status);
            lstProfileInsuranceMonthlyInDb = lstProfileInsuranceMonthlyInDb.Where(p => p.IsSocialInsurance.HasValue && p.IsSocialInsurance.Value).ToList();
            if (!string.IsNullOrEmpty(codeEmp))
            {
                lstProfileInsuranceMonthlyInDb = lstProfileInsuranceMonthlyInDb.Where(p => p.CodeEmp == codeEmp).ToList();
            }

            if (socialInsPlaceIDs != null)
            {
                socialInsPlaceIDs = socialInsPlaceIDs.Where(p => p != null && p != Guid.Empty).ToList();
                if (socialInsPlaceIDs.Any())
                {
                    lstProfileInsuranceMonthlyInDb = lstProfileInsuranceMonthlyInDb.Where(p => socialInsPlaceIDs != null && socialInsPlaceIDs.Count > 0 && socialInsPlaceIDs.Contains(p.SocialInsPlaceID ?? Guid.Empty)).ToList();
                }
            }
            foreach (var item in lstProfileInsuranceMonthlyInDb)
            {
                // var preMonthSalaryInsurance = lstProfileInsurancePreMonthlyInDb.Where(p => p.ProfileID == item.ProfileID).FirstOrDefault();
                var profile = lstProfile.Where(p => p.ID == item.ProfileID).FirstOrDefault();
                //if (preMonthSalaryInsurance != null)
                //{
                //    item.MoneySocialInsurancePreMonth = preMonthSalaryInsurance.MoneySocialInsurance;
                //}
                if (profile != null)
                {
                    item.CostCentreName = profile.CostCentreName;
                    var org = LibraryService.GetNearestParent(profile.OrgStructureID ?? Guid.Empty, OrgUnit.E_DEPARTMENT, orgstructures, orgstructureTypes);
                    item.SocialInsNo = profile.SocialInsNo;
                    item.HealthInsNo = profile.HealthInsNo;
                    item.DateOfBirth = profile.DateOfBirth;
                    item.Gender = profile.Gender;
                    if (!string.IsNullOrEmpty(profile.Gender) && (profile.Gender == EnumDropDown.Sexual.E_MALE.ToString() || profile.Gender == EnumDropDown.Sexual.E_FEMALE.ToString()))
                    {
                        item.Gender = EnumDropDown.GetEnumDescription<EnumDropDown.Sexual>((EnumDropDown.Sexual)Enum.Parse(typeof(EnumDropDown.Sexual), profile.Gender, true));
                    }
                    item.Location = profile.PAddress;
                    item.PAddress = profile.PAddress;
                    item.OrgStructureName = org.OrgStructureName;
                    item.OrgStructureCode = org.Code;
                    item.CostCentreID = profile.CostCentreID;
                    item.IDNo = profile.IDNo;
                    item.IDDateOfIssue = profile.IDDateOfIssue;
                    item.IDPlaceOfIssue = profile.IDPlaceOfIssue;

                    if (item.IsSocialInsurance.HasValue && !item.IsSocialInsurance.Value)
                    {
                        item.SalaryInsurance = null;
                    }
                    if (item.IsHealthInsurance.HasValue && !item.IsHealthInsurance.Value)
                    {
                        item.SalaryHealthInsurance = null;
                    }
                    if (item.IsUnEmpInsurance.HasValue && !item.IsUnEmpInsurance.Value)
                    {
                        item.SalaryUnEmpInsurance = null;
                    }
                    item.STT = stt++;
                }
            }

            return lstProfileInsuranceMonthlyInDb;
        }
        #endregion

        #region BC Chức Danh

        public List<Ins_ReportJobNameMonthlyEntity> ReportJobNameMonthly(string orgs, DateTime monthCheck, string codeEmp, List<Guid> socialInsPlaceIDs, string userLogin)
        {
            string status = string.Empty;
            var preMonth = new DateTime(monthCheck.Year, monthCheck.AddMonths(-1).Month, InsuranceServices.PeriodInsuranceDayPreMonthDefault);
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_SalaryClass = new Cat_SalaryClassRepository(unitOfWork);

                var salaryClasses = repoCat_SalaryClass.FindBy(p => p.IsDelete == null).ToList();


                var orgstructures = GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).Translate<Cat_OrgStructureEntity>().ToList();

                var listObj = new List<object> { orgs, string.Empty, string.Empty };
                var lstProfile = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).Translate<Hre_ProfileEntity>();

                #region lich su bao hiem thang chọn
                var listInsMonthlyObj = new List<object> { orgs, monthCheck, null, 1, int.MaxValue - 1 };
                if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                {
                    listInsMonthlyObj.Add("id");
                }
                var lstProfileInsuranceMonthlyInDb = GetData<Ins_ReportJobNameMonthlyEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, userLogin, ref status);
                #endregion

                #region Lịch sử Bảo Hiểm tháng trước
                var listInsPreMonthlyObj = new List<object> { orgs, monthCheck.AddMonths(-1), null, 1, int.MaxValue - 1 };
                if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                {
                    listInsPreMonthlyObj.Add("id");
                }
                var lstProfileInsurancePreMonthlyInDb = GetData<Ins_ReportJobNameMonthlyEntity>(listInsPreMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, userLogin, ref status);
                #endregion


                #region đk lọc
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    lstProfileInsuranceMonthlyInDb = lstProfileInsuranceMonthlyInDb.Where(p => p.CodeEmp == codeEmp).ToList();
                }

                if (socialInsPlaceIDs != null && socialInsPlaceIDs.Any())
                {
                    socialInsPlaceIDs = socialInsPlaceIDs.Where(p => p != null && p != Guid.Empty).ToList();
                    if (socialInsPlaceIDs.Any())
                    {
                        lstProfileInsuranceMonthlyInDb = lstProfileInsuranceMonthlyInDb.Where(p => socialInsPlaceIDs != null && socialInsPlaceIDs.Count > 0 && socialInsPlaceIDs.Contains(p.SocialInsPlaceID ?? Guid.Empty)).ToList();
                    }
                }
                #endregion

                //lịch sử NV đóng bảo hiểm
                //  lstProfileInsuranceMonthlyInDb = lstProfileInsuranceMonthlyInDb.Where(p => p.IsDecreaseWorkingDays == null || p.IsPregnant == null).ToList();
                List<Ins_ReportJobNameMonthlyEntity> lstJobNameMonthlies = new List<Ins_ReportJobNameMonthlyEntity>();
                foreach (var item in lstProfileInsuranceMonthlyInDb)
                {
                    var profile = lstProfile.Where(p => p.ID == item.ProfileID).FirstOrDefault();

                    if (item.IsDecreaseWorkingDays.HasValue || item.IsPregnant.HasValue)
                    {
                        var inFirst = lstProfileInsurancePreMonthlyInDb.FirstOrDefault(p => p.ProfileID == item.ProfileID && p.MonthYear.HasValue && p.MonthYear.Value.Month == monthCheck.AddMonths(-1).Month);
                        var inNow = lstProfileInsuranceMonthlyInDb.FirstOrDefault(p => p.ProfileID == item.ProfileID && p.MonthYear.HasValue && p.MonthYear.Value.Month == monthCheck.Month);
                        #region Thiết lập
                        var inFirstIsPregnant = false;
                        var inFirstIs14days = false;
                        var inNowIsPregnant = false;
                        var inNowIs14days = false;
                        if (inNow != null)
                        {
                            inNowIsPregnant = inNow.IsPregnant ?? false;
                            inNowIs14days = inNow.IsDecreaseWorkingDays ?? false;
                        }
                        if (inFirst != null)
                        {
                            inFirstIsPregnant = inFirst.IsPregnant ?? false;
                            inFirstIs14days = inFirst.IsDecreaseWorkingDays ?? false;
                        }
                        if (inNowIsPregnant && inFirstIsPregnant && (!profile.DateQuit.HasValue))
                        {
                            //tháng trước nghỉ thai sản
                            continue;
                        }
                        if (inNowIs14days && inFirstIs14days && (!profile.DateQuit.HasValue ) )
                        {
                            //tháng trước nghỉ 14 ngày
                            continue;
                        }
                    }
                        #endregion


                    if (profile != null && profile.DateQuit.HasValue && profile.DateQuit.Value < preMonth)
                    {
                        //bắt đầu nghỉ việc từ tháng trước
                        continue;
                    }


                    var salaryClassObj = salaryClasses.FirstOrDefault(p => profile != null && profile.SalaryClassID.HasValue && p.ID == profile.SalaryClassID);
                    if (salaryClassObj != null)
                    {
                        //Mã Lương
                        item.Rank = salaryClassObj.SalaryClassName;
                    }

                    if (profile != null)
                    {
                        item.DateHire = profile.DateHire;
                        item.CostCentreName = profile.CostCentreName;
                        item.CostCentreID = profile.CostCentreID;
                        
                        //  item.Location = GetCodeOrgStructure(orgstructures, profile.OrgStructureID ?? Guid.Empty);
                        item.E_DEPARTMENT = profile.E_DEPARTMENT;
                        item.E_DIVISION = profile.E_DIVISION;
                        item.E_SECTION = profile.E_SECTION;
                        item.E_TEAM = profile.E_TEAM;
                        item.E_UNIT = profile.E_UNIT;
                    }
                    lstJobNameMonthlies.Add(item);
                }
                return lstJobNameMonthlies;
            }

        }
        #endregion

        #region  BC Dữ Liệu BH NV Theo Tháng (Group Theo CostCenter) (form kế toán)
        public List<Ins_ReportInsCostCenterMonthlyEntity> ReportInsCostCenterMonthly(string orgs, DateTime monthCheck, string costCentreName, List<Guid> socialInsPlaceIDs, string userLogin)
        {
            string status = string.Empty;

            //var orgstructures = GetData<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, ref status).Translate<Cat_OrgStructureEntity>().ToList();

            var listObj = new List<object> { orgs, string.Empty, string.Empty };
            var lstProfile = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).Translate<Hre_ProfileEntity>();
            lstProfile = lstProfile.Where(p => p.CostCentreID.HasValue).ToList();
            if (!string.IsNullOrEmpty(costCentreName))
            {
                lstProfile = lstProfile.Where(p => p.CostCentreName == costCentreName).ToList();
            }
            var objCostCenter = lstProfile.Select(p => new { p.CostCentreID, p.CostCentreName }).ToList();
            var costCenterIds = lstProfile.Select(p => p.CostCentreID ?? Guid.Empty).Distinct().ToList();
            var dicCostCenter = new Dictionary<Guid, List<Guid>>();
            foreach (var costCenterId in costCenterIds)
            {
                var profiles = lstProfile.Where(p => p.CostCentreID == costCenterId).Select(p => p.ID).ToList();
                if (profiles.Any())
                {
                    dicCostCenter.Add(costCenterId, profiles);
                }
            }



            var listInsMonthlyObj = new List<object> { orgs, monthCheck, monthCheck, null, true, null, null, null, 1, int.MaxValue - 1 };
            if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
            {
                listInsMonthlyObj.Add("id");
            }
            var lstProfileInsuranceMonthlyInDb = GetData<Ins_ReportInsCostCenterMonthlyEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthlyFromTo, userLogin, ref status);

            if (socialInsPlaceIDs != null)
            {
                socialInsPlaceIDs = socialInsPlaceIDs.Where(p => p != null && p != Guid.Empty).ToList();
                if (socialInsPlaceIDs != null && socialInsPlaceIDs.Any())
                {
                    lstProfileInsuranceMonthlyInDb = lstProfileInsuranceMonthlyInDb.Where(p => socialInsPlaceIDs != null && socialInsPlaceIDs.Count > 0 && socialInsPlaceIDs.Contains(p.SocialInsPlaceID ?? Guid.Empty)).ToList();
                }
            }


            var lstReportInsCostCenterMonthly = new List<Ins_ReportInsCostCenterMonthlyEntity>();
            Ins_ReportInsCostCenterMonthlyEntity reportInsCostCenterMonthly;
            foreach (KeyValuePair<Guid, List<Guid>> pair in dicCostCenter)
            {
                var key = pair.Key;
                var value = pair.Value;
                var lstProfInsMonthly = lstProfileInsuranceMonthlyInDb.Where(p => (p.IsPayback == null || p.IsPayback.Value == false) && value.Contains(p.ProfileID ?? Guid.Empty)).ToList();
                var lstProfInsMonthlyPayBack = lstProfileInsuranceMonthlyInDb.Where(p => (p.IsPayback != null && p.IsPayback.Value == true) && value.Contains(p.ProfileID ?? Guid.Empty)).ToList();

                var costCenter = objCostCenter.Where(p => p.CostCentreID == key).FirstOrDefault();
                if (lstProfInsMonthly.Any())
                {
                    #region Tien BHXH,BHYT,BHTN (chưa điều chỉnh BH)
                    reportInsCostCenterMonthly = new Ins_ReportInsCostCenterMonthlyEntity();
                    if (costCenter != null)
                    {
                        reportInsCostCenterMonthly.CostCentreName = costCenter.CostCentreName;
                    }
                    reportInsCostCenterMonthly.ProfileAmount = lstProfInsMonthly.Count();
                    reportInsCostCenterMonthly.IsPayback = false;
                    //BHXH
                    reportInsCostCenterMonthly.SocialInsEmpAmount = lstProfInsMonthly.Sum(p => p.SocialInsEmpAmount);
                    reportInsCostCenterMonthly.SocialInsComAmount = lstProfInsMonthly.Sum(p => p.SocialInsComAmount);
                    reportInsCostCenterMonthly.MoneySocialInsurance = lstProfInsMonthly.Sum(p => p.MoneySocialInsurance);

                    //BHYT
                    reportInsCostCenterMonthly.HealthInsEmpAmount = lstProfInsMonthly.Sum(p => p.HealthInsEmpAmount);
                    reportInsCostCenterMonthly.HealthInsComAmount = lstProfInsMonthly.Sum(p => p.HealthInsComAmount);
                    reportInsCostCenterMonthly.MoneyHealthInsurance = lstProfInsMonthly.Sum(p => p.MoneyHealthInsurance);

                    //BHTN
                    reportInsCostCenterMonthly.UnemployEmpAmount = lstProfInsMonthly.Sum(p => p.UnemployEmpAmount);
                    reportInsCostCenterMonthly.UnemployComAmount = lstProfInsMonthly.Sum(p => p.UnemployComAmount);
                    reportInsCostCenterMonthly.MoneyUnEmpInsurance = lstProfInsMonthly.Sum(p => p.MoneyUnEmpInsurance);
                    lstReportInsCostCenterMonthly.Add(reportInsCostCenterMonthly);
                    #endregion

                    

                    #region Tien BHXH,BHYT,BHTN (đã điều chỉnh BH)
                    if (lstProfInsMonthlyPayBack.Any())
                    {
                        reportInsCostCenterMonthly = new Ins_ReportInsCostCenterMonthlyEntity();
                        if (costCenter != null)
                        {
                            reportInsCostCenterMonthly.CostCentreName = costCenter.CostCentreName;
                        }
                        reportInsCostCenterMonthly.ProfileAmount = lstProfInsMonthlyPayBack.Count();
                        reportInsCostCenterMonthly.IsPayback = true;
                        //BHXH
                        reportInsCostCenterMonthly.SocialInsEmpAmount = lstProfInsMonthlyPayBack.Sum(p => p.SocialInsEmpAmount);
                        reportInsCostCenterMonthly.SocialInsComAmount = lstProfInsMonthlyPayBack.Sum(p => p.SocialInsComAmount);
                        reportInsCostCenterMonthly.MoneySocialInsurance = lstProfInsMonthlyPayBack.Sum(p => p.MoneySocialInsurance);

                        //BHYT
                        reportInsCostCenterMonthly.HealthInsEmpAmount = lstProfInsMonthlyPayBack.Sum(p => p.HealthInsEmpAmount);
                        reportInsCostCenterMonthly.HealthInsComAmount = lstProfInsMonthlyPayBack.Sum(p => p.HealthInsComAmount);
                        reportInsCostCenterMonthly.MoneyHealthInsurance = lstProfInsMonthlyPayBack.Sum(p => p.MoneyHealthInsurance);

                        //BHTN
                        reportInsCostCenterMonthly.UnemployEmpAmount = lstProfInsMonthlyPayBack.Sum(p => p.UnemployEmpAmount);
                        reportInsCostCenterMonthly.UnemployComAmount = lstProfInsMonthlyPayBack.Sum(p => p.UnemployComAmount);
                        reportInsCostCenterMonthly.MoneyUnEmpInsurance = lstProfInsMonthlyPayBack.Sum(p => p.MoneyUnEmpInsurance);
                        lstReportInsCostCenterMonthly.Add(reportInsCostCenterMonthly); 
                    }
                    #endregion

                }
            }
            lstReportInsCostCenterMonthly = lstReportInsCostCenterMonthly.OrderByDescending(m => m.IsPayback).ToList();
            return lstReportInsCostCenterMonthly;
        }
        #endregion

        #region BC Tong Hop Thanh Toan Bao Hiem
        public DataTable CreateReportInsuranceScheme()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Ins_ReportInsuranceTotalEntity");
                tb.Columns.Add(Ins_ReportInsuranceTotalEntity.FieldNames.Stt, typeof(int));
                tb.Columns.Add(Ins_ReportInsuranceTotalEntity.FieldNames.Name, typeof(string));
                tb.Columns.Add(Ins_ReportInsuranceTotalEntity.FieldNames.TotalEmp, typeof(int));
                tb.Columns.Add(Ins_ReportInsuranceTotalEntity.FieldNames.BHXH, typeof(double));
                tb.Columns.Add(Ins_ReportInsuranceTotalEntity.FieldNames.BHYT, typeof(double));
                tb.Columns.Add(Ins_ReportInsuranceTotalEntity.FieldNames.BHTN, typeof(double));
                tb.Columns.Add(Ins_ReportInsuranceTotalEntity.FieldNames.TotalAll, typeof(double));
                return tb;
            }
        }
        public DataTable BC_INSURANCETOTAL(DateTime DateStart, DateTime DateEnd, string orgs, List<Guid> socialInsPlaceIDs, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfileInsMonthly = new Ins_ProfileInsuranceMonthlyRepository(unitOfWork);
                var repoCat_Region = new Cat_RegionRepository(unitOfWork);
                var repoCat_Province = new Cat_ProvinceRepository(unitOfWork);
                string status = string.Empty;

                #region get province
                var provinces = repoCat_Province.FindBy(p => p.IsDelete == null).ToList();
                #endregion

                #region Get Profiles by orgs
                var listObj = new List<object> { orgs, string.Empty, string.Empty };
                var lstProfiles = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status);
                #endregion

                List<Guid> lstProfileID = lstProfiles.Select(m => m.ID).ToList();
                // Cần phải where thêm điều kiện là lấy 1 trong 2 loại  (có một loại là chính và một loại là tạm - 2 loại này dùng để ra báo cáo chênh lệch)
                //Ps. Hàm bên dưới thiếu điều kiện where đó.
                List<Ins_ProfileInsuranceMonthly> lstProfileInsuranceMonthly = repoProfileInsMonthly.FindBy(m => m.ProfileID != null && m.MonthYear >= DateStart && m.MonthYear <= DateEnd).ToList();
                lstProfileInsuranceMonthly = lstProfileInsuranceMonthly.Where(m => lstProfileID.Contains(m.ProfileID.Value)).ToList();

                if (socialInsPlaceIDs != null)
                {
                    socialInsPlaceIDs = socialInsPlaceIDs.Where(p => p != null && p != Guid.Empty).ToList();
                    if (socialInsPlaceIDs.Any())
                    {
                        lstProfileInsuranceMonthly = lstProfileInsuranceMonthly.Where(p => socialInsPlaceIDs != null && socialInsPlaceIDs.Count > 0 && socialInsPlaceIDs.Contains(p.SocialInsPlaceID ?? Guid.Empty)).ToList();
                    }
                }

                var socialInsPlaceObjs = lstProfileInsuranceMonthly.Select(p => p.SocialInsPlaceID).Distinct().ToList();
                provinces = provinces.Where(p => socialInsPlaceObjs.Contains(p.ID)).ToList();

                DataTable dt = CreateReportInsuranceScheme();
                int stt = 0;
                for (DateTime DateCheck = DateStart; DateCheck <= DateEnd; DateCheck = DateCheck.AddMonths(1))
                {
                    //danh sach du lieu BH tháng năm đang kiểm tra
                    var lstProfileInsuranceMonthly_ByMonth = lstProfileInsuranceMonthly.Where(m => m.IsSocialInsurance.HasValue && m.IsSocialInsurance.Value && m.MonthYear.HasValue && m.MonthYear.Value.Year == DateCheck.Year
                        && m.MonthYear.Value.Month == DateCheck.Month).ToList();
                    if (!lstProfileInsuranceMonthly_ByMonth.Any())
                    {
                        continue;
                    }

                    // stt++;
                    DataRow dr = dt.NewRow();
                    #region Thêm dòng đầu (dòng ngày tháng và tổng số lượng NV , tổng tiền đóng BH)
                    stt++;
                    dr[Ins_ReportInsuranceTotalEntity.FieldNames.Stt] = stt;
                    dr[Ins_ReportInsuranceTotalEntity.FieldNames.Name] = DateCheck.ToString(ConstantFormat.HRM_Format_MonthYear);
                    var TotalEmp = lstProfileInsuranceMonthly_ByMonth.Select(m => m.ProfileID).Distinct().ToList().Count;
                    dr[Ins_ReportInsuranceTotalEntity.FieldNames.TotalEmp] = TotalEmp;
                    #region Tổng Lương BHXH +BHYT+ BHTN (khong cộng phần điều chỉnh)
                    var totalSocial = lstProfileInsuranceMonthly_ByMonth.Where(m => m.IsPayback == null || m.IsPayback.Value == false).Sum(m => m.SocialInsComAmount + m.SocialInsEmpAmount);
                    var totalHealth = lstProfileInsuranceMonthly_ByMonth.Where(m => m.IsPayback == null || m.IsPayback.Value == false).Sum(m => m.HealthInsComAmount + m.HealthInsEmpAmount);
                    var totalUnEmp = lstProfileInsuranceMonthly_ByMonth.Where(m => m.IsPayback == null || m.IsPayback.Value == false).Sum(m => m.UnemployComAmount + m.UnemployEmpAmount);  
                    #endregion
                    dr[Ins_ReportInsuranceTotalEntity.FieldNames.BHXH] = lstProfileInsuranceMonthly_ByMonth.Sum(m => m.SocialInsComAmount + m.SocialInsEmpAmount);
                    dr[Ins_ReportInsuranceTotalEntity.FieldNames.BHYT] = lstProfileInsuranceMonthly_ByMonth.Sum(m => m.HealthInsComAmount + m.HealthInsEmpAmount); ;
                    dr[Ins_ReportInsuranceTotalEntity.FieldNames.BHTN] = lstProfileInsuranceMonthly_ByMonth.Sum(m => m.UnemployComAmount + m.UnemployEmpAmount); ;
                    dr[Ins_ReportInsuranceTotalEntity.FieldNames.TotalAll] = totalSocial + totalHealth + totalUnEmp;
                    dt.Rows.Add(dr);
                    #endregion

                    #region Ds nơi đóng BH theo tháng năm của NV
                    var tempSocialInsPlaceIds = lstProfileInsuranceMonthly_ByMonth.Select(p => p.SocialInsPlaceID).Distinct().ToList();
                    var provinceByInsProfileMonthlys = provinces.Where(p => tempSocialInsPlaceIds.Contains(p.ID)).ToList();

                    foreach (var insSocialPlace in provinceByInsProfileMonthlys)
                    {
                       
                        dr = dt.NewRow();
                        var profileInsuranceMonthly_ByMonths = lstProfileInsuranceMonthly_ByMonth.Where(p => p.SocialInsPlaceID == insSocialPlace.ID).ToList();

                        dr[Ins_ReportInsuranceTotalEntity.FieldNames.Stt] = DBNull.Value;
                        dr[Ins_ReportInsuranceTotalEntity.FieldNames.Name] = insSocialPlace.ProvinceName;
                        dr[Ins_ReportInsuranceTotalEntity.FieldNames.TotalEmp] = profileInsuranceMonthly_ByMonths.Select(m => m.ProfileID).Distinct().ToList().Count;
                        #region Tổng Lương BHXH +BHYT+ BHTN (khong cộng phần điều chỉnh)
                        var wtotalSocial = profileInsuranceMonthly_ByMonths.Where(m => m.IsPayback == null || m.IsPayback.Value == false).Sum(m => m.SocialInsComAmount + m.SocialInsEmpAmount);
                        var wtotalHealth = profileInsuranceMonthly_ByMonths.Where(m => m.IsPayback == null || m.IsPayback.Value == false).Sum(m => m.HealthInsComAmount + m.HealthInsEmpAmount);
                        var wtotalUnEmp = profileInsuranceMonthly_ByMonths.Where(m => m.IsPayback == null || m.IsPayback.Value == false).Sum(m => m.UnemployComAmount + m.UnemployEmpAmount); 
                        #endregion
                        dr[Ins_ReportInsuranceTotalEntity.FieldNames.BHXH] = profileInsuranceMonthly_ByMonths.Sum(m => m.SocialInsComAmount + m.SocialInsEmpAmount); ;
                        dr[Ins_ReportInsuranceTotalEntity.FieldNames.BHYT] = profileInsuranceMonthly_ByMonths.Sum(m => m.HealthInsComAmount + m.HealthInsEmpAmount); ;
                        dr[Ins_ReportInsuranceTotalEntity.FieldNames.BHTN] = profileInsuranceMonthly_ByMonths.Sum(m => m.UnemployComAmount + m.UnemployEmpAmount); ;
                        dr[Ins_ReportInsuranceTotalEntity.FieldNames.TotalAll] = wtotalSocial+wtotalHealth+wtotalUnEmp;
                        dt.Rows.Add(dr);
                    }
                    stt = 0;
                    #endregion
                }
                return dt.ConfigTable(true);
            }
        }
        public class OrderRegion
        {
            public Guid ID { get; set; }
            public string RegionName { get; set; }
            public double Num { get; set; }
        }
        #endregion

        #region BC NV huong tro cap nang nhoc, doc hai, nguy hiem
        /// <summary>Schema BC NV huong tro cap nang nhoc , doc hai , nguy hiem</summary>
        /// <param name="DateStart"></param>
        /// <param name="DateEnd"></param>
        /// <returns></returns>
        public DataTable CreateReportEmpHardJobScheme(DateTime DateStart, DateTime DateEnd)
        {
            using (var context = new VnrHrmDataContext())
            {
                // List<Guid> lstProfileIds = lstProfileIds.Select(m => m.ID).ToList();
                string status = string.Empty;
                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                string E_PREGNANCY_SUCKLE = InsuranceRecordType.E_PREGNANCY_SUCKLE.ToString();
                DataTable tb = new DataTable("Ins_ReportEmpHardJoblEntity");
                //tb.Columns.Add(Ins_ReportEmpHardJoblEntity.FieldNames.Stt, typeof(int));
                tb.Columns.Add(Ins_ReportEmpHardJoblEntity.FieldNames.CodeEmp, typeof(string));
                tb.Columns.Add(Ins_ReportEmpHardJoblEntity.FieldNames.ProfileName, typeof(string));

                bool isfirstRow = true;
                int SttData = 0;

                if (isfirstRow)
                {
                    isfirstRow = false;
                    int SttDataHead = 0;
                    for (DateTime datecheck = DateStart; datecheck <= DateEnd; datecheck = datecheck.AddMonths(1))
                    {
                        tb.Columns.Add(datecheck.ToString(MONTHYEAR), typeof(string));
                    }
                }

                return tb;
            }
        }

        /// <summary> BC NV huong tro cap nang nhoc, doc hai, nguy hiem </summary>
        /// <param name="DateStart"></param>
        /// <param name="DateEnd"></param>
        /// <param name="orgIds"></param>
        /// <returns></returns>
        public DataTable BC_EmpHardJob(DateTime DateStart, DateTime DateEnd, string orgIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCatExchangeRate = new Cat_ExchangeRateRepository(unitOfWork);
                var repoCatValueEntity = new Cat_ValueEntityRepository(unitOfWork);
                var repoSal_InsuranceSalary = new Sal_InsuranceSalaryRepository(unitOfWork);
                var repoIns_ProfileInsuranceMonthly = new Ins_ProfileInsuranceMonthlyRepository(unitOfWork);
                var repoAtt_LeaveDay = new Att_LeavedayRepository(unitOfWork);
                var repoIns_InsuranceRecord = new Ins_InsuranceRecordRepository(unitOfWork);

                #region DS Profile
                int orgOrderNumber = 0;
                var lstOrgOrderNumber = orgIds.Split(',').Where(p => int.TryParse(p, out orgOrderNumber)).Select(p => orgOrderNumber).ToList();
                lstOrgOrderNumber = lstOrgOrderNumber.Where(p => p != 0).ToList();
                var catOrgIDs = unitOfWork.CreateQueryable<Cat_OrgStructure>(Guid.Empty, p => p.IsDelete == null && lstOrgOrderNumber.Contains(p.OrderNumber)).Select(p => p.ID).ToList();
                var lstProfiles = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty, p => catOrgIDs.Contains(p.OrgStructureID ?? Guid.Empty)).Select(p => new
                {
                    p.ProfileName,
                    p.ID,
                    p.DateHire,
                    p.CodeEmp,
                    p.SocialInsNo,
                    p.DateQuit,
                    p.SalaryClassID,
                }).ToList();
                #endregion
                DataTable dt = CreateReportEmpHardJobScheme(DateStart, DateEnd);

                List<Guid> lstProfileIDTotal = lstProfiles.Select(m => m.ID).ToList();

                foreach (var lstProfileID in lstProfileIDTotal.Chunk(500))
                {
                    //Lấy ds trích nộp BH (được hưởng tiền HDTJob) theo NV vào khoảng thời gian truyền vào 
                    var lstProfileMonthly = unitOfWork.CreateQueryable<Ins_ProfileInsuranceMonthly>(Guid.Empty,
                  m => m.IsDelete == null && m.ProfileID != null
                      && lstProfileID.Contains(m.ProfileID.Value)
                      && m.MonthYear >= DateStart
                      && m.MonthYear <= DateEnd
                      && m.MonthYear != null
                      && (m.AmountHDTIns != null && m.AmountHDTIns.Value > 0)
                      )
                  .Select(m => new CustomProfileMonthlyEntity()
                  {
                      ProfileID = m.ProfileID,
                      SalaryInsurance = m.SalaryInsurance,
                      IsSocialInsurance = m.IsSocialInsurance,
                      MonthYear = m.MonthYear.Value,
                      MoneyHDTJob = m.AmountHDTIns,
                      IsPregnant = m.IsPregnant,
                      IsDecreaseWorkingDays = m.IsDecreaseWorkingDays
                  })
                  .ToList();

                    int stt = 0;
                    foreach (var profileID in lstProfileID)
                    {
                        stt++;
                        var hasValue = false;
                        DataRow dr = null;
                        for (DateTime datecheck = DateStart; datecheck <= DateEnd; datecheck = datecheck.AddMonths(1))
                        {
                            if (dr == null)
                            {
                                dr = dt.NewRow();
                            }

                            #region Kiểm tra xem có đóng HDT Job ko
                            var Ins = lstProfileMonthly.Where(p => p.MonthYear == datecheck && p.ProfileID == profileID).FirstOrDefault();
                            if (Ins != null)
                            {
                                if (Ins.MoneyHDTJob != null && Ins.MoneyHDTJob > 0)
                                {
                                    if (Ins.IsPregnant != null && Ins.IsPregnant.Value)
                                    {
                                        dr[datecheck.ToString(MONTHYEAR)] = ConstantDisplay.HRM_Ins_ReportEmpHardJob_PregnantLeave.TranslateString();
                                        hasValue = true;
                                    }
                                    else if (Ins.IsDecreaseWorkingDays != null && Ins.IsDecreaseWorkingDays.Value)
                                    {
                                        dr[datecheck.ToString(MONTHYEAR)] = ConstantDisplay.HRM_Ins_ReportEmpHardJob_DecreaseWorkingDays.TranslateString();
                                        hasValue = true;
                                    }
                                    else
                                    {
                                        dr[datecheck.ToString(MONTHYEAR)] = String.Format("{0:#,###,###.##}", Ins.MoneyHDTJob);
                                        hasValue = true;
                                    }
                                }
                            }
                            #endregion
                        }

                        if (hasValue)
                        {
                            var profile = lstProfiles.Where(m => m.ID == profileID).FirstOrDefault();
                            if (profile != null)
                            {
                                dr[Ins_ReportEmpHardJoblEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                                dr[Ins_ReportEmpHardJoblEntity.FieldNames.ProfileName] = profile.ProfileName;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt.ConfigTable(true);
            }
        }


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
            DateTime date = DateTime.MinValue;
            Cat_Currency curIns = null;
            //Lay muc tran tai thoi diem monthYear => VND
            Double amountCeilingLasted = GetMaxMinAmountOfMonth(monthYear, lstInsuranceAmountCeiling);
            //Lay luong toi thieu tai thoi diem monthYear => VND
            Double amountMinimumSalary = GetMaxMinAmountOfMonth(monthYear, lstInsAmountMinimum);

            date = InsuranceSalary.DateEffect ?? monthYear;
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
        #endregion

        #region Báo cáo dữ liệu HDT lần 2
        public List<Ins_ReportEmpHardJob2ndEntity> ReportEmpHardJob2nd(string orgs, DateTime monthCheck, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string status = string.Empty;
                #region ds NV đóng Bảo Hiểm theo phòng ban và tháng (Ins_ProfileInsuranceMonthly)
                var listInsMonthlyObj = new List<object> { orgs, monthCheck, null, 1, int.MaxValue - 1 };
                if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                {
                    listInsMonthlyObj.Add("id");
                }
                var lstProfileInsuranceMonthlyInDb = GetData<Ins_ProfileInsuranceMonthlyEntity>(listInsMonthlyObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, userLogin, ref status).Translate<Ins_ProfileInsuranceMonthly>();
                #endregion
                var hreProfileIds = lstProfileInsuranceMonthlyInDb.Select(m => m.ProfileID).ToList();
                var hreProfiles = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty, m => hreProfileIds.Contains(m.ID)).Select(m => new { ProfileID = m.ID, ProfileName = m.ProfileName, CodeEmp = m.CodeEmp }).ToList();
                List<Ins_ReportEmpHardJob2ndEntity> reportEmpHardJob2nds = new List<Ins_ReportEmpHardJob2ndEntity>();
                var stt = 1;


                #region hdtJOb
                string E_APPROVE = HDTJobStatus.E_APPROVE.ToString();
                var profileIDs = lstProfileInsuranceMonthlyInDb.Select(m => m.ProfileID).Distinct().ToList();
                var approved = RosterStatus.E_APPROVED.ToString();
                string app = LeaveDayStatus.E_APPROVED.ToString();
                var beginMonth = new DateTime(monthCheck.Year, monthCheck.Month, 1);
                var endMonth = beginMonth.AddMonths(1).AddMinutes(-1);
                InsuranceServices insService = new InsuranceServices();

                List<string> lstLeaveInsuranceType = String.Join(",", insService.leaveDayInsuranceTypes).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var leaveDayTypeIDs = unitOfWork.CreateQueryable<Cat_LeaveDayType>(Guid.Empty, m => m.PaidRate == 0 || lstLeaveInsuranceType.Contains(m.InsuranceType)).Select(m => m.ID).ToList();

                var rosters = unitOfWork.CreateQueryable<Att_Roster>(Guid.Empty, p => p.Status == approved
                                           && (p.DateStart <= endMonth && p.DateEnd >= beginMonth)
                                           && profileIDs.Contains(p.ProfileID))
                                           .Select(m => new CustomRosterEntity() { ProfileID = m.ProfileID, DateStart = m.DateStart, DateEnd = m.DateEnd, MonShiftID = m.MonShiftID, TueShiftID = m.TueShiftID, WedShiftID = m.WedShiftID, ThuShiftID = m.ThuShiftID, FriShiftID = m.FriShiftID, SatShiftID = m.SatShiftID, SunShiftID = m.SunShift2ID })
                                           .ToList();
                var lstLeaveDay = unitOfWork.CreateQueryable<Att_LeaveDay>(Guid.Empty, m => m.Status == app && m.DateStart <= endMonth && m.DateEnd >= beginMonth && profileIDs.Contains(m.ProfileID))
                                        .Select(m => new CustomLeavedayEntity() { ProfileID = m.ProfileID, DateStart = m.DateStart, DateEnd = m.DateEnd, LeaveDayTypeID = m.LeaveDayTypeID }).ToList();

                lstLeaveDay = lstLeaveDay.Where(m => m.LeaveDayTypeID != null && leaveDayTypeIDs.Contains(m.LeaveDayTypeID.Value)).ToList();
                var lstHDTJob = unitOfWork.CreateQueryable<Hre_HDTJob>(Guid.Empty, m => m.Status == E_APPROVE && m.DateFrom <= endMonth && m.DateTo >= beginMonth
                                         && m.ProfileID.HasValue && profileIDs.Contains(m.ProfileID.Value)).ToList();

                var lstProfileEntitiesTemp = lstProfileInsuranceMonthlyInDb.Where(m => m.ProfileID.HasValue && profileIDs.Contains(m.ProfileID)).Select(m => new Hre_ProfileEntity
                {
                    ID = m.ProfileID.Value,
                    IsDecreaseWorkingDays = m.IsDecreaseWorkingDays,
                    NumdayNonHDTJob = 0,
                    NumdayHDTJobTypeIV = 0,
                    NumdayHDTJobTypeV = 0
                }).ToList();

                InsuranceServices.GetHDTJobDayByProfile(lstProfileEntitiesTemp, leaveDayTypeIDs, monthCheck, rosters, lstLeaveDay, lstHDTJob);



                #endregion


                foreach (var profileMonthly in lstProfileInsuranceMonthlyInDb)
                {
                    var empHard2nd = new Ins_ReportEmpHardJob2ndEntity();
                    empHard2nd.Stt = stt++;
                    var profileObj = hreProfiles.Where(m => m.ProfileID == profileMonthly.ProfileID).FirstOrDefault();
                    var profileHdtJob = lstProfileEntitiesTemp.Where(m => m.ID == profileMonthly.ProfileID).FirstOrDefault();
                    if (profileObj != null)
                    {
                        empHard2nd.CodeEmp = profileObj.CodeEmp;
                        empHard2nd.ProfileName = profileObj.ProfileName;
                    }
                    if (profileObj != null)
                    {
                        if (profileHdtJob.NumdayNonHDTJob.HasValue && profileHdtJob.NumdayNonHDTJob.Value == int.MaxValue)
                        {
                            profileHdtJob.NumdayNonHDTJob = 0;
                        }
                        empHard2nd.DaysNonHDTJob = profileHdtJob.NumdayNonHDTJob ?? 0;
                        if (profileHdtJob.NumdayHDTJobTypeV >= profileHdtJob.NumdayHDTJobTypeIV)
                        {
                            empHard2nd.HDTJobType = EnumDropDown.HDTJobType.E_TYPE5.ToString();
                        }
                        else
                        {
                            empHard2nd.HDTJobType = EnumDropDown.HDTJobType.E_TYPE4.ToString();
                        }
                    }
                    empHard2nd.JobName = profileMonthly.JobName;
                    empHard2nd.AmountHDTIns = profileMonthly.AmountHDTIns;
                    reportEmpHardJob2nds.Add(empHard2nd);
                }
                return reportEmpHardJob2nds;
            }
            return null;
        }

        #endregion

        #region  Common

        /// <summary>Thai Sản</summary>
        /// <param name="MonthYear"></param>
        /// <param name="lstLeaveDayPregByProfile"></param>
        /// <param name="lstInsuranceRecordPregByProfile"></param>
        /// <returns></returns>
        public bool HasPregnant(DateTime MonthYear, List<Att_LeaveDay> lstLeaveDayPregByProfile, List<Ins_InsuranceRecord> lstInsuranceRecordPregByProfile)
        {
            InsuranceServices service = new InsuranceServices();
            var isPreg = service.HasPregnant(MonthYear, lstLeaveDayPregByProfile, lstInsuranceRecordPregByProfile);
            return isPreg;
        }
        public bool HasPregnantTmp(DateTime MonthYear, List<CustomLeavedayEntity> lstLeaveDayPregByProfile, List<CustomInsuranceRecordEntity> lstInsuranceRecordPregByProfile)
        {
            var isPreg = HasPregnantCal(MonthYear, lstLeaveDayPregByProfile, lstInsuranceRecordPregByProfile);
            return isPreg;
        }

        public bool HasPregnantCal(DateTime MonthYear, List<CustomLeavedayEntity> lstLeaveDayPregByProfile, List<CustomInsuranceRecordEntity> lstInsuranceRecordPregByProfile)
        {
            //14 thang MonthYear
            DateTime MonthPresent = new DateTime(MonthYear.Year, MonthYear.Month, InsuranceServices.PeriodInsuranceDayCurrentMonthDefault);
            if (lstLeaveDayPregByProfile.Any(m => m.DateStart <= MonthPresent && m.DateEnd > MonthPresent) ||
                lstInsuranceRecordPregByProfile.Any(m => m.DateStart <= MonthPresent && m.DateEnd > MonthPresent))
            {
                return true;
            }
            return false;
        }

        #endregion

    }
}

