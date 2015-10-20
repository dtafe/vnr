using System.Collections.Generic;
using System.Web.Http;
using HRM.Presentation.Attendance.Models;
using HRM.Business.Hr.Domain;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Att_AnalyzeAnnualController : ApiController
    {
        #region MyRegion
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
        #endregion
        public IEnumerable<Att_AnnualLeaveDetailModel> Post(Att_AnnualLeaveDetailModel model)
        {
            var service = new Att_AnnualLeaveDetailServices();
            var status = string.Empty;
            var result = service.GetDataNotParam<Att_AnnualLeaveDetailModel>(ConstantSql.hrm_att_sp_get_AnnualLeaveDetail, UserLogin, ref status);
            return result;
        }
	}
}