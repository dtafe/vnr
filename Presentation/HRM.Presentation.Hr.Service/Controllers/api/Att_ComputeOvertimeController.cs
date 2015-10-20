using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Presentation.Attendance.Models;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Att_ComputeOvertimeController : ApiController
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
        public IEnumerable<Att_OvertimeModel> Post(Att_ComputeOvertimeModel model)
        {
            Att_WorkDayServices serviceWorkDay = new Att_WorkDayServices();
            var status = string.Empty;
            var lstWorkDay = serviceWorkDay.GetWorkDaysByInOut(model.DateFrom, model.DateTo);
            Cat_ShiftServices serviceShift = new Cat_ShiftServices();
            var lstShift = serviceShift.GetDataNotParam<Cat_ShiftEntity>(ConstantSql.hrm_cat_sp_get_Shift, UserLogin, ref status);

            //Cat_ShiftItemServices serviceShiftItem = new Cat_ShiftItemServices();
            //var lstShiftItem = serviceShiftItem.GetCatShiftItem();
            var lstShiftItem = new List<Cat_ShiftItemEntity>();
            Cat_DayOffServices servicesDayOff = new Cat_DayOffServices();
            var lstDayOff = servicesDayOff.GetAllUseEntity<Cat_DayOffEntity>(ref status).ToList();

            Cat_OvertimeTypeServices servicesOvertimeType = new Cat_OvertimeTypeServices();
            var lstOvertimeType = servicesOvertimeType.GetDataNotParam<Cat_OvertimeTypeEntity>(ConstantSql.hrm_cat_sp_get_OvertimeType, UserLogin, ref status);

            var Att_OvertimeInfoFillterAnalyze = new Att_OvertimeInfoFillterAnalyze()
            {
                isAllowGetAfterShift = model.isAllowGetAfterShift,
                isAllowGetBeforeShift = model.isAllowGetBeforeShift,
                isAllowGetInShift = model.isAllowGetInShift,
                isAllowGetOTOutterShift = model.isAllowGetOTOutterShift,
                isAllowGetTypeBaseOnActualDate = model.isAllowGetTypeBaseOnActualDate,
                isAllowGetTypeBaseOnBeginShift = model.isAllowGetTypeBaseOnBeginShift,
                isAllowGetTypeBaseOnEndShift = model.isAllowGetTypeBaseOnEndShift,
                MininumOvertimeHour = model.MininumOvertimeHour
            };

            var service = new Att_OvertimeServices();
            var result = service.AnalyzeOvertime(lstWorkDay, 
                                                lstShift, 
                                                lstShiftItem, 
                                                lstDayOff, 
                                                lstOvertimeType,
                                                Att_OvertimeInfoFillterAnalyze, UserLogin).ToList().Translate<Att_OvertimeModel>();
            return result;
        }
    }
}
