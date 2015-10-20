using System.ComponentModel;
using System.Reflection;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_AttOvertimePermitConfigServices : BaseService
    {
        #region Properties

        /// <summary> ds Sys_AllSetting (Tạo mới) </summary>
        private List<Sys_AllSetting> allSettingAdding = new List<Sys_AllSetting>();
        /// <summary> ds Sys_AllSetting (Chỉnh Sửa) </summary>
        private List<Sys_AllSetting> allSettingEditing = new List<Sys_AllSetting>();

        List<Sys_AllSetting> _allSettings;
        public List<Sys_AllSetting> AllSettings
        {
            get
            {
                if (_allSettings == null)
                {
                    using (var context = new VnrHrmDataContext())
                    {
                        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                        var repoSys_AllSetting = new CustomBaseRepository<Sys_AllSetting>(unitOfWork);
                        var sys = new Sys_AllSetting();
                        string status = string.Empty;
                        var lstO = new List<object> { null, 1, 10000 };
                        _allSettings = GetData<Sys_AllSetting>(lstO, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);
                        return _allSettings;
                    }
                }
                else
                {
                    return _allSettings;
                }

            }
        }

        #endregion

        #region Public method

        #region Chấm Công

        /// <summary> Lưu cấu hình Chấm Công </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SaveOvertimePermitConfig(Sys_AttConfigEntity entity)
        {
            #region Tao config - Chưa savechange

            #region HRM_ATT_OT_OTPERMIT - Cấu Hình Giới Hạn Tăng Ca
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY, entity.limitHour_ByDay);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV1, entity.limitHour_ByDay_Lev1);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV2, entity.limitHour_ByDay_Lev2);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK, entity.limitHour_ByWeek);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV1, entity.limitHour_ByWeek_Lev1);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV2, entity.limitHour_ByWeek_Lev2);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH, entity.limitHour_ByMonth);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV1, entity.limitHour_ByMonth_Lev1);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV2, entity.limitHour_ByMonth_Lev2);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR, entity.limitHour_ByYear);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV1, entity.limitHour_ByYear_Lev1);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV2, entity.limitHour_ByYear_Lev2);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR, entity.limitColor);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV1, entity.limitColor_Lev1);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV2, entity.limitColor_Lev2);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL, entity.IsAllowOverLimit_Normal);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV1, entity.IsAllowOverLimit_Normal_Lev1);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV2, entity.IsAllowOverLimit_Normal_Lev2);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER, entity.IsAllowOverLimit_AllowOver);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV1, entity.IsAllowOverLimit_AllowOver_Lev1);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV2, entity.IsAllowOverLimit_AllowOver_Lev2);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWSPLIT, entity.IsAllowSplit);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_ISALLOWCUT, entity.IsAllowCut);
            #endregion

            #region  HRM_ATT_WORKDAY_SUMMARY - Cấu Hình Chấm Công
            CreateOrUpdateConfig(AppConfig.HRM_ATT_WORKDAY_SUMMARY_MAXHOURSONESHIFT, entity.MaxHoursOneShift);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_WORKDAY_SUMMARY_MAXHOURSNEXTINOUT, entity.MaxHoursNextInOut);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_WORKDAY_SUMMARY_MINMINUTESSAMEATT, entity.MinMinutesSameAtt);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_WORKDAY_SUMMARY_TYPELOADDATA, entity.TypeLoadData);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_WORKDAY_SUMMARY_SYMBOL, entity.SymbolIn);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_WORKDAY_SUMMARY_DETECTSHIFT, entity.DetectShift);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_VALIDATE_ROSTER_NON_CONTINUE_12HOUR, entity.IsRosterNonContinue12Hour);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_SHIFT_CODE_ROSTERGROUP, entity.ShiftCodeRosterGroup);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_NAME_ROSTERGROUP, entity.NameRosterGroup);

            
            #endregion

            #region HRM_ATT_LEAVE - Cấu Hình Phép
            CreateOrUpdateConfig(AppConfig.HRM_ATT_ANNUALLEAVE_ANNUALBEGINMONTHTOENDMONTH, entity.AnnualBeginMonthToEndMonth);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_ANNUALLEAVE_FORMULARCONFIG, entity.AnnualLeave_FormularConfig);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_ANNUALLEAVE_AllowFinalizationHolidays, entity.Leave_AllowFinalizationHolidays);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_ANNUALLEAVE_FORMULARCOMPUTE, entity.AnnualLeave_FormularCompute);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_ANNUALSICKLEAVE_FORMULARCONFIG, entity.AnnualSickLeave_FormularConfig);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_ANNUALSICKLEAVE_FORMULARCOMPUTE, entity.AnnualSickLeave_FormularCompute);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_ANNUALINSURANCELEAVE_FORMULARCONFIG, entity.AnnualInsuranceLeave_FormularConfig);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_ANNUALINSURANCELEAVE_FORMULARCOMPUTE, entity.AnnualInsuranceLeave_FormularCompute);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_ANNUALDETAIL_ALLOWANALYZEANNUAL, entity.Leave_AllowAnalyzeAnnual);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_ANL_MONTHBEGINYEAR, entity.AnnualMonthBeginYear);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_ANL_DAYBEGIN_FULLMONTH, entity.AnnualDayBeginFullMonth);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_ANL_SENIOR_MONTH, entity.AnnualSeniorMonth);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_ANL_DAY_PER_MONTH, entity.AnnualDayPerMonth);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_ANL_ROUND_UP, entity.AnnualRoundUp);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_ANL_TYPE_PROFILE_BEGIN, entity.AnnualTypeProfileBegin);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL, entity.AnnualDayMaxInMonthGetActual);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR, entity.AnnualNormalCanGetFullYear);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL, entity.AnnualSeniorCanGetEachLevel);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL, entity.AnnualHDT4CanGetMoreNormal);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL, entity.AnnualHDT5CanGetMoreNormal);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_LEAVE_NON_ALN_CODES, entity.AnnualNonALNCodes);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_MONTH_INYEAR_TO_COMPUTE_SENIOR, entity.AnnualMonthInYearToComputeSenior);

            #endregion

            #region HRM_ATT_OT - Tăng Ca
            CreateOrUpdateConfig(AppConfig.HRM_ATT_NOTAUTOREGISTERHOLIDAYLEAVE, entity.NotAutoRegisterHolidayLeave);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_HOLIDAYSCOMPUTE400, entity.HolidaysCompute400);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_MISSTAM_LEAVETYPE, entity.MisstamLeaveType);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_ISALLOWADDHOURWHENOTTHANTWOHOUR, entity.IsAllowAddHourWhenOtThanTwoHour);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_DONOTSPLITOTBREAKTIME, entity.DoNotSplitOtBreaktime);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_ALLOWREGISTEROTWHENINMATERNITYREGIME, entity.AllowRegisterOtWhenInmaternityregime);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_NOCUTOVERTIMEPASSDAY, entity.NoCutOvertimePassday);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_BYPERIODOFTIME, entity.ByperiodOfTime);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_NIGHTSHIFTFROM, entity.NightShiftFrom);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_NIGHTSHIFTTO, entity.NightShiftTo);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_OVERTIMESTATUS, entity.OverTimeStatus);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_ALLOWSAVEDUPLICATE, entity.AllowSaveDuplicate);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_OT_DONOTAPPROVEOTMYSELF, entity.DoNotApproveOTMySeft);
            CreateOrUpdateConfig(AppConfig.HRM_ATT_CONFIG_NUMBER_LEAVE_APPROVE_OVERTIME, entity.NumberLeaveApproveOverTime);            
            #endregion

            #region Tải Dữ Liệu Vào Ra
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.SERVERNAME.ToString(), entity.Att_ServerName1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.USERID.ToString(), entity.Att_UserID1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.PASSWORD.ToString(), entity.Att_Password1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.DBNAME.ToString(), entity.Att_DBName1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.TABLENAME.ToString(), entity.Att_TableName1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.CARDCOLNAME.ToString(), entity.Att_CardColName1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.DATACOLNAME.ToString(), entity.Att_DataColName1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.MACHINECODE.ToString(), entity.Att_SerialNumber1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.TYPE.ToString(), entity.Att_In_Out1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATA.ToString(), entity.Att_FilterData1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.FILTERDATASPLIT.ToString(), entity.Att_FilterDataSplit1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.ISCODEEMP.ToString(), entity.Att_IsCodeEmp1);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString() + AppConfig.ISACTIVATED.ToString(), entity.Att_IsActive1);

            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.SERVERNAME.ToString(), entity.Att_ServerName2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.USERID.ToString(), entity.Att_UserID2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.PASSWORD.ToString(), entity.Att_Password2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.DBNAME.ToString(), entity.Att_DBName2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.TABLENAME.ToString(), entity.Att_TableName2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.CARDCOLNAME.ToString(), entity.Att_CardColName2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.DATACOLNAME.ToString(), entity.Att_DataColName2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.MACHINECODE.ToString(), entity.Att_SerialNumber2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.TYPE.ToString(), entity.Att_In_Out2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATA.ToString(), entity.Att_FilterData2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.FILTERDATASPLIT.ToString(), entity.Att_FilterDataSplit2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.ISCODEEMP.ToString(), entity.Att_IsCodeEmp2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString() + AppConfig.ISACTIVATED.ToString(), entity.Att_IsActive2);
            #endregion

            #endregion

            SaveChangeSysAllSetting(allSettingAdding, allSettingEditing);
            return "0";
        }

        /// <summary>Lấy dữ liệu value1 (Module Chấm Công)  </summary>
        /// <returns></returns>
        public Sys_AttConfigEntity GetOTPermit()
        {
            var dataReturn = new Sys_AttConfigEntity
            {
                #region Get Data value1

                #region HRM_ATT_OT_OTPERMIT
                limitHour_ByDay = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY),
                limitHour_ByDay_Lev1 = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV1),
                limitHour_ByDay_Lev2 = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV2),
                limitHour_ByWeek = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK),
                limitHour_ByWeek_Lev1 = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV1),
                limitHour_ByWeek_Lev2 = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV2),
                limitHour_ByMonth = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH),
                limitHour_ByMonth_Lev1 = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV1),
                limitHour_ByMonth_Lev2 = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV2),
                limitHour_ByYear = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR),
                limitHour_ByYear_Lev1 = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV1),
                limitHour_ByYear_Lev2 = GetConfigValue<double?>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV2),
                limitColor = GetConfigValue<string>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR),
                limitColor_Lev1 = GetConfigValue<string>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV1),
                limitColor_Lev2 = GetConfigValue<string>(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV2),
                IsAllowOverLimit_Normal = GetConfigValue<bool?>(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL),
                IsAllowOverLimit_Normal_Lev1 = GetConfigValue<bool?>(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV1),
                IsAllowOverLimit_Normal_Lev2 = GetConfigValue<bool?>(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV2),
                IsAllowOverLimit_AllowOver = GetConfigValue<bool?>(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER),
                IsAllowOverLimit_AllowOver_Lev1 = GetConfigValue<bool?>(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV1),
                IsAllowOverLimit_AllowOver_Lev2 = GetConfigValue<bool?>(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV2),
                IsAllowSplit = GetConfigValue<bool?>(AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWSPLIT),
                IsAllowCut = GetConfigValue<bool?>(AppConfig.HRM_ATT_OT_ISALLOWCUT),
                #endregion

                #region  HRM_ATT_WORKDAY_SUMMARY
                MaxHoursOneShift = GetConfigValue<double?>(AppConfig.HRM_ATT_WORKDAY_SUMMARY_MAXHOURSONESHIFT),
                MaxHoursNextInOut = GetConfigValue<double?>(AppConfig.HRM_ATT_WORKDAY_SUMMARY_MAXHOURSNEXTINOUT),
                MinMinutesSameAtt = GetConfigValue<double?>(AppConfig.HRM_ATT_WORKDAY_SUMMARY_MINMINUTESSAMEATT),
                TypeLoadData = GetConfigValue<string>(AppConfig.HRM_ATT_WORKDAY_SUMMARY_TYPELOADDATA),
                SymbolIn = GetConfigValue<string>(AppConfig.HRM_ATT_WORKDAY_SUMMARY_SYMBOL),
                DetectShift = GetConfigValue<bool?>(AppConfig.HRM_ATT_WORKDAY_SUMMARY_DETECTSHIFT),
                IsRosterNonContinue12Hour = GetConfigValue<bool?>(AppConfig.HRM_ATT_CONFIG_VALIDATE_ROSTER_NON_CONTINUE_12HOUR),
                ShiftCodeRosterGroup = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_SHIFT_CODE_ROSTERGROUP),
                NameRosterGroup = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_NAME_ROSTERGROUP),   
                #endregion

                #region HRM_ATT_LEAVE
                AnnualLeave_FormularConfig = GetConfigValue<string>(AppConfig.HRM_ATT_ANNUALLEAVE_FORMULARCONFIG),
                Leave_AllowFinalizationHolidays = GetConfigValue<bool>(AppConfig.HRM_ATT_ANNUALLEAVE_AllowFinalizationHolidays),
                AnnualLeave_FormularCompute = GetConfigValue<string>(AppConfig.HRM_ATT_ANNUALLEAVE_FORMULARCOMPUTE),
                AnnualSickLeave_FormularConfig = GetConfigValue<string>(AppConfig.HRM_ATT_ANNUALSICKLEAVE_FORMULARCONFIG),
                AnnualSickLeave_FormularCompute = GetConfigValue<string>(AppConfig.HRM_ATT_ANNUALSICKLEAVE_FORMULARCOMPUTE),
                AnnualInsuranceLeave_FormularConfig = GetConfigValue<string>(AppConfig.HRM_ATT_ANNUALINSURANCELEAVE_FORMULARCONFIG),
                AnnualInsuranceLeave_FormularCompute = GetConfigValue<string>(AppConfig.HRM_ATT_ANNUALINSURANCELEAVE_FORMULARCOMPUTE),
                Leave_AllowAnalyzeAnnual = GetConfigValue<bool?>(AppConfig.HRM_ATT_ANNUALDETAIL_ALLOWANALYZEANNUAL),
                AnnualBeginMonthToEndMonth = GetConfigValue<bool>(AppConfig.HRM_ATT_ANNUALLEAVE_ANNUALBEGINMONTHTOENDMONTH),
                AnnualMonthBeginYear = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_ANL_MONTHBEGINYEAR),
                AnnualDayBeginFullMonth = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_ANL_DAYBEGIN_FULLMONTH),
                AnnualSeniorMonth = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_ANL_SENIOR_MONTH),
                AnnualDayPerMonth = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_ANL_DAY_PER_MONTH),
                AnnualRoundUp = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_ANL_ROUND_UP),
                AnnualTypeProfileBegin = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_ANL_TYPE_PROFILE_BEGIN),
                AnnualDayMaxInMonthGetActual = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL),
                AnnualNormalCanGetFullYear = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR),
                AnnualSeniorCanGetEachLevel = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL),
                AnnualHDT4CanGetMoreNormal = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL),
                AnnualHDT5CanGetMoreNormal = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL),
                AnnualNonALNCodes = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_LEAVE_NON_ALN_CODES),
                AnnualMonthInYearToComputeSenior = GetConfigValue<string>(AppConfig.HRM_ATT_CONFIG_MONTH_INYEAR_TO_COMPUTE_SENIOR),
                        
                #endregion

                #region HRM_ATT_OT
                NotAutoRegisterHolidayLeave = GetConfigValue<bool>(AppConfig.HRM_ATT_NOTAUTOREGISTERHOLIDAYLEAVE),
                HolidaysCompute400 = GetConfigValue<bool>(AppConfig.HRM_ATT_OT_HOLIDAYSCOMPUTE400),
                MisstamLeaveType = GetConfigValue<string>(AppConfig.HRM_ATT_MISSTAM_LEAVETYPE),
                IsAllowAddHourWhenOtThanTwoHour = GetConfigValue<bool>(AppConfig.HRM_ATT_OT_ISALLOWADDHOURWHENOTTHANTWOHOUR),
                DoNotSplitOtBreaktime = GetConfigValue<bool>(AppConfig.HRM_ATT_OT_DONOTSPLITOTBREAKTIME),
                AllowRegisterOtWhenInmaternityregime = GetConfigValue<bool>(AppConfig.HRM_ATT_OT_ALLOWREGISTEROTWHENINMATERNITYREGIME),
                NoCutOvertimePassday = GetConfigValue<bool>(AppConfig.HRM_ATT_OT_NOCUTOVERTIMEPASSDAY),
                ByperiodOfTime = GetConfigValue<string>(AppConfig.HRM_ATT_OT_BYPERIODOFTIME),
                NightShiftFrom = GetConfigValue<DateTime?>(AppConfig.HRM_ATT_OT_NIGHTSHIFTFROM),
                NightShiftTo = GetConfigValue<DateTime?>(AppConfig.HRM_ATT_OT_NIGHTSHIFTTO),
                OverTimeStatus = GetConfigValue<string>(AppConfig.HRM_ATT_OT_OVERTIMESTATUS),
                AllowSaveDuplicate = GetConfigValue<bool?>(AppConfig.HRM_ATT_ALLOWSAVEDUPLICATE),
                DoNotApproveOTMySeft = GetConfigValue<bool?>(AppConfig.HRM_ATT_OT_DONOTAPPROVEOTMYSELF),
                NumberLeaveApproveOverTime = GetConfigValue<int?>(AppConfig.HRM_ATT_CONFIG_NUMBER_LEAVE_APPROVE_OVERTIME),
                #endregion

                #region Hrm_Att_InOut : tải dữ liệu vào ra
                Att_ServerName1 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString()  + AppConfig.SERVERNAME.ToString() ),
                Att_UserID1 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString()  + AppConfig.USERID.ToString() ),
                Att_Password1 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString()  + AppConfig.PASSWORD.ToString() ),
                Att_DBName1 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString()  + AppConfig.DBNAME.ToString() ),
                Att_TableName1 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString()  + AppConfig.TABLENAME.ToString() ),
                Att_CardColName1 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString()  + AppConfig.CARDCOLNAME.ToString() ),
                Att_DataColName1 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString()  + AppConfig.DATACOLNAME.ToString() ),
                Att_SerialNumber1 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString()  + AppConfig.MACHINECODE.ToString() ),
                Att_In_Out1 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString()  + AppConfig.TYPE.ToString() ),
                Att_CodeAttendance1 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString()  + AppConfig.CODEFIELDNAME.ToString() ),
                Att_ServerName2 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString()  + AppConfig.SERVERNAME.ToString() ),
                Att_UserID2 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString()  + AppConfig.USERID.ToString() ),
                Att_Password2 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString()  + AppConfig.PASSWORD.ToString() ),
                Att_DBName2 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString()  + AppConfig.DBNAME.ToString() ),
                Att_TableName2 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString()  + AppConfig.TABLENAME.ToString() ),
                Att_CardColName2 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString()  + AppConfig.CARDCOLNAME.ToString() ),
                Att_SerialNumber2 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString()  + AppConfig.MACHINECODE.ToString() ),
                Att_In_Out2 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString()  + AppConfig.TYPE.ToString() ),
                Att_CodeAttendance2 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString()  + AppConfig.CODEFIELDNAME.ToString() ),
                Att_DataColName2 = GetConfigValue<string>(AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString()  + AppConfig.DATACOLNAME.ToString() ),
                #endregion

                #endregion
            };

            return dataReturn;

            #region Code Cũ

            //string HRM_ATT_OT_OTPERMIT_ = AppConfig.HRM_ATT_OT_OTPERMIT_.ToString();
            //string status = string.Empty;
            //List<object> lstO = new List<object>();
            //lstO.Add(null);
            //lstO.Add(1);
            //lstO.Add(10000);
            //var config = GetData<Sys_AllSetting>(lstO, ConstantSql.hrm_sys_sp_get_AllSetting, ref status);
            //Sys_AttConfigEntity result = new Sys_AttConfigEntity();
            ////HRM_ATT_OT_OTPERMIT
            //#region HRM_ATT_OT_OTPERMIT
            //var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByDay = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV1.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByDay_Lev1 = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV2.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByDay_Lev2 = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByWeek = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV1.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByWeek_Lev1 = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV2.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByWeek_Lev2 = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByMonth = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV1.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByMonth_Lev1 = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV2.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByMonth_Lev2 = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByYear = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV1.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByYear_Lev1 = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV2.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitHour_ByYear_Lev2 = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitColor = objConfig.Value1;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV1.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitColor_Lev1 = objConfig.Value1;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV2.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.limitColor_Lev2 = objConfig.Value1;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.IsAllowOverLimit_Normal = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV1.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.IsAllowOverLimit_Normal_Lev1 = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV2.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.IsAllowOverLimit_Normal_Lev2 = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.IsAllowOverLimit_AllowOver = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV1.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.IsAllowOverLimit_AllowOver_Lev1 = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV2.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.IsAllowOverLimit_AllowOver_Lev2 = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWSPLIT.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.IsAllowSplit = bool.Parse(objConfig.Value1);
            //#endregion

            ////HRM_ATT_WORKDAY_SUMMARY
            //#region HRM_ATT_WORKDAY_SUMMARY
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_MAXHOURSONESHIFT.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.MaxHoursOneShift = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_MAXHOURSNEXTINOUT.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.MaxHoursNextInOut = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_MINMINUTESSAMEATT.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.MinMinutesSameAtt = double.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_TYPELOADDATA.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.TypeLoadData = objConfig.Value1;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_SYMBOL.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.SymbolIn = objConfig.Value1;
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value2) && objConfig.Value2 != "(null)")
            //    result.SymbolOut = objConfig.Value2;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_DETECTSHIFT.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.DetectShift = bool.Parse(objConfig.Value1);
            //#endregion

            ////HRM_ATT_LEAVE
            //#region HRM_ATT_LEAVE
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALLEAVE_FORMULARCONFIG.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.AnnualLeave_FormularConfig = objConfig.Value1;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALLEAVE_FORMULARCOMPUTE.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.AnnualLeave_FormularCompute = objConfig.Value1;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALSICKLEAVE_FORMULARCONFIG.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.AnnualSickLeave_FormularConfig = objConfig.Value1;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALSICKLEAVE_FORMULARCOMPUTE.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.AnnualSickLeave_FormularCompute = objConfig.Value1;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALINSURANCELEAVE_FORMULARCONFIG.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.AnnualInsuranceLeave_FormularConfig = objConfig.Value1;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALINSURANCELEAVE_FORMULARCOMPUTE.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.AnnualInsuranceLeave_FormularCompute = objConfig.Value1;
            //objConfig = config.FirstOrDefault(s => s.Name == AppConfig.HRM_ATT_ANNUALLEAVE_AllowFinalizationHolidays.ToString());
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //{
            //    bool isAllowFinalizationHolidays = false;
            //    Boolean.TryParse(objConfig.Value1, out isAllowFinalizationHolidays);
            //    result.Leave_AllowFinalizationHolidays = isAllowFinalizationHolidays;
            //}

            //#endregion
            ////HRM_ATT_OT
            //#region HRM_ATT_OT
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_NOTAUTOREGISTERHOLIDAYLEAVE.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.NotAutoRegisterHolidayLeave = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_HOLIDAYSCOMPUTE400.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.HolidaysCompute400 = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_MISSTAM_LEAVETYPE.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.MisstamLeaveType = objConfig.Value1;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_ISALLOWADDHOURWHENOTTHANTWOHOUR.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.IsAllowAddHourWhenOtThanTwoHour = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_DONOTSPLITOTBREAKTIME.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.DoNotSplitOtBreaktime = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_ALLOWREGISTEROTWHENINMATERNITYREGIME.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.AllowRegisterOtWhenInmaternityregime = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_NOCUTOVERTIMEPASSDAY.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.NoCutOvertimePassday = bool.Parse(objConfig.Value1);
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_BYPERIODOFTIME.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.ByperiodOfTime = objConfig.Value1;
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_NIGHTSHIFTFROM.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //{
            //    DateTime dt = new DateTime();
            //    string strHours = objConfig.Value1.Substring(0, 2);
            //    string strMinutes = objConfig.Value1.Substring(objConfig.Value1.IndexOf(":") + 1, 2);
            //    dt = dt.AddHours(int.Parse(strHours));
            //    dt = dt.AddMinutes(int.Parse(strMinutes));
            //    result.NightShiftFrom = dt;
            //}
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_NIGHTSHIFTTO.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //{
            //    DateTime dt = new DateTime();
            //    string strHours = objConfig.Value1.Substring(0, 2);
            //    string strMinutes = objConfig.Value1.Substring(objConfig.Value1.IndexOf(":") + 1, 2);
            //    dt = dt.AddHours(int.Parse(strHours));
            //    dt = dt.AddMinutes(int.Parse(strMinutes));
            //    result.NightShiftTo = dt;
            //}
            //objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OVERTIMESTATUS.ToString()).FirstOrDefault();
            //if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            //    result.OverTimeStatus = objConfig.Value1;

            //#endregion
            //return result; 
            #endregion
        }

        #endregion

        #region Nhân Sự

        /// <summary>Lấy dữ liệu value1 ( module Nhân Sự)  </summary>
        /// <returns></returns>
        public Sys_HreConfigEntity GetHrConfig()
        {
            var hrDataReturn = new Sys_HreConfigEntity
            {
                #region tab 1
                IsGroupByOrgProfileQuit = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_GROUPBYORGPROFILEQUIT),
                IsCheckDuplicateProfile = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATEPROFILENAME),
                IsCheckDuplicateCodeEmp = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATECODEEMP),
                IsCheckDuplicateCodeAtt = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATECODEATT),
                IsCheckDuplicateRelatives = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATERELATIVES),
                IsCheckDuplicateProfileAndBirthday = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATEPROFILENAMEANDBIRTHDAY),
                IsCheckDuplicateIDNo = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATEIDNO),
                ProfileQuitColor = GetConfigValue<string>(AppConfig.HRM_HRE_CONFIG_PROFILEQUITCOLOR),
                ProfileQuitBackGroundColor = GetConfigValue<string>(AppConfig.HRM_HRE_CONFIG_PROFILEQUITBACKGROUNDCOLOR),
                GenerateCodeContractFormular = GetConfigValue<string>(AppConfig.HRM_HRE_GENERATE_CODE_CONTRACT),
                DefautCurrency = GetConfigValue<string>(AppConfig.HRM_HRE_CONFIG_DEFAULTCURRENCY),
                IsShowExConByConResult = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_SHOWEXCONBYCONRESULT),
                DaySuspenseExpiry = GetConfigValue<int?>(AppConfig.HRM_HRE_CONFIG_DAYSUSPENSEEXPIRY),
                DayStopWorkingExpiry = GetConfigValue<int?>(AppConfig.HRM_HRE_CONFIG_DAYSTOPWORKINGEXPIRY),
                DayComeBackExpiry = GetConfigValue<int?>(AppConfig.HRM_HRE_CONFIG_DAYCOMEBACKEXPIRY),
                IsInputGeneralCandidateManual = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_INPUTGENERALCANDIDATEMANUAL),
                IsUseCodeEmpOfCustomer = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_ISUSECODEEMPOFCUSTOMER),
                IsAlertIfNumberOfEmpExceedPlan = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_ISALERTIFNUMBEROFEMPEXCEEDPLAN),
                FieldValidateHireProfile = GetConfigValue<string>(AppConfig.HRM_HRE_CONFIG_FIELDVALIDATEHIREPROFILE),
                IsNotUseExpiryContractLoop = GetConfigValue<bool?>(AppConfig.HRM_HRE_CONFIG_ISNOTUSEEXPIRYCONTRACTLOOP),
                #endregion
            };
            return hrDataReturn;
        }

        /// <summary> Lưu cấu hình Nhân Sự </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SaveHrConfig(Sys_HreConfigEntity entity)
        {
            #region Tao config - Chưa savechange

            #region tab 1
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_GROUPBYORGPROFILEQUIT, entity.IsGroupByOrgProfileQuit);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATEPROFILENAME, entity.IsCheckDuplicateProfile);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATECODEEMP, entity.IsCheckDuplicateCodeEmp);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATECODEATT, entity.IsCheckDuplicateCodeAtt);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATERELATIVES, entity.IsCheckDuplicateRelatives);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATEPROFILENAMEANDBIRTHDAY, entity.IsCheckDuplicateProfileAndBirthday);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATEIDNO, entity.IsCheckDuplicateIDNo);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_PROFILEQUITCOLOR, entity.ProfileQuitColor);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_PROFILEQUITBACKGROUNDCOLOR, entity.ProfileQuitBackGroundColor);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_GENERATE_CODE_CONTRACT, entity.GenerateCodeContractFormular);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_DEFAULTCURRENCY, entity.DefautCurrency);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_SHOWEXCONBYCONRESULT, entity.IsShowExConByConResult);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_DAYSUSPENSEEXPIRY, entity.DaySuspenseExpiry);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_DAYSTOPWORKINGEXPIRY, entity.DayStopWorkingExpiry);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_DAYCOMEBACKEXPIRY, entity.DayComeBackExpiry);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_INPUTGENERALCANDIDATEMANUAL, entity.IsInputGeneralCandidateManual);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_ISUSECODEEMPOFCUSTOMER, entity.IsUseCodeEmpOfCustomer);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_ISALERTIFNUMBEROFEMPEXCEEDPLAN, entity.IsAlertIfNumberOfEmpExceedPlan);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_FIELDVALIDATEHIREPROFILE, entity.FieldValidateHireProfile);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONFIG_ISNOTUSEEXPIRYCONTRACTLOOP, entity.IsNotUseExpiryContractLoop);
            #endregion

            #region tab2

            #endregion

            #endregion

            SaveChangeSysAllSetting(allSettingAdding, allSettingEditing);
            return "0";
        }

        #endregion
        #region Cat_TaskAfterImport
        public Sys_CatTaskAfterImportConfigEntity GetCatAfterConfig()
        {
            var hrDataReturn = new Sys_CatTaskAfterImportConfigEntity
            {
                IsAllowAutoUpdateHistoryAttendanceCode = GetConfigValue<bool?>(AppConfig.E_TASK_AFTER_IMPORT_PROFILE)
            };
            return hrDataReturn;
        }

        public string SaveCatAfterConfig(Sys_CatTaskAfterImportConfigEntity entity)
        {
            CreateOrUpdateConfig(AppConfig.E_TASK_AFTER_IMPORT_PROFILE, entity.IsAllowAutoUpdateHistoryAttendanceCode);
            SaveChangeSysAllSetting(allSettingAdding, allSettingEditing);
            return "0";
        }
        #endregion
        #region Lương
        /// <summary>Lấy dữ liệu value1 ( module Lương)  </summary>
        /// <returns></returns>
        public Sys_SalConfigEntity GetPayrollConfig()
        {
            var hrDataReturn = new Sys_SalConfigEntity
            {
                #region tab 1
                ConfigElement = GetConfigValue<string>(AppConfig.HRM_SAL_PAYROLL_CONFIGELEMENT),
                Value8 = GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE8),
                Value52 = GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE52),
                Value53 = GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE53),
                Value54 = GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE54),
                Value55 = GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE55),
                Value56 = GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE56),
                Value57 = GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE57),
                Value58 = GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE58),
                Value59 = GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE59),
                HRM_SAL_HOLDSALARY_ELEMENT = GetConfigValue<string>(AppConfig.HRM_SAL_HOLDSALARY_ELEMENT),
                HRM_SAL_HOLDSALARY_ELEMENT_AFTERTAX = GetConfigValue<string>(AppConfig.HRM_SAL_HOLDSALARY_ELEMENT_AFTERTAX),
                HRM_SAL_DATECLOSE_ALLOWANCE = GetConfigValue<DateTime>(AppConfig.HRM_SAL_DATECLOSE_ALLOWANCE),
                HRM_SAL_DATECLOSE_SALARY = GetConfigValue<double?>(AppConfig.HRM_SAL_DATECLOSE_SALARY),
                HRM_SAL_ELEMENT_REALWAGES = GetConfigValue<string>(AppConfig.HRM_SAL_ELEMENT_REALWAGES),
                HRM_SAL_COMPUTEPAYROLL_ORDERNUMBER = GetConfigValue<bool>(AppConfig.HRM_SAL_COMPUTEPAYROLL_ORDERNUMBER),
                #endregion
            };
            return hrDataReturn;
        }

        /// <summary> Lưu cấu hình Lương </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SavePayrollConfig(Sys_SalConfigEntity entity)
        {
            #region Tao config - Chưa savechange

            #region tab 1
            CreateOrUpdateConfig(AppConfig.HRM_SAL_PAYROLL_CONFIGELEMENT, entity.ConfigElement);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_CONFIG_VALUE8, entity.Value8);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_CONFIG_VALUE52, entity.Value52);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_CONFIG_VALUE53, entity.Value53);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_CONFIG_VALUE54, entity.Value54);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_CONFIG_VALUE55, entity.Value55);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_CONFIG_VALUE56, entity.Value56);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_CONFIG_VALUE57, entity.Value57);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_CONFIG_VALUE58, entity.Value58);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_CONFIG_VALUE59, entity.Value59);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_HOLDSALARY_ELEMENT, entity.HRM_SAL_HOLDSALARY_ELEMENT);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_HOLDSALARY_ELEMENT_AFTERTAX, entity.HRM_SAL_HOLDSALARY_ELEMENT_AFTERTAX);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_DATECLOSE_ALLOWANCE, entity.HRM_SAL_DATECLOSE_ALLOWANCE);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_DATECLOSE_SALARY, entity.HRM_SAL_DATECLOSE_SALARY);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_ELEMENT_REALWAGES, entity.HRM_SAL_ELEMENT_REALWAGES);
            CreateOrUpdateConfig(AppConfig.HRM_SAL_COMPUTEPAYROLL_ORDERNUMBER, entity.HRM_SAL_COMPUTEPAYROLL_ORDERNUMBER);
            

            
            #endregion

            #region tab2

            #endregion

            #endregion

            SaveChangeSysAllSetting(allSettingAdding, allSettingEditing);
            return "0";
        }
        #endregion

        #region Cấu hình chung

        /// <summary>Lấy dữ liệu value1 ( module Nhân Sự)  </summary>
        /// <returns></returns>
        public Sys_GeneralConfigEntity GetGeneralConfig()
        {
            var hrDataReturn = new Sys_GeneralConfigEntity
            {
                #region tab 1
                MailPassword = GetConfigValue<string>(AppConfig.HRM_SYS_CONFIG_MAIL_MAILPASSWORD),
                MailServer = GetConfigValue<string>(AppConfig.HRM_SYS_CONFIG_MAIL_MAILSERVER),
                MailUserName = GetConfigValue<string>(AppConfig.HRM_SYS_CONFIG_MAIL_MAILUSERNAME),
                IMAP2 = GetConfigValue<bool?>(AppConfig.HRM_SYS_CONFIG_MAIL_IMAP2),
                IMAP4 = GetConfigValue<bool?>(AppConfig.HRM_SYS_CONFIG_MAIL_IMAP4),
                NoTLS = GetConfigValue<bool?>(AppConfig.HRM_SYS_CONFIG_MAIL_NOTLS),
                SSL = GetConfigValue<bool?>(AppConfig.HRM_SYS_CONFIG_MAIL_SSL),
                TLS = GetConfigValue<bool?>(AppConfig.HRM_SYS_CONFIG_MAIL_TLS),
                ExcelPasswordReadonly = GetConfigValue<string>(AppConfig.HRM_CAT_CONFIG_EXPORT_EXCELPASSREADONLY),
                ExcelPasswordOpenFile = GetConfigValue<string>(AppConfig.HRM_CAT_CONFIG_EXPORT_EXCELPASSOPENFILE),

                ShowValue1 = GetConfigValue<bool>(AppConfig.HRM_HRE_CONTRACT_ALERT_EXPRIDAY),
                ShowAfterDate1 = GetConfigValue<string>(AppConfig.HRM_HRE_CONTRACT_ALERT_EXPRIDAY_VALUEAFTE),
                ShowBeforDate1 = GetConfigValue<string>(AppConfig.HRM_HRE_CONTRACT_ALERT_EXPRIDAY_VALUEBEFOR),

                ShowValue2 = GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY),
                ShowAfterDate2 = GetConfigValue<string>(AppConfig.HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY_VALUEAFTER),
                ShowBeforDate2 = GetConfigValue<string>(AppConfig.HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY_VALUEBEFOR),

                ShowValue3 = GetConfigValue<bool>(AppConfig.E_NEWEMPLOYEE),
                ShowAfterDate3 = GetConfigValue<string>(AppConfig.E_NEWEMPLOYEE_VALUEAFTER),
                ShowBeforDate3 = GetConfigValue<string>(AppConfig.E_NEWEMPLOYEE_VALUEBEFOR),

                ShowValue12 = GetConfigValue<bool>(AppConfig.SAL_SETTLEMENT),
                ShowAfterDate12 = GetConfigValue<string>(AppConfig.SAL_SETTLEMENT_VALUEAFTER),
                ShowBeforDate12 = GetConfigValue<string>(AppConfig.SAL_SETTLEMENT_VALUEBEFOR),

                ShowValue4 = GetConfigValue<bool>(AppConfig.E_STOPWORKING),
                ShowAfterDate4 = GetConfigValue<string>(AppConfig.E_STOPWORKING_VALUEAFTER),
                ShowBeforDate4 = GetConfigValue<string>(AppConfig.E_STOPWORKING_VALUEBEFOR),

                ShowValue5 = GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY),
                ShowAfterDate5 = GetConfigValue<string>(AppConfig.HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY_VALUEAFTER),
                ShowBeforDate5 = GetConfigValue<string>(AppConfig.HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY_VALUEBEFOR),

                ShowValue6 = GetConfigValue<bool>(AppConfig.VISAED),
                ShowAfterDate6 = GetConfigValue<string>(AppConfig.VISAED_VALUEAFTER),
                ShowBeforDate6 = GetConfigValue<string>(AppConfig.VISAED_VALUEBEFOR),

                ShowValue7 = GetConfigValue<bool>(AppConfig.PAPRlABORPHEPSLED),
                ShowAfterDate7 = GetConfigValue<string>(AppConfig.PAPRlABORPHEPSLED_VALUEAFTER),
                ShowBeforDate7 = GetConfigValue<string>(AppConfig.PAPRlABORPHEPSLED_VALUEBEFOR),

                ShowValue8 = GetConfigValue<bool>(AppConfig.USOKPERMITED_EMP_WORTH),
                ShowAfterDate8 = GetConfigValue<string>(AppConfig.USOKPERMITED_EMP_WORTH_VALUEAFTER),
                ShowBeforDate8 = GetConfigValue<string>(AppConfig.USOKPERMITED_EMP_WORTH_VALUEBEFOR),

                ShowValue9 = GetConfigValue<bool>(AppConfig.EMP_LETTERED),
                ShowAfterDate9 = GetConfigValue<string>(AppConfig.EMP_LETTERED_VALUEAFTER),
                ShowBeforDate9 = GetConfigValue<string>(AppConfig.EMP_LETTERED_VALUEBEFOR),

                ShowValue10 = GetConfigValue<bool>(AppConfig.E_REQUEST),
                ShowAfterDate10 = GetConfigValue<string>(AppConfig.E_REQUEST_VALUEAFTER),
                ShowBeforDate10 = GetConfigValue<string>(AppConfig.E_REQUESt_VALUEBEFOR),

                ShowValue11 = GetConfigValue<bool>(AppConfig.TRA_CERTIFICATED),
                ShowAfterDate11 = GetConfigValue<string>(AppConfig.TRA_CERTIFICATED_VALUEAFTER),
                ShowBeforDate11 = GetConfigValue<string>(AppConfig.TRA_CERTIFICATED_VALUEBEFOR),
                DefaultApproved = GetConfigValue<bool?>(AppConfig.HRM_CONFIG_DEFAULTAPPROVED),
                DayEndFinance=GetConfigValue<DateTime?>(AppConfig.HRM_EVA_CONFIG_DAYENDYEARFINANCE),
                DefaultLanguage = GetConfigValue<string>(AppConfig.HRM_SYS_SETTING_LANGUAGE_DEFAULT),

                #endregion
                #region tab3
                IsShowBasicSalaryWaitingAprove=GetConfigValue<bool>(AppConfig.HRM_SAL_BASICSALARY_ALERT_WAITINGAPROVE),
                DayShowBasicSalaryWaitingAproveFrom = GetConfigValue<int?>(AppConfig.HRM_SAL_BASICSALARY_ALERT_DAYWAITINGAPROVEFROM),
                DayShowBasicSalaryWaitingAproveTo = GetConfigValue<int?>(AppConfig.HRM_SAL_BASICSALARY_ALERT_DAYWAITINGAPROVETO),



                IsShowContractWaitingAprove = GetConfigValue<bool>(AppConfig.HRM_HRE_CONTRACT_ALERT_WAITINGAPROVE),
                DayShowContractWaitingAproveFrom = GetConfigValue<int?>(AppConfig.HRM_HRE_CONTRACT_ALERT_DAYWAITINGAPROVEFROM),
                DayShowContractWaitingAproveTo = GetConfigValue<int?>(AppConfig.HRM_HRE_CONTRACT_ALERT_DAYWAITINGAPROVETO),



                IsShowContractAppendixExpriday = GetConfigValue<bool>(AppConfig.HRM_HRE_CONTRACT_ALERT_APPENDDIXEXPRIDAY),
                DayShowContractAppendixExpridayFrom = GetConfigValue<int?>(AppConfig.HRM_HRE_CONTRACT_ALERT_DAYAPPENDDIXEXPRIDAYFROM),
                DayShowContractAppendixExpridayTo = GetConfigValue<int?>(AppConfig.HRM_HRE_CONTRACT_ALERT_DAYAPPENDDIXEXPRIDAYTO),


                IsShowHDTJobWaitingApproved = GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_HDTJOBWAINTINGAPPROVED),
                DayShowHDTJobWaitingApprovedFrom = GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYHDTJOBWAINTINGAPPROVEDFROM),
                DayShowHDTJobWaitingApprovedTo = GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYHDTJOBWAINTINGAPPROVEDTO),


                IsShowDaySuspenseExpiry = GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRY),
                DayShowDaySuspenseExpiryFrom = GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRYFROM),
                DayShowDaySuspenseExpiryTo = GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRYTO),


                IsShowDayStopWorking = GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSTOPWORKING),
                DayShowDayStopWorkingFrom = GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSTOPWORKINGFROM),
                DayShowDayStopWorkingTo = GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSTOPWORKINGTO),



                IsShowDayComeBackExpiry = GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRY),
                DayShowDayComeBackExpiryFrom = GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRYFROM),
                DayShowDayComeBackExpiryTo = GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRYTO),


                IsShowEvalutionWaiting = GetConfigValue<bool>(AppConfig.HRM_EVA_PROFILE_ALERT_EVALUTIONWAITING),
                DayShowEvalutionWaitingFrom = GetConfigValue<int?>(AppConfig.HRM_EVA_PROFILE_ALERT_DAYEVALUTIONWAITINGFROM),
                DayShowEvalutionWaitingTo = GetConfigValue<int?>(AppConfig.HRM_EVA_PROFILE_ALERT_DAYEVALUTIONWAITINGTO),

                #endregion
            };
            return hrDataReturn;
        }

        /// <summary> Lưu cấu hình chung </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SaveGeneralConfig(Sys_GeneralConfigEntity entity)
        {
            #region Tao config - Chưa savechange

            #region tab 1
            CreateOrUpdateConfig(AppConfig.HRM_SYS_CONFIG_MAIL_MAILPASSWORD, entity.MailPassword);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_CONFIG_MAIL_MAILSERVER, entity.MailServer);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_CONFIG_MAIL_MAILUSERNAME, entity.MailUserName);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_CONFIG_MAIL_IMAP2, entity.IMAP2);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_CONFIG_MAIL_IMAP4, entity.IMAP4);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_CONFIG_MAIL_NOTLS, entity.NoTLS);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_CONFIG_MAIL_SSL, entity.SSL);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_CONFIG_MAIL_TLS, entity.TLS);
            CreateOrUpdateConfig(AppConfig.HRM_CAT_CONFIG_EXPORT_EXCELPASSREADONLY, entity.ExcelPasswordReadonly);
            CreateOrUpdateConfig(AppConfig.HRM_CAT_CONFIG_EXPORT_EXCELPASSOPENFILE, entity.ExcelPasswordOpenFile);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONTRACT_ALERT_EXPRIDAY, entity.ShowValue1);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONTRACT_ALERT_EXPRIDAY_VALUEAFTE, entity.ShowAfterDate1);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_CONTRACT_ALERT_EXPRIDAY_VALUEBEFOR, entity.ShowBeforDate1);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY, entity.ShowValue2);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY_VALUEBEFOR, entity.ShowBeforDate2);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY_VALUEAFTER, entity.ShowAfterDate2);
            CreateOrUpdateConfig(AppConfig.E_NEWEMPLOYEE, entity.ShowValue3);
            CreateOrUpdateConfig(AppConfig.E_NEWEMPLOYEE_VALUEAFTER, entity.ShowAfterDate3);
            CreateOrUpdateConfig(AppConfig.E_NEWEMPLOYEE_VALUEBEFOR, entity.ShowBeforDate3);
            CreateOrUpdateConfig(AppConfig.E_STOPWORKING, entity.ShowValue4);
            CreateOrUpdateConfig(AppConfig.E_STOPWORKING_VALUEAFTER, entity.ShowAfterDate4);
            CreateOrUpdateConfig(AppConfig.E_STOPWORKING_VALUEBEFOR, entity.ShowBeforDate4);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY, entity.ShowValue5);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY_VALUEAFTER, entity.ShowAfterDate5);
            CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY_VALUEBEFOR, entity.ShowBeforDate5);
            CreateOrUpdateConfig(AppConfig.VISAED, entity.ShowValue6);
            CreateOrUpdateConfig(AppConfig.VISAED_VALUEAFTER, entity.ShowAfterDate6 );
            CreateOrUpdateConfig(AppConfig.VISAED_VALUEBEFOR, entity.ShowBeforDate6);
            CreateOrUpdateConfig(AppConfig.PAPRlABORPHEPSLED, entity.ShowValue7);
            CreateOrUpdateConfig(AppConfig.PAPRlABORPHEPSLED_VALUEAFTER, entity.ShowAfterDate7);
            CreateOrUpdateConfig(AppConfig.PAPRlABORPHEPSLED_VALUEBEFOR, entity.ShowBeforDate7);
            CreateOrUpdateConfig(AppConfig.USOKPERMITED_EMP_WORTH, entity.ShowValue8);
            CreateOrUpdateConfig(AppConfig.USOKPERMITED_EMP_WORTH_VALUEAFTER, entity.ShowAfterDate8);
            CreateOrUpdateConfig(AppConfig.USOKPERMITED_EMP_WORTH_VALUEBEFOR, entity.ShowBeforDate8);
            CreateOrUpdateConfig(AppConfig.EMP_LETTERED, entity.ShowValue9);
            CreateOrUpdateConfig(AppConfig.EMP_LETTERED_VALUEAFTER, entity.ShowAfterDate9);
            CreateOrUpdateConfig(AppConfig.EMP_LETTERED_VALUEBEFOR,  entity.ShowBeforDate9);
            CreateOrUpdateConfig(AppConfig.E_REQUEST, entity.ShowValue10);
            CreateOrUpdateConfig(AppConfig.E_REQUEST_VALUEAFTER, entity.ShowAfterDate10);
            CreateOrUpdateConfig(AppConfig.E_REQUESt_VALUEBEFOR, entity.ShowBeforDate10);
            CreateOrUpdateConfig(AppConfig.TRA_CERTIFICATED, entity.ShowValue11);
            CreateOrUpdateConfig(AppConfig.TRA_CERTIFICATED_VALUEAFTER, entity.ShowAfterDate11);
            CreateOrUpdateConfig(AppConfig.TRA_CERTIFICATED_VALUEBEFOR, entity.ShowBeforDate11);
            CreateOrUpdateConfig(AppConfig.SAL_SETTLEMENT, entity.ShowValue12);
            CreateOrUpdateConfig(AppConfig.SAL_SETTLEMENT_VALUEAFTER, entity.ShowAfterDate12);
            CreateOrUpdateConfig(AppConfig.SAL_SETTLEMENT_VALUEBEFOR, entity.ShowBeforDate12);
            CreateOrUpdateConfig(AppConfig.HRM_CONFIG_DEFAULTAPPROVED, entity.DefaultApproved);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_DAYENDYEARFINANCE, entity.DayEndFinance);
            CreateOrUpdateConfig(AppConfig.HRM_SYS_SETTING_LANGUAGE_DEFAULT, entity.DefaultLanguage);
            #endregion

            #region tab2

            #endregion
            #region Tab3
             CreateOrUpdateConfig(AppConfig.HRM_SAL_BASICSALARY_ALERT_WAITINGAPROVE, entity.IsShowBasicSalaryWaitingAprove);
             CreateOrUpdateConfig(AppConfig.HRM_SAL_BASICSALARY_ALERT_DAYWAITINGAPROVEFROM, entity.DayShowBasicSalaryWaitingAproveFrom);
             CreateOrUpdateConfig(AppConfig.HRM_SAL_BASICSALARY_ALERT_DAYWAITINGAPROVETO, entity.DayShowBasicSalaryWaitingAproveTo);


             CreateOrUpdateConfig(AppConfig.HRM_HRE_CONTRACT_ALERT_WAITINGAPROVE, entity.IsShowContractWaitingAprove);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_CONTRACT_ALERT_DAYWAITINGAPROVEFROM, entity.DayShowContractWaitingAproveFrom);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_CONTRACT_ALERT_DAYWAITINGAPROVETO, entity.DayShowContractWaitingAproveTo);



             CreateOrUpdateConfig(AppConfig.HRM_HRE_CONTRACT_ALERT_APPENDDIXEXPRIDAY, entity.IsShowContractAppendixExpriday);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_CONTRACT_ALERT_DAYAPPENDDIXEXPRIDAYFROM, entity.DayShowContractAppendixExpridayFrom);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_CONTRACT_ALERT_DAYAPPENDDIXEXPRIDAYTO, entity.DayShowContractAppendixExpridayTo);


             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_HDTJOBWAINTINGAPPROVED, entity.IsShowHDTJobWaitingApproved);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_DAYHDTJOBWAINTINGAPPROVEDFROM, entity.DayShowHDTJobWaitingApprovedFrom);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_DAYHDTJOBWAINTINGAPPROVEDTO, entity.DayShowHDTJobWaitingApprovedTo);


             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRY, entity.IsShowDaySuspenseExpiry);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRYFROM, entity.DayShowDaySuspenseExpiryFrom);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRYTO, entity.DayShowDaySuspenseExpiryTo);



             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSTOPWORKING, entity.IsShowDayStopWorking);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSTOPWORKINGFROM, entity.DayShowDayStopWorkingFrom);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSTOPWORKINGTO, entity.DayShowDayStopWorkingTo);


             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRY, entity.IsShowDayComeBackExpiry);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRYFROM, entity.DayShowDayComeBackExpiryFrom);
             CreateOrUpdateConfig(AppConfig.HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRYTO, entity.DayShowDayComeBackExpiryTo);


             CreateOrUpdateConfig(AppConfig.HRM_EVA_PROFILE_ALERT_EVALUTIONWAITING, entity.IsShowEvalutionWaiting);
             CreateOrUpdateConfig(AppConfig.HRM_EVA_PROFILE_ALERT_DAYEVALUTIONWAITINGFROM, entity.DayShowEvalutionWaitingFrom);
             CreateOrUpdateConfig(AppConfig.HRM_EVA_PROFILE_ALERT_DAYEVALUTIONWAITINGTO, entity.DayShowEvalutionWaitingTo);


            #endregion
            #endregion

            SaveChangeSysAllSetting(allSettingAdding, allSettingEditing);
            return "0";
        }


        #endregion

        #region Đánh Giá
        /// <summary>Lấy dữ liệu value1 ( module Đánh Giá)  </summary>
        /// <returns></returns>
        public Sys_EvaConfigEntity GetEvaConfig()
        {
            var hrDataReturn = new Sys_EvaConfigEntity
            {
                #region tab 1

                DateStartMark = GetConfigValue<DateTime?>(AppConfig.HRM_EVA_CONFIG_DATESTARTMARK),
                DateEndMark = GetConfigValue<DateTime?>(AppConfig.HRM_EVA_CONFIG_DATEENDMARK),

                DateStartTime1 = GetConfigValue<DateTime?>(AppConfig.HRM_EVA_CONFIG_DATESTARTNODETIME1),
                DateEndTime1 = GetConfigValue<DateTime?>(AppConfig.HRM_EVA_CONFIG_DATEENDNODETIME1),
                DateStartTime2 = GetConfigValue<DateTime?>(AppConfig.HRM_EVA_CONFIG_DATESTARTNODETIME2),
                DateEndTime2 = GetConfigValue<DateTime?>(AppConfig.HRM_EVA_CONFIG_DATEENDNODETIME2),

                #endregion
                #region tab 2
                IsEmployeeEvaluation = GetConfigValue<bool?>(AppConfig.HRM_EVA_CONFIG_EMAIL_ISEMPLOYEEEVALUATION),
                //EmployeeEvaluationContent = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_EMAIL_EMPLOYEEEVALUATION_CONTENT),
                IsUserApproveEvaluation1 = GetConfigValue<bool?>(AppConfig.HRM_EVA_CONFIG_EMAIL_ISUSERAPPROVEVALUATION1),
                //UserApproveEvaluationContent1 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_EMAIL_USERAPPROVEVALUATION_CONTENT1),
                IsUserApproveEvaluation2 = GetConfigValue<bool?>(AppConfig.HRM_EVA_CONFIG_EMAIL_ISUSERAPPROVEVALUATION2),
                //UserApproveEvaluationContent2 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_EMAIL_USERAPPROVEVALUATION_CONTENT2),
                IsCycle = GetConfigValue<bool?>(AppConfig.HRM_EVA_CONFIG_EMAIL_ISCYCLE),
                CycleDay1 = GetConfigValue<int?>(AppConfig.HRM_EVA_CONFIG_EMAIL_CYCLE_DAY1),
                CycleDay2 = GetConfigValue<int?>(AppConfig.HRM_EVA_CONFIG_EMAIL_CYCLE_DAY2),
                CycleDay3 = GetConfigValue<int?>(AppConfig.HRM_EVA_CONFIG_EMAIL_BYCYCLE_DAY3),
                IsTime = GetConfigValue<bool?>(AppConfig.HRM_EVA_CONFIG_EMAIL_ISTIME),
                TimeDateFrom = GetConfigValue<int?>(AppConfig.HRM_EVA_CONFIG_EMAIL_TIME_DATEFROM),
                TimeDateTo = GetConfigValue<int?>(AppConfig.HRM_EVA_CONFIG_EMAIL_TIME_DATETO),
                #endregion
                #region tab 3
                DataC1 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C1),
                DataC2 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C2),
                DataC3 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C3),
                DataC4 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C4),
                DataC5 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C5),
                DataC6 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C6),
                DataC7 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C7),
                DataC8 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C8),
                DataC9 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C9),
                DataC10 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C10),
                DataC11 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C11),
                DataC12 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C12),
                DataC13 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C13),
                DataC14 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C14),
                DataC15 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C15),
                DataC16 = GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C16),
                #endregion
            
            };
            return hrDataReturn;
        }

        /// <summary> Lưu cấu hình Đánh Giá </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SaveEvaConfig(Sys_EvaConfigEntity entity)
        {
            #region Tao config - Chưa savechange

            #region tab 1
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_DATESTARTMARK, entity.DateStartMark);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_DATEENDMARK, entity.DateEndMark);

            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_DATESTARTNODETIME1, entity.DateStartTime1);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_DATEENDNODETIME1, entity.DateEndTime1);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_DATESTARTNODETIME2, entity.DateStartTime2);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_DATEENDNODETIME2, entity.DateEndTime2);
            #endregion
            #region tab 2
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_ISEMPLOYEEEVALUATION, entity.IsEmployeeEvaluation);
            //CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_EMPLOYEEEVALUATION_CONTENT, entity.EmployeeEvaluationContent);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_ISUSERAPPROVEVALUATION1, entity.IsUserApproveEvaluation1);
            //CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_USERAPPROVEVALUATION_CONTENT1, entity.UserApproveEvaluationContent1);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_ISUSERAPPROVEVALUATION2, entity.IsUserApproveEvaluation2);
            //CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_USERAPPROVEVALUATION_CONTENT2, entity.UserApproveEvaluationContent2);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_ISCYCLE, entity.IsCycle);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_CYCLE_DAY1, entity.CycleDay1);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_CYCLE_DAY2, entity.CycleDay2);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_BYCYCLE_DAY3, entity.CycleDay3);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_ISTIME, entity.IsTime);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_TIME_DATEFROM, entity.TimeDateFrom);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_EMAIL_TIME_DATETO, entity.TimeDateTo);
            #endregion

            #endregion
            #region Tab 3
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C1, entity.DataC1);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C2, entity.DataC2);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C3, entity.DataC3);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C4, entity.DataC4);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C5, entity.DataC5);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C6, entity.DataC6);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C7, entity.DataC7);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C8, entity.DataC8);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C9, entity.DataC9);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C10, entity.DataC10);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C11, entity.DataC11);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C12, entity.DataC12);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C13, entity.DataC13);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C14, entity.DataC14);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C15, entity.DataC15);
            CreateOrUpdateConfig(AppConfig.HRM_EVA_CONFIG_SAVE_C16, entity.DataC16);

            #endregion

            SaveChangeSysAllSetting(allSettingAdding, allSettingEditing);
            return "0";
        }
        #endregion

        #region Đào Tạo
        /// <summary>Lấy dữ liệu value1 ( module Đào Tạo)  </summary>
        /// <returns></returns>
        public Sys_TraConfigEntity GetTraConfig()
        {
            var hrDataReturn = new Sys_TraConfigEntity
            {
                #region tab 1

                ExpiredDayWarningFrom = GetConfigValue<int?>(AppConfig.HRM_TRA_CONFIG_TRAIN_EXPIR_DAY_WARRING_FROM),
                ExpiredDayWarningTo = GetConfigValue<int?>(AppConfig.HRM_TRA_CONFIG_TRAIN_EXPIR_DAY_WARRING_TO),
                TraineeInRequirementDetail = GetConfigValue<bool?>(AppConfig.HRM_TRA_CONFIG_TRAINEE_IN_REQUIREMENTDETAIL),

                #endregion
            };
            return hrDataReturn;
        }

        /// <summary> Lưu cấu hình Đào Tạo </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SaveTraConfig(Sys_TraConfigEntity entity)
        {
            #region Tao config - Chưa savechange

            #region tab 1
            CreateOrUpdateConfig(AppConfig.HRM_TRA_CONFIG_TRAIN_EXPIR_DAY_WARRING_FROM, entity.ExpiredDayWarningFrom);
            CreateOrUpdateConfig(AppConfig.HRM_TRA_CONFIG_TRAIN_EXPIR_DAY_WARRING_TO, entity.ExpiredDayWarningTo);
            CreateOrUpdateConfig(AppConfig.HRM_TRA_CONFIG_TRAINEE_IN_REQUIREMENTDETAIL, entity.TraineeInRequirementDetail);

            #endregion

            #endregion

            SaveChangeSysAllSetting(allSettingAdding, allSettingEditing);
            return "0";
        }
        #endregion

        #region Bảo Hiểm
        /// <summary>Lấy dữ liệu value1 ( module Bảo Hiểm)  </summary>
        /// <returns></returns>
        public Sys_InsConfigEntity GetInsConfig()
        {
            var insDataReturn = new Sys_InsConfigEntity
            {
                #region tab 1

                SumDataTempt = GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_SUMDATATEMPT),
                SumDataMain = GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_SUMDATAMAIN),
                PeriodInsuranceDay = GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEYDAY),
                PeriodInsuranceDayDecreaseFrom = GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYDESCREASEFROM),
                PeriodInsuranceDayDecreaseTo = GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYDESCREASETO),
                PeriodInsuranceDayDefaultFrom = GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYDEFAULTFROM),
                PeriodInsuranceDayDefaultTo = GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYDEFAULTTO),
                PeriodInsurance14DayFrom = GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCE14DAYFROM),
                PeriodInsurance14DayTo = GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCE14DAYTO),
                PeriodInsuranceDayChangeSalFrom = GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYCHANGESALFROM),
                PeriodInsuranceDayChangeSalTo = GetConfigValue<int?>(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYCHANGESALTO),                
                #endregion
            };
            return insDataReturn;
        }

        /// <summary> Lưu cấu hình Bảo Hiểm </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SaveInsConfig(Sys_InsConfigEntity entity)
        {
            #region Tao config - Chưa savechange

            #region tab 1
            CreateOrUpdateConfig(AppConfig.HRM_INS_CONFIG_SUMDATATEMPT, entity.SumDataTempt);
            CreateOrUpdateConfig(AppConfig.HRM_INS_CONFIG_SUMDATAMAIN, entity.SumDataMain);
            CreateOrUpdateConfig(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEYDAY, entity.PeriodInsuranceDay);
            CreateOrUpdateConfig(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYDESCREASEFROM, entity.PeriodInsuranceDayDecreaseFrom);
            CreateOrUpdateConfig(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYDESCREASETO, entity.PeriodInsuranceDayDecreaseTo);
            CreateOrUpdateConfig(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYDEFAULTFROM, entity.PeriodInsuranceDayDefaultFrom);
            CreateOrUpdateConfig(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYDEFAULTTO, entity.PeriodInsuranceDayDefaultTo);
            CreateOrUpdateConfig(AppConfig.HRM_INS_CONFIG_PERIODINSURANCE14DAYFROM, entity.PeriodInsurance14DayFrom);
            CreateOrUpdateConfig(AppConfig.HRM_INS_CONFIG_PERIODINSURANCE14DAYTO, entity.PeriodInsurance14DayTo);
            CreateOrUpdateConfig(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYCHANGESALFROM, entity.PeriodInsuranceDayChangeSalFrom);
            CreateOrUpdateConfig(AppConfig.HRM_INS_CONFIG_PERIODINSURANCEDAYCHANGESALTO, entity.PeriodInsuranceDayChangeSalTo);

            #endregion

            #endregion

            SaveChangeSysAllSetting(allSettingAdding, allSettingEditing);
            return "0";
        }
        #endregion

        public IEnumerable<Sys_AllSettingEntity> GetSys_AllSetting()
        {
            string HRM_ATT_OT_OTPERMIT_ = AppConfig.HRM_ATT_OT_OTPERMIT_.ToString();
            string status = string.Empty;
            List<object> lstO = new List<object>();
            lstO.Add(HRM_ATT_OT_OTPERMIT_);
            lstO.Add(1);
            lstO.Add(10000);
            var config = GetData<Sys_AllSettingEntity>(lstO, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);
            return config;
        }

        #endregion 

        #region Utilities [Tung.Ly 20141210]

        #region Private Method

        /// <summary>[Tung.Ly 20141210] Tạo mới hoặc chỉnh sửa Config (Chưa saveChange) </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isSaveChange">Cho phép saveChange không (mặc định không saveChange)</param>
        private void CreateOrUpdateConfig(AppConfig key, object value, bool isSaveChange = false)
        {
            CreateOrUpdateConfig(key.ToString(), value, isSaveChange);
        }

        /// <summary>[Tung.Ly 20141210] Tạo mới hoặc chỉnh sửa Config (Chưa saveChange) </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isSaveChange">Cho phép saveChange không (mặc định không saveChange)</param>
        private void CreateOrUpdateConfig(string key, object value, bool isSaveChange = false)
        {
            Sys_AllSetting objConfig = null;
            string moduleName = null;

            if (key.Length > 7)
            {
                var moduleKeys = Enum.GetNames(typeof(ModuleKey)).ToList();

                if (moduleKeys.Contains(key.Substring(4, 3)))
                {
                    moduleName = key.Substring(4, 3);
                }
            }

            var _value = value != null ? value.ToString() : null;
            //kt co key ko ?
            objConfig = AllSettings.FirstOrDefault(s => s.Name == key);
            if (objConfig != null)
            {
                //kt bien allSetting  da chua key chua?
                if (allSettingEditing.FirstOrDefault(p => p.Name == key) == null && objConfig.Value1 != _value)
                {
                    //co key (edit)
                    objConfig.Value1 = _value;
                    objConfig.ModuleName = moduleName;
                    allSettingEditing.Add(objConfig);
                }
            }
            else
            {
                //ko co key (add)
                objConfig = new Sys_AllSetting
                {
                    ID = Guid.NewGuid(),
                    Name = key,
                    ModuleName = moduleName,
                    Value1 = _value
                };
                //kt bien allSetting  da chua key chua?
                if (allSettingAdding.FirstOrDefault(p => p.Name == key) == null)
                {
                    allSettingAdding.Add(objConfig);
                }
            }
            if (isSaveChange)
            {
                SaveChangeSysAllSetting(allSettingAdding, allSettingEditing);
            }
        }

        /// <summary> Lưu dữ liệu vào bảng Sys_AllSetting </summary>
        /// <param name="sysAllSettingAdding"></param>
        /// <param name="sysAllSettingEditing"></param>
        private void SaveChangeSysAllSetting(List<Sys_AllSetting> sysAllSettingAdding, List<Sys_AllSetting> sysAllSettingEditing)
        {
            #region SaveChange - Lưu vào database
            var hasDataChanging = false;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_AllSetting = new CustomBaseRepository<Sys_AllSetting>(unitOfWork);
                if (sysAllSettingEditing.Any())
                {
                    //edit
                    repoSys_AllSetting.Edit(sysAllSettingEditing);
                    hasDataChanging = true;
                }
                if (sysAllSettingAdding.Any())
                {
                    //add
                    repoSys_AllSetting.Add(sysAllSettingAdding);
                    hasDataChanging = true;
                }
                repoSys_AllSetting.SaveChanges();

                #region Sau khi saveChange thì cập nhật lại list Sys_AllSetting

                if (hasDataChanging)
                {
                    //set null để khi lấy dữ liệu sẽ tự động lấy data từ db lên
                    _allSettings = null;
                }
                #endregion

            }
            #endregion
        }

        #endregion

        #region Pubic method ra ngoài sử dụng

        /// <summary>[Tung.Ly 20141210] Tạo mới hoặc chỉnh sửa Config (Đã saveChange) </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void CreateOrUpdateSysAllSetting(AppConfig key, object value)
        {
            CreateOrUpdateConfig(key, value, true);
        }

        /// <summary>[Tung.Ly 20141210] Tạo mới hoặc chỉnh sửa Config (Đã saveChange) </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void CreateOrUpdateSysAllSetting(string key, object value)
        {
            CreateOrUpdateConfig(key, value, true);
        }

        /// <summary> [Tung.Ly 20141210] Lấy giá trị value1 theo key (Sys_AllSetting) </summary>
        /// <typeparam name="T">Kiểm dữ liệu của value1</typeparam>
        /// <param name="key">Enum AppConfig</param>
        /// <returns></returns>
        public T GetConfigValue<T>(AppConfig key)
        {
            return GetConfigValue<T>(key.ToString());
        }

        /// <summary> [Tung.Ly 20141210] Lấy giá trị value1 theo key (Sys_AllSetting) </summary>
        /// <typeparam name="T">Kiểm dữ liệu của value1</typeparam>
        /// <param name="key">Enum AppConfig</param>
        /// <returns></returns>
        public T GetConfigValue<T>(string key)
        {
            T temp = default(T);
            var objConfig = AllSettings.FirstOrDefault(s => s.Name == key);
            if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1) && objConfig.Value1 != "(null)")
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                //chuyển sang kiểu dữ liệu giống kiểu dữ liệu value1
                return (T)converter.ConvertFrom(objConfig.Value1);
            }
            return temp;
        }

        #endregion

        #endregion

    }
}
