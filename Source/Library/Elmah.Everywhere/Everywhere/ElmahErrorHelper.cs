using System;
using System.Linq;
using System.Diagnostics;
using System.Web;
using System.Collections.Specialized;
using System.Reflection;


namespace Elmah.Everywhere
{
    public class ElmahErrorHelper
    {
        public string LogException(ErrorInfo errorInfo)
        {
            if (errorInfo == null)
            {
                throw new ArgumentNullException("errorInfo");
            }
            Error error = ToError(errorInfo);
            var errorId = LogInternal(error, null);
            return errorId;
        }

        public void LogException(Exception exception, HttpContext httpContext)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            var args = new ExceptionFilterEventArgs(exception, httpContext);
            this.OnFiltering(this, args);
            if (args.Dismissed)
            {
                return;
            }
            var error = new Error(exception, httpContext);
            LogInternal(error, httpContext);
        }

        protected virtual string LogInternal(Error error, object context)
        {
            string errorId = null;
            ErrorLogEntry entry = null;
            try
            {
                var errorLog = ErrorLog.GetDefault((HttpContext)context); 
                errorId = errorLog.Log(error);
                EmailHandler(error);
                entry = new ErrorLogEntry(errorLog, errorId, error);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                if (entry != null)
                {
                    this.OnLogged(this, new ErrorLoggedEventArgs(entry));
                }
            }
            return errorId;
        }

        private void OnFiltering(object sender, EventArgs e)
        {
            ExceptionFilterEventHandler filtering = this.FilteringEventArgs;
            if (filtering != null)
            {
                filtering.Invoke(this, (ExceptionFilterEventArgs)e);
            }
        }

        private void OnLogged(object sender, EventArgs e)
        {
            ErrorLoggedEventHandler logged = this.LoggedEventArgs;
            if (logged != null)
            {
                logged.Invoke(this, (ErrorLoggedEventArgs)e);
            }
        }

        public static Error ToError(ErrorInfo errorInfo)
        {
            var error = new Error
            {
                ApplicationName = errorInfo.ApplicationName,
                HostName = errorInfo.Host,
                Type = errorInfo.ErrorType,
                Source = errorInfo.Source,
                Message = errorInfo.Message,
                Detail = errorInfo.BuildMessage(),
                Time = errorInfo.Date,
                User = errorInfo.User,
                StatusCode = errorInfo.StatusCode
            };

            CopyToCollections(errorInfo, error.Cookies, "Cookies");
            CopyToCollections(errorInfo, error.Form, "Form");
            CopyToCollections(errorInfo, error.QueryString, "QueryString");
            CopyToCollections(errorInfo, error.ServerVariables, "ServerVariables");

            return error;
        }

        private static void CopyToCollections(ErrorInfo errorInfo, NameValueCollection collection, string collectionName)
        {
            var detail = errorInfo.ErrorDetails.SingleOrDefault(x => x.Name == collectionName);
            if(detail != null)
            {
                foreach (var pair in detail.Items)
                {
                    collection.Add(pair.Key, pair.Value);
                }
            }
        }

        public event ErrorLoggedEventHandler LoggedEventArgs;

        public event ExceptionFilterEventHandler FilteringEventArgs;

        protected virtual void EmailHandler(Error error)
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                return;
            }
            object module = httpContext.ApplicationInstance.Modules["ErrorMail"];
            if (module != null)
            {
                var method = module.GetType().GetMethod("ReportError", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(Error) }, null);
                if (method != null)
                {
                    method.Invoke(module, new[] { error });
                }
            }
        }
    }
}