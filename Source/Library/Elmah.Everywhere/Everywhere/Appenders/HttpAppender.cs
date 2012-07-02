using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Security.Principal;


namespace Elmah.Everywhere.Appenders
{
    public class HttpAppender : BaseAppender
    {
        public override void Append(ErrorInfo error)
        {
            HttpContextBase httpContext = GetContext();
            if (httpContext != null)
            {
                var pairs = new Dictionary<string, string>();

                pairs.Add("Web server", GetWebServer());
                pairs.Add("Integrated pipeline", HttpRuntime.UsingIntegratedPipeline.ToString());

                error.AddDetail(this.Name, pairs);

                HttpRequestBase httpRequest = httpContext.Request;

                error.AddDetail("ServerVariables", ToDictionary(httpRequest.ServerVariables));
                error.AddDetail("QueryString", ToDictionary(httpRequest.QueryString));
                error.AddDetail("Form", ToDictionary(httpRequest.ServerVariables));
                error.AddDetail("Cookies", ToDictionary(CopyCollection(httpRequest.Cookies)));

                HttpException httpException = error.Exception as HttpException;
                if (httpException != null)
                {
                    error.StatusCode = httpException.GetHttpCode();
                }

                error.User = "";
                IPrincipal webUser = httpContext.User;
                if (webUser != null && webUser.Identity.Name.Length > 0)
                {
                    error.User = webUser.Identity.Name;
                }
            }
        }

        private static IEnumerable<KeyValuePair<string, string>> ToDictionary(NameValueCollection collection)
        {
            return collection.Keys.Cast<string>().ToDictionary(key => key, key => collection[key]);
        }

        protected virtual HttpContextBase GetContext()
        {
            return HttpContext.Current == null ? null : new HttpContextWrapper(HttpContext.Current);
        }

        private static string GetWebServer()
        {
            string serverSoftware = "Unknown";
            string processName = DetailAppender.GetWorkerProcess();
            string iisVersion = HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
            if (String.IsNullOrEmpty(iisVersion))
            {
                iisVersion = "Unknown";
            }
            serverSoftware = (iisVersion == "Unknown" && processName != null &&
                              processName.StartsWith("WebDev.WebServer", StringComparison.OrdinalIgnoreCase))
                                 ? "Visual Studio web server"
                                 : iisVersion;
            return serverSoftware;
        }

        private static NameValueCollection CopyCollection(HttpCookieCollection cookies)
        {
            if (cookies == null)
            {
                throw new ArgumentNullException("cookies");
            }
            var copy = new NameValueCollection(cookies.Count);
            for (int i = 0; i < cookies.Count; i++)
            {
                HttpCookie cookie = cookies[i];
                copy.Add(cookie.Name, cookie.Value);
            }
            return copy;
        }

        public override int Order
        {
            get { return 3; }
        }

        public override string Name
        {
            get { return "Http Appender"; }
        }
    }
}
