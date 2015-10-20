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
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{

    public class Att_AnnualInsuranceLeaveDetailController : ApiController
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
        public bool Post([FromBody]Att_AnnualInsuranceLeaveDetailSearchModel model)
        {
            var insLeaveDetailServices = new Att_AnnualInsuranceLeaveDetailServices();
            var isScucess = insLeaveDetailServices.ComputeInsuranceLeaveDetail(model.Year, model.OrgStructureID, model.ProfileStatus, UserLogin);

            return isScucess;
        }


    }
}