using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;


namespace Elmah.Everywhere.Handlers
{
    public abstract class ExceptionWritterBase
    {
        public abstract void Write(Exception exception, ExceptionDefaults parameters, IDictionary<string, object> propeties);

        protected static string AggregateErrorText(params string[] items)
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

        protected static string GetExceptionData(IDictionary exceptionData)
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

        protected static string GetPropertiesData(IDictionary<string, object> exceptionData)
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
