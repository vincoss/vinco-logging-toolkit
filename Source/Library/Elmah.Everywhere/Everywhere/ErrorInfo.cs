using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Globalization;
using Elmah.Properties;


namespace Elmah.Everywhere
{
    // Add token
    // Add user

    public class ErrorInfo
    {
        private readonly Guid _id;
        private readonly Exception _exception;
        private readonly StringBuilder _sb = new StringBuilder();
        private IDictionary<string, object> _properties;

        public ErrorInfo()
        {
            _id = Guid.NewGuid();
        }

        public ErrorInfo(Exception exception, ExceptionDefaults defaults, IDictionary<string, object> propeties) : this()
        {
            if(exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if(propeties != null)
            {
                foreach (var propety in propeties)
                {
                    this.Properties.Add(propety.Key, propety.Value);
                }
            }
            _exception = exception;

            Token = defaults.Token;
            ApplicationName = defaults.ApplicationName;
            Host = defaults.Host;
            Type = exception.GetBaseException().GetType().FullName;

#if SILVERLIGHT
                Source = defaults.Host;
#else
            Source = exception.GetBaseException().Source;
#endif
            Message = exception.GetBaseException().Message;
            Date = DateTime.Now;
        }

     

        private static void AppendText(StringBuilder sb, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                sb.AppendLine(message);
            }
        }

        private static string GetValues(IDictionary exceptionData)
        {
            var sb = new StringBuilder();
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

        private static string GetValues(IDictionary<string, object> propertiesData)
        {
            var sb = new StringBuilder();
            if (propertiesData != null && propertiesData.Count > 0)
            {
                foreach (var key in propertiesData.Keys)
                {
                    sb.AppendLine();
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}: {1}", key, propertiesData[key]);
                }
            }
            return sb.ToString();
        } 

        public Guid Id
        {
            get { return _id; }
        }

        public string Token { get; set; }

        public string ApplicationName { get; set; }

        public string Host { get; set; }

        public string Type { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string Error { get; set; }

        public string User { get; set; }

        public int StatusCode { get; set; }

        public DateTime Date { get; set; }

        public Exception Exception
        {
            get { return _exception; }
        }

        public IDictionary<string, object> Properties
        {
            get { return _properties ?? (_properties = new Dictionary<string, object>()); }
        }

        internal void EnsureErrorDetails()
        {
            DumpHelper.WithException(this.Exception, _sb);
            DumpHelper.WithExceptionData(Exception.Data, _sb);
            DumpHelper.WithProperties(Properties, _sb);
            DumpHelper.WithDetail(_sb);
            DumpHelper.WithMemory(_sb);
            DumpHelper.WithAssembly(_sb);
            Error = _sb.ToString();
        }
    }
}