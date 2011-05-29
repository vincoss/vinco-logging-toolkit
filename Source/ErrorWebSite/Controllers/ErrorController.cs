using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;

using System.Collections.Specialized;
using System.Text.RegularExpressions;

using Vinco.ElmahHandler;


namespace WebSite.Controllers
{
    // TODO: 
    // Validate mandatory properties
    // Encrypt and encode data

    public class ErrorController : Controller
    {
        [HttpPost]
        public ActionResult Put(string token, string applicationName, string host, string type, string source, string message, string error, DateTime? date)
        {
            if (TokenValid(token))
            {
                dynamic errorInfo = new ExpandoObject();
                errorInfo.ApplicationName = applicationName;
                errorInfo.HostName = host;
                errorInfo.Type = type;
                errorInfo.Source = source;
                errorInfo.Message = message;
                errorInfo.Detail = error;
                errorInfo.Time = date ?? DateTime.Now;

                ElmahErrorHelper helper = new ElmahErrorHelper();
                helper.LogException(errorInfo, null);

                return new HttpStatusCodeResult(200);
            }
            return new HttpStatusCodeResult(403);
        }

        private bool TokenValid(string token)
        {
            return true;
        }
    }
}