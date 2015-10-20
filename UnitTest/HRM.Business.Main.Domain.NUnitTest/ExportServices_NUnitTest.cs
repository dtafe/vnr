using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
using System.Diagnostics;
using System.Collections.Generic;
using NUnit.Framework;
using HRM.Data.Category.Model;
using HRM.Data.Main.Sql;

namespace HRM.Business.Main.Domain.NUnitTest
{
    [TestFixture]
    //[TestClass]
    public class ExportServices_NUnitTest
    {
        [Test]
        //[TestMethod]
        public void ExportData()
        {
            using (var context = new VnrHrmDataContext())
            {
                #region Cat_Export
                List<Cat_Export> listExport = new List<Cat_Export>();
                Cat_Export _export = new Cat_Export()
                {
                    Id = 1,
                    ExportName = "Test Export",
                    ExportCode = "Test",
                    ScreenName = "Test Screen",
                    ObjectName = "Hre_Profile",
                    IsReadOnly = true,
                    IsProtected = true,
                    SheetIndex = 1,
                    StartColumnIndex = 1,
                    StartRowIndex = 2,
                    TemplateFile = "Test Template File",
                    IsGroup = true
                };
                listExport.Add(_export);
                #endregion

                #region Cat_ExportItem
                List<Cat_ExportItem> listExportItem = new List<Cat_ExportItem>();
                Cat_ExportItem _exportItemEntity1 = new Cat_ExportItem()
                {
                    ExportID = 1,
                    DataField = "ProfileName",
                    ExcelField = "B"
                };
                Cat_ExportItem _exportItemEntity2 = new Cat_ExportItem()
                {
                    ExportID = 1,
                    DataField = "CodeEmp",
                    ExcelField = "C"
                };
                Cat_ExportItem _exportItemEntity3 = new Cat_ExportItem()
                {
                    ExportID = 1,
                    DataField = "DateHire",
                    ExcelField = "D"
                };
                listExportItem.Add(_exportItemEntity1);
                listExportItem.Add(_exportItemEntity2);
                listExportItem.Add(_exportItemEntity3);
                #endregion

                context.Cat_ExportItem.Add(_exportItemEntity1);
                context.Cat_ExportItem.Add(_exportItemEntity2);
                context.Cat_ExportItem.Add(_exportItemEntity3);
                context.Cat_Export.Add(_export);
                context.SaveChanges();
            }

            string passwordwrite = "123";
            string passwordread = "123";

            string filename = "D:\\New Microsoft Excel Worksheet.xls";
            string templatePath = "D:\\Template";

            #region Datasource
            List<Hre_ProfileEntity> listdataProfile = new List<Hre_ProfileEntity>();
            Hre_ProfileEntity _Profile1 = new Hre_ProfileEntity()
            {
                ProfileName = "Name1",
                CodeEmp = "001",
                DateHire = DateTime.Now
            };
            Hre_ProfileEntity _Profile2 = new Hre_ProfileEntity()
            {
                ProfileName = "Name2",
                CodeEmp = "002",
                DateHire = DateTime.Now
            };
            listdataProfile.Add(_Profile1);
            listdataProfile.Add(_Profile2);
            #endregion

            ExportService service = new ExportService();
            service.ExportByTemplate(1, filename, passwordwrite, passwordread, templatePath, null, listdataProfile);
            Process.Start(filename);
        }

    }
}
