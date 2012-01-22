using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Globalization;
using Elmah.Properties;


namespace Elmah.Everywhere
{
    public class ErrorInfo
    {
        private readonly Guid _id;
        private readonly Exception _exception;
        private readonly StringBuilder sb = new StringBuilder();

        public ErrorInfo()
        {
            _id = Guid.NewGuid();
        }

        public void AppendReport(string report)
        {
            if(string.IsNullOrWhiteSpace(report))
            {
                throw new ArgumentNullException("report");
            }

            AppendText(sb, this.Error);
            sb.AppendLine();

            // Dump report
            sb.AppendLine(Strings.Dump_Report);
            AppendText(sb, report);

            this.Error = sb.ToString();
        }

        public ErrorInfo(Exception exception, ExceptionDefaults defaults, IDictionary<string, object> propeties, string report) : this()
        {
            if(exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (propeties == null)
            {
                throw new ArgumentNullException("propeties");
            }
            _exception = exception;

            Exception baseException = exception.GetBaseException();
            AppendText(sb, baseException.ToString());

            // Exception data
            AppendText(sb, GetValues(exception.Data));
            sb.AppendLine();

            // Exception properties
            AppendText(sb, GetValues(propeties));
            sb.AppendLine();

            // Dump report
            sb.AppendLine(Strings.Dump_Report);
            AppendText(sb, report);

            Token = defaults.Token;
            ApplicationName = defaults.ApplicationName;
            Host = defaults.Host;
            Type = baseException.GetType().FullName;

#if SILVERLIGHT
                Source = defaults.Host;
#else
            Source = baseException.Source;
#endif
            Message = baseException.Message;
            Error = sb.ToString();
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
    }
}