using System;
using System.Linq;
using System.Diagnostics;
using System.Web;
using System.Collections.Specialized;


namespace Elmah.Everywhere
{
    public class ElmahErrorHelper
    {
        public void LogException(ErrorInfo errorInfo)
        {
            if (errorInfo == null)
            {
                throw new ArgumentNullException("errorInfo");
            }
            Error error = ToError(errorInfo);
            LogInternal(error, null);
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

        protected virtual void LogInternal(Error error, object context)
        {
            ErrorLogEntry entry = null;
            try
            {
                var errorLog = ErrorLog.GetDefault((HttpContext)context); 
                string errorId = errorLog.Log(error);
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
        }

        private void OnFiltering(object sender, EventArgs e)
        {
            ExceptionFilterEventHandler filtering = this.Filtering;
            if (filtering != null)
            {
                filtering.Invoke(this, (ExceptionFilterEventArgs)e);
            }
        }

        private void OnLogged(object sender, EventArgs e)
        {
            ErrorLoggedEventHandler logged = this.Logged;
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

        public event ErrorLoggedEventHandler Logged;

        public event ExceptionFilterEventHandler Filtering;

    }
}