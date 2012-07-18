using System;
using System.Collections;

using Elmah.Everywhere.Diagnostics;


namespace Elmah.Everywhere
{
    /// <summary>
    /// Logs an error to a remote site.
    /// </summary>
    public sealed class HttpErrorLog : ErrorLog
    {
        private readonly IDictionary _config;

        public HttpErrorLog(IDictionary config)
        {
            if(config == null)
            {
                throw new ArgumentNullException("config");
            }
            _config = config;
        }

        public override ErrorLogEntry GetError(string id)
        {
            throw new NotSupportedException();
        }

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            throw new NotSupportedException();
        }

        public override string Log(Error error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }
            return ExceptionHandler.Report(error.Exception).Id;
        }

        /// <summary>
        /// Error log name. Logs an error to a remote site.
        /// </summary>
        public override string Name
        {
            get { return "Http Error Log"; }
        }
    }
}