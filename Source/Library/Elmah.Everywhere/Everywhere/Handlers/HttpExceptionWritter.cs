using System;
using System.Net;
using System.Collections.Generic;


namespace Elmah.Everywhere.Handlers
{
    public class HttpExceptionWritter : ExceptionWritterBase
    {
        public override void Write(Exception exception, ExceptionDefaults parameters, IDictionary<string, object> propeties)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if(propeties == null)
            {
                throw new ArgumentNullException("propeties");
            }

            Exception baseException = exception.GetBaseException();

            string exceptionString = baseException.ToString();
            string propertiesData = GetPropertiesData(propeties);
            string exceptionData = GetExceptionData(exception.Data);
            string exceptionErrorText = AggregateErrorText(exceptionString, propertiesData, exceptionData);

            ErrorInfo data = new ErrorInfo()
                                 {
                                     Token = parameters.Token,
                                     ApplicationName = parameters.ApplicationName,
                                     Host = parameters.Host,
                                     Type = baseException.GetType().FullName,

#if SILVERLIGHT
                                                     Source = parameters.Host,
#else
                                     Source = baseException.Source,
#endif
                                     Message = baseException.Message,
                                     Error = exceptionErrorText,
                                     Date = DateTime.UtcNow,
                                 };
            string postData = FormHelper.FormData(data);
            this.Post(postData);
        }

        protected virtual void Post(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException("message");
            }
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                webClient.UploadStringCompleted += WebClient_UploadStringCompleted;
                webClient.UploadStringAsync(RequestUri, "POST", message, null);
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }

        private void WebClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Exception = e.Error;
            }
        }

        #region Properties

        public Uri RequestUri { get; set; }

        public Exception Exception { get; private set; }

        #endregion
    }
}