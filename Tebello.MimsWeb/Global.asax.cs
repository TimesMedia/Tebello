using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Subs.Data;

namespace Tebello.MimsWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);

                string lConnectionString = global::Tebello.MimsWeb.Properties.Settings.Default.ConnectionString;

                if (lConnectionString == "")
                {
                    throw new Exception("No connection string has been set.");
                }
                else
                {
                    Settings.ConnectionString = lConnectionString;
                    Settings.DirectoryPath = global::Tebello.MimsWeb.Properties.Settings.Default.DirectoryPath;
                    //Settings.EMailHostIp = global::MimsWeb.Properties.Settings.Default.EMailHostIp;
                }
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Application_Start", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
        }
    }
}
