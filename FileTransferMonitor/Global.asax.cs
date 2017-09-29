using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FileTransferMonitor
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            string path = Request.PhysicalApplicationPath + "Report\\user-reguest-data.txt";

            using ( StreamWriter sw = File.AppendText(path) )
            {
                sw.WriteLine("--------------// " + DateTime.Now.ToString() + " //--------------");
                sw.WriteLine("HttpMethod: " + Request.HttpMethod);
                sw.WriteLine("User Label: " + Request.LogonUserIdentity.Label);
                sw.WriteLine("Url: " + Request.Url.ToString());
                sw.WriteLine("UserAgent: " + Request.UserAgent);
                sw.WriteLine("UserHostAddress: " + Request.UserHostAddress);
                sw.WriteLine("UserHostName: " + Request.UserHostName);
                sw.WriteLine("UserLanguages: " + Request.UserLanguages[Request.UserLanguages.Rank]);
                sw.WriteLine("ContentType: " + Request.ContentType);
                sw.WriteLine("//-------------- End write data of user request ----------------//");
            }

        }
    }

}
