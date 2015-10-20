//using System;
//using System.Collections.Generic;
//using Kendo.Mvc.UI;
//using NUnit.Framework;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using HRM.Presentation.Category.Service.Controllers;
//using HRM.Presentation.Category.Service.Models;
//using System.Web.Mvc;
////using Kendo.Mvc.UI;
//using HRM.Data.Category.Model;
//using System.Net.Http;
//using HRM.Business.Category.Domain;

//namespace HRM.Presentation.Category.Service.NUnitTest
//{
//    [TestFixture]
//    public class CatEthnicGroupDataSourceController_NUnitTest
//    {

//        [Test]
//        public void NUnit_Service_GetCatEthnicGroup()
//        {
//            CatEthnicGroupDataSourceController controller = new CatEthnicGroupDataSourceController();
//            DataSourceRequest request = new DataSourceRequest();
//            var result = ((JsonResult)controller.GetCatEthnicGroup(request)).Data;
//            var rs = (Kendo.Mvc.UI.DataSourceResult)result;
//            Console.Write(rs.Total);
//        }

//        [Test] 
//        public void NUnit_Service_GetCatEthnicGroupByID()
//        {
//            CatEthnicGroupDataSourceController controller = new CatEthnicGroupDataSourceController();
//            DataSourceRequest request = new DataSourceRequest();
//            var EthnicGroup = new Cat_EthnicGroup { Id = 2 };
//            var result = ((JsonResult)controller.GetCatEthnicGroupById(request, EthnicGroup)).Data as Cat_EthnicGroup;
//            NUnit.Framework.Assert.AreNotEqual(0, result.Id);
//            Console.Write("SearchResult: " + result.Id + " | " + result.EthnicGroupName + " | " + result.EthnicGroupCode );
//        }

//        [Test]
//        public void NUnit_Service_AddCatEthnicGroup()
//        {
//            CatEthnicGroupDataSourceController controller = new CatEthnicGroupDataSourceController();
//            HttpRequestMessage request = new HttpRequestMessage();
//            var EthnicGroup = new Cat_EthnicGroup
//            {
//                EthnicGroupName = "ABCService",
//                EthnicGroupCode = "123123yyy",
//            };
//            var result = ((JsonResult)controller.AddCatEthnicGroup(request, EthnicGroup)).Data as Cat_EthnicGroup;
//            NUnit.Framework.Assert.AreNotEqual(0,result.Id);
//            Console.Write("{ID:EthnicGroupName:EthnicGroupCode} = " + result.Id + " | " + result.EthnicGroupName + " | " + result.EthnicGroupCode );
//        }

//        [Test]
//        public void NUnit_Service_UpdateCatEthnicGroup()
//        {
//            CatEthnicGroupDataSourceController controller = new CatEthnicGroupDataSourceController();
//            HttpRequestMessage request = new HttpRequestMessage();
//            var EthnicGroup = new Cat_EthnicGroup
//            {
//                Id = 12,
//                EthnicGroupName = "ABCServiceU",
//                EthnicGroupCode = "BC123",
//            };
//            var result = ((JsonResult)controller.UpdateCatEthnicGroup(request, EthnicGroup)).Data as Cat_EthnicGroup;
//            NUnit.Framework.Assert.IsNotNull(result);
//            Console.Write("{ID:EthnicGroupName:EthnicGroupCode} =>> Results After Update : " + result.Id + " | " + result.EthnicGroupName + " | " + result.EthnicGroupCode);

//        }

//        [Test]
//        public void NUnit_Service_DeleteCatEthnicGroup()
//        {
//            CatEthnicGroupDataSourceController controller = new CatEthnicGroupDataSourceController();
//            HttpRequestMessage request = new HttpRequestMessage();
//            var EthnicGroup = new Cat_EthnicGroup { Id = 44 };
//            var result = ((JsonResult)controller.DeleteCatEthnicGroup(request, EthnicGroup)).Data as Cat_EthnicGroup;
//            NUnit.Framework.Assert.AreEqual(0, result.Id);
//        }

        
//    }
//}
