using System;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Collections.Generic;


namespace Elmah.Everywhere.ServiceModel
{
    public class HttpErrorHandler : IErrorHandler
    {
        #region IErrorHandler Members

        public bool HandleError(Exception error)
        {
            return false;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error == null)
            {
                return;
            }
            Diagnostics.ExceptionHandler.Report(error, null);
        }

        #endregion
    }
}
