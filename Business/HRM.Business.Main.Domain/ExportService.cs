using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using VnResource.Exporter;
using VnResource.Exporter;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using HRM.Data.Entity;
using System.Data;
using System.Collections;
using System.Linq.Dynamic;
using Aspose.Cells;
using System.Reflection;
using System.Text.RegularExpressions;
using VnResource.Helper.Utility;

namespace HRM.Business.Main.Domain
{
    public class ExportService
    {
        #region Properties

        public string templatePath = string.Empty;
        public string downloadPath = string.Empty;
        public string passwordWrite = string.Empty;
        public string passwordProtect = string.Empty;

        public event BeforeExportEventHandler BeforeExport;

        /// <summary>
        /// Đường dẫn thư mục hoặc file template
        /// </summary>
        public string TemplatePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(templatePath))
                {
                    templatePath = Common.GetPath(Common.TemplateURL);
                    templatePath = templatePath.Replace("/", "\\");
                }

                return templatePath;
            }
            set { templatePath = value; }
        }

        public string DownloadPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(downloadPath))
                {
                    downloadPath = Common.GetPath(Common.DownloadURL);
                    downloadPath = downloadPath.Replace("/", "\\");
                }

                return downloadPath;
            }
            set { downloadPath = value; }
        }

        /// <summary>
        /// Mật khẩu không cho mở file.
        /// </summary>
        public string PasswordWrite
        {
            get
            {
                if (string.IsNullOrWhiteSpace(passwordWrite))
                {
                    using (var context = new VnrHrmDataContext())
                    {
                        var unitOfWork = new UnitOfWork(context);

                        var settingQueryable = unitOfWork.CreateQueryable<Sys_AllSetting>(d =>
                            d.Name == AppConfig.HRM_CAT_CONFIG_EXPORT_EXCELPASSOPENFILE.ToString());

                        passwordWrite = settingQueryable.Select(d => d.Value1).FirstOrDefault();
                    }
                }

                return passwordWrite;
            }
            set { passwordWrite = value; }
        }

        /// <summary>
        /// Mật khẩu không cho chỉnh sửa file.
        /// </summary>
        public string PasswordProtect
        {
            get
            {
                if (string.IsNullOrWhiteSpace(passwordProtect))
                {
                    using (var context = new VnrHrmDataContext())
                    {
                        var unitOfWork = new UnitOfWork(context);

                        var settingQueryable = unitOfWork.CreateQueryable<Sys_AllSetting>(d =>
                            d.Name == AppConfig.HRM_CAT_CONFIG_EXPORT_EXCELPASSREADONLY.ToString());

                        passwordProtect = settingQueryable.Select(d => d.Value1).FirstOrDefault();
                    }
                }

                return passwordProtect;
            }
            set { passwordProtect = value; }
        }

        public static string PasswordReadonly
        {
            get
            {
                var passwordReadonly = string.Empty;
                try
                {
                    using (var context = new VnrHrmDataContext())
                    {
                        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                        var repoAllSetting = new Sys_AllSettingRepository(unitOfWork);
                        var allSettingQueryalbe = repoAllSetting.FindBy(d => !d.IsDelete.HasValue || !d.IsDelete.Value);
                        var allSettingInfo = allSettingQueryalbe.Where(d => d.Name == AppConfig.HRM_CAT_CONFIG_EXPORT_EXCELPASSREADONLY.ToString()).FirstOrDefault();
                        if (allSettingInfo != null && !string.IsNullOrEmpty(allSettingInfo.Name))
                        {
                            passwordReadonly = allSettingInfo.Value1;
                        }
                    }
                }
                catch (Exception)
                {
                    passwordReadonly = string.Empty;
                }
                return passwordReadonly;
            }
        }

        public static string PasswordOpenFile
        {
            get
            {
                var passwordOpenFile = string.Empty;
                try
                {
                    using (var context = new VnrHrmDataContext())
                    {
                        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                        var repoAllSetting = new Sys_AllSettingRepository(unitOfWork);
                        var allSettingQueryalbe = repoAllSetting.FindBy(d => !d.IsDelete.HasValue || !d.IsDelete.Value);
                        var allSettingInfo = allSettingQueryalbe.Where(d => d.Name == AppConfig.HRM_CAT_CONFIG_EXPORT_EXCELPASSOPENFILE.ToString()).FirstOrDefault();
                        if (allSettingInfo != null && !string.IsNullOrEmpty(allSettingInfo.Name))
                        {
                            passwordOpenFile = allSettingInfo.Value1;
                        }
                    }
                }
                catch (Exception)
                {
                    passwordOpenFile = string.Empty;
                }
                return passwordOpenFile;
            }
        }

        #endregion

        #region ExportWord

        public static void ExportWord(string fileName, string templatePath, object dataSource)
        {
            WordExporter exportHelper = new WordExporter();
            var template = new WordTemplate();
            template.TemplateFile = templatePath;
            template.FileName = fileName;
            template.DataSource = dataSource;
            exportHelper.ExportByTemplate(template);
        }

        public static void ExportWithRegions(string fileName, string templatePath, object dataSource)
        {
            WordExporter exportHelper = new WordExporter();
            exportHelper.ImagePath = Common.GetPath("Images");
            WordRegions template = new WordRegions();
            template.TemplateFile = templatePath;
            template.FileName = fileName;
            template.DataSource = dataSource;
            exportHelper.ExportWithRegions(template);
        }

        public static void ExportWrodByDataSet(string fileName, string templatePath, DataTable dataTabel, DataSet dataSource)
        {
            Aspose.Words.Document doc = new Aspose.Words.Document(templatePath);
            doc.MailMerge.Execute(dataTabel);
            doc.MailMerge.ExecuteWithRegions(dataSource);
            DataRow row = dataTabel.Rows[0];
            string strUrl = "";
            if (!string.IsNullOrEmpty(row["ImagePath"].ToString()))
            {
                string strFileName = row["ImagePath"].ToString();
                strUrl = Common.GetPath("Images/") + strFileName;
                doc.MailMerge.Execute(new string[] { "MyImage" }, new object[] { getImageByte(strUrl) });
            }
            doc.Save(fileName, Aspose.Words.SaveFormat.Doc);
        }

        public static byte[] getImageByte(string strPath)
        {
            FileInfo file = new FileInfo(strPath);
            int fileLength = (int)file.Length;
            byte[] fileByte = new byte[fileLength];
            FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            fileStream.Read(fileByte, 0, fileLength);
            return fileByte;
        }
        #endregion

        #region ExportNormal

        public string ExportNormal(string outputFile, object dataSource)
        {
            return ExportNormal(outputFile, null, dataSource);
        }

        public string ExportNormal(string outputFile, string[] valueFields, object dataSource)
        {
            return ExportNormal(outputFile, null, valueFields, dataSource);
        }

        public string ExportNormal(string outputFile, Dictionary<string, string> formatFields,
            string[] valueFields, object dataSource)
        {
            return ExportNormal(outputFile, formatFields, valueFields, null, dataSource);
        }

        public string ExportNormal(string outputFile, string[] valueFields,
            Dictionary<string, string> displayFields, object dataSource)
        {
            return ExportNormal(outputFile, null, valueFields, displayFields, dataSource);
        }

        public string ExportNormal(string outputFile, Dictionary<string, string> formatFields,
            string[] valueFields, Dictionary<string, string> displayFields, object dataSource)
        {
            return ExportByTemplate(outputFile, formatFields, valueFields,
                displayFields, Guid.Empty, null, dataSource);
        }

        #endregion

        #region ExportByTemplate

        public string ExportByTemplate(string outputFile, Guid templateID, params object[] dataSources)
        {
            return ExportByTemplate(outputFile, null, templateID, new Dictionary<string, object>(), dataSources);
        }

        public string ExportByTemplate(string outputFile, Guid templateID,
            Dictionary<string, object> fixedCellValues, params object[] dataSources)
        {
            return ExportByTemplate(outputFile, null, templateID, fixedCellValues, dataSources);
        }

        public string ExportByTemplate(string outputFile, string[] valueFields, Guid templateID,
            Dictionary<string, object> fixedCellValues, params object[] dataSources)
        {
            return ExportByTemplate(outputFile, null, valueFields, templateID, fixedCellValues, dataSources);
        }

        public string ExportByTemplate(string outputFile, Dictionary<string, string> formatFields, string[] valueFields,
            Guid templateID, Dictionary<string, object> fixedCellValues, params object[] dataSources)
        {
            return ExportByTemplate(outputFile, formatFields, valueFields, null, templateID, fixedCellValues, dataSources);
        }

        public string ExportByTemplate(string outputFile, string[] valueFields, Dictionary<string, string> displayFields,
            Guid templateID, Dictionary<string, object> fixedCellValues, params object[] dataSources)
        {
            return ExportByTemplate(outputFile, null, valueFields, displayFields, templateID, fixedCellValues, dataSources);
        }

        /// <summary>
        /// Kết xuất dữ liệu ra file excel hoặc pdf tùy vào đuôi của outputFile.
        /// </summary>
        /// <param name="templateID">ID của template nếu có</param>
        /// <param name="templatePath">Đường dẫn thư mục hoặc file template</param>
        /// <param name="outputFile">Đường dẫn file kết quả sau export</param>
        /// <param name="passwordWrite">Mật khẩu không cho mở file.</param>
        /// <param name="passwordProtect">Mật khẩu không cho chỉnh sửa file.</param>
        /// <param name="valueFields">Nếu không có template hoặc template không có chi tiết mappings.</param>
        /// <param name="displayFields">Nếu không có template hoặc template không có chi tiết mappings.</param>
        /// <param name="fixedCellValues">Datasource để export vào những cột cố định trong template.</param>
        /// <param name="dataSources">Những datasource sắp xếp theo thứ tự template.</param>
        public string ExportByTemplate(string outputFile, Dictionary<string, string> formatFields,
            string[] valueFields, Dictionary<string, string> displayFields, Guid templateID,
            Dictionary<string, object> fixedCellValues, params object[] dataSources)
        {
            return ExportData(outputFile, formatFields, valueFields, displayFields,
                templateID, fixedCellValues, dataSources);
        }

        #endregion

        #region GeneralExportData

        /// <summary>
        /// Kết xuất dữ liệu ra file excel hoặc pdf tùy vào đuôi của outputFile.
        /// </summary>
        /// <param name="templateID">ID của template nếu có</param>
        /// <param name="outputFile">Đường dẫn file kết quả sau export</param>
        /// <param name="valueFields">Nếu không có template hoặc template không có chi tiết mappings.</param>
        /// <param name="displayFields">Nếu không có template hoặc template không có chi tiết mappings.</param>
        /// <param name="formatFields">Nếu không có template hoặc template không có chi tiết mappings.</param>
        /// <param name="fixedCellValues">Datasource để export vào những cột cố định trong template.</param>
        /// <param name="dataSources">Những datasource sắp xếp theo thứ tự template.</param>
        private string ExportData(string outputFile, Dictionary<string, string> formatFields,
            string[] valueFields, Dictionary<string, string> displayFields, Guid templateID,
            Dictionary<string, object> fixedCellValues, params object[] dataSources)
        {
            List<ExportItemInfo> exportItems = new List<ExportItemInfo>();
            bool isColumnDynamic = true;
            bool isSmartMarkers = false;

            if (templateID != Guid.Empty)
            {
                #region Tải cấu hình từ database

                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = new UnitOfWork(context);

                    var exportInfo = unitOfWork.CreateQueryable<Cat_Export>().Where(d =>
                        d.ID == templateID).Select(d => new
                        {
                            d.IsGroup,
                            d.TemplateFile,
                            d.ScreenName,
                            d.ObjectName,
                            d.SheetIndex,
                            d.StartRowIndex,
                            d.StartColumnIndex,
                            d.IsColumnDynamic,
                            d.IsSmartMarkers,
                            d.IsProtected,
                            d.IsReadOnly
                        }).FirstOrDefault();

                    if (exportInfo != null)
                    {
                        isColumnDynamic = exportInfo.IsColumnDynamic.GetBoolean();
                        isSmartMarkers = exportInfo.IsSmartMarkers.GetBoolean();

                        var itemQueryable = unitOfWork.CreateQueryable<Cat_ExportItem>(d =>
                           (!d.Cat_Export.IsDelete.HasValue || !d.Cat_Export.IsDelete.Value));

                        if (exportInfo.IsGroup.GetBoolean())
                        {
                            itemQueryable = itemQueryable.Where(d => d.Cat_Export.IsGroup.HasValue && d.Cat_Export.IsGroup.Value
                                && d.Cat_Export.ScreenName == exportInfo.ScreenName && d.Cat_Export.TemplateFile == exportInfo.TemplateFile);
                        }
                        else
                        {
                            itemQueryable = itemQueryable.Where(d => d.ExportID == templateID);
                        }

                        var listExportItem = itemQueryable.Select(d => new
                        {
                            d.Cat_Export.ObjectName,
                            d.Cat_Export.ScreenName,
                            d.Cat_Export.SheetIndex,
                            d.Cat_Export.StartColumnIndex,
                            d.Cat_Export.StartRowIndex,
                            d.Cat_Export.TemplateFile,
                            d.Cat_Export.IsProtected,
                            d.Cat_Export.IsReadOnly,
                            d.Cat_Export.IsGroup,
                            d.ExportID,
                            d.DataField,
                            d.ExcelField,
                            d.FormatString,
                            d.GroupOrder,
                            d.SubTotal,
                            d.IsFixedCell
                        }).ToList();

                        exportItems = listExportItem.Select(d => new ExportItemInfo
                        {
                            ObjectName = d.ObjectName,
                            ScreenName = d.ScreenName,
                            SheetIndex = (int?)d.SheetIndex,
                            StartColumnIndex = (int?)d.StartColumnIndex,
                            StartRowIndex = (int?)d.StartRowIndex,
                            TemplateFile = d.TemplateFile,
                            IsProtected = d.IsProtected,
                            IsReadOnly = d.IsReadOnly,
                            TemplateID = d.ExportID,
                            DataField = d.DataField,
                            ExcelField = d.ExcelField,
                            FormatString = d.FormatString,
                            GroupOrder = (int?)d.GroupOrder,
                            SubTotal = d.SubTotal,
                            IsFixedCell = d.IsFixedCell
                        }).ToList();

                        if (exportItems.Count() == 0)
                        {
                            //Nếu template không có chi tiết
                            exportItems.Add(new ExportItemInfo
                            {
                                TemplateFile = exportInfo.TemplateFile,
                                IsProtected = exportInfo.IsProtected,
                                IsReadOnly = exportInfo.IsReadOnly,
                                StartRowIndex = (int?)exportInfo.StartRowIndex,
                                StartColumnIndex = (int?)exportInfo.StartColumnIndex,
                                SheetIndex = (int?)exportInfo.SheetIndex,
                                TemplateID = templateID
                            });
                        }
                    }
                }

                #endregion
            }

            if (exportItems.Count() == 0)
            {
                //Tạo ra một template mặc định
                exportItems.Add(new ExportItemInfo
                {
                    TemplateFile = TemplatePath,
                    TemplateID = templateID
                });
            }

            #region Kiểm tra thông tin hiển thị

            if (isColumnDynamic)
            {
                if (valueFields == null || valueFields.Count() == 0)
                {
                    if (!exportItems.Any(d => !d.IsFixedCell.GetBoolean()
                        && !string.IsNullOrWhiteSpace(d.DataField)))
                    {
                        if (dataSources != null && dataSources.Count() > 0)
                        {
                            valueFields = ExcelExporter.GetColumns(dataSources[0]);
                        }
                    }
                }
            }

            if (valueFields != null && valueFields.Count() > 0)
            {
                var listDisplayField = new Dictionary<string, string>();
                List<string> listvalueField = new List<string>();

                foreach (var valueField in valueFields)
                {
                    //Xóa các ký tự đặc biệt khi chuyển dữ liệu thành tên column
                    string field = Regex.Replace(valueField, @"[^0-9a-zA-Z_]+", "");

                    //Kiểm tra column bắt đầu bằng số 
                    if (Regex.IsMatch(field, @"^\d"))
                    {
                        field = "_" + field;
                    }

                    if (displayFields == null || displayFields.Count() == 0)
                    {
                        listDisplayField.Add(field, field.TranslateString());
                    }

                    listvalueField.Add(field);
                }

                if (displayFields == null || displayFields.Count() == 0)
                {
                    displayFields = listDisplayField;
                }

                valueFields = listvalueField.ToArray();
            }

            #endregion

            ExcelExporter exporter = new ExcelExporter();

            exporter.InvisibleFields = new string[] 
            { 
              Constant.TempColumn  
            };

            var listExportTemplate = exporter.BuildTemplates(TemplatePath, outputFile,
                PasswordWrite, PasswordProtect, formatFields, valueFields, displayFields,
                exportItems, fixedCellValues, dataSources);

            var exportTemplates = OnBeforeExport(listExportTemplate);

            if (isSmartMarkers)
            {
                exporter.ExportByMarkers(exportTemplates);
            }
            else
            {
                exporter.ExportByTemplate(exportTemplates);
            }

            var listOutputFile = exportTemplates.Where(d =>
                !string.IsNullOrWhiteSpace(d.FileName)).Select(d => d.FileName).Distinct();

            if (listOutputFile != null && listOutputFile.Count() == 1)
            {
                return listOutputFile.FirstOrDefault();
            }
            else
            {
                string zipFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                return ZipHelper.CreateZipFile(zipFileName, listOutputFile.ToArray());
            }
        }

        /// <summary>
        /// Dùng sự kiện BeforeExport để can thiệp template trước khi xuất.
        /// </summary>
        /// <param name="listExportTemplate"></param>
        /// <returns></returns>
        private ExportTemplate[] OnBeforeExport(ExportTemplate[] listExportTemplate)
        {
            ExportTemplate[] result = listExportTemplate;

            if (BeforeExport != null)
            {
                var eventArgs = new BeforeExportEventArgs(listExportTemplate);
                BeforeExport(eventArgs);

                if (eventArgs.ExportTemplates != null)
                {
                    result = eventArgs.ExportTemplates;
                }
            }

            return result;
        }

        #endregion

        #region Methods

        public static string Export<T>(Guid exportId, List<T> listModel, ExportFileType exportFileType = ExportFileType.Excel)
        {
            return Export<T>(exportId, listModel, null, exportFileType);
        }

        /// <summary> Xuất báo cáo theo mẫu </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exportId"></param>
        /// <param name="listModel"></param>
        /// <param name="exportFileType">Loại file excel hay Pdf (mặc định không truyền sẽ là excel)</param>
        /// <returns></returns>
        public static string Export<T>(Guid exportId, List<T> listModel, string[] valueField, ExportFileType exportFileType = ExportFileType.Excel)
        {
            return Export<T>(exportId, listModel, null, valueField, exportFileType);
        }

        public static string Export<T>(Guid exportId, List<T> listModel, List<HeaderInfo> headerInfo, string[] valueField, ExportFileType exportFileType = ExportFileType.Excel)
        {
            string status = string.Empty;
            BaseService baseService = new BaseService();
            Cat_Export exportTemplate = baseService.GetById<Cat_Export>(exportId, ref status);
            var ext = Path.GetExtension(exportTemplate.TemplateFile);
            var templateFile = exportTemplate.TemplateFile;
            if (templateFile != null && ext != ".doc")
            {
                string templatepath = Common.GetPath(Common.TemplateURL);

                string fileExt = Path.GetExtension(templateFile);
                string filename = Path.GetFileNameWithoutExtension(templateFile);


                //string[] splits = exportTemplate.TemplateFile.Split('.');
                //string filename = splits[0];
                fileExt = exportFileType == ExportFileType.Excel ? fileExt : ExportFileType.PDF.ToString();
                string fileSuffix = DateTime.Now.ToString("HHmmss");
                string outputFileName = filename + fileSuffix + fileExt;
                string dirpath = Common.GetPath(Common.DownloadURL);
                string outputPath = dirpath + outputFileName;
                ExportService exportService = new ExportService();
                outputPath = outputPath.Replace("/", "\\");
                templatepath = templatepath.Replace("/", "\\");
                var exportInfo = GetExportInfo(exportId);

                var isProtected = false;
                if (exportInfo.IsProtected == true)
                {
                    isProtected = true;
                }
                ExportTemplate templateInfo = new ExportTemplate()
                {
                    TemplateFile = exportInfo.TemplateFile,
                    IsProtected = isProtected
                };
                if (exportInfo.IsReadOnly == true)
                {
                    templateInfo.PasswordWrite = PasswordOpenFile;
                }
                if (isProtected)
                {
                    templateInfo.PasswordProtect = PasswordReadonly;
                }

                var success = false;
                if (exportInfo != null)
                {
                    if (exportInfo.IsSmartMarkers == true)
                    {
                        success = exportService.ExportNew(outputPath, templatepath, templateInfo, headerInfo, listModel);
                    }
                    else
                    {
                        exportService.TemplatePath = templatepath;
                        string fileDownload = exportService.ExportByTemplate(outputPath, valueField, exportId, null, listModel);
                        fileDownload = Common.MultiExport(ConstantPathWeb.Hrm_Hre_Service + Common.DownloadURL, false, fileDownload);

                        success = true;
                        return fileDownload;
                    }
                }
                if (success)
                {
                    string fileDownload = Common.MultiExport(ConstantPathWeb.Hrm_Hre_Service + Common.DownloadURL, false, outputPath);
                    return fileDownload;
                }
            }

            return "-1";
        }

        public static string Export(Guid exportId, object datasource, ExportFileType exportFileType = ExportFileType.Excel)
        {
            return Export(exportId, null, datasource, exportFileType);
        }

        public static string Export(Guid exportId, object datasource, List<HeaderInfo> headerInfo, ExportFileType exportFileType = ExportFileType.Excel)
        {
            return Export(exportId, null, headerInfo, datasource, exportFileType);
        }

        /// <summary>  Xuất báo cáo theo mẫu </summary>
        /// <param name="exportId"></param>
        /// <param name="datasource"></param>
        /// <param name="exportFileType"></param>
        /// <returns></returns>
        public static string Export(Guid exportId, string[] valueField, object datasource, ExportFileType exportFileType = ExportFileType.Excel)
        {
            return Export(exportId, valueField, null, datasource, exportFileType);
        }

        public static string Export(Guid exportId, string[] valueField, List<HeaderInfo> headerInfo, object datasource, ExportFileType exportFileType = ExportFileType.Excel)
        {
            string status = string.Empty;
            BaseService baseService = new BaseService();
            Cat_Export exportTemplate = baseService.GetById<Cat_Export>(exportId, ref status);

            if (exportTemplate != null)
            {
                var templateFile = exportTemplate.TemplateFile;
                string templatepath = Common.GetPath(Common.TemplateURL);
                string fileExt = Path.GetExtension(templateFile);
                string filename = Path.GetFileNameWithoutExtension(templateFile);

                if (exportFileType == ExportFileType.PDF && fileExt.IndexOf(".xls") == 0)
                {
                    fileExt = "." + ExportFileType.PDF.ToString();
                }
                else {
                    fileExt = exportFileType == ExportFileType.Excel ? fileExt : ExportFileType.PDF.ToString();
                }
                string fileSuffix = DateTime.Now.ToString("HHmmss");
                string outputFileName = filename + fileSuffix + fileExt;
                string dirpath = Common.GetPath(Common.DownloadURL);
                string outputPath = dirpath + outputFileName;
                outputPath = outputPath.Replace("/", "\\");
                templatepath = templatepath.Replace("/", "\\");
                ExportService exportService = new ExportService();
                exportService.TemplatePath = templatepath;

                var exportInfo = GetExportInfo(exportId);
                var isProtected = false;
                if (exportInfo.IsProtected == true)
                {
                    isProtected = true;
                }
                ExportTemplate templateInfo = new ExportTemplate()
                {
                    TemplateFile = exportInfo.TemplateFile,
                    IsProtected = isProtected
                };
                if (exportInfo.IsReadOnly == true)
                {
                    templateInfo.PasswordWrite = PasswordOpenFile;
                }
                if (isProtected)
                {
                    templateInfo.PasswordProtect = PasswordReadonly;
                }

                var success = false;
                if (exportInfo != null)
                {
                    if (exportInfo.IsSmartMarkers == true)
                    {
                        success = exportService.ExportNew(outputPath, templatepath, templateInfo, headerInfo, datasource);
                    }
                    else
                    {
                        string fileDownload = exportService.ExportByTemplate(outputPath, valueField, exportId, null, datasource);
                        fileDownload = Common.MultiExport(ConstantPathWeb.Hrm_Hre_Service + Common.DownloadURL, false, fileDownload);

                        success = true;
                        return fileDownload;
                    }
                }
                if (success)
                {
                    string fileDownload = Common.MultiExport(ConstantPathWeb.Hrm_Hre_Service + Common.DownloadURL, false, outputPath);
                    return fileDownload;
                }

            }
            return "-1";
        }

        //Xuất nhiếu payslip vào trong folderSave tự tạo
        public static string ExportPlaysip(Guid exportId, string profileName, string foldersave, object datasource, ExportFileType exportFileType = ExportFileType.Excel)
        {
            string status = string.Empty;
            BaseService baseService = new BaseService();
            Cat_Export exportTemplate = baseService.GetById<Cat_Export>(exportId, ref status);

            if (exportTemplate != null)
            {
                var templateFile = exportTemplate.TemplateFile;
                string templatepath = Common.GetPath(Common.TemplateURL);
                string fileExt = Path.GetExtension(templateFile);
                string filename = Path.GetFileNameWithoutExtension(templateFile);

                fileExt = exportFileType == ExportFileType.Excel ? fileExt : ExportFileType.PDF.ToString();
                string fileSuffix = DateTime.Now.ToString("HHmmss");
                string outputFileName = Common.ChuyenTVKhongDau(profileName) + fileSuffix + fileExt;

                string dirpath = Common.GetPath(Common.DownloadURL);
                if (foldersave != null && foldersave != string.Empty)
                {
                    dirpath = dirpath + foldersave + "/";
                    Directory.CreateDirectory(dirpath);
                }

                string outputPath = dirpath + outputFileName;
                outputPath = outputPath.Replace("/", "\\");
                templatepath = templatepath.Replace("/", "\\");
                ExportService exportService = new ExportService();
                exportService.TemplatePath = templatepath;

                var exportInfo = GetExportInfo(exportId);
                var isProtected = false;
                if (exportInfo.IsProtected == true)
                {
                    isProtected = true;
                }
                ExportTemplate templateInfo = new ExportTemplate()
                {
                    TemplateFile = exportInfo.TemplateFile,
                    IsProtected = isProtected
                };
                if (exportInfo.IsReadOnly == true)
                {
                    templateInfo.PasswordWrite = PasswordOpenFile;
                }
                if (isProtected)
                {
                    templateInfo.PasswordProtect = PasswordReadonly;
                }

                var success = false;
                if (exportInfo != null)
                {
                    if (exportInfo.IsSmartMarkers == true)
                    {
                        success = exportService.ExportNew(outputPath, templatepath, templateInfo, null, datasource);
                    }
                }
                if (success)
                {
                    string fileDownload = Common.MultiExport(ConstantPathWeb.Hrm_Hre_Service + Common.DownloadURL, false, outputPath);
                    return fileDownload;
                }

            }
            return "-1";
        }

        /// <summary> [Chuc.Nguyen] - Xuất dữ liệu ra Excel </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listModel"></param>
        /// <param name="valueField"></param>
        /// <returns></returns>
        public static string Export<T>(List<T> listModel, string[] valueField, ExportFileType exportFileType = ExportFileType.Excel)
        {
            return Export<T>(listModel, valueField, null, exportFileType);
        }

        public static string Export<T>(List<T> listModel, string[] valueField, Dictionary<string, string> formatFields, ExportFileType exportFileType = ExportFileType.Excel)
        {
            return Export<T>(Guid.Empty, listModel, valueField, formatFields, exportFileType);
        }

        /// <summary> [Chuc.Nguyen] - Xuất dữ liệu ra Excel </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listModel"></param>
        /// <param name="valueField"></param>
        /// <returns></returns>
        public static string Export<T>(Guid templateID, List<T> listModel, string[] valueField, Dictionary<string, string> formatFields, ExportFileType exportFileType = ExportFileType.Excel)
        {
            string filename = listModel.GetElementType().Name;
            string fileExt = exportFileType == ExportFileType.Excel ? "xls" : ExportFileType.PDF.ToString();
            string fileSuffix = DateTime.Now.ToString("HHmmss");
            string outputFileName = filename + fileSuffix + "." + fileExt;
            string dirpath = Common.GetPath(Common.DownloadURL);
            string outputPath = dirpath + outputFileName;
            ExportService exportService = new ExportService();
            outputPath = outputPath.Replace("/", "\\");
            string fileDownload = exportService.ExportByTemplate(outputPath, formatFields, valueField, templateID, null, listModel);
            string status = Common.MultiExport(ConstantPathWeb.Hrm_Hre_Service + Common.DownloadURL, false, fileDownload);
            return status;
        }

        public static string ExportPortal<T>(List<T> listModel, string[] valueField, ExportFileType exportFileType = ExportFileType.Excel)
        {
            return ExportPortal<T>(listModel, valueField, null, exportFileType);
        }

        public static string ExportPortal<T>(List<T> listModel, string[] valueField, Dictionary<string, string> formatFields, ExportFileType exportFileType = ExportFileType.Excel)
        {
            string filename = listModel.GetElementType().Name;
            string fileExt = exportFileType == ExportFileType.Excel ? "xls" : ExportFileType.PDF.ToString();
            string fileSuffix = DateTime.Now.ToString("HHmmss");
            string outputFileName = filename + fileSuffix + "." + fileExt;
            string dirpath = Common.GetPathPortal(Common.DownloadURL);
            string outputPath = dirpath + outputFileName;
            ExportService exportService = new ExportService();
            outputPath = outputPath.Replace("/", "\\");

            exportService.ExportNormal(outputPath, formatFields, valueField, listModel);
            var status = NotificationType.Success.ToString();

            if (status == NotificationType.Success.ToString())
            {
                status = Common.MultiExport(ConstantPathWeb.Hrm_Hre_Service + Common.DownloadURL, false, outputPath);
            }
            return status;
        }

        #region Export New

        public static Cat_Export GetExportInfo(Guid id)
        {
            try
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var repo = new Cat_ExportRepository(unitOfWork);
                    var exportQueryalbe = repo.FindBy(d => !d.IsDelete.HasValue || !d.IsDelete.Value);
                    var exportInfo = exportQueryalbe.Where(d => d.ID == id).FirstOrDefault();

                    return exportInfo;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public bool ExportNew(string outPutPath, string templatePath, ExportTemplate templateInfo, List<HeaderInfo> headerInfo, object dataSource, SaveFormat saveFormat = SaveFormat.Auto)
        {
            try
            {
                if (templateInfo != null && !string.IsNullOrEmpty(templateInfo.TemplateFile))
                {
                    var passwordWrite = templateInfo.PasswordWrite;
                    var passwordProtect = templateInfo.PasswordProtect;

                    //Create a workbook from Smart Markers template file
                    Workbook workbook = new Workbook(templatePath + templateInfo.TemplateFile);
                    if (!string.IsNullOrEmpty(passwordWrite))
                    {
                        workbook.Settings.Password = passwordWrite;
                    }

                    Worksheet worksheet = workbook.Worksheets[workbook.Worksheets.ActiveSheetIndex];
                    if (templateInfo.IsProtected && worksheet != null && !worksheet.IsProtected)
                    {
                        if (!string.IsNullOrEmpty(passwordProtect))
                        {
                            worksheet.Protect(ProtectionType.Contents, passwordProtect, string.Empty);
                        }
                        else
                        {
                            worksheet.Protect(ProtectionType.Contents);
                        }
                    }

                    //Instantiate a new WorkbookDesigner
                    WorkbookDesigner designer = new WorkbookDesigner();
                    //Specify the Workbook
                    designer.Workbook = workbook;
                    if (headerInfo != null)
                    {
                        ////Create Students DataTable
                        DataTable dt = new DataTable("HeaderInfo");
                        foreach (var item in headerInfo)
                        {
                            DataColumn dcName = new DataColumn();
                            dcName.ColumnName = item.Name;
                            if (item != null && item.Value != null && item.Value.GetType().Name == "DateTime")
                            {
                                dcName.DataType = typeof(DateTime);
                            }

                            dt.Columns.Add(dcName);
                        }
                        DataRow drName = dt.NewRow();
                        foreach (var item1 in headerInfo)
                        {
                            drName[item1.Name] = item1.Value;
                        }
                        dt.Rows.Add(drName);
                        designer.SetDataSource(dt);
                    }
                    if (dataSource != null)
                    {
                        if (dataSource.GetType() == typeof(DataTable))
                        {
                            DataTable dt = (DataTable)dataSource;
                            designer.SetDataSource(dt);
                        }
                        else
                        {
                            var listObj = (IEnumerable)dataSource;
                            var variableName = listObj.GetElementType().Name;
                            designer.SetDataSource(variableName, listObj);
                        }
                    }
                    //Process the smart markers
                    designer.Process();
                    // Save the Excel file
                    workbook.Save(outPutPath, saveFormat);

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region Xử lý export kiểu sinh code ngoài template

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý Export loại tự sinh mã
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static string ExportByGenCode(ConfigExport config)
        {
            var success = true;
            if (config != null && config.ExportId != null && config.ExportId != Guid.Empty)
            {
                #region Gán các biến cần thiết
                //Xử lý một số thuộc tính để export
                config = NewConfigExport(config);
                Guid exportId = config.ExportId;
                string outPutPath = config.OutPutPath;
                string templatePath = config.TemplatePath;
                string templateFile = config.TemplateFileName;
                var headerInfo = config.HeaderInfo;
                var dataSource = config.DataSource;
                #endregion

                #region Xử lý Export
                try
                {
                    //Create a workbook from Smart Markers template file
                    Workbook workbook = new Workbook(templatePath + templateFile);
                    //Instantiate a new WorkbookDesigner
                    WorkbookDesigner designer = new WorkbookDesigner();
                    //Specify the Workbook
                    designer.Workbook = workbook;

                    #region Xử lý phần header nếu có
                    if (headerInfo != null)
                    {
                        ////Create Students DataTable
                        DataTable dt = new DataTable("HeaderInfo");
                        foreach (var item in headerInfo)
                        {
                            DataColumn dcName = new DataColumn(item.Name, typeof(DateTime));
                            dt.Columns.Add(dcName);
                        }
                        DataRow drName = dt.NewRow();
                        foreach (var item1 in headerInfo)
                        {
                            drName[item1.Name] = item1.Value;
                        }
                        dt.Rows.Add(drName);
                        designer.SetDataSource(dt);
                    }
                    #endregion

                    #region Xử lý dữ liệu
                    if (dataSource != null)
                    {
                        if (dataSource.GetType() == typeof(DataTable))
                        {
                            DataTable dt = (DataTable)dataSource;
                            designer.SetDataSource(dt);
                        }
                        else
                        {
                            var listObj = (IEnumerable)dataSource;
                            var variableName = config.VariableName;
                            if (string.IsNullOrEmpty(variableName))
                            {
                                variableName = listObj.GetElementType().Name;
                            }
                            designer.SetDataSource(variableName, listObj);
                        }
                    }
                    #endregion

                    //Process the smart markers
                    designer.Process();
                    // Save the Excel file
                    workbook.Save(outPutPath, config.SaveFormat);
                    success = true;
                }
                catch (Exception)
                {
                    success = false;
                }
                #endregion

                if (success)
                {
                    return Common.MultiExport(config.DownloadPath, false, outPutPath);
                }
            }

            return "-1";
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý lại một số thuộc tính của config: như hậu tố file
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static ConfigExport NewConfigExport(ConfigExport config)
        {
            if (config != null)
            {
                var templatePath = config.TemplatePath;
                var outPutPath = config.OutPutPath;
                var outputFileName = config.FileName;
                var downloadPath = config.DownloadPath;
                var folderName = Common.TemplateURL;
                if (config.IsExport)
                {
                    folderName = Common.DownloadURL;
                }

                if (config.ExportId != null && config.ExportId != Guid.Empty)
                {
                    var exportInfo = GetExportInfo(config.ExportId);
                    //Loại file
                    string fileExt = Path.GetExtension(exportInfo.TemplateFile);
                    fileExt = config.ExportFileType == ExportFileType.Excel ? fileExt : ExportFileType.PDF.ToString();
                    if (exportInfo != null && !string.IsNullOrEmpty(exportInfo.TemplateFile))
                    {
                        if (string.IsNullOrEmpty(outputFileName))
                        {
                            //Tên file
                            string fileName = Path.GetFileNameWithoutExtension(exportInfo.TemplateFile);
                            //Hậu tố để đổi tên file
                            string fileSuffix = DateTime.Now.ToString("HHmmss");
                            //Tên file sau khi đổi tên
                            outputFileName = fileName + fileSuffix + fileExt;
                        }
                    }

                    config.TemplateFileName = exportInfo.TemplateFile;
                }
                if (string.IsNullOrEmpty(templatePath))
                {
                    //Thư mục chứ template
                    templatePath = Common.GetPath(Common.TemplateURL).Replace("/", "\\");
                }
                if (string.IsNullOrEmpty(downloadPath))
                {
                    downloadPath = ConstantPathWeb.Hrm_Main_Web + folderName;
                }
                if (string.IsNullOrEmpty(outPutPath))
                {
                    //Thư mục chứ file tải về
                    string dirpath = Common.GetPath(folderName);
                    outPutPath = (dirpath + outputFileName).Replace("/", "\\");
                }

                //Gán lại config
                config.TemplatePath = templatePath;
                config.OutPutPath = outPutPath;
                config.FileName = outputFileName;
                config.DownloadPath = downloadPath;
            }

            return config;
        }

        public string CreateTemplate(ConfigExport config)
        {
            if (config != null)
            {
                config = NewConfigExport(config);
                #region Gán các biến cần thiết
                string variableName = config.VariableName;
                string outPutPath = config.OutPutPath;
                string fileName = config.FileName;
                string defaultExt = ".xls";
                var headerInfo = config.HeaderInfo;
                var obj = config.Object;
                #endregion

                #region Kiểm tra tên file và thư mục
                //Xử lý fileName và ouPutPath
                if (!string.IsNullOrEmpty(outPutPath))
                {
                    //Kiểm tra trong ouput path có tồn tại tên file không
                    //Nếu có thì cắt bỏ tên file
                    string strFile = Path.GetExtension(outPutPath);
                    if (!string.IsNullOrEmpty(strFile))
                    {
                        strFile = Path.GetFileName(outPutPath);
                    }
                    if (!string.IsNullOrEmpty(strFile))
                    {
                        outPutPath = outPutPath.Substring(0, outPutPath.Length - strFile.Length);
                    }
                    //Nếu chưa tồn tại thư mục thì tạo ra thư mục
                    if (!Directory.Exists(outPutPath))
                    {
                        Directory.CreateDirectory(outPutPath);
                    }
                }
                else
                {
                    return "Chưa có OutPutPath";
                }

                if (!string.IsNullOrEmpty(fileName))
                {
                    //Kiểm tra fileName có hợp lệ không
                    string strExt = Path.GetExtension(fileName);
                    if (string.IsNullOrEmpty(strExt))
                    {
                        fileName = fileName + defaultExt;
                    }
                }

                #endregion

                #region Tạo mã code
                //Instantiate the workbookdesigner object.
                WorkbookDesigner wd = new WorkbookDesigner();
                if (headerInfo != null)
                {
                    ////Create Students DataTable
                    DataTable dt = new DataTable("HeaderInfo");
                    foreach (var item in headerInfo)
                    {
                        DataColumn dcName = new DataColumn(item.Name, typeof(string));
                        dt.Columns.Add(dcName);
                    }
                    foreach (var item1 in headerInfo)
                    {
                        DataRow drName = dt.NewRow();
                        drName[item1.Name] = item1.Value;
                    }

                    CreateCode(dt, ref wd);
                }

                if (obj != null)
                {
                    //Nếu fileName = null thì lấy tên đối tương làm tên file
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = obj.GetType().Name + defaultExt;
                    }

                    //Nếu variableName = null thì lấy tên đối tương làm tên variableName
                    if (string.IsNullOrEmpty(variableName))
                    {
                        variableName = obj.GetType().Name;
                    }
                    CreateCode(obj, 5, config.IsDataTable, variableName, ref wd);
                }
                #endregion

                //Process the smart markers to fill the data into the worksheets.
                wd.Process(true);
                string fullPath = outPutPath + "\\" + fileName;

                #region Lưu template
                try
                {
                    wd.Workbook.Save(fullPath);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return ex.InnerException.Message;
                    }
                    else
                    {
                        return ex.Message;
                    }
                }
                #endregion

                if (!string.IsNullOrEmpty(config.DownloadPath))
                {
                    return "Success," + config.DownloadPath + "/" + fileName;
                }
                else
                {
                    return "Đã lưu file vào thư mục vật lý, không có downloadPath";
                }
            }

            return "ConfigInfo Null";
        }

        #region Tạo code

        private void CreateCode(object obj, ref WorkbookDesigner wd)
        {
            CreateCode(obj, 1, true, ref wd);
        }
        private void CreateCode(object obj, int startRow, ref WorkbookDesigner wd)
        {
            CreateCode(obj, startRow, true, ref wd);
        }
        private void CreateCode(object obj, int startRow, bool isDataTable, ref WorkbookDesigner wd)
        {
            CreateCode(obj, startRow, isDataTable, string.Empty, ref wd);
        }
        private void CreateCode(object obj, int startRow, bool isDataTable, string variableName, ref WorkbookDesigner wd)
        {
            if (obj != null)
            {
                //Get the first worksheet(default sheet) in the workbook.
                Aspose.Cells.Worksheet w = wd.Workbook.Worksheets[0];
                var temp = 0;

                if (isDataTable)
                {
                    DataTable dt = (DataTable)obj;
                    foreach (DataColumn column in dt.Columns)
                    {
                        //Input some markers to the cells.
                        w.Cells[startRow, temp].PutValue(column.ColumnName.TranslateString());
                        w.Cells[startRow + 1, temp].PutValue("&=" + dt.TableName + "." + column.ColumnName);
                        temp++;
                    }
                }
                else
                {
                    //Lấy tất cả thuộc tính của đối tượng
                    var properties = obj.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        //Input some markers to the cells.
                        w.Cells[startRow, temp].PutValue(property.Name.TranslateString());
                        w.Cells[startRow + 1, temp].PutValue("&=" + variableName + "." + property.Name);
                        temp++;
                    }
                }

                //Set the datatable as the data source.
                wd.SetDataSource(variableName, obj);
            }
        }

        #endregion

        #endregion
    }

    public delegate void BeforeExportEventHandler(BeforeExportEventArgs e);

    public class BeforeExportEventArgs
    {
        public ExportTemplate[] ExportTemplates { get; set; }

        public BeforeExportEventArgs(ExportTemplate[] exportTemplates)
        {
            ExportTemplates = exportTemplates;
        }
    }

    public class HeaderInfo
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class ConfigExport
    {
        //Nếu export thì phải có
        public Guid ExportId { get; set; }
        public object DataSource { get; set; }

        //Nếu tạo template thì phải có
        public object Object { get; set; }
        public bool IsDataTable { get; set; }

        //Còn lại là chung, có thể có hoặc không
        public string OutPutPath { get; set; }
        public string TemplatePath { get; set; }
        public string DownloadPath { get; set; }
        public List<HeaderInfo> HeaderInfo { get; set; }
        public string VariableName { get; set; }
        public string FileName { get; set; }
        public string TemplateFileName { get; set; }
        public bool IsExport { get; set; }
        public SaveFormat SaveFormat { get; set; }
        public ExportFileType ExportFileType { get; set; }
    }
}
