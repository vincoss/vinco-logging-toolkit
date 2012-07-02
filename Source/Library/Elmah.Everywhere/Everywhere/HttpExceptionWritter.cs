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
                var data = CreatePostData(token, error);
                var webClient = CreateWebClient();
                webClient.UploadString(RequestUri, "POST", data);
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
            finally
            {
                OnCompleted(EventArgs.Empty);
            }
        }
    }
}