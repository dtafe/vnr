using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.Main.Domain;
using HRM.Business.Attendance.Models;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using HRM.Business.HrmSystem.Models;
using HRM.Data.BaseRepository;

namespace HRM.Business.Attendance.Domain
{
    public class Att_OvertimePermitConfigServices : BaseService
    {
        public string SaveOvertimePermitConfig(OvertimePermitEntity entity, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_AllSetting = new CustomBaseRepository<Sys_AllSetting>(unitOfWork);
                Sys_AllSetting sys = new Sys_AllSetting();

                string HRM_ATT_OT_OTPERMIT_ = AppConfig.HRM_ATT_OT_OTPERMIT_.ToString();
                string status = string.Empty;
                List<object> lstO = new List<object>();
                lstO.Add(HRM_ATT_OT_OTPERMIT_);
                lstO.Add(null);
                lstO.Add(null);

                var config = GetData<Sys_AllSetting>(lstO, ConstantSql.hrm_sys_sp_get_AllSetting, userLogin, ref status);
                if (config != null)
                {
                    if (entity.limitHour_ByDay.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByDay.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByDay.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY.ToString();
                            objConfig.Value1 = entity.limitHour_ByDay.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.limitHour_ByDay_Lev1.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV1.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByDay_Lev1.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByDay_Lev1.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV1.ToString();
                            objConfig.Value1 = entity.limitHour_ByDay_Lev1.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.limitHour_ByDay_Lev2.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV2.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByDay_Lev2.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByDay_Lev2.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV2.ToString();
                            objConfig.Value1 = entity.limitHour_ByDay_Lev2.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.limitHour_ByWeek.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByWeek.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByWeek.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK.ToString();
                            objConfig.Value1 = entity.limitHour_ByWeek.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.limitHour_ByWeek_Lev1.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV1.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByWeek_Lev1.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByWeek_Lev1.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV1.ToString();
                            objConfig.Value1 = entity.limitHour_ByWeek_Lev1.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.limitHour_ByWeek_Lev2.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV2.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByWeek_Lev2.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByWeek_Lev2.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV2.ToString();
                            objConfig.Value1 = entity.limitHour_ByWeek_Lev2.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.limitHour_ByMonth.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByMonth.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByMonth.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH.ToString();
                            objConfig.Value1 = entity.limitHour_ByMonth.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.limitHour_ByMonth_Lev1.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV1.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByMonth_Lev1.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByMonth_Lev1.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV1.ToString();
                            objConfig.Value1 = entity.limitHour_ByMonth_Lev1.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.limitHour_ByMonth_Lev2.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV2.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByMonth_Lev2.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByMonth_Lev2.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV2.ToString();
                            objConfig.Value1 = entity.limitHour_ByMonth_Lev2.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.limitHour_ByYear.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByYear.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByYear.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR.ToString();
                            objConfig.Value1 = entity.limitHour_ByYear.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.limitHour_ByYear_Lev1.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV1.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByYear_Lev1.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByYear_Lev1.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV1.ToString();
                            objConfig.Value1 = entity.limitHour_ByYear_Lev1.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.limitHour_ByYear_Lev2.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV2.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitHour_ByYear_Lev2.Value.ToString())
                            {
                                objConfig.Value1 = entity.limitHour_ByYear_Lev2.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV2.ToString();
                            objConfig.Value1 = entity.limitHour_ByYear_Lev2.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (!string.IsNullOrEmpty(entity.limitColor))
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitColor)
                            {
                                objConfig.Value1 = entity.limitColor;
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR.ToString();
                            objConfig.Value1 = entity.limitColor;
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (!string.IsNullOrEmpty(entity.limitColor_Lev1))
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV1.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitColor_Lev1)
                            {
                                objConfig.Value1 = entity.limitColor_Lev1;
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV1.ToString();
                            objConfig.Value1 = entity.limitColor_Lev1;
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (!string.IsNullOrEmpty(entity.limitColor_Lev2))
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV2.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.limitColor_Lev2)
                            {
                                objConfig.Value1 = entity.limitColor_Lev2;
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV2.ToString();
                            objConfig.Value1 = entity.limitColor_Lev2;
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.IsAllowOverLimit_Normal.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.IsAllowOverLimit_Normal.Value.ToString())
                            {
                                objConfig.Value1 = entity.IsAllowOverLimit_Normal.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL.ToString();
                            objConfig.Value1 = entity.IsAllowOverLimit_Normal.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.IsAllowOverLimit_Normal_Lev1.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV1.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.IsAllowOverLimit_Normal_Lev1.Value.ToString())
                            {
                                objConfig.Value1 = entity.IsAllowOverLimit_Normal_Lev1.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV1.ToString();
                            objConfig.Value1 = entity.IsAllowOverLimit_Normal_Lev1.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.IsAllowOverLimit_Normal_Lev2.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV2.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.IsAllowOverLimit_Normal_Lev2.Value.ToString())
                            {
                                objConfig.Value1 = entity.IsAllowOverLimit_Normal_Lev2.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV2.ToString();
                            objConfig.Value1 = entity.IsAllowOverLimit_Normal_Lev2.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.IsAllowOverLimit_AllowOver.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.IsAllowOverLimit_AllowOver.Value.ToString())
                            {
                                objConfig.Value1 = entity.IsAllowOverLimit_AllowOver.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER.ToString();
                            objConfig.Value1 = entity.IsAllowOverLimit_AllowOver.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.IsAllowOverLimit_AllowOver_Lev1.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV1.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.IsAllowOverLimit_AllowOver_Lev1.Value.ToString())
                            {
                                objConfig.Value1 = entity.IsAllowOverLimit_AllowOver_Lev1.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV1.ToString();
                            objConfig.Value1 = entity.IsAllowOverLimit_AllowOver_Lev1.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.IsAllowOverLimit_AllowOver_Lev2.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV2.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.IsAllowOverLimit_AllowOver_Lev2.Value.ToString())
                            {
                                objConfig.Value1 = entity.IsAllowOverLimit_AllowOver_Lev2.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV2.ToString();
                            objConfig.Value1 = entity.IsAllowOverLimit_AllowOver_Lev2.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                    if (entity.IsAllowSplit.HasValue)
                    {
                        var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWSPLIT.ToString()).FirstOrDefault();
                        if (objConfig != null)
                        {
                            if (objConfig.Value1 != entity.IsAllowSplit.Value.ToString())
                            {
                                objConfig.Value1 = entity.IsAllowSplit.Value.ToString();
                                repoSys_AllSetting.Edit(objConfig);
                            }
                        }
                        else
                        {
                            objConfig.ID = Guid.NewGuid();
                            objConfig.Name = AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWSPLIT.ToString();
                            objConfig.Value1 = entity.IsAllowSplit.Value.ToString();
                            repoSys_AllSetting.Add(objConfig);
                        }
                    }
                }
                

                repoSys_AllSetting.SaveChanges();
                return "0";
            }
        }

