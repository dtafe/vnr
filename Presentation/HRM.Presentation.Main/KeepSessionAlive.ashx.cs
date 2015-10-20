using System.Web;
using System.Web.SessionState;
using System;
using HRM.Infrastructure.Utilities;


namespace HRM.Presentation.Main
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class KeepSessionAlive : IHttpHandler, IRequiresSessionState
    {
        //private IEntityService _entityService;
        //public IEntityService EntityService
        //{
        //    get { return _entityService ?? (_entityService = VnResourceWeb.Library.Services.EntityService.Instance); }
        //}

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("10");
            CheckSessionAliveStatus(context);
        }

        private void CheckSessionAliveStatus(HttpContext context)
        {
            if (context.Session[SessionObjects.UserId] != null)
            {
                if (context.Session[SessionObjects.UsingUserLastCheck] != null)
                {
                    //Hien.pham - 20121205 - 0016914: Cập nhật trạng thái online của user
                    DateTime usingUserDate = (DateTime)context.Session[SessionObjects.UsingUserLastCheck];

                    if (DateTime.Now.Subtract(usingUserDate).TotalMinutes >= Common.SessionTimeOut - 1)
                    {
                        context.Session.Abandon();
                        context.Session.RemoveAll();
                    }
                }
                else
                {
                    //Hien.pham - 20121205 - 0016914: Lần cuối cùng cập nhật trạng thái
                    context.Session[SessionObjects.UsingUserLastCheck] = DateTime.Now;
                }
            } 
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
