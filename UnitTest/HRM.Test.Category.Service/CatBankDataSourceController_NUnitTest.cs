using System;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRM.Presentation.Category.Service.Controllers;
using HRM.Presentation.Category.Service.Models;
using System.Web.Mvc;
//using Kendo.Mvc.UI;
using HRM.Data.Category.Model;
using System.Net.Http;
using HRM.Business.Category.Domain;

namespace HRM.Presentation.Category.Service.NUnitTest
{
    [TestFixture]
    public class CatBankDataSourceController_NUnitTest
    {

        //[Test]
        //public void NUnit_Service_GetCatBanks()
        //{
        //    CatBankDataSourceController controller = new CatBankDataSourceController();
        //    DataSourceRequest request = new DataSourceRequest();
        //    var result = ((JsonResult)controller.GetCatBanks(request)).Data;
        //    var rs = (Kendo.Mvc.UI.DataSourceResult)result;
        //    Console.Write(rs.Total);
        //}

        //[Test]
        //public void NUnit_Service_GetCatBankByID()
        //{
        //    CatBankDataSourceController controller = new CatBankDataSourceController();
        //    DataSourceRequest request = new DataSourceRequest();
        //    var bank = new Cat_Bank { Id = 22 };
        //    var result = ((JsonResult)controller.GetCatBankById(request, bank)).Data as Cat_Bank;
        //    NUnit.Framework.Assert.AreNotEqual(0, result.Id);
        //    Console.Write("SearchResult: " + result.Id + " | " + result.BankName + " | " + result.BankCode + " | " + result.Notes);
        //}

        //[Test]
        //public void NUnit_Service_AddCatBank()
        //{
        //    CatBankDataSourceController controller = new CatBankDataSourceController();
        //    HttpRequestMessage request = new HttpRequestMessage();
        //    var bank = new Cat_Bank
        //    {
        //        BankName = "ABCService",
        //        BankCode = "123123yyy",
        //        Notes = "NUnit Test"
        //    };
        //    var result = ((JsonResult)controller.AddCatBank(request, bank)).Data as Cat_Bank;
        //    NUnit.Framework.Assert.AreNotEqual(0,result.Id);
        //    Console.Write("{ID:BankName:BankCode:Notes} = " + result.Id + " | " + result.BankName + " | " + result.BankCode + " | " + result.Notes);
        //}

        //[Test]
        //public void NUnit_Service_UpdateCatBank()
        //{
        //    CatBankDataSourceController controller = new CatBankDataSourceController();
        //    HttpRequestMessage request = new HttpRequestMessage();
        //    var bank = new Cat_Bank
        //    {
        //        Id = 12,
        //        BankName = "ABCServiceU",
        //        BankCode = "BC123",
        //        Notes = "NUnit Test"
        //    };
        //    var result = ((JsonResult)controller.UpdateCatBank(request, bank)).Data as Cat_Bank;
        //    NUnit.Framework.Assert.IsNotNull(result);
        //    Console.Write("{ID:BankName:BankCode:Notes} =>> Results After Update : " + result.Id + " | " + result.BankName + " | " + result.BankCode + " | " + result.Notes);

        //}

        //[Test]
        //public void NUnit_Service_DeleteCatBank()
        //{
        //    CatBankDataSourceController controller = new CatBankDataSourceController();
        //    HttpRequestMessage request = new HttpRequestMessage();
        //    var bank = new Cat_Bank { Id = 44 };
        //    var result = ((JsonResult)controller.DeleteCatBank(request, bank)).Data as Cat_Bank;
        //    NUnit.Framework.Assert.AreEqual(0, result.Id);
        //}

        
    }
}
