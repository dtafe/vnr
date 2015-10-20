using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Category.Models;
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
    public class Att_OvertimeConfirmController : ApiController
    {       
        /// <summary>
        /// Xử lí lưu xác nhận tăng ca
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>       
        public Att_OvertimeModel Post(IEnumerable<Att_OvertimeModel> listmodel)
        {
            Att_OvertimeModel model = new Att_OvertimeModel();
            var service = new Att_OvertimeServices();            
            string result = service.SaveOvertimeConfirm(listmodel.Select(item => new Att_OvertimeEntity
            {
                ID = item.ID,
                ConfirmHours = item.ConfirmHours
            }
            ));
            return model;
        }
    }
}