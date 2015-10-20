using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
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

    public class Att_AnnualSickLeaveDetailController : ApiController
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
        [System.Web.Mvc.RouteAttribute("api/Att_AnnualSickLeaveDetail")]
        public bool Post([FromBody]Att_AnnualSickLeaveDetailSearchModel model)
        {
            /*
             * -get leaveDetail : type , year
             *   - get List Profile :
             *      + Theo phong ban
             *      + Theo trạng thai StatusEmpleaveDetail
             * 
             */
            /*
             * -get leaveDetail : type , year
             *   - get List Profile :
             *      + Theo phong ban
             *      + Theo trạng thai StatusEmpleaveDetail
             */
            Att_AnnualSickLeaveDetailServices services = new Att_AnnualSickLeaveDetailServices();
            bool result = services.ComputeAnnualSick(model.Year, model.OrgStructureID, model.ProfileStatus, true, UserLogin);
            if (result != null)
            {
                return true;
            }
            return false;
        }


    }
}