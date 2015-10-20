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
    public class CatCostCentreController_NUnitTest
    {
        //[Test]
        //public void NUnit_Service_GetCatCostCentres()
        //{
        //    CatCostCentreController controller = new CatCostCentreController();
        //    DataSourceRequest request = new DataSourceRequest();
        //    var result = ((JsonResult)controller.GetCatCostCentres(request)).Data;
        //    var rs = (Kendo.Mvc.UI.DataSourceResult)result;
        //    Console.Write(rs.Total);
        //}

        //[Test]
        //public void NUnit_Service_GetCatCostCentreByID()
        //{
        //    CatCostCentreController controller = new CatCostCentreController();
        //    DataSourceRequest request = new DataSourceRequest();
        //    var bank = new Cat_Bank { Id = 22 };
        //    var result = ((JsonResult)controller.GetCatCostCentreById(request, bank)).Data as Cat_Bank;
        //    NUnit.Framework.Assert.AreNotEqual(0, result.Id);
        //    Console.Write("SearchResult: " + result.Id + " | " + result.BankName + " | " + result.BankCode + " | " + result.Notes);
        //}

        //[Test]
        //public void NUnit_Service_AddCatCostCentre()
        //{
        //    CatCostCentreController controller = new CatCostCentreController();
        //    HttpRequestMessage request = new HttpRequestMessage();
        //    var bank = new Cat_Bank
        //    {
        //        BankName = "ABCService",
        //        BankCode = "123123yyy",
        //        Notes = "NUnit Test"
        //    };
        //    var result = ((JsonResult)controller.AddCatCostCentre(request, bank)).Data as Cat_Bank;
        //    NUnit.Framework.Assert.AreNotEqual(0,result.Id);
        //    Console.Write("{ID:BankName:BankCode:Notes} = " + result.Id + " | " + result.BankName + " | " + result.BankCode + " | " + result.Notes);
        //}

        //[Test]
        //public void NUnit_Service_UpdateCatCostCentre()
        //{
        //    CatCostCentreController controller = new CatCostCentreController();
        //    HttpRequestMessage request = new HttpRequestMessage();
        //    var bank = new Cat_Bank
        //    {
        //        Id = 12,
        //        BankName = "ABCServiceU",
        //        BankCode = "BC123",
        //        Notes = "NUnit Test"
        //    };
        //    var result = ((JsonResult)controller.UpdateCatCostCentre(request, bank)).Data as Cat_Bank;
        //    NUnit.Framework.Assert.IsNotNull(result);
        //    Console.Write("{ID:BankName:BankCode:Notes} =>> Results After Update : " + result.Id + " | " + result.BankName + " | " + result.BankCode + " | " + result.Notes);

        //}

        //[Test]
        //public void NUnit_Service_DeleteCatCostCentre()
        //{
        //    CatCostCentreController controller = new CatCostCentreController();
        //    HttpRequestMessage request = new HttpRequestMessage();
        //    var bank = new Cat_Bank { Id = 44 };
        //    var result = ((JsonResult)controller.DeleteCatCostCentre(request, bank)).Data as Cat_Bank;
        //    NUnit.Framework.Assert.AreEqual(0, result.Id);
        //}

        
    }
}
