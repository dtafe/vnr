using System;
using System.Collections.Generic;
using VnResource.Helper.Setting;
using System.Linq;
using System.Text;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace VnResource.ARTS.Library
{
    public class AppSettings
    {
        #region Properties

        public static string DateFormat { get; set; }
        public static string DecimalFormat { get; set; }
        public static string IntegerFormat { get; set; }
        public static string ShortTimeFormat { get; set; }
        public static string LongTimeFormat { get; set; }
        public static string DateTimeFormat { get; set; }
        public static string MonthFormat { get; set; }
        public static string YearFormat { get; set; }
        public static string[] Languages { get; set; }

        /// <summary>
        ///chuỗi cộng thêm vào MD5
        /// </summary>
        public static string MD5Pass
        {
            get
            {
                return "Q!W@E#R$";
            }
        }
        
        #endregion

        #region Methods

        public static void LoadDefaultSettings()
        {
            LoadDefaultSettings(string.Empty);
        }

        public static void LoadDefaultSettings(string settingPath)
        {
            SettingManager settingManager = new SettingManager();

            if (!string.IsNullOrWhiteSpace(settingPath))
            {
                settingManager.SettingPath = settingPath;
            }

            var listSettingInfo = settingManager.GetSettings();

            if (listSettingInfo != null)
            {
                YearFormat = listSettingInfo.Where(d => d.Name == Constant.YearFormat).Select(d => d.Value).FirstOrDefault();
                MonthFormat = listSettingInfo.Where(d => d.Name == Constant.MonthFormat).Select(d => d.Value).FirstOrDefault();
                DateFormat = listSettingInfo.Where(d => d.Name == Constant.DateFormat).Select(d => d.Value).FirstOrDefault();
                DecimalFormat = listSettingInfo.Where(d => d.Name == Constant.DecimalFormat).Select(d => d.Value).FirstOrDefault();
                IntegerFormat = listSettingInfo.Where(d => d.Name == Constant.IntegerFormat).Select(d => d.Value).FirstOrDefault();
                ShortTimeFormat = listSettingInfo.Where(d => d.Name == Constant.ShortTimeFormat).Select(d => d.Value).FirstOrDefault();
                LongTimeFormat = listSettingInfo.Where(d => d.Name == Constant.LongTimeFormat).Select(d => d.Value).FirstOrDefault();
                DateTimeFormat = listSettingInfo.Where(d => d.Name == Constant.DateTimeFormat).Select(d => d.Value).FirstOrDefault();
                string languages = listSettingInfo.Where(d => d.Name == Constant.Languages).Select(d => d.Value).FirstOrDefault();
                Languages = languages.GetString().Split(',').Select(d => d.Trim()).Distinct().ToArray();
            }
        }

        #endregion
    }
}