        public OvertimePermitEntity GetOTPermit(string userLogin)
        {
            string HRM_ATT_OT_OTPERMIT_ = AppConfig.HRM_ATT_OT_OTPERMIT_.ToString();
            string status = string.Empty;
            List<object> lstO = new List<object>();
            lstO.Add(HRM_ATT_OT_OTPERMIT_);
            lstO.Add(null);
            lstO.Add(null);
            var config = GetData<Sys_AllSetting>(lstO, ConstantSql.hrm_sys_sp_get_AllSetting, userLogin, ref status);
            OvertimePermitEntity result = new OvertimePermitEntity();
            if (config != null)
            {
                var objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByDay = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV1.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByDay_Lev1 = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV2.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByDay_Lev2 = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByWeek = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV1.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByWeek_Lev1 = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV2.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByWeek_Lev2 = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByMonth = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV1.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByMonth_Lev1 = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV2.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByMonth_Lev2 = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByYear = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV1.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByYear_Lev1 = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV2.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitHour_ByYear_Lev2 = double.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitColor = objConfig.Value1;
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV1.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitColor_Lev1 = objConfig.Value1;
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV2.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.limitColor_Lev2 = objConfig.Value1;
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.IsAllowOverLimit_Normal = bool.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV1.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.IsAllowOverLimit_Normal_Lev1 = bool.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV2.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.IsAllowOverLimit_Normal_Lev2 = bool.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.IsAllowOverLimit_AllowOver = bool.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV1.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.IsAllowOverLimit_AllowOver_Lev1 = bool.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV2.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.IsAllowOverLimit_AllowOver_Lev2 = bool.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWSPLIT.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.IsAllowSplit = bool.Parse(objConfig.Value1);
                objConfig = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_ISALLOWCUT.ToString()).FirstOrDefault();
                if (objConfig != null && !string.IsNullOrEmpty(objConfig.Value1))
                    result.IsAllowCut = bool.Parse(objConfig.Value1);
            }
            
            return result;
        }
    }
}
