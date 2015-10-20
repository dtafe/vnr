using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnResource.Helper.Utility;

namespace HRM.Infrastructure.Utilities
{
    ///// <summary>
    ///// Lưu danh sách các từ, câu được dịch
    ///// </summary>
    //public class LanguageTable
    //{
    //    public string Name { get; set; }
    //    public string Value { get; set; }
    //}
    ///// <summary>
    ///// [Chuc.Nguyen] - Dịch theo ngôn ngữ
    ///// </summary>
    //public static class TranslateService
    //{
    //    #region Properties

    //    private static List<LanguageTable> LanguageData { get; set; }

    //    #endregion

    //    #region Const

    //    public static string LanguageID = LanguageHelper.LanguageCode;
    //    private static string XmlFileDefault = Common.GetPath(@"Settings\LANG_" + LanguageID + ".XML");

    //    #endregion

    //    #region Constructor

    //    /// <summary>
    //    /// Dịch theo ngôn ngữ hiện hành.
    //    /// </summary>
    //    /// <param name="value">Giá trị cần dịch</param>
    //    /// <returns></returns>
    //    public static string TranslateString(this string value)
    //    {
    //        return value.TranslateString(LanguageID);
    //    }

    //    public static string TranslateP(this string value)
    //    {
    //        var xmlFile = Common.GetPathPortal(@"Settings\LANG_" + LanguageID + ".XML");
    //        bool reRead = false;
    //        if (xmlFile != XmlFileDefault)
    //        {
    //            reRead = true;
    //        }
    //        return value.TranslateString(LanguageID, xmlFile, reRead);
    //    }
    //    /// <summary>
    //    /// Dịch theo ngôn ngữ được chỉ định.
    //    /// </summary>
    //    /// <param name="value">Giá trị cần dịch</param>
    //    /// <param name="languageID">Ngôn ngữ dịch chỉ định</param>
    //    /// <returns></returns>
    //    public static string TranslateString(this string value, string languageID)
    //    {
    //        var xmlFile = Common.GetPath(@"Settings\LANG_" + languageID + ".XML");
    //        if (languageID != LanguageID)
    //        {
    //            XmlFileDefault = xmlFile;
    //        }
    //        return value.TranslateString(languageID, xmlFile);
    //    }
    //    public static string TranslateString(this string value, string languageID, string xmlFile)
    //    {
    //        return value.TranslateString(languageID, xmlFile, false);
    //    }
    //    public static string TranslateString(this string value, string languageID, string xmlFile, bool reReadXml)
    //    {
    //        if (reReadXml)
    //        {
    //            LanguageData = Common.GetDataFromXml<LanguageTable>(xmlFile, "Language");
    //            LanguageID = languageID;
    //        }
    //        else
    //        {
    //            LanguageData = new List<LanguageTable>();
    //            //Nếu ngôn ngữ chuyển đổi khác nhau
    //            if (LanguageID != languageID)
    //            {
    //                //Xóa Cache và đọc lại dữ liệu khi chuyển đổi ngôn ngữ
    //                LanguageData = new List<LanguageTable>();
    //            }
    //            if (LanguageData == null) LanguageData = new List<LanguageTable>();
    //            //Nếu dữ liệu để dịch không có thì đọc lại file xml
    //            if (LanguageData.Count <= 0)
    //            {
    //                LanguageData = Common.GetDataFromXml<LanguageTable>(xmlFile, "Language");
    //                LanguageID = languageID;
    //            }
    //        }
    //        //Tìm và dịch từ
    //        var valueTranslate = LanguageData.Where(w => w.Name == value).Select(s => s.Value).FirstOrDefault();

    //        //If valueTranslate != null thì return valueTranslate ngược lại return value
    //        return valueTranslate ?? value;
    //    }
    //    #endregion
    //}

    public static class TranslateService
    {
        private static string languagePath;
        public static bool IsPortal { get; set; }

        //public static string LanguageCode
        //{
        //    get
        //    {
        //        return LanguageHelper.LanguageCode;
        //    }
        //    set
        //    {
        //        LanguageHelper.LanguageCode = value;
        //    }
        //}

        public static string LanguagePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(languagePath))
                {
                    if (IsPortal)
                    {
                        languagePath = Common.GetPathPortal("Settings");
                    }
                    else
                    {
                        languagePath = Common.GetPath("Settings");
                    }
                }

                return languagePath;
            }
            set
            {
                languagePath = value;
            }
        }

        public static string TranslateString(this string value)
        {
            LanguageHelper.SettingPath = LanguagePath;
            return LanguageHelper.Translate(value);
        }
    }
}
