using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System;


namespace Elmah.Everywhere.Web
{
    public class ElmahResult : ActionResult
    {
        private readonly string _resouceType;

        public ElmahResult(string resouceType)
        {
            _resouceType = resouceType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;
            var path = request.Path;
            var queryString = request.QueryString.ToString();

            if (string.IsNullOrWhiteSpace(_resouceType) == false)
            {
                string resourcePath = GetResourcePath(httpContext);
                string pathInfo = string.Format(CultureInfo.InvariantCulture, "/{0}", _resouceType);
                httpContext.RewritePath(resourcePath, pathInfo, queryString);
            }
            else
            {
                if (path.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    const string CON = "elmah";
                    var newPath = path.Remove(path.Length - 1);
                    if(newPath.EndsWith(CON, StringComparison.OrdinalIgnoreCase) == false)
                    {
                        newPath = CON;
                    }
                    httpContext.RewritePath(newPath, null, queryString);
                }
            }
            ProcessRequest(httpContext);
        }

        protected virtual void ProcessRequest(HttpContextBase httpContext)
        {
            var unwrappedHttpContext = httpContext.ApplicationInstance.Context;
            var handler = new ErrorLogPageFactory().GetHandler(unwrappedHttpContext, null, null, null);
            handler.ProcessRequest(unwrappedHttpContext);
        }

        private string GetResourcePath(HttpContextBase httpContext)
        {
            return _resouceType != "stylesheet" ? httpContext.Request.Path.Replace(string.Format(CultureInfo.InvariantCulture, "/{0}", _resouceType), string.Empty) : httpContext.Request.Path;
        }
    }
}
