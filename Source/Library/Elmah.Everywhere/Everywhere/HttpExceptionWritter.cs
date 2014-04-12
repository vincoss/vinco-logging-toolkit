using Elmah.Everywhere.Utils;
using System;
using System.Net;
using System.Text;
using System.Web;


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
               var r = webClient.UploadString(RequestUri, "POST", data);
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