using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace finalProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Trace.Listeners.Add(new TextWriterTraceListener(new FileStream(Server.MapPath("/logs/Info.log"), FileMode.Append, FileAccess.Write, FileShare.Read)));
            Trace.WriteLine(DateTime.Now.ToString("F") + "," + "Application_Start");
            Trace.WriteLine("_____________________________________________________");
            Trace.Flush();
        }

        protected void Application_End()
        {
            Trace.WriteLine(DateTime.Now.ToString("F") + "," + "Application_Ended");
            Trace.WriteLine("_____________________________________________________");
            Trace.Close();
        }
    }
}
