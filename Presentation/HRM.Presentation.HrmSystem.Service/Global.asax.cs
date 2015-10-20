using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VnResource.Tasks;
using HRM.Infrastructure.Utilities;
using HRM.Infrastructure.Security;
namespace HRM.Presentation.HrmSystem.Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LicenseService.SetLicense();
            // LibraryService.StartTaskSchedulerGlobal();//khởi chạy backup dữ liệu
        }

        //private void StartTaskScheduler()
        //{
        //    TaskScheduler scheduler = new TaskScheduler();
        //    scheduler.TimerInterval = 60000;//30 phút
        //    string status = string.Empty;

        //    Sys_AutoBackupServices service = new Sys_AutoBackupServices();
        //    var lstObj = new List<object>();
        //    lstObj.Add(null);
        //    lstObj.Add(1);
        //    lstObj.Add(int.MaxValue - 1);
        //    var listAutoBackup = service.GetData<Sys_AutoBackupEntity>(lstObj, ConstantSql.hrm_sys_sp_get_AutoBackup, ref status).ToList();
        //    //var listAutoBackup = new List<Sys_AutoBackupEntity>();

        //    foreach (var autoBackup in listAutoBackup)
        //    {
        //        scheduler.TaskItems.Add(new TaskItem
        //        {
        //            CycleValue = autoBackup.TimeWaiting == null ? 0 : autoBackup.TimeWaiting.Value,
        //            DateExpired = autoBackup.DateExpired,
        //            DateStart = autoBackup.DateStart.Value,
        //            Description = autoBackup.Description,
        //            IsActivate = autoBackup.IsActivate == null ? false : autoBackup.IsActivate.Value,
        //            LastStart = autoBackup.LastStart,
        //            TaskArgs = autoBackup.AutoBackupName
        //            + "|" + autoBackup.ProcedureName
        //            + "|" + autoBackup.Email,
        //            TaskName = autoBackup.AutoBackupName,
        //            TaskType = typeof(Task_AutoBackup),
        //            Type = (SchedulerType)Enum.Parse(typeof(SchedulerType), autoBackup.Type.ToString()),
        //        });
        //    }

        //    scheduler.Start();
        //}
    }
}
