using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Main.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HRM.Presentation.Main.Controllers
{
    public class SessionApiController : MainBaseController
    {
        public bool setSession(SessionObjectsModel model)
        {
            //this.Session.Add(SessionObjectsModel.FieldsName.UserId, model.UserId);
            //this.Session.Add(SessionObjectsModel.FieldsName.LoginUserName, model.LoginUserName);
            //this.Session.Add(SessionObjectsModel.FieldsName.IsActive, model.IsActive);

            return true;
        }
    }
}
