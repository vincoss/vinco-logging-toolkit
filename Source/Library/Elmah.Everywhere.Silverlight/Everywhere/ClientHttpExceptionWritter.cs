using System;


namespace Elmah.Everywhere
{
    public class ClientHttpExceptionWritter : HttpExceptionWritterBase
    {
        public override void Write(string token, ErrorInfo error)
        {
            if(string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException("token");
            }
            if(error == null)
            {
                throw new ArgumentNullException("error");
            }
            try
            {
                var webClient = CreateWebClient();
                webClient.UploadStringCompleted += (s, e) =>
                                                       {
                                                           if (e.Error != null)
                                                           {
                                                               this.Exception = e.Error;
                                                           } 
                                                           OnCompleted(new WritterEventArgs(error));
                                                       };
                string postData = CreatePostData(token, error);
                webClient.UploadStringAsync(RequestUri, "POST", postData, null);
            }
            catch (Exception exception)
            {
                Exception = exception;
                OnCompleted(new WritterEventArgs(error));
            }
        }
    }
}
