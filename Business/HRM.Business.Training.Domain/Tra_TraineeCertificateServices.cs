using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Training.Domain
{
    public class Tra_TraineeCertificateServices : BaseService
    {
        public int DateWarningExpireCertificateFrom()
        {
            int returnValue = 0;
            var UserLogin = string.Empty;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigTra = AppConfig.HRM_TRA_CONFIG.ToString(); 
            lstOb.Add(ConfigTra);
            lstOb.Add(null);
            lstOb.Add(null);
            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, UserLogin, ref status);
           
            if (config == null)
            {
                return 0;
            }
            string valueConfig = AppConfig.HRM_TRA_CONFIG_TRAIN_EXPIR_DAY_WARRING_FROM.ToString();
            var valueReturn = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (valueReturn != null && valueReturn.Value1 != null)
            {
                returnValue = int.Parse(valueReturn.Value1);
            }

            return returnValue;
        }

        public int DateWarningExpireCertificateTo()
        {
            var UserLogin = string.Empty;
            int returnValue = 0;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigTra = AppConfig.HRM_TRA_CONFIG.ToString();
            lstOb.Add(ConfigTra);
            lstOb.Add(null);
            lstOb.Add(null);
            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, UserLogin, ref status);

            if (config == null)
            {
                return 0;
            }
            string valueConfig = AppConfig.HRM_TRA_CONFIG_TRAIN_EXPIR_DAY_WARRING_TO.ToString();
            var valueReturn = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (valueReturn != null && valueReturn.Value1 != null)
            {
                returnValue = int.Parse(valueReturn.Value1);
            }

            return returnValue;
        }
    }
}
