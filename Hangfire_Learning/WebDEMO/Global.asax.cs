using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebDEMO
{
    using Hangfire;
    using Common;
    using Jobs;

    public class MvcApplication : System.Web.HttpApplication
    {
        private BackgroundJobServer _backgroundJobServer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.UseSqlServerStorage(Common.Util.Key.ConnectionStrs.HangfireConnStr);
            //By default, polling interval is equal to 15 seconds
            //var op = new BackgroundJobServerOptions { SchedulePollingInterval = TimeSpan.FromSeconds(30)};
            _backgroundJobServer = new BackgroundJobServer();//(op);

            //Register schedules
            JobManager.SetupJobs();
            
        }

        protected void Application_End(object sender, EventArgs e)
        {
            _backgroundJobServer.Dispose();
        }
    }
}
