using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Utility;
using System.Reflection;
using HRM.Data.Entity;
using HRM.Data.BaseRepository;
using System.Collections;
using System.Linq.Dynamic;
using VnResource.Helper.Linq;

namespace HRM.Business.Main.Domain
{
    public class DataFieldInfo
    {
        private string _fieldName = string.Empty;
        public string Name { get; set; }
        public bool CheckDuplicate { get; set; }
        public string CheckDuplicateField { get; set; }
        public int DuplicateGroup { get; set; }
        public string DuplicateMessage { get; set; }
        public string Alias { get; set; }
        public string FieldName
        {
            get
            {
                if (string.IsNullOrEmpty(_fieldName))
                {
                    _fieldName = Name;
                }
                return _fieldName;
            }
            set
            {
                _fieldName = value;
            }
        }
        public string FormatName { get; set; }
        public bool IsEmail { get; set; }
        public bool IsNumeric { get; set; }
        public int? MaxLenght { get; set; }
        public string MaxValue { get; set; }
        public int? MinLenght { get; set; }
        public string MinValue { get; set; }
        public bool CompareCurrentDate { get; set; }
        public bool Nullable { get; set; }
        public string TableName { get; set; }
        public string FieldCompare { get; set; }
        public string Operator{ get; set; }
        public bool JustDay { get; set; }

        public string MaxValueNumber { get; set; }
        public string MinValueNumber { get; set; }
    }

    public static class ValidatorService
    {
        /// <summary>
        /// Validate đối tượng, mặc định tên đối tượng là tên để tìm trong file XML
        /// </summary>
        /// <typeparam name="TModel">Đối tượng chứa các field cần validate</typeparam>
        /// <param name="model">Giá trị thuộc tính của đối tượng để xét validate</param>
        /// <returns></returns>
        public static bool OnValidateData<TModel>(TModel model)
        {
            var strMessages = string.Empty;
            return OnValidateData<TModel>(model, string.Empty, null, ref strMessages);
        }
        /// <summary>
        /// Validate không cần thông báo nếu có lỗi
        /// </summary>
        /// <typeparam name="TModel">Đối tượng chứa các field cần validate</typeparam>
        /// <param name="model">Giá trị thuộc tính của đối tượng để xét validate</param>
        /// <param name="tableXmlName">Tên của đối tượng trong file xml info</param>
        /// <returns></returns>
        public static bool OnValidateData<TModel>(TModel model, string tableXmlName)
        {
            var strMessages = string.Empty;
            return OnValidateData<TModel>(model, tableXmlName, null, ref strMessages);
        }
        /// <summary>
        /// Validate theo tên table trong XML
        /// </summary>
        /// <typeparam name="TModel">Đối tượng chứa các field cần validate</typeparam>
        /// <param name="model">Giá trị thuộc tính của đối tượng để xét validate</param>
        /// <param name="tableXmlName">Tên của đối tượng trong file xml info</param>
        /// <param name="message">Tin nhắn trả về sau khi xử lý</param>
        /// <returns></returns>
        public static bool OnValidateData<TModel>(TModel model, string tableXmlName, ref string message)
        {
            return OnValidateData<TModel>(model, tableXmlName, null, ref message);
        }
        /// <summary>
        /// Validate trong trường hợp tên đối tượng đặt trong XML khác tên đối tượng trong database
        /// </summary>
        /// <typeparam name="TModel">Đối tượng chứa các field cần validate</typeparam>
        /// <param name="model">Giá trị thuộc tính của đối tượng để xét validate</param>
        /// <param name="tableXmlName">Tên của đối tượng trong file xml info</param>
        /// <param name="message">Tin nhắn trả về sau khi xử lý</param>
        /// <returns></returns>
        public static bool OnValidateData<TModel>(TModel model, string tableXmlName, string tableDbName, ref string message)
        {
            bool result = true;
            if (model != null)
            {
                var xmlFile = Common.GetPath(@"Settings\FIELD_INFO.XML");
                var xmlFileSpec = Common.GetPath(@"Settings\FIELD_INFO_SPEC.XML");
                var tblXmlName = tableXmlName;
                var tblDbName = tableDbName;
                var strOperator = ">=";

                //Nếu tên table cần lấy field validate null thì lấy tên đối tượng truyền vào để tìm
                if (string.IsNullOrEmpty(tblXmlName))
                {
                    var objName =  model.GetType().Name;
                    tblXmlName = objName.Substring(0, objName.Length - 5);
                }
                if (string.IsNullOrEmpty(tableDbName))
                {
                    tableDbName = tblXmlName;
                }
                List<DataFieldInfo> listInfo = null;
                bool checkSpect = true;
                if (!File.Exists(xmlFileSpec))
                {
                    checkSpect = false;
                }
                if (checkSpect)
                {
                    //Load file XML
                    List<DataFieldInfo> listAllFieldInfoSpec = Common.GetDataFromXml<DataFieldInfo>(xmlFileSpec, "Field");
                    listInfo = listAllFieldInfoSpec.Where(d => d.TableName == tableXmlName).ToList();
                }
                if (File.Exists(xmlFile) && (listInfo == null || listInfo.Count == 0))
                {
                    List<DataFieldInfo> listAllFieldInfo = Common.GetDataFromXml<DataFieldInfo>(xmlFile, "Field");
                    //Lọc theo table cần validate
                    listInfo = listAllFieldInfo.Where(d => d.TableName == tableXmlName).ToList();
                }
                if (listInfo != null)
                {
                    //Duyệt các field config trong xml và kiểm tra
                    foreach (var fieldInfo in listInfo)
                    {
                        string fieldName = fieldInfo.FieldName;
                        string fieldAlias = fieldInfo.Alias;
                        var defaultdate = new DateTime(0001, 01, 01);
                        if (!string.IsNullOrEmpty(fieldInfo.Operator))
                        {
                            strOperator = fieldInfo.Operator;
                        }

                        //Trong trường hợp field bị đổi ID ngoài giao diện dạng: AAA_Code : CustomName_FieldName thì cắt dấu _ và lấy tên field để kiểm tra
                        if (fieldName.Contains("_"))
                        {
                            var splitFieldName = fieldName.Split('_');
                            if (splitFieldName != null && splitFieldName.Count() > 1)
                            {
                                //Lấy phần tử cuối cùng làm tên field
                                var coItem = splitFieldName.Count();
                                fieldName = splitFieldName[coItem - 1];
                            }
                        }

                        //Nếu alias trống thì gán bằng name
                        if (string.IsNullOrWhiteSpace(fieldAlias))
                        {
                            fieldAlias = fieldName;
                        }

                        //Kiểm tra model có tồn tại thuộc tính trong file xml ko
                        if (model.HasProperty(fieldName))
                        {
                            Type propertyType = model.GetPropertyType(fieldName);
                            object propertyValue = model.GetPropertyValue(fieldName);
                            bool isHasValue = propertyValue.HasValue();

                            #region Kiểm tra null
                            if (!fieldInfo.Nullable)
                            {
                                //Kiểm tra đối tượng có giá trị không
                                if (!isHasValue)
                                {
                                    message = string.Format(ConstantMessages.FieldNotAllowNull.TranslateString(), fieldAlias.TranslateString());
                                    result = false;
                                    break;
                                }
                            }
                            #endregion

                            #region So sánh hai giá trị với nhau
                            var propertyCompareName = fieldInfo.FieldCompare;
                            if (!string.IsNullOrEmpty(propertyCompareName) && model.HasProperty(propertyCompareName))
                            {
                                //Nếu field compare không có giá trị nhưng vẫn cho phép null
                                if (!isHasValue)
                                {
                                    message = string.Format(ConstantMessages.FieldNull.TranslateString(), fieldAlias.TranslateString());
                                }
                                //Kiểm tra nếu file compare tốn tại và có giá trị thì mới so sánh
                                Type propertyCompareType = model.GetPropertyType(propertyCompareName);
                                object propertyCompareValue = model.GetPropertyValue(propertyCompareName);
                                bool fieldCompareHasValue = propertyCompareValue.HasValue();
                                if (!fieldCompareHasValue)
                                {
                                    message = string.Format(ConstantMessages.FieldNull.TranslateString(), propertyCompareName.TranslateString());
                                }

                                bool checkCompare = propertyValue.CompareByOperator(propertyCompareValue, strOperator);
                                if (!checkCompare)
                                {
                                    message = string.Format(ConstantMessages.FieldMustGreater.TranslateString(), fieldAlias.TranslateString(), strOperator, fieldInfo.FieldCompare.TranslateString());
                                    result = false;
                                    break; ;
                                }
                            }
                            #endregion

                            #region So sánh với ngày hiện tại
                            if (fieldInfo.CompareCurrentDate)
                            {
                                bool checkCompare = propertyValue.CompareByOperator(DateTime.Now, strOperator);
                                if (!checkCompare)
                                {
                                    message = string.Format(ConstantMessages.FieldMustGreater.TranslateString(), fieldAlias.TranslateString(), strOperator, fieldInfo.FieldCompare.TranslateString());
                                    result = false;
                                    break;
                                }
                            }
                            #endregion

                            #region Kiểm tra email hợp lệ
                            if (fieldInfo.IsEmail)
                            {
                                if (!string.IsNullOrEmpty(propertyValue.GetString()) && !propertyValue.GetString().IsValidEmail())
                                {
                                    message = string.Format(ConstantMessages.FieldInvalid.TranslateString(), fieldAlias.TranslateString());
                                    result = false;
                                    break;
                                }
                            }
                            #endregion

                            #region Kiểm tra số hợp lệ
                            if (fieldInfo.IsNumeric)
                            {
                                if (!string.IsNullOrEmpty(propertyValue.GetString()) && !propertyValue.GetString().IsNumeric())
                                {
                                    message = string.Format(ConstantMessages.FieldInvalidFormat.TranslateString(), fieldAlias.TranslateString());
                                    result = false;
                                    break;
                                }
                            }
                            #endregion

                            #region Kiểm tra giá trị nhỏ nhất hợp lệ
                            if (fieldInfo.MinLenght != null)
                            {
                                if (!string.IsNullOrEmpty(propertyValue.GetString()) && propertyValue.GetString().Length < fieldInfo.MinLenght)
                                {
                                    message = string.Format(ConstantMessages.FieldInvalidMinLenght.TranslateString(), fieldAlias.TranslateString(), fieldInfo.MinLenght);
                                    result = false;
                                    break;
                                }
                            }
                            #endregion

                            #region Kiểm tra giá trị lớn nhất hợp lệ
                            if (fieldInfo.MaxLenght != null)
                            {
                                if (propertyValue.GetString().Length > fieldInfo.MaxLenght)
                                {
                                    message = string.Format(ConstantMessages.FieldInvalidMaxLenght.TranslateString(), fieldAlias.TranslateString(), fieldInfo.MaxLenght);
                                    result = false;
                                    break;
                                }
                            }
                            #endregion

                            #region Kiểm tra độ dài nhỏ nhất hợp lệ
                            if (!string.IsNullOrWhiteSpace(fieldInfo.MinValue))
                            {
                                if (propertyValue.Compare(fieldInfo.MinValue.TryGetValue(propertyType)) < 0)
                                {
                                    message = string.Format(ConstantMessages.FieldMustGreater.TranslateString(), fieldAlias.TranslateString(), fieldInfo.MinValue);
                                    result = false;
                                    break;
                                }
                            }
                            #endregion

                            #region Kiểm tra độ dài lớn nhất hợp lệ
                            if (!string.IsNullOrWhiteSpace(fieldInfo.MaxValue))
                            {
                                if (propertyValue.Compare(fieldInfo.MaxValue.TryGetValue(propertyType)) > 0)
                                {
                                    message = string.Format(ConstantMessages.FieldMustLessThan.TranslateString(), fieldAlias.TranslateString(), fieldInfo.MaxValue);
                                    result = false;
                                    break;
                                }
                            }
                            #endregion

                            #region so sánh giá trị số nhỏ hơn giá trị X : MinValueNumber

                            var propertyMinValueNumberName = fieldInfo.MinValueNumber;
                            if (!string.IsNullOrEmpty(propertyMinValueNumberName) && model.HasProperty(fieldName))
                            {
                                if (!string.IsNullOrEmpty(propertyMinValueNumberName.GetString()) && !propertyMinValueNumberName.GetString().IsNumeric())
                                {
                                    result = false;
                                    break;
                                }

                                //Nếu field compare không có giá trị nhưng vẫn cho phép null
                                if (!isHasValue)
                                {
                                    message = string.Format(ConstantMessages.FieldNull.TranslateString(), fieldAlias.TranslateString());
                                }

                                //VnResource.Helper.Data.DataHelper.Validate()
                                object propertyMinValueNumberValue = propertyMinValueNumberName.TryGetValue(propertyType);
                                //Kiểm tra nếu file compare tốn tại và có giá trị thì mới so sánh
                                //object propertyMinValueNumberValue = Convert.ChangeType(propertyMinValueNumberName.ToString(), propertyType); 
                                bool fieldMinValueNumberHasValue = propertyMinValueNumberValue.HasValue();
                                if (!fieldMinValueNumberHasValue)
                                {
                                    message = string.Format(ConstantMessages.FieldNull.TranslateString(), propertyMinValueNumberName.TranslateString());
                                }

                                bool checkCompare = propertyValue.CompareByOperator(propertyMinValueNumberValue, ">");
                                if (!checkCompare)
                                {
                                    message = string.Format(ConstantMessages.FieldMustMoreThan.TranslateString(), fieldAlias.TranslateString(), propertyMinValueNumberName.ToString());
                                    result = false;
                                    break; ;
                                }
                            }
                            #endregion

                            #region so sánh giá trị số lớn hơn giá trị X : MaxValueNumber

                            var propertyMaxValueNumberName = fieldInfo.MaxValueNumber;
                            if (!string.IsNullOrEmpty(propertyMaxValueNumberName) && model.HasProperty(fieldName))
                            {
                                if (!string.IsNullOrEmpty(propertyMaxValueNumberName.GetString()) && !propertyMaxValueNumberName.GetString().IsNumeric())
                                {
                                    result = false;
                                    break;
                                }

                                //Nếu field compare không có giá trị nhưng vẫn cho phép null
                                if (!isHasValue)
                                {
                                    message = string.Format(ConstantMessages.FieldNull.TranslateString(), fieldAlias.TranslateString());
                                }
                                //Kiểm tra nếu file compare tốn tại và có giá trị thì mới so sánh
                                object propertyMaxValueNumberValue = propertyMaxValueNumberName.ToString();
                                bool fieldMaxValueNumberHasValue = propertyMaxValueNumberValue.HasValue();
                                if (!fieldMaxValueNumberHasValue)
                                {
                                    message = string.Format(ConstantMessages.FieldNull.TranslateString(), propertyMaxValueNumberName.TranslateString());
                                }

                                bool checkCompare = propertyValue.CompareByOperator(propertyMaxValueNumberValue, "<");
                                if (!checkCompare)
                                {
                                    message = string.Format(ConstantMessages.FieldMustLessThan.TranslateString(), fieldAlias.TranslateString(), propertyMaxValueNumberName.ToString());
                                    result = false;
                                    break; ;
                                }
                            }
                            #endregion
                        }

                        if (!result)
                        {
                            break;
                        }
                    }

                    #region Kiểm tra trùng dữ liệu
                    if (result && model != null && listInfo.Any(d => d.CheckDuplicate))
                    {
                        foreach (var duplicateField in listInfo)
                        {
                            if (duplicateField.CheckDuplicate)
                            {
                                string fieldName = duplicateField.FieldName;
                                string alias = duplicateField.Alias;
                                if (string.IsNullOrEmpty(alias))
                                {
                                    alias = fieldName;
                                }

                                Assembly assembly = typeof(Sys_UserInfo).Assembly;
                                Type entityType = tableDbName.GetEntityType(assembly);
                                if (entityType != null)
                                {
                                    using (var context = new VnrHrmDataContext())
                                    {
                                        try
                                        {
                                            var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                                            List<string> duplicateFields = new List<string>() { fieldName };
                                            List<string> duplicateFieldsName = new List<string>();
                                            List<object> duplicateFieldsValue = new List<object>();
                                            var id = (Guid)model.GetPropertyValue("ID");
                                            var strPredicate = string.Empty;
                                            var intParameter = 0;

                                            //Nếu có checkduplicate cùng 1 số field khác thì add vào list
                                            if (!string.IsNullOrEmpty(duplicateField.CheckDuplicateField))
                                            {
                                                duplicateFields.AddRange(duplicateField.CheckDuplicateField.Split(','));
                                            }
                                            //Loại trừ các field có giá trị null và khởi tạo chuổi điều kiện
                                            foreach (var field in duplicateFields)
                                            {
                                                var strFieldName = field;
                                                //Trong trường hợp field bị đổi ID ngoài giao diện dạng: AAA_Code : CustomName_FieldName thì cắt dấu _ và lấy tên field để kiểm tra
                                                if (strFieldName.Contains("_"))
                                                {
                                                    var splitFieldName = strFieldName.Split('_');
                                                    if (splitFieldName != null && splitFieldName.Count() > 1)
                                                    {
                                                        //Lấy phần tử cuối cùng làm tên field
                                                        var coItem = splitFieldName.Count();
                                                        strFieldName = splitFieldName[coItem - 1];
                                                    }
                                                }
                                                var value = model.GetPropertyValue(strFieldName);
                                                if (value != null && value.ToString() != string.Empty)
                                                {
                                                    duplicateFieldsValue.Add(value);
                                                    duplicateFieldsName.Add(strFieldName);
                                                    strPredicate += strFieldName + " == @" + intParameter + " AND ";
                                                    intParameter++;
                                                }
                                            }
                                            strPredicate += "IsDelete != True";

                                            if (duplicateFieldsName.Count == 0)
                                            {
                                                continue;
                                            }
                                            if (duplicateFieldsValue.Count > 0)
                                            {
                                                //Lấy dữ liệu hiện tại trong database
                                                var currentData = unitOfWork.CreateQueryable(entityType, strPredicate, duplicateFieldsValue.ToArray()).GetList();
                                                if (currentData != null)
                                                {
                                                    foreach (var item in currentData)
                                                    {
                                                        if ((Guid)item.GetPropertyValue("ID") != id)
                                                        {
                                                            if (!string.IsNullOrWhiteSpace(duplicateField.DuplicateMessage))
                                                            {
                                                                message = duplicateField.DuplicateMessage.TranslateString();
                                                            }
                                                            else
                                                            {
                                                                var strTranslate = string.Empty;
                                                                var aliasTranslate = false;
                                                                foreach (var field in duplicateFieldsName)
                                                                {
                                                                    if (!aliasTranslate)
                                                                    {
                                                                        strTranslate += alias.TranslateString() + ", ";
                                                                        aliasTranslate = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        strTranslate += field.TranslateString() + ", ";
                                                                    }
                                                                }
                                                                strTranslate = strTranslate.Substring(0, strTranslate.Length - 2);//-2 là dấu , và khoảng trắng
                                                                message = string.Format(ConstantMessages.FieldDuplicate.TranslateString(), strTranslate);
                                                            }

                                                            result = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                            if (!result)
                                            {
                                                break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            message = ex.Message;
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }

            return result;
        }
    }
}
