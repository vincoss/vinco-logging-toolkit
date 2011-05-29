using System;
using System.Net;
using System.IO;
using System.Text;


namespace Vinco.ElmahHandler.Handlers
{
    public class HttpHandler
    {
        public void Post(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException("message");
            }
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(WebClient_UploadStringCompleted);
                webClient.UploadStringAsync(RequestUri, "Post", message, null);
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