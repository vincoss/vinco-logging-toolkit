using System;
using System.Web.Mvc;
using Elmah;
using System.Web;


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
            var httpHandlerFactory = new ErrorLogPageFactory();

            if (!string.IsNullOrEmpty(_resouceType))
            {
                string resourcePath = GetResourcePath(context);
                string pathInfo = string.Format("/{0}", _resouceType);
                context.HttpContext.RewritePath(resourcePath, pathInfo, context.HttpContext.Request.QueryString.ToString());
            }
            
            var application = (HttpApplication)context.HttpContext.GetService(typeof(HttpApplication));
            var httpContext = application.Context;

            var httpHandler = httpHandlerFactory.GetHandler(httpContext, null, null, null);
            if (httpHandler is IHttpAsyncHandler)
            {
                var asyncHttpHandler = (IHttpAsyncHandler)httpHandler;
                asyncHttpHandler.BeginProcessRequest(httpContext, (r) => { }, null);
            }
            else
            {
                httpHandler.ProcessRequest(httpContext);
            }
        }

        private string GetResourcePath(ControllerContext context)
        {
            return _resouceType != "stylesheet" ? context.HttpContext.Request.Path.Replace(String.Format("/{0}", _resouceType), string.Empty) : context.HttpContext.Request.Path;
        }
    }
}
