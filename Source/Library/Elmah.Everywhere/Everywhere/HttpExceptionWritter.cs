using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

#if SILVERLIGHT
using System.Windows.Browser;
#else
using System.Web;
#endif


namespace Elmah.Everywhere
{
    public class HttpExceptionWritter : ExceptionWritterBase
    {
        public override void Write(ErrorInfo error)
        {
            string postData = FormData(error);
            if (string.IsNullOrWhiteSpace(postData))
            {
                throw new ArgumentNullException("error");
            }
            try
            {
#if SILVERLIGHT
                InternalWriteAsync(postData);
#else
                WebClient webClient = CreateWebClient();
                string end = webClient.UploadString(RequestUri, postData); // TODO: response check
#endif
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }

        private void InternalWriteAsync(string data)
        {
            WebClient webClient = CreateWebClient();
            webClient.UploadStringCompleted += WebClient_UploadStringCompleted;
            webClient.UploadStringAsync(RequestUri, "POST", data, null);
        }

        protected virtual WebClient CreateWebClient()
        {
            WebClient webClient = new WebClient();
            webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            return webClient;
        }

        private void WebClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Exception = e.Error;
            }
        }

        public static string FormData(object formValues)
        {
            if (formValues != null)
            {
                IDictionary<string, object> result = AnonymousObjectToFormValues(formValues);
                if (result.Any())
                {
                    var sb = new StringBuilder();
                    foreach (var pair in result)
                    {
                        if ((sb.Length > 0) && (sb[sb.Length - 1] != '&'))
                        {
                            sb.Append("&");
                        }
                        sb.Append(pair.Key);
                        sb.Append("=");

                        if (pair.Value != null)
                        {
                            string value = pair.Value.ToString();
                            sb.Append(HttpUtility.UrlEncode(value));
                        }
                    }
                    return sb.ToString();
                }
            }
            return string.Empty;
        }

        private static IDictionary<string, object> AnonymousObjectToFormValues(object formValues)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            if (formValues != null)
            {
                PropertyInfo[] properties =
                    formValues.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public |
                                                       BindingFlags.Instance);
                foreach (var p in properties)
                {
                    result.Add(p.Name, p.GetValue(formValues, null));
                }
            }
            return result;
        }

        #region Properties

        public Uri RequestUri { get; set; }

        public Exception Exception { get; private set; }

        #endregion
    }
}