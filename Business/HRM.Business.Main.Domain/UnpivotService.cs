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
using VnResource.Importer;
using System.Text.RegularExpressions;

namespace HRM.Business.Main.Domain
{
    public class UnpivotService
    {
        #region Properties

        private string fileName;
        private Guid? pivotTemplateID;
        private Guid userID;

        public Guid UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public Guid? PivotTemplateID
        {
            get { return pivotTemplateID; }
            set { pivotTemplateID = value; }
        }

        #endregion

        public string[] Unpivot()
        {
            string[] result = null;

            if (!string.IsNullOrWhiteSpace(FileName))
            {
                using (var context = new VnrHrmDataContext())
                {
                    IUnitOfWork unitOfWork = new UnitOfWork(context);

                    if (PivotTemplateID != Guid.Empty)
                    {
                        var listPivotItem = unitOfWork.CreateQueryable<Cat_PivotItem>(Guid.Empty,
                            d => d.PivotID == PivotTemplateID).Select(d => new
                            {
                                d.StartColumnIndex,
                                d.StartRowIndex,
                                d.SkipRowNumbers,
                                d.PivotColumnEnd,
                                d.PivotColumnName,
                                d.PivotColumnStart,
                                d.PivotHeaderRowIndex,
                                d.PivotSheetIndex,
                                d.TargetSheetIndex,
                                d.TargetSheetName
                            }).ToList();

                        string fileExt = Path.GetExtension(FileName);
                        fileExt = string.IsNullOrWhiteSpace(fileExt) ? "xls" : fileExt;
                        fileExt = fileExt.StartsWith(".") ? fileExt : "." + fileExt;

                        string downloadPath = Common.GetPath(Common.DownloadURL);
                        downloadPath = downloadPath.Replace("/", "\\");

                        string fileSuffix = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string outputFile = Path.GetFileNameWithoutExtension(FileName);
                        outputFile = downloadPath + "\\" + outputFile + fileSuffix + fileExt;
                        outputFile = Regex.Replace(outputFile, @"[^0-9a-zA-Z_\\:.]+", "");

                        var listPivotTemplate = listPivotItem.Select(d => new PivotTemplate
                        {
                            PivotFileName = FileName,
                            TargetFileName = outputFile,
                            StartColumnIndex = d.StartColumnIndex.GetInteger(),
                            StartRowIndex = d.StartRowIndex.GetInteger(),
                            SkipRowNumbers = d.SkipRowNumbers.GetInteger(),
                            PivotColumnEnd = d.PivotColumnEnd.GetInteger(),
                            PivotColumnName = d.PivotColumnName,
                            PivotColumnStart = d.PivotColumnStart.GetInteger(),
                            PivotHeaderRowIndex = d.PivotHeaderRowIndex.GetInteger(),
                            PivotSheetIndex = d.PivotSheetIndex.GetInteger(),
                            TargetSheetIndex = d.TargetSheetIndex.GetInteger(),
                            TargetSheetName = d.TargetSheetName
                        }).ToArray();

                        ExcelImporter importer = new ExcelImporter();
                        result = importer.Unpivot(listPivotTemplate);
                    }
                }
            }

            return result;
        }
    }
}
