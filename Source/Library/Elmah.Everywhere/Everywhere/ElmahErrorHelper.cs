using System;
using System.Diagnostics;
using System.Web;


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

        public void LogException(Exception exception, HttpContext context)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            ExceptionFilterEventArgs args = new ExceptionFilterEventArgs(exception, context);
            this.OnFiltering(args);
            if (!args.Dismissed)
            {
                Error error = new Error(exception, context);
                LogInternal(error, context);
            }
        }

        protected virtual void LogInternal(Error error, object context)
        {
            ErrorLogEntry entry = null;
            try
            {
                ErrorLog errorLog = ErrorLog.GetDefault((HttpContext)context); 
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
                    this.OnLogged(new ErrorLoggedEventArgs(entry));
                }
            }
        }

        protected void OnFiltering(ExceptionFilterEventArgs args)
        {
            ExceptionFilterEventHandler filtering = this.Filtering;
            if (filtering != null)
            {
                filtering.Invoke(this, args);
            }
        }

        protected void OnLogged(ErrorLoggedEventArgs args)
        {
            ErrorLoggedEventHandler logged = this.Logged;
            if (logged != null)
            {
                logged.Invoke(this, args);
            }
        }

        private static Error ToError(ErrorInfo properties)
        {
            return new Error
            {
                ApplicationName =properties.ApplicationName,
                HostName = properties.Host,
                Type = properties.Type,
                Source = properties.Source,
                Message = properties.Message,
                Detail = properties.Error,
                Time = properties.Date
            };
        }

        public event ErrorLoggedEventHandler Logged;

        public event ExceptionFilterEventHandler Filtering;

    }
}