using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Elmah.Properties;


namespace Elmah.Everywhere
{
    public abstract class ExceptionWritterBase
    {
        protected abstract void Write(ErrorInfo error);

        public void Report(Exception exception, ExceptionDefaults defaults, IDictionary<string, object> propeties, string dumpReport)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (defaults == null)
            {
                throw new ArgumentNullException("defaults");
            }
            if (propeties == null)
            {
                throw new ArgumentNullException("propeties");
            }

            var sb = new StringBuilder();
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
            AppendText(sb, dumpReport);

            var data = new ErrorInfo()
            {
                Token = defaults.Token,
                ApplicationName = defaults.ApplicationName,
                Host = defaults.Host,
                Type = baseException.GetType().FullName,

#if SILVERLIGHT
                Source = defaults.Host,
#else
                Source = baseException.Source,
#endif
                Message = baseException.Message,
                Error = sb.ToString(),
                Date = DateTime.Now,
            };
            this.Write(data);
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
    }
}
