using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Elmah.Everywhere;
using Elmah.Everywhere.Diagnostics;

namespace Silverlight_Sample.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ConfigureElmahLogger();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private static void ConfigureElmahLogger()
        {
            // Configure
            var writter = new HttpExceptionWritter
            {
                RequestUri = new Uri("http://localhost:11079/error/log", UriKind.Absolute)
            };

            var defaults = new ExceptionDefaults
            {
                Token = "Test-Token",
                ApplicationName = "Silverlight-Web-Sample",
                Host = Environment.MachineName
            };

            ExceptionHandler.WithParameters(defaults, writter);
        }
    }
}