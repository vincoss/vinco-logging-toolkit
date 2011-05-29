using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Collections;


namespace Vinco.ElmahHandler.Handlers
{
    public class HttpExceptionWritter : ExceptionWritter
    {
        private readonly HttpHandler _handler;

        public HttpExceptionWritter(HttpHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            _handler = handler;
        }

        public override void Write(Exception exception, ExceptionParameters parameters, IDictionary<string, object> propeties)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            Exception baseException = exception.GetBaseException();

            string exceptionString = baseException.ToString();
            string propertiesData = GetPropertiesData(propeties);
            string exceptionData = GetExceptionData(exception.Data);
            string exceptionErrorText = AggregateErrorText(exceptionString, propertiesData, exceptionData);

            var data = new
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
                Date = DateTime.Now,
            };
            string postData = FormHelper.FormData(data);
            _handler.Post(postData);
        }

        private static string AggregateErrorText(params string[] items)
        {
            if (items == null || items.Length == 0)
            {
                throw new ArgumentNullException("items");
            }
            StringBuilder sb = new StringBuilder();
            foreach (string str in items)
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    sb.AppendLine(str);
                }
            }
            return sb.ToString();
        }

        private static string GetExceptionData(IDictionary exceptionData)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.Append("Exception-Dump-Report:");

            if (exceptionData != null && exceptionData.Count > 0)
            {
                foreach (var key in exceptionData.Keys)
                {
                    sb.AppendLine();
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}: {1}", key, exceptionData[key]);
                }
            }
            return sb.ToString();
        }

        private static string GetPropertiesData(IDictionary<string, object> exceptionData)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.Append("Properties-Dump-Report:");

            if (exceptionData != null && exceptionData.Count > 0)
            {
                foreach (var key in exceptionData.Keys)
                {
                    sb.AppendLine();
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}: {1}", key, exceptionData[key]);
                }
            }
            return sb.ToString();
        }
    }
}
