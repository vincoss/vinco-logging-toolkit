using System;


namespace Elmah.Everywhere
{
    public class HttpExceptionWritter : HttpExceptionWritterBase
    {
        public override void Write(string token, ErrorInfo error)
        {
            if(string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException("token");
            }
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }
            try
            {
                WriteInternal(CreatePostData(token, error));
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
            finally
            {
                OnCompleted(new WritterEventArgs(error));
            }
        }

        protected virtual void WriteInternal(string data)
        {
            var webClient = CreateWebClient();
            webClient.UploadString(RequestUri, "POST", data);
        }
    }
}