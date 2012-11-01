using System;
using System.Linq;
using System.Diagnostics;
using System.Web;
using System.Collections.Specialized;


namespace Elmah.Everywhere
{
    public class ElmahErrorHelper
    {
        public void LogException(ErrorInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            Error error = ToError(info);
            LogInternal(error, null);
        }

        public void LogException(Exception exception, HttpContext context)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if(context == null)
            {
                throw new ArgumentNullException("context");
            }
            var args = new ExceptionFilterEventArgs(exception, context);
            this.OnFiltering(this, args);
            if (args.Dismissed)
            {
                return;
            }
            var error = new Error(exception, context);
            LogInternal(error, context);
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

        protected void OnFiltering(object sender, EventArgs e)
        {
            ExceptionFilterEventHandler filtering = this.Filtering;
            if (filtering != null)
            {
                filtering.Invoke(this, (ExceptionFilterEventArgs)e);
            }
        }

        protected void OnLogged(object sender, EventArgs e)
        {
            ErrorLoggedEventHandler logged = this.Logged;
            if (logged != null)
            {
                logged.Invoke(this, (ErrorLoggedEventArgs)e);
            }
        }

        public static Error ToError(ErrorInfo info)
        {
            var error = new Error
            {
                ApplicationName = info.ApplicationName,
                HostName = info.Host,
                Type = info.Type,
                Source = info.Source,
                Message = info.Message,
                Detail = info.BuildMessage(),
                Time = info.Date,
                User = info.User,
                StatusCode = info.StatusCode
            };

            CopyToCollections(info, error.Cookies, "Cookies");
            CopyToCollections(info, error.Form, "Form");
            CopyToCollections(info, error.QueryString, "QueryString");
            CopyToCollections(info, error.ServerVariables, "ServerVariables");

            return error;
        }

        private static void CopyToCollections(ErrorInfo info, NameValueCollection collection, string collectionName)
        {
            var detail = info.ErrorDetails.SingleOrDefault(x => x.Name == collectionName);
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