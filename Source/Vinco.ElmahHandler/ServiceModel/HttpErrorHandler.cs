using System;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Collections.Generic;

using Vinco.ElmahHandler.Handlers;


namespace Vinco.ElmahHandler.ServiceModel
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
            Vinco.ElmahHandler.Diagnostics.ExceptionHandler.Report(error, null);
        }

        #endregion
    }
}
