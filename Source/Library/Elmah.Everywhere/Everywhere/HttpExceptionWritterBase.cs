using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Elmah.Everywhere.Utils;
using System.Diagnostics;


#if SILVERLIGHT
using System.Windows.Browser;
#else
using System.Web;
#endif

namespace Elmah.Everywhere
{
    public abstract class HttpExceptionWritterBase : ExceptionWritterBase
    {
        public static string FormData(object formValues, Func<string, string> encoder)
        {
            if (encoder == null)
            {
                throw new ArgumentNullException("encoder");
            }
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
                            sb.Append(encoder(value));
                        }
                    }
                    return sb.ToString();
                }
            }
            return string.Empty;
        }

        #region Private methods

        protected WebClient CreateWebClient()
        {
            var webClient = new WebClient();
            webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            return webClient;
        }

        protected string CreatePostData(string token, ErrorInfo info)
        {
            string xml = Utility.SerializeXml(info);
            string error = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
            return FormData(new { token, error }, HttpUtility.UrlEncode);
        }

        private static IDictionary<string, object> AnonymousObjectToFormValues(object formValues)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            if (formValues != null)
            {
                PropertyInfo[] properties = formValues.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                foreach (var p in properties)
                {
                    result.Add(p.Name, p.GetValue(formValues, null));
                }
            }
            return result;
        }

        #endregion

        public Uri RequestUri { get; set; }
    }
}
