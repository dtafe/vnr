using HRM.Data.Category.Model;
using Kendo.Mvc.UI;
using System;
//using System.Collections.Generic;
using Kendo.Mvc.Extensions;
using System.Web.Mvc;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category;
using System.Net.Http;
using System.Net;

namespace HRM.Presentation.Category.Service.Controllers
{
    public class CatBankDataSourceController : Controller, IDisposable
    {
        //
        // GET: /CatBankDataSource/

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetCatBanks([DataSourceRequest] DataSourceRequest request)
        {
            Cat_BankServices service = new Cat_BankServices();
            var list = service.GetCatBanks();
            return Json(list.ToDataSourceResult(request));
        }

        public JsonResult GetCatBankById([DataSourceRequest] DataSourceRequest request, Cat_Bank catbank)
        {
            Cat_BankServices service = new Cat_BankServices();
            var CatBank = service.GetByIdCatBanks(catbank.Id);
            return Json(CatBank);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetCatBanksTest()
        {
            Cat_BankServices service = new Cat_BankServices();
            var list = service.GetCatBanks();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddCatBank(HttpRequestMessage request, Cat_Bank bank)
        {
            var model = new Cat_Bank();
            model.Id = bank.Id;
            model.BankName = bank.BankName;
            model.BankCode = bank.BankCode;
            model.Notes = bank.Notes;
            Cat_BankServices service = new Cat_BankServices();
            var result = service.AddCatBanks(model);
            if (result == false) model = null;
            return Json(model);
        }
        public JsonResult UpdateCatBank(HttpRequestMessage request, Cat_Bank bank)
        {
            
            var model = new Cat_Bank();
            model.Id = bank.Id;
            model.BankName = bank.BankName;
            model.BankCode = bank.BankCode;
            model.Notes = bank.Notes;
            model.UserCreate = bank.UserCreate;
            model.UserLockID = bank.UserLockID;
            model.UserUpdate = bank.UserUpdate;
            model.DateCreate = bank.DateCreate;
            model.DateLock = bank.DateLock;
            model.DateUpdate = bank.DateUpdate;
            model.IPCreate = bank.IPCreate;
            model.IPUpdate = bank.IPUpdate;
            model.IsDelete = bank.IsDelete;
            model.ServerCreate = bank.ServerCreate;
            model.ServerUpdate = bank.ServerUpdate;
            
            Cat_BankServices service = new Cat_BankServices();
            var result = service.UpdateCatBank(model);
            if (result == false) model = null;
            return Json(model);
        }
        public JsonResult DeleteCatBank(HttpRequestMessage request, Cat_Bank bank)          
        {
            Cat_Bank catBank = new Cat_Bank();
            Cat_BankServices service = new Cat_BankServices();
            var result = service.DeleteCatBank(bank.Id);
            if (result == false) catBank = null;
            return Json(catBank);
        }
    }
}