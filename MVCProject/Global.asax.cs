using System;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FLS.MyFirstProject.Infrastructure;

namespace MVCProject
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private readonly NLog.Logger m_logger = NLog.LogManager.GetCurrentClassLogger();

        public static Facade Facade
        {
            get
            {
                return IocContainer.Container.GetInstance<Facade>();
            }
        }

        protected void Application_Start()
        {
            m_logger.Info("Start!");
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var baseProjectPath = AppDomain.CurrentDomain.BaseDirectory;
            var appDataPath = Path.Combine(baseProjectPath, ".\\..\\App_Data");
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath(appDataPath));
        }
    }
}