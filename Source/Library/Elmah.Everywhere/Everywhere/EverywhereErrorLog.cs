using System;
using System.Collections;

using Elmah.Everywhere.Diagnostics;


namespace Elmah.Everywhere
{
    // TODO: Possible to pass some config parameters.

    public sealed class EverywhereErrorLog : ErrorLog
    {
        private readonly IDictionary _config;

        public EverywhereErrorLog(IDictionary config)
        {
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
            ErrorInfo info = ElmahErrorHelper.ToInfo(error);
            ExceptionHandler.Report(info);
            return info.Id.ToString();
        }
    }
}