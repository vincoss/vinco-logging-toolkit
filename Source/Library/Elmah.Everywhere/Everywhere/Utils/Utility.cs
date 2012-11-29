using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Globalization;


namespace Elmah.Everywhere.Utils
{
    public static class Utility
    {
        public static string SerializeXml(ErrorInfo info)
        {
            if(info == null)
            {
                throw new ArgumentNullException("info");
            }
            XElement element = new XElement("Error");
            element.Add(CreateAttribute("Id", info.Id));
            element.Add(CreateAttribute("Host",  info.Host));
            element.Add(CreateAttribute("Type", info.ErrorType));
            element.Add(CreateAttribute("Source", info.Source));
            element.Add(CreateAttribute("Message", info.Message));
            element.Add(CreateAttribute("Detail", info.Detail));
            element.Add(CreateAttribute("ApplicationName", info.ApplicationName));
            element.Add(CreateAttribute("User", info.User));
            element.Add(CreateAttribute("StatusCode", info.StatusCode));
            element.Add(CreateAttribute("Date", info.Date));

            SerializeDetailToXml(element, info.ErrorDetails);

            return element.ToString();
        }

        public static ErrorInfo DeserializeXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentNullException("xml");
            }
            XElement element = XElement.Parse(xml);

            ErrorInfo info = new ErrorInfo();
            info.Id = GetAttributeValue(element, "Id");
            info.Host = GetAttributeValue(element, "Host");
            info.ErrorType = GetAttributeValue(element, "Type");
            info.Source = GetAttributeValue(element, "Source");
            info.Message = GetAttributeValue(element, "Message");
            info.Detail = GetAttributeValue(element, "Detail");
            info.ApplicationName = GetAttributeValue(element, "ApplicationName");
            info.User = GetAttributeValue(element, "User");
            info.StatusCode = Int32.Parse(GetAttributeValue(element, "StatusCode"), CultureInfo.InvariantCulture);
            info.Date = DateTime.Parse(GetAttributeValue(element, "Date"), CultureInfo.InvariantCulture);

            DeserializeXmlToDetail(info, element.Element("Details"));

            return info;
        }

        /// <summary>
        /// Gets exception detail including inner exceptions.
        /// </summary>
        /// <param name="exception">Exception to get detail.</param>
        /// <returns>Exception detail.</returns>
        public static string GetExceptionString(this Exception exception)
        {
            var sb = new StringBuilder();
            if (exception != null)
            {
                string className;
                string message = exception.Message;
                if (string.IsNullOrWhiteSpace(message))
                {
                    className = exception.GetType().Name;
                }
                else
                {
                    className = exception.GetType() + ": " + message;
                }
                sb.Append(className);
                if (exception.InnerException != null)
                {
                    sb.Append(" ---> " + exception.InnerException.GetExceptionString() + Environment.NewLine + "   ");
                    sb.Append("--- End of inner exception stack trace ---");
                }
                string stackTrace = exception.StackTrace;
                if (stackTrace != null)
                {
                    sb.AppendLine();
                    sb.Append(stackTrace);
                }
                sb.AppendLine();
                sb.AppendLine();
                WithExceptionData(exception.Data, sb);
            }
            return sb.ToString();
        }

        #region Private methods

        private static void DeserializeXmlToDetail(ErrorInfo info, XElement element)
        {
            foreach (var detail in element.Elements())
            {
                var pairs = new List<KeyValuePair<string, string>>();
                foreach (var item in detail.Elements())
                {
                    var pair = new KeyValuePair<string, string>(GetAttributeValue(item, "Name"), GetAttributeValue(item, "Value"));
                    pairs.Add(pair);
                }
                info.AddDetail(GetAttributeValue(detail, "Name"), pairs);
            }
        }

        private static void SerializeDetailToXml(XElement parent, IEnumerable<DetailInfo> items)
        {
            var element = new XElement("Details");
            foreach (var item in items)
            {
                var detail = new XElement("Detail");

                detail.Add(CreateAttribute("Name", item.Name));

                foreach (var pair in item.Items)
                {
                    XElement e = new XElement("Item");
                    e.Add(CreateAttribute("Name", pair.Key));
                    e.Add(CreateAttribute("Value", pair.Value));
                    detail.Add(e);
                }
                element.Add(detail);
            }
            parent.Add(element);
        }

        private static XAttribute CreateAttribute(string name, object value)
        {
            return new XAttribute(name, value ?? "");
        }

        private static string GetAttributeValue(XElement element, string name)
        {
            string value = "";
            XAttribute attribute = element.Attribute(name);
            if (attribute != null)
            {
                value = attribute.Value;
            }
            return value;
        }

        private static void WithExceptionData(IDictionary exceptionData, StringBuilder sb)
        {
            sb.AppendLine("Exception Data");
            if (exceptionData != null && exceptionData.Count > 0)
            {
                foreach (var key in exceptionData.Keys)
                {
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", key, exceptionData[key]));
                }
            }
        } 

        #endregion
    }
}
