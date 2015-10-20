using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;
using HRM.Business.HrmSystem.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_MasterDataGroupDashBoardController : ApiController
    {
        /// <summary> </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_MasterDataGroupDashBoard")]
        public string Post([Bind]Cat_MasterDataGroupDashBoardModel model)
        {
            var listMasterDataGroupID = model.MasterDataGroupIDs.Where(d => d.HasValue).Select(d => d.Value).ToList();
            new Sys_UserServices().SetUserMasterDataGroup(model.UserID.GetGuid(), listMasterDataGroupID);
            return "Success";
        }
    }
}
