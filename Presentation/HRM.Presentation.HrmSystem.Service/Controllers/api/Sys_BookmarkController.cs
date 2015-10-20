using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace HRM.Presentation.HrmSystem.Service.Controllers
{
    public class Sys_BookmarkController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Sys_BookmarkModel> Get()
        {
            string status = string.Empty;
            var service = new Sys_BookmarkServices();
            var list = service.GetAllUseEntity<Sys_BookmarkEntity>(ref status);
            return list.Select(item => new Sys_BookmarkModel
            {
                ID = item.ID,
                Link = item.Link,
                Name = item.Name
            });
        }

        // GET api/<controller>/5
        public string Get(Guid id)
        {
            return "value";
        }
        // POST api/<controller>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_Bookmark")]
        public Sys_BookmarkModel Post([FromBody]Sys_BookmarkModel bookmark)
        {
            Guid userId = bookmark.UserID;
            var model = new Sys_BookmarkEntity
            {
                ID = bookmark.ID,
                Link  = bookmark.Link,
                Name = bookmark.Name,
                UserID = userId,
                UserCreate = bookmark.UserCreate
                
            };
            var service = new Sys_BookmarkServices();
            service.Add<Sys_BookmarkEntity>(model);

            return bookmark;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}