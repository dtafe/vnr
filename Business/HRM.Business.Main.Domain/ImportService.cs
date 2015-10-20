using HRM.Data.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using System.Collections;
using VnResource.Helper.Utility;
using System.Reflection;
using System.Globalization;
using VnResource.Helper.Linq;
using HRM.Business.BaseModel;
using HRM.Data.Entity;
using VnResource.Exporter;
using System.IO;
using Aspose.Cells;
using System.Data;
using VnResource.Helper.Setting;

namespace HRM.Business.Main.Domain
{
    public class ImportService
    {
        #region Properties

        private string fileName;
        private Guid userID;
        private Guid? importTemplateID;
        private ImportDataMode importMode;
        private string dateTimeFormat;

        private Type importObjectType;
        private IList listEntityResult;
        private List<string> listValueField;
        private List<string> listDisplayField;
        private List<InvalidImportData> listInvalidData;
        private Dictionary<string, object> defaultValues;
        public event ProgressEventHandler ProgressChanged;
        private IList listImportData = null;
        private bool isDataSaved = false;

        public Guid UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// Kiểu dữ liệu ảo để nhận data.
        /// </summary>
        public Type ImportObjectType
        {
            get { return importObjectType; }
        }

        /// <summary>
        /// Gán giá trị mặc định cho một số field.
        /// </summary>
        public Dictionary<string, object> DefaultValues
        {
            get
            {
                if (defaultValues == null)
                {
                    defaultValues = new Dictionary<string, object>();
                }

                return defaultValues;
            }
        }

        public List<string> ListDisplayField
        {
            get
            {
                if (listDisplayField == null)
                {
                    listDisplayField = new List<string>();
                }
                return listDisplayField;
            }
        }

        public List<string> ListValueField
        {
            get
            {
                if (listValueField == null)
                {
                    listValueField = new List<string>();
                }
                return listValueField;
            }
        }

        /// <summary>
        /// Dữ liệu thực tế để lưu vào db.
        /// </summary>
        public IList ListEntityResult
        {
            get { return listEntityResult; }
        }

        /// <summary>
        /// Dữ liệu tải từ file excel.
        /// </summary>
        public IList ListImportData
        {
            get { return listImportData; }
        }

        /// <summary>
        /// Dữ liệu lỗi khi tải từ file excel.
        /// </summary>
        public List<InvalidImportData> ListInvalidData
        {
            get { return listInvalidData; }
        }

        public string DateTimeFormat
        {
            get
            {
                if (string.IsNullOrEmpty(dateTimeFormat))
                {
                    dateTimeFormat = "dd/MM/yyyy";
                }

                return dateTimeFormat;
            }
            set { dateTimeFormat = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public ImportDataMode ImportMode
        {
            get { return importMode; }
            set { importMode = value; }
        }

        public Guid? ImportTemplateID
        {
            get { return importTemplateID; }
            set { importTemplateID = value; }
        }

        private int firstIndex = 0;

        public int FirstIndex
        {
            get { return firstIndex; }
            set { firstIndex = value; }
        }

        #endregion

        public Type GetAssembly(string tableName)
        {
            Type entityType = null;

            entityType = tableName.GetEntityType(typeof(Cat_Import).Assembly);

            return entityType;
        }

        #region ImportData

        public void Import()
        {
            Import(string.Empty);
        }

        public DataTable ImportNew(string url, string FileName, Guid ImportTemplateID)
        {
            DataTable dataTable = new DataTable();
            if (!string.IsNullOrEmpty(FileName))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                    if (ImportTemplateID != Guid.Empty)
                    {
                        var importTemplate = unitOfWork.CreateQueryable<Cat_Import>(Guid.Empty,
                            d => d.ID == ImportTemplateID).Select(d => new
                            {
                                d.StartColumnIndex,
                                d.StartRowIndex,
                                d.SheetIndex,
                                d.ObjectName
                            }).FirstOrDefault();
                        if (importTemplate != null)
                        {

                            string fileName = FileName;
                            string fileP = url;
                            Aspose.Cells.WorkbookDesigner designer = getWorkbookDesigner(fileP);
                            Workbook workbook = designer.Workbook;
                            Worksheet worksheet = workbook.Worksheets[0];
                            int startRowIndex = (int)importTemplate.StartRowIndex.Value;
                            int startColumnIndex = (int)importTemplate.StartColumnIndex.Value;
                            int maxRowCount = worksheet.Cells.End.Row;
                            int maxColumnCount = worksheet.Cells.End.Column + 1;
                            dataTable = worksheet.Cells.ExportDataTable(startRowIndex - 1, startColumnIndex - 1, maxRowCount, maxColumnCount, false);

                        }
                    }
                }
            }
            return dataTable;

        }

        public static WorkbookDesigner getWorkbookDesigner(string fileTemplate)
        {
            WorkbookDesigner designer = new WorkbookDesigner();
            FileInfo fileInfo = new FileInfo(fileTemplate);

            if (fileInfo != null && fileInfo.Extension.ToLower() == ".xls")
            {
                designer.Workbook.Open(fileTemplate, FileFormatType.Excel97To2003);
            }
            else if (fileInfo != null && fileInfo.Extension.ToLower() == ".csv")
            {
                designer.Workbook.Open(fileTemplate, FileFormatType.CSV);
            }
            else
            {
                try
                {
                    designer.Workbook.Open(fileTemplate, FileFormatType.Excel97To2003);
                }
                catch
                {
                    //Common.MessageBoxs("Mgs", LanguageManager.GetString("PleaseSaveAsFileToFormatExcel2003"),
                    //MessageBox.Icon.INFO, string.Empty);
                }
            }
            return designer;

        }
        public void Import(string password)
        {
            listInvalidData = null;
            listImportData = null;
            listValueField = null;
            listDisplayField = null;
            listEntityResult = null;
            isDataSaved = false;

            if (!string.IsNullOrWhiteSpace(FileName))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                    if (ImportTemplateID != Guid.Empty)
                    {
                        var importTemplate = unitOfWork.CreateQueryable<Cat_Import>(Guid.Empty,
                            d => d.ID == ImportTemplateID).Select(d => new
                            {
                                d.StartColumnIndex,
                                d.StartRowIndex,
                                d.SheetIndex,
                                d.ObjectName
                            }).FirstOrDefault();

                        if (importTemplate != null)
                        {
                            #region InitExcel

                            Aspose.Cells.WorkbookDesigner excelPackage = null;
                            excelPackage = new Aspose.Cells.WorkbookDesigner();
                            excelPackage.Workbook = new Aspose.Cells.Workbook(FileName);

                            Type objectType = GetAssembly(importTemplate.ObjectName);
                            importObjectType = null;//kiểu dữ liệu ảo để nhận data

                            if (!string.IsNullOrEmpty(password))
                            {
                                excelPackage.Workbook.Settings.Password = password;
                            }

                            Int64 startRowIndex = importTemplate.StartRowIndex != null ? importTemplate.StartRowIndex.Value : 0;
                            Int64 startColIndex = importTemplate.StartColumnIndex != null ? importTemplate.StartColumnIndex.Value : 0;
                            Int64 sheetIndex = importTemplate.SheetIndex != null ? importTemplate.SheetIndex.Value : 0;
                            int skipRowNumber = (int)startColIndex;

                            startRowIndex = startRowIndex < FirstIndex ? FirstIndex : startRowIndex;
                            startColIndex = startColIndex < FirstIndex ? FirstIndex : startColIndex;
                            sheetIndex = sheetIndex < FirstIndex ? FirstIndex : sheetIndex;
                            Aspose.Cells.Worksheet worksheet = null;

                            if (excelPackage.Workbook != null && excelPackage.Workbook.Worksheets != null)
                            {
                                if (excelPackage.Workbook.Worksheets.Count > sheetIndex)
                                {
                                    worksheet = excelPackage.Workbook.Worksheets[(int)sheetIndex];
                                }

                                if (worksheet == null)
                                {
                                    int activeSheetIndex = excelPackage.Workbook.Worksheets.ActiveSheetIndex;
                                    worksheet = excelPackage.Workbook.Worksheets[activeSheetIndex];
                                }
                            }

                            #endregion

                            #region ImportData

                            if (worksheet != null && worksheet.Cells != null)
                            {
                                if (worksheet.Cells != null && worksheet.Cells.End != null)
                                {
                                    int maxRowCount = worksheet.Cells.End.Row + 1;

                                    if (maxRowCount > 0)
                                    {
                                        #region GetImportItem

                                        //Danh sách data những đối tượng khóa ngoại của đổi tượng cần import
                                        Dictionary<Type, IList> listParentObject = new Dictionary<Type, IList>();

                                        var listImportItem = unitOfWork.CreateQueryable<Cat_ImportItem>(Guid.Empty, d => d.ImportID == ImportTemplateID
                                                && (d.ExcelField != null || (d.IsDefaultValue.HasValue && d.IsDefaultValue.Value))
                                                && d.IsDelete == null).Select(d => new
                                                {
                                                    d.ChildFieldLevel1,
                                                    d.ChildFieldLevel2,
                                                    d.ExcelField,
                                                    d.AllowNull,
                                                    d.AllowDuplicate,
                                                    d.DuplicateGroup,
                                                    d.IsDefaultValue,
                                                    d.DefaultValue
                                                }).OrderBy(d => d.ExcelField).ToList();

                                        #region DynamicType

                                        var typeBuilder = new DynamicTypeBuilder(string.Empty);
                                        List<string> listFieldName = new List<string>();

                                        foreach (var importItem in listImportItem)
                                        {
                                            string childFieldLevel1 = importItem.ChildFieldLevel1.TrimAll();
                                            string childFieldLevel2 = importItem.ChildFieldLevel2.TrimAll();
                                            PropertyInfo propertyType = objectType.GetProperty(childFieldLevel1);

                                            if (propertyType != null)
                                            {
                                                if (unitOfWork.IsMetadataType(propertyType.PropertyType))
                                                {
                                                    propertyType = propertyType.PropertyType.GetProperty(childFieldLevel2);
                                                    string importFieldName = unitOfWork.GetFieldConstraint(objectType, childFieldLevel1);
                                                    string valueField = importFieldName + "_" + childFieldLevel2;

                                                    if (!listFieldName.Contains(importFieldName))
                                                    {
                                                        typeBuilder.AddProperty(importFieldName, objectType.GetPropertyType(importFieldName));
                                                        listFieldName.Add(importFieldName);
                                                    }

                                                    if (!listFieldName.Contains(valueField))
                                                    {
                                                        typeBuilder.AddProperty(valueField, propertyType.PropertyType);
                                                        listFieldName.Add(valueField);
                                                    }

                                                    if (!ListValueField.Contains(valueField))
                                                    {
                                                        ListValueField.Add(valueField);
                                                    }
                                                }
                                                else
                                                {
                                                    string valueField = childFieldLevel1;

                                                    if (!listFieldName.Contains(valueField))
                                                    {
                                                        typeBuilder.AddProperty(valueField, propertyType.PropertyType);
                                                        listFieldName.Add(valueField);
                                                    }

                                                    if (!ListValueField.Contains(valueField))
                                                    {
                                                        ListValueField.Add(valueField);
                                                    }
                                                }

                                                string displayField = childFieldLevel2;
                                                if (string.IsNullOrWhiteSpace(displayField))
                                                {
                                                    displayField = childFieldLevel1;
                                                }

                                                if (!ListDisplayField.Contains(displayField))
                                                {
                                                    ListDisplayField.Add(displayField);
                                                }
                                            }
                                        }

                                        if (!typeBuilder.HasProperty(Constant.ID))
                                        {
                                            //thêm thuộc tính ID để làm key cho dữ liệu
                                            typeBuilder.AddProperty(Constant.ID, typeof(Guid));
                                        }
                                        if (!typeBuilder.HasProperty(Constant.RowIndex))
                                        {
                                            //thêm thuộc tính RowIndex để xác định vị trí trên excel
                                            typeBuilder.AddProperty(Constant.RowIndex, typeof(int));
                                        }

                                        if (!typeBuilder.HasProperty(Constant.ObjectType))
                                        {
                                            //thêm thuộc tính RowIndex để xác định vị trí trên excel
                                            typeBuilder.AddProperty(Constant.ObjectType, typeof(Type));
                                        }

                                        if (!typeBuilder.HasProperty(Constant.Checking))
                                        {
                                            //thêm thuộc tính ID để làm key cho dữ liệu
                                            typeBuilder.AddProperty(Constant.Checking, typeof(string));
                                        }

                                        //Dynamic type dùng để nhận giá trị từ excel file
                                        importObjectType = typeBuilder.BuildType();

                                        listInvalidData = new List<InvalidImportData>();
                                        IList listAllData = importObjectType.CreateList();
                                        listImportData = importObjectType.CreateList();

                                        #endregion

                                        #endregion

                                        #region GetExcelData

                                        for (int i = (int)startRowIndex; i <= maxRowCount; i = i + 1 + skipRowNumber)
                                        {
                                            object importData = importObjectType.CreateInstance();
                                            importData.SetPropertyValue(Constant.ObjectType, objectType);
                                            importData.SetPropertyValue(Constant.RowIndex, i);

                                            bool isRowInvalidData = false;
                                            bool hasValue = false;

                                            foreach (var importItem in listImportItem)
                                            {
                                                #region CheckDataType

                                                string childFieldLevel1 = importItem.ChildFieldLevel1.TrimAll();
                                                string childFieldLevel2 = importItem.ChildFieldLevel2.TrimAll();

                                                InvalidDataType invalidDataType = InvalidDataType.None;
                                                bool isInvalidData = false;

                                                PropertyInfo propertyLevel1 = objectType.GetProperty(childFieldLevel1);
                                                PropertyInfo propertyLevel2 = null;

                                                string importFieldName = childFieldLevel1;
                                                Type importFieldType = null;

                                                //Nếu có hai thuộc tính cấp một giống nhau thì có nghĩa là 2 thuộc tính cấp 2 của nó sẽ làm key
                                                var listSameObject = listImportItem.Where(d => d.ChildFieldLevel1.TrimAll() == childFieldLevel1 &&
                                                    d.ExcelField != importItem.ExcelField && !string.IsNullOrWhiteSpace(d.ChildFieldLevel2.TrimAll())
                                                    && d.ChildFieldLevel2.TrimAll() != childFieldLevel2).ToList();

                                                if (propertyLevel1 != null)
                                                {
                                                    importFieldType = propertyLevel1.PropertyType;

                                                    if (unitOfWork.IsMetadataType(propertyLevel1.PropertyType))
                                                    {
                                                        propertyLevel2 = propertyLevel1.PropertyType.GetProperty(childFieldLevel2);
                                                        importFieldName = unitOfWork.GetFieldConstraint(objectType, childFieldLevel1);
                                                        importFieldType = objectType.GetPropertyType(importFieldName);

                                                        if (i == startRowIndex)
                                                        {
                                                            List<string> listField = new List<string>();
                                                            listField.Add(Constant.ID);//Luôn lấy cột ID
                                                            listField.Add(childFieldLevel2);

                                                            foreach (var sameItem in listSameObject)
                                                            {
                                                                if (!listField.Contains(sameItem.ChildFieldLevel2.TrimAll()))
                                                                {
                                                                    listField.Add(sameItem.ChildFieldLevel2.TrimAll());
                                                                }
                                                            }

                                                            if (propertyLevel1.PropertyType == objectType)
                                                            {
                                                                foreach (var sameItem in listImportItem.Where(d => !d.AllowDuplicate))
                                                                {
                                                                    string duplicateField = sameItem.ChildFieldLevel1.TrimAll();

                                                                    if (!string.IsNullOrWhiteSpace(sameItem.ChildFieldLevel2))
                                                                    {
                                                                        duplicateField = unitOfWork.GetFieldConstraint(objectType, duplicateField);
                                                                    }

                                                                    if (!listField.Contains(duplicateField))
                                                                    {
                                                                        listField.Add(duplicateField);
                                                                    }
                                                                }
                                                            }

                                                            if (!listParentObject.ContainsKey(propertyLevel1.PropertyType))
                                                            {
                                                                listParentObject.Add(propertyLevel1.PropertyType, unitOfWork.CreateQueryable(Guid.Empty,
                                                                    propertyLevel1.PropertyType, string.Empty).SelectFields(null, listField.ToArray()).GetList());
                                                            }
                                                        }
                                                    }
                                                }

                                                #endregion

                                                #region GetFieldData

                                                object excelValue = null;
                                                string excelAddress = string.Empty;

                                                if (importItem.IsDefaultValue.GetBoolean())
                                                {
                                                    excelValue = importItem.DefaultValue;
                                                }
                                                else
                                                {
                                                    excelAddress = importItem.ExcelField + i;
                                                    var excelRange = worksheet.Cells[excelAddress];
                                                    excelValue = excelRange.Value;

                                                    if (!string.IsNullOrWhiteSpace(excelValue.GetString()))
                                                    {
                                                        hasValue = true;//có dữ liệu ở một cell bất kỳ
                                                    }

                                                    string defaultValueField = childFieldLevel1;

                                                    if (!string.IsNullOrWhiteSpace(childFieldLevel2))
                                                    {
                                                        defaultValueField += "." + childFieldLevel2;
                                                    }

                                                    if (DefaultValues.ContainsKey(defaultValueField))
                                                    {
                                                        excelValue = DefaultValues[defaultValueField];
                                                    }
                                                }

                                                object value = excelValue;

                                                if (propertyLevel2 != null && propertyLevel2.PropertyType != null)
                                                {
                                                    if (listParentObject.ContainsKey(propertyLevel1.PropertyType))
                                                    {
                                                        IQueryable objectQueryable = null;

                                                        if (propertyLevel2.PropertyType == typeof(string))
                                                        {
                                                            objectQueryable = listParentObject[propertyLevel1.PropertyType].AsQueryable().Where(childFieldLevel2 + " != null && "
                                                                + childFieldLevel2 + ".ToString().Trim().ToLower() = @0", value.GetString().Trim().ToLower());
                                                        }
                                                        else
                                                        {
                                                            objectQueryable = listParentObject[propertyLevel1.PropertyType].AsQueryable()
                                                                .Where(childFieldLevel2 + "=@0", value.TryGetValue(propertyLevel2.PropertyType));
                                                        }

                                                        if (listSameObject != null && listSameObject.Count() > 0)
                                                        {
                                                            foreach (var sameItem in listSameObject)
                                                            {
                                                                string sameExcelAddress = sameItem.ExcelField + i;
                                                                var sameExcelRange = worksheet.Cells[sameExcelAddress];
                                                                object sameValue = sameExcelRange.Value;

                                                                string sameChildFieldLevel2 = sameItem.ChildFieldLevel2.TrimAll();
                                                                Type samePropertyType2 = propertyLevel1.PropertyType.GetPropertyType(sameChildFieldLevel2);

                                                                if (propertyLevel2.PropertyType == typeof(string))
                                                                {
                                                                    objectQueryable = objectQueryable.Where(sameChildFieldLevel2 + " != null && " + sameChildFieldLevel2
                                                                        + ".ToString().Trim().ToLower() = @0", sameValue.GetString().Trim().ToLower());
                                                                }
                                                                else
                                                                {
                                                                    objectQueryable = objectQueryable.Where(sameChildFieldLevel2 + "=@0", sameValue.TryGetValue(samePropertyType2));
                                                                }
                                                            }
                                                        }

                                                        value = objectQueryable.Select(Constant.ID).FirstOrDefault();

                                                        if (value.IsNullOrEmpty() || value.GetString() == Guid.Empty.ToString())
                                                        {
                                                            if (!string.IsNullOrWhiteSpace(excelValue.GetString()) || !importItem.AllowNull)
                                                            {
                                                                invalidDataType = InvalidDataType.ReferenceNotFound;
                                                                isInvalidData = true;
                                                            }
                                                        }
                                                    }
                                                }

                                                #endregion

                                                #region CheckInvalid

                                                if (!isInvalidData)
                                                {
                                                    if (!value.IsNullOrEmpty())
                                                    {
                                                        if (importFieldType.IsDateTime())
                                                        {
                                                            if (!value.IsTypeOf(typeof(DateTime)))
                                                            {
                                                                DateTime currentValue = DateTime.Now;

                                                                if (DateTime.TryParseExact(value.GetString(), DateTimeFormat,
                                                                   CultureInfo.GetCultureInfo("vi-vn"), DateTimeStyles.None,
                                                                    out currentValue))
                                                                {
                                                                    value = currentValue;
                                                                }
                                                                else
                                                                {
                                                                    object dateValue = value.TryGetValue(importFieldType, out isInvalidData);

                                                                    if (isInvalidData)
                                                                    {
                                                                        dateValue = value.TryGetValue(typeof(double),
                                                                            out isInvalidData);

                                                                        if (!isInvalidData)
                                                                        {
                                                                            value = DateTime.FromOADate(dateValue.TryGetValue<double>());
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        value = dateValue;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            value = value.TryGetValue(importFieldType, out isInvalidData);

                                                            if (isInvalidData)
                                                            {
                                                                invalidDataType = InvalidDataType.InvalidFormat;
                                                            }
                                                            else if (!string.IsNullOrWhiteSpace(value.GetString()) && importFieldType == typeof(string))
                                                            {
                                                                //value = value.GetString().Replace("<", string.Empty).Replace("lt;", string.Empty).Replace(">", string.Empty)
                                                                //    .Replace("gt;", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty).Replace("\"", string.Empty)
                                                                //    .Replace("'", string.Empty).Replace("amp;", string.Empty).Replace("quot;", string.Empty)
                                                                //    .Replace("#x27;", string.Empty).Replace("#x2F;", string.Empty).Replace("&", string.Empty);

                                                                string checkField = propertyLevel2 != null ? propertyLevel2.Name : propertyLevel1.Name;
                                                                Type checkType = propertyLevel2 != null ? propertyLevel1.PropertyType : objectType;

                                                                if (value.GetString().Length > unitOfWork.GetMaxLength(checkType, checkField))
                                                                {
                                                                    invalidDataType = InvalidDataType.TruncateData;
                                                                    isInvalidData = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!importItem.AllowNull)
                                                        {
                                                            invalidDataType = InvalidDataType.NullData;
                                                            isInvalidData = true;
                                                        }
                                                    }
                                                }

                                                if (childFieldLevel1 == Constant.Checking)
                                                {
                                                    if (!unitOfWork.IsMetadataField(objectType, Constant.Checking))
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(excelValue.GetString()))
                                                        {
                                                            invalidDataType = InvalidDataType.Custom;
                                                            isInvalidData = true;
                                                        }
                                                    }
                                                }

                                                importData.SetPropertyValue(importFieldName + "_"
                                                    + childFieldLevel2, excelValue.GetString());

                                                if (isInvalidData)
                                                {
                                                    isRowInvalidData = true;//1 cell lỗi là cả row lỗi
                                                    InvalidImportData invalidData = new InvalidImportData();

                                                    if (invalidDataType == InvalidDataType.Custom)
                                                    {
                                                        invalidData.Desciption = excelValue.GetString();
                                                    }
                                                    else
                                                    {
                                                        invalidData.Desciption = invalidDataType.ToString().TranslateString();
                                                        invalidData.DataField = importItem.ChildFieldLevel1.TrimAll().TranslateString();
                                                        invalidData.ValueType = importFieldType.Name;
                                                        invalidData.ExcelValue = excelValue;
                                                    }

                                                    invalidData.ExcelField = excelAddress;
                                                    invalidData.ImportData = importData;
                                                    invalidData.Type = invalidDataType;
                                                    listInvalidData.Add(invalidData);
                                                }
                                                else
                                                {
                                                    importData.SetPropertyValue(importFieldName,
                                                        value.TryGetValue(importFieldType));
                                                }

                                                #endregion
                                            }

                                            if (!isRowInvalidData && hasValue)
                                            {
                                                listAllData.Add(importData);
                                            }
                                            else if (!hasValue)
                                            {
                                                listInvalidData = listInvalidData.Where(d =>
                                                    d.ImportData != importData).ToList();
                                            }

                                            if (ProgressChanged != null)
                                            {
                                                double percent = ((double)i / (double)(maxRowCount + (int)startRowIndex)) * 100;
                                                ProgressChanged(new ProgressEventArgs
                                                {
                                                    ID = UserID,
                                                    Name = "ReadData",
                                                    Value = "ReadData".TranslateString(),
                                                    Percent = (int)percent
                                                });
                                            }
                                        }

                                        #endregion

                                        #region CheckDuplicate

                                        if (listAllData != null && listAllData.Count > 0)
                                        {
                                            var listImportItemDuplicate = listImportItem.Where(d =>
                                                !d.AllowDuplicate).ToList();

                                            var listDuplicateGroup = listImportItemDuplicate.GroupBy(d =>
                                                d.DuplicateGroup).ToList();

                                            if (listDuplicateGroup.Count() > 0)
                                            {
                                                var listDuplicateByType = listImportItemDuplicate.Where(d => unitOfWork.IsMetadataType(objectType.GetPropertyType(d.ChildFieldLevel1.TrimAll()))).ToList();
                                                var listFields = listDuplicateByType.Select(d => unitOfWork.GetFieldConstraint(objectType, d.ChildFieldLevel1.TrimAll())).ToList();
                                                listFields.AddRange(listImportItemDuplicate.Where(d => !listDuplicateByType.Contains(d)).Select(d => d.ChildFieldLevel1.TrimAll()));
                                                listFields = listFields.Where(d => !string.IsNullOrWhiteSpace(d)).ToList();

                                                if (listFields.Count() > 0 && !listParentObject.ContainsKey(objectType))
                                                {
                                                    if (!listFields.Contains(Constant.ID))
                                                    {
                                                        listFields.Add(Constant.ID);
                                                    }

                                                    listParentObject.Add(objectType, unitOfWork.CreateQueryable(Guid.Empty,
                                                        objectType, string.Empty).SelectFields(null, listFields.Distinct().ToArray()).GetList());
                                                }

                                                foreach (var importData in listAllData)
                                                {
                                                    foreach (var itemDuplicate in listDuplicateGroup)
                                                    {
                                                        #region CheckData

                                                        Dictionary<string, object> listExcelValue = new Dictionary<string, object>();
                                                        Dictionary<string, string> listCheckField = new Dictionary<string, string>();

                                                        IQueryable dbQueryable = null;
                                                        IQueryable fileQueryable1 = null;
                                                        IQueryable fileQueryable2 = null;

                                                        bool isDuplicateFile = false;
                                                        bool isDuplicateDb = false;

                                                        foreach (var importItem in itemDuplicate)
                                                        {
                                                            string checkField = importItem.ChildFieldLevel1.TrimAll();
                                                            string templateField = checkField;

                                                            if (unitOfWork.IsMetadataType(objectType.GetPropertyType(checkField)))
                                                            {
                                                                checkField = unitOfWork.GetFieldConstraint(objectType, checkField);
                                                                templateField = checkField + "_" + importItem.ChildFieldLevel2.TrimAll();
                                                            }

                                                            if (dbQueryable == null && listParentObject.ContainsKey(objectType))
                                                            {
                                                                dbQueryable = listParentObject[objectType].AsQueryable();
                                                            }

                                                            if (fileQueryable1 == null)
                                                            {
                                                                fileQueryable1 = listAllData.AsQueryable().Where("it!=@0", importData);
                                                            }

                                                            if (fileQueryable2 == null)
                                                            {
                                                                fileQueryable2 = listInvalidData.Where(d => d.ImportData != null
                                                                    && d.ImportData != importData).Select(d => d.ImportData).AsQueryable();
                                                            }

                                                            if (!string.IsNullOrWhiteSpace(checkField))
                                                            {
                                                                Type checkFieldType = importData.GetRealPropertyType(checkField);
                                                                listCheckField.Add(checkField, templateField);

                                                                if (dbQueryable != null)
                                                                {
                                                                    if (checkFieldType == typeof(string))
                                                                    {
                                                                        dbQueryable = dbQueryable.Where(checkField + " != null && " + checkField + ".ToString().Trim().ToLower() = @0",
                                                                            importData.GetPropertyValue(checkField).TryGetValue(checkFieldType).GetString().Trim().ToLower());
                                                                    }
                                                                    else
                                                                    {
                                                                        dbQueryable = dbQueryable.Where(checkField + "=@0",
                                                                            importData.GetPropertyValue(checkField).TryGetValue(checkFieldType));
                                                                    }
                                                                }

                                                                if (fileQueryable1 != null)
                                                                {
                                                                    if (checkFieldType == typeof(string))
                                                                    {
                                                                        fileQueryable1 = fileQueryable1.Where(checkField + " != null && " + checkField + ".ToString().Trim().ToLower() = @0",
                                                                            importData.GetPropertyValue(checkField).TryGetValue(checkFieldType).GetString().Trim().ToLower());
                                                                    }
                                                                    else
                                                                    {
                                                                        fileQueryable1 = fileQueryable1.Where(checkField + "=@0",
                                                                            importData.GetPropertyValue(checkField).TryGetValue(checkFieldType));
                                                                    }
                                                                }

                                                                if (fileQueryable2 != null)
                                                                {
                                                                    fileQueryable2 = fileQueryable2.OfType<object>().Where(d =>
                                                                        d.GetPropertyValue(checkField) == importData.GetPropertyValue(checkField));
                                                                }
                                                            }
                                                        }

                                                        if (dbQueryable != null)
                                                        {
                                                            object selectedID = dbQueryable.Select(Constant.ID).FirstOrDefault();
                                                            if (selectedID != null && selectedID.ToString() != Guid.Empty.ToString())
                                                            {
                                                                isDuplicateDb = true;//dữ liệu import bị trùng với database.
                                                                importData.SetPropertyValue(Constant.ID, selectedID);

                                                                if (listCheckField != null && listCheckField.Count() > 0)
                                                                {
                                                                    foreach (var checkField in listCheckField)
                                                                    {
                                                                        var excelValue = importData.GetPropertyValue(checkField.Value);

                                                                        if (!listExcelValue.ContainsKey(checkField.Value))
                                                                        {
                                                                            listExcelValue.Add(checkField.Value, excelValue);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (fileQueryable1 != null && fileQueryable1.Any())
                                                        {
                                                            isDuplicateFile = true;//dữ liệu bị trùng với file.

                                                            if (listCheckField != null && listCheckField.Count() > 0)
                                                            {
                                                                foreach (var checkField in listCheckField)
                                                                {
                                                                    var excelValue = fileQueryable1.Select(checkField.Value).FirstOrDefault();

                                                                    if (!listExcelValue.ContainsKey(checkField.Value))
                                                                    {
                                                                        listExcelValue.Add(checkField.Value, excelValue);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (fileQueryable2 != null && fileQueryable2.Any())
                                                        {
                                                            isDuplicateFile = true;//dữ liệu bị trùng với file.

                                                            if (listCheckField != null && listCheckField.Count() > 0)
                                                            {
                                                                foreach (var checkField in listCheckField)
                                                                {
                                                                    var excelValue = fileQueryable2.OfType<object>().Select(d =>
                                                                        d.GetPropertyValue(checkField.Value)).FirstOrDefault();

                                                                    if (!listExcelValue.ContainsKey(checkField.Value))
                                                                    {
                                                                        listExcelValue.Add(checkField.Value, excelValue);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        #endregion

                                                        #region ProcessData

                                                        if (isDuplicateDb || isDuplicateFile)
                                                        {
                                                            if (ImportMode == ImportDataMode.Update
                                                                || ImportMode == ImportDataMode.Skip)
                                                            {
                                                                if (isDuplicateDb)
                                                                {
                                                                    if (!listImportData.Contains(importData))
                                                                    {
                                                                        //Cập nhật dữ liệu vào database
                                                                        listImportData.Add(importData);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    foreach (var importItem in itemDuplicate)
                                                                    {
                                                                        InvalidImportData invalidData = new InvalidImportData();
                                                                        object row = importData.GetPropertyValue(Constant.RowIndex);
                                                                        invalidData.ExcelField = importItem.ExcelField + row.GetString();
                                                                        invalidData.DataField = importItem.ChildFieldLevel1.TrimAll().TranslateString();
                                                                        string childFieldLevel2 = importItem.ChildFieldLevel2.TrimAll();
                                                                        string checkField = importItem.ChildFieldLevel1.TrimAll();

                                                                        if (unitOfWork.IsMetadataType(objectType.GetPropertyType(checkField)))
                                                                        {
                                                                            checkField = unitOfWork.GetFieldConstraint(objectType, checkField);
                                                                            checkField = checkField + "_" + childFieldLevel2;
                                                                        }

                                                                        if (listExcelValue.ContainsKey(checkField))
                                                                        {
                                                                            invalidData.ExcelValue = listExcelValue[checkField];
                                                                        }

                                                                        if (!string.IsNullOrWhiteSpace(childFieldLevel2))
                                                                        {
                                                                            invalidData.DataField += "." + childFieldLevel2.TranslateString();
                                                                        }

                                                                        invalidData.Type = InvalidDataType.DataNotFound;//không tìm thấy
                                                                        invalidData.Desciption = invalidData.Type.ToString().TranslateString();
                                                                        invalidData.ImportData = importData;
                                                                        listInvalidData.Add(invalidData);

                                                                        if (listImportData.Contains(importData))
                                                                        {
                                                                            listImportData.Remove(importData);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                foreach (var importItem in itemDuplicate)
                                                                {
                                                                    InvalidImportData invalidData = new InvalidImportData();
                                                                    object row = importData.GetPropertyValue(Constant.RowIndex);
                                                                    invalidData.ExcelField = importItem.ExcelField + row.GetString();
                                                                    invalidData.DataField = importItem.ChildFieldLevel1.TrimAll().TranslateString();
                                                                    string childFieldLevel2 = importItem.ChildFieldLevel2.TrimAll();
                                                                    string checkField = importItem.ChildFieldLevel1.TrimAll();

                                                                    if (unitOfWork.IsMetadataType(objectType.GetPropertyType(checkField)))
                                                                    {
                                                                        checkField = unitOfWork.GetFieldConstraint(objectType, checkField);
                                                                        checkField = checkField + "_" + childFieldLevel2;
                                                                    }

                                                                    if (listExcelValue.ContainsKey(checkField))
                                                                    {
                                                                        invalidData.ExcelValue = listExcelValue[checkField];
                                                                    }

                                                                    if (!string.IsNullOrWhiteSpace(childFieldLevel2))
                                                                    {
                                                                        invalidData.DataField += "." + childFieldLevel2.TranslateString();
                                                                    }

                                                                    if (isDuplicateDb && isDuplicateFile)
                                                                    {
                                                                        invalidData.Type = InvalidDataType.Duplicate;
                                                                    }
                                                                    else if (isDuplicateDb)
                                                                    {
                                                                        invalidData.Type = InvalidDataType.DuplicateInDb;
                                                                    }
                                                                    else if (isDuplicateFile)
                                                                    {
                                                                        invalidData.Type = InvalidDataType.DuplicateInFile;
                                                                    }

                                                                    invalidData.Desciption = invalidData.Type.ToString().TranslateString();
                                                                    invalidData.ImportData = importData;
                                                                    listInvalidData.Add(invalidData);

                                                                    if (listImportData.Contains(importData))
                                                                    {
                                                                        listImportData.Remove(importData);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (ImportMode == ImportDataMode.Update)
                                                            {
                                                                foreach (var importItem in itemDuplicate)
                                                                {
                                                                    InvalidImportData invalidData = new InvalidImportData();
                                                                    object row = importData.GetPropertyValue(Constant.RowIndex);
                                                                    invalidData.ExcelField = importItem.ExcelField + row.GetString();
                                                                    invalidData.DataField = importItem.ChildFieldLevel1.TrimAll().TranslateString();
                                                                    string childFieldLevel2 = importItem.ChildFieldLevel2.TrimAll();
                                                                    string checkField = importItem.ChildFieldLevel1.TrimAll();

                                                                    if (unitOfWork.IsMetadataType(objectType.GetPropertyType(checkField)))
                                                                    {
                                                                        checkField = unitOfWork.GetFieldConstraint(objectType, checkField);
                                                                        checkField = checkField + "_" + childFieldLevel2;
                                                                    }

                                                                    if (listExcelValue.ContainsKey(checkField))
                                                                    {
                                                                        invalidData.ExcelValue = listExcelValue[checkField];
                                                                    }

                                                                    if (!string.IsNullOrWhiteSpace(childFieldLevel2))
                                                                    {
                                                                        invalidData.DataField += "." + childFieldLevel2.TranslateString();
                                                                    }

                                                                    invalidData.Type = InvalidDataType.DataNotFound;//không tìm thấy
                                                                    invalidData.Desciption = invalidData.Type.ToString().TranslateString();
                                                                    invalidData.ImportData = importData;
                                                                    listInvalidData.Add(invalidData);

                                                                    if (listImportData.Contains(importData))
                                                                    {
                                                                        listImportData.Remove(importData);
                                                                    }
                                                                }
                                                            }
                                                            else if (!listImportData.Contains(importData) &&
                                                                !listInvalidData.Any(d => d.ImportData == importData))
                                                            {
                                                                listImportData.Add(importData);
                                                            }
                                                        }

                                                        #endregion
                                                    }

                                                    if (ProgressChanged != null)
                                                    {
                                                        double percent = ((double)listAllData.IndexOf(importData) / (double)listAllData.Count) * 100;
                                                        ProgressChanged(new ProgressEventArgs
                                                        {
                                                            ID = UserID,
                                                            Name = "CheckDuplicate",
                                                            Value = "CheckDuplicate".TranslateString(),
                                                            Percent = (int)percent
                                                        });
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                listImportData = listAllData;
                                            }
                                        }

                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region ImportCompleted

                            if (ProgressChanged != null)
                            {
                                ProgressChanged(new ProgressEventArgs
                                {
                                    ID = UserID,
                                    Name = "ReadData",
                                    Value = "ReadData".TranslateString(),
                                    Percent = 100
                                });
                            }

                            #endregion
                        }
                    }
                }
            }
        }

        #endregion

        public string Export(Guid importTemplateID, IList listInvalidData, string outputPath)
        {
            string result = string.Empty;

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                if (importTemplateID != Guid.Empty)
                {
                    var importTemplate = unitOfWork.CreateQueryable<Cat_Import>(Guid.Empty,
                        d => d.ID == importTemplateID).Select(d => new
                        {
                            d.StartColumnIndex,
                            d.StartRowIndex,
                            d.SheetIndex,
                            d.ObjectName,
                            d.TemplateFile
                        }).FirstOrDefault();

                    var listImportItem = unitOfWork.CreateQueryable<Cat_ImportItem>(Guid.Empty,
                        d => d.ImportID == ImportTemplateID && d.ExcelField != null && d.IsDelete == null).Select(d =>
                            new
                            {
                                d.ChildFieldLevel1,
                                d.ChildFieldLevel2,
                                d.ExcelField,
                                d.AllowNull,
                                d.AllowDuplicate,
                                d.DuplicateGroup
                            }).OrderBy(d => d.ExcelField).ToList();

                    ExcelExporter exporter = new ExcelExporter();
                    ExportTemplate template = new ExportTemplate();
                    template.TemplateID = importTemplateID;
                    template.DataSource = listInvalidData;

                    template.SheetIndex = (int)importTemplate.SheetIndex.GetLong();
                    template.StartRowIndex = (int)importTemplate.StartRowIndex.GetLong();
                    template.StartColumnIndex = (int)importTemplate.StartColumnIndex.GetLong();
                    Type objectType = GetAssembly(importTemplate.ObjectName);

                    string templatepath = Common.GetPath(Common.TemplateURL).Replace("/", "\\");
                    template.TemplateFile = templatepath + "\\" + importTemplate.TemplateFile;

                    string fileSuffix = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileExt = Path.GetExtension(importTemplate.TemplateFile);
                    fileExt = string.IsNullOrWhiteSpace(fileExt) ? "xls" : fileExt;
                    fileExt = fileExt.StartsWith(".") ? fileExt : "." + fileExt;

                    string filename = Path.GetFileNameWithoutExtension(importTemplate.TemplateFile);
                    template.FileName = outputPath.Replace("/", "\\") + filename + fileSuffix + fileExt;

                    foreach (var importItem in listImportItem)
                    {
                        string childFieldLevel1 = importItem.ChildFieldLevel1.TrimAll();
                        string childFieldLevel2 = importItem.ChildFieldLevel2.TrimAll();
                        PropertyInfo propertyType = objectType.GetProperty(childFieldLevel1);

                        if (propertyType != null)
                        {
                            if (string.IsNullOrWhiteSpace(childFieldLevel2))
                            {
                                template.MappingFields.Add(childFieldLevel1, importItem.ExcelField);
                            }
                            else
                            {
                                if (unitOfWork.IsMetadataType(propertyType.PropertyType))
                                {
                                    propertyType = propertyType.PropertyType.GetProperty(childFieldLevel2);
                                    string importFieldName = unitOfWork.GetFieldConstraint(objectType, childFieldLevel1);
                                    template.MappingFields.Add(importFieldName + "_" + childFieldLevel2, importItem.ExcelField);
                                }
                            }
                        }
                    }

                    exporter.ExportByTemplate(template);
                    result = template.FileName;
                }
            }

            return result;
        }

        #region SaveData

        /// <summary>
        /// Convert đối tượng import thành đối tượng dữ liệu entity.
        /// </summary>
        /// <returns></returns>
        public IList GetImportObjects()
        {
            return GetImportObjects(ListImportData);
        }

        /// <summary>
        /// Convert đối tượng import thành đối tượng dữ liệu entity.
        /// </summary>
        /// <param name="listImportData"></param>
        /// <returns></returns>
        public IList GetImportObjects(IList listImportData)
        {
            IList listEntity = null;

            if (listImportData != null && listImportData.Count > 0)
            {
                var objectType = listImportData[0].GetPropertyValue(Constant.ObjectType);
                listEntity = listImportData.Translate((Type)objectType);
            }

            return listEntity;
        }

        /// <summary>
        /// Convert và lưu dữ liệu import vào database.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool Save(Guid userID, string userLogin)
        {
            return Save(userID, ListImportData, userLogin);
        }

        /// <summary>
        /// Convert và lưu dữ liệu import vào database.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="listImportData"></param>
        /// <returns></returns>
        public bool Save(Guid userID, IList listImportData, string userLogin)
        {
            bool result = false;

            if (!isDataSaved)
            {
                if (listImportData != null && listImportData.Count > 0)
                {
                    using (var context = new VnrHrmDataContext())
                    {
                        IUnitOfWork unitOfWork = new UnitOfWork(context);
                        var objectType = listImportData[0].GetPropertyValue(Constant.ObjectType);

                        IList listObj = new ArrayList();
                        var status = DataErrorCode.Success;

                        if (objectType != null)
                        {
                            if (this.ImportMode == ImportDataMode.Update || this.ImportMode == ImportDataMode.Skip)
                            {
                                var listObjectID = listImportData.AsQueryable().Select(Constant.ID).GetList().OfType<Guid>().Distinct().ToArray();
                                listEntityResult = unitOfWork.CreateQueryable(Guid.Empty, (Type)objectType, "@0.Contains(outerIt." + Constant.ID + ")", listObjectID).GetList();

                                foreach (var importData in listImportData)
                                {
                                    var objectID = importData.GetPropertyValue(Constant.ID).TryGetValue<Guid>();
                                    var objectDuplicate = listEntityResult.AsQueryable().Where(Constant.ID + " = @0", objectID).FirstOrDefault();

                                    if (objectDuplicate != null)
                                    {
                                        //Copy dữ liệu import vào dữ liệu trong database
                                        importData.CopyData(objectDuplicate, Constant.ID);
                                    }
                                    else if (this.ImportMode == ImportDataMode.Skip)
                                    {
                                        objectDuplicate = ((Type)objectType).CreateInstance();
                                        importData.CopyData(objectDuplicate, Constant.ID);
                                        unitOfWork.AddObject((Type)objectType, objectDuplicate);
                                    }

                                    if (objectDuplicate != null)
                                    {
                                        listObj.Add(objectDuplicate);
                                    }

                                    var currentIndex = listImportData.IndexOf(importData);

                                    if ((currentIndex + 1) == listImportData.Count || (currentIndex > 0
                                        && listImportData.IndexOf(importData) % 9999 == 0))
                                    {
                                        status = unitOfWork.SaveChanges(userID, true);
                                    }

                                    if (ProgressChanged != null)
                                    {
                                        double percent = ((double)listImportData.IndexOf(importData) / (double)listImportData.Count) * 100;
                                        ProgressChanged(new ProgressEventArgs
                                        {
                                            ID = UserID,
                                            Name = "SaveData",
                                            Value = "SaveData".TranslateString(),
                                            Percent = (int)percent
                                        });
                                    }
                                }

                                result = true;
                            }
                            else
                            {
                                listEntityResult = ((Type)objectType).CreateList();

                                foreach (var importData in listImportData)
                                {
                                    var objectDuplicate = ((Type)objectType).CreateInstance();
                                    importData.CopyData(objectDuplicate, Constant.ID);
                                    unitOfWork.AddObject((Type)objectType, objectDuplicate);

                                    var currentIndex = listImportData.IndexOf(importData);

                                    if ((currentIndex + 1) == listImportData.Count || (currentIndex > 0
                                        && listImportData.IndexOf(importData) % 9999 == 0))
                                    {
                                        status = unitOfWork.SaveChanges(userID, true);
                                    }

                                    if (ProgressChanged != null)
                                    {
                                        double percent = ((double)listImportData.IndexOf(importData) / (double)listImportData.Count) * 100;
                                        ProgressChanged(new ProgressEventArgs
                                        {
                                            ID = UserID,
                                            Name = "SaveData",
                                            Value = "SaveData".TranslateString(),
                                            Percent = (int)percent
                                        });
                                    }
                                }

                                listObj = listEntityResult;
                                result = true;
                            }

                            isDataSaved = true;

                            //Xử lý task vụ sau khi import
                            if (status == DataErrorCode.Success)
                            {
                                if ((Type)objectType == typeof(Rec_Candidate))
                                {
                                    if (listObj.Count > 0)
                                    {
                                        var listGuidID = new List<Guid>();

                                        foreach (var item in listObj)
                                        {
                                            if (item != null)
                                            {
                                                listGuidID.Add((Guid)item.GetPropertyValue(Constant.ID));
                                            }
                                        }

                                        DoActionAfterImport afterImport = new DoActionAfterImport();
                                        afterImport.ImportRecruitmentHistory(listGuidID, userLogin);
                                    }
                                }
                            }
                            else
                            {
                                result = false;
                            }

                            #region ImportCompleted

                            if (ProgressChanged != null)
                            {
                                ProgressChanged(new ProgressEventArgs
                                {
                                    ID = UserID,
                                    Name = "SaveData",
                                    Value = "SaveData".TranslateString(),
                                    Percent = 100
                                });
                            }

                            #endregion
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
    public enum ImportDataMode
    {
        Insert, Update, Skip
    }
    public enum InvalidDataType
    {
        None,
        /// <summary>
        /// Dữ liệu null
        /// </summary>
        NullData,
        /// <summary>
        /// Dữ liệu update không tìm thấy trong Db
        /// </summary>
        DataNotFound,
        /// <summary>
        /// Dữ liệu sai định dạng
        /// </summary>
        InvalidFormat,
        /// <summary>
        /// Dữ liệu liên kết khóa ngoại không tìm thấy
        /// </summary>
        ReferenceNotFound,
        /// <summary>
        /// Dữ liệu trùng lặp trong file import
        /// </summary>
        DuplicateInFile,
        /// <summary>
        /// Dữ liệu trùng lặp trong database
        /// </summary>
        DuplicateInDb,
        /// <summary>
        /// Dữ liệu trùng cả trong file lẫn Db
        /// </summary>
        Duplicate,
        /// <summary>
        /// Độ dài chuỗi vượt quy định
        /// </summary>
        TruncateData,
        /// <summary>
        /// Thông báo lỗi của người dùng
        /// </summary>
        Custom
    }
    public class InvalidImportData
    {
        public object ImportData { get; set; }
        public InvalidDataType Type { get; set; }
        public string Desciption { get; set; }
        public string ValueType { get; set; }
        public string DataField { get; set; }
        public string ExcelField { get; set; }
        public object ExcelValue { get; set; }

        public string InvalidValue
        {
            get
            {
                return ExcelValue.GetString();
            }
        }

        public partial class FieldNames
        {
            public const string Type = "Type";
            public const string Desciption = "Desciption";
            public const string ValueType = "ValueType";
            public const string DataField = "DataField";
            public const string ExcelField = "ExcelField";
            public const string ExcelValue = "ExcelValue";
            public const string InvalidValue = "InvalidValue";
        }
    }
}
