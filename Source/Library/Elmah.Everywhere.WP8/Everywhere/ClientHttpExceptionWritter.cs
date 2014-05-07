using Elmah.Everywhere.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Elmah.Everywhere
{
    public class ClientHttpExceptionWritter : HttpExceptionWritterBase
    {
        public override void Write(string token, ErrorInfo error)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException("token");
            }
            if (error == null)
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

        protected override string CreatePostData(string token, ErrorInfo info)
        {
            string xml = Utility.SerializeXml(info);
            string error = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
            return FormData(new { token, error }, HttpUtility.UrlEncode);
        }
    }
}
