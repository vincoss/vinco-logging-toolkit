using System;
using Elmah;
using System.Diagnostics;
using System.Web;
using System.Collections.Specialized;


namespace Vinco.ElmahHandler
{
    public class ElmahErrorHelper
    {
        public event ErrorLoggedEventHandler Logged;
        public event ExceptionFilterEventHandler Filtering;

        protected virtual ErrorLog GetErrorLog(HttpContext context)
        {
            return ErrorLog.GetDefault(context);
        }

        public virtual void LogException(dynamic errorProperties, HttpContext context)
        {
            if (errorProperties == null)
            {
                throw new ArgumentNullException("errorProperties");
            }
            ErrorLogEntry entry = null;
            try
            {
                Error error = ToError(errorProperties);
                ErrorLog errorLog = this.GetErrorLog(context);
                string str = errorLog.Log(error);
                entry = new ErrorLogEntry(errorLog, str, error);
            }
            catch (Exception exception2)
            {
                Trace.WriteLine(exception2);
            }
            if (entry != null)
            {
                this.OnLogged(new ErrorLoggedEventArgs(entry));
            }
        }

        public virtual void LogException(Exception exception, HttpContext context)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            ExceptionFilterEventArgs args = new ExceptionFilterEventArgs(exception, context);
            this.OnFiltering(args);
            if (!args.Dismissed)
            {
                ErrorLogEntry entry = null;
                try
                {
                    Error error = new Error(exception, context);
                    ErrorLog errorLog = this.GetErrorLog(context);
                    string str = errorLog.Log(error);
                    entry = new ErrorLogEntry(errorLog, str, error);
                }
                catch (Exception exception2)
                {
                    Trace.WriteLine(exception2);
                }
                if (entry != null)
                {
                    this.OnLogged(new ErrorLoggedEventArgs(entry));
                }
            }
        }

        protected virtual void OnFiltering(ExceptionFilterEventArgs args)
        {
            ExceptionFilterEventHandler filtering = this.Filtering;
            if (filtering != null)
            {
                filtering.Invoke(this, args);
            }
        }

        protected virtual void OnLogged(ErrorLoggedEventArgs args)
        {
            ErrorLoggedEventHandler logged = this.Logged;
            if (logged != null)
            {
                logged.Invoke(this, args);
            }
        }

        private Error ToError(dynamic properties)
        {
            Error error = new Error();
            error.ApplicationName = properties.ApplicationName;
            error.HostName = properties.HostName;
            error.Type = properties.Type;
            error.Source = properties.Source;
            error.Message = properties.Message;
            error.Detail = properties.Detail;
            error.Time = properties.Time;
            return error;
        }
    }
}