using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Globalization;


namespace Elmah.Everywhere.Appenders
{
    public class HttpAppender : BaseAppender
    {
        public override void Append(ErrorInfo errorInfo)
        {
            HttpContextBase httpContext = GetContext();

            if (httpContext != null)
            {
                var pairs = new Dictionary<string, string>();

                pairs.Add("Web server", GetWebServer());
                pairs.Add("Integrated pipeline", HttpRuntime.UsingIntegratedPipeline.ToString());

                errorInfo.AddDetail(this.Name, pairs);

                HttpRequestBase httpRequest = httpContext.Request;

                errorInfo.AddDetail("ServerVariables", ToDictionary(httpRequest.ServerVariables));
                errorInfo.AddDetail("QueryString", ToDictionary(httpRequest.QueryString));
                errorInfo.AddDetail("Form", ToDictionary(httpRequest.ServerVariables));
                errorInfo.AddDetail("Cookies", ToDictionary(CopyCollection(httpRequest.Cookies)));

                HttpException httpException = errorInfo.Exception as HttpException;
                if (httpException != null)
                {
                    errorInfo.StatusCode = httpException.GetHttpCode();
                }

                IPrincipal webUser = httpContext.User;
                if (webUser != null && webUser.Identity.Name.Length > 0)
                {
                    errorInfo.User = webUser.Identity.Name;
                }
            }

            if (string.IsNullOrWhiteSpace(errorInfo.User))
            {
                errorInfo.User = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", Environment.UserDomainName, Environment.UserName).Trim('\\');
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
