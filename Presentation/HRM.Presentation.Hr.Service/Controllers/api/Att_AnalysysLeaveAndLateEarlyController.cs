using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Presentation.Attendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace HRM.Presentation.Hr.Service.Controllers.api
{

    public class Att_AnalysysLeaveAndLateEarlyController : ApiController
    {
        // GET api/<controller>
        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        public List<Att_AnalysysLeaveAndLateEarlyEntity> Get()
        {
            var service = new Att_ReportServices();
            List<Att_AnalysysLeaveAndLateEarlyEntity> list = new List<Att_AnalysysLeaveAndLateEarlyEntity>();
            //List<Att_ReportSumaryInOutEntity> list = service.GetReportSumaryInOut();
            return list;
        }
    }
}