using System;
using System.Net;


namespace Elmah.Everywhere
{
    public class HttpExceptionWritter : HttpExceptionWritterBase
    {
        public override void Write(string token, ErrorInfo errorInfo)
        {
            if(string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException("token");
            }
            if (errorInfo == null)
            {
                throw new ArgumentNullException("errorInfo");
            }
            try
            {
                WriteInternal(CreatePostData(token, errorInfo));
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
            finally
            {
                OnCompleted(new WritterEventArgs(errorInfo));
            }
        }

        protected virtual void WriteInternal(string data)
        {
            using (var webClient = CreateWebClient())
            {
                webClient.UploadString(RequestUri, "POST", data);
            }
        }
    }
}