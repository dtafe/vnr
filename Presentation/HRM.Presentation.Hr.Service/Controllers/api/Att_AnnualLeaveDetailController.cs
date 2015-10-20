using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace HRM.Presentation.Hr.Service.Controllers.api
{

    public class Att_AnnualLeaveDetailController : ApiController
    {
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    userLogin = headerValues.FirstOrDefault();
                }
                return userLogin;
            }
        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_AnnualLeaveDetail")]
        public bool Post([FromBody]Att_AnnualLeaveDetailSearchModel model)
        {
            /*
             * -get leaveDetail : type , year
             *   - get List Profile :
             *      + Theo phong ban
             *      + Theo trạng thai StatusEmpleaveDetail
             */
            Att_AnnualLeaveDetailServices services = new Att_AnnualLeaveDetailServices();
            bool result = services.ComputeAnnualLeaveDetail(model.Year, model.OrgStructureID, model.Type, UserLogin);
            if(result != null){
                return true;
            }
            return false;
        }
    }
}