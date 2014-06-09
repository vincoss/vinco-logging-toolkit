using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;

using Elmah.Everywhere.Utils;
using Elmah.Everywhere.Appenders;
using System.IO;


namespace Elmah.Everywhere
{
    [DebuggerDisplay("Message : {Message}")]
    public class ErrorInfo
    {
        private List<DetailInfo> _errorDetails;

        public ErrorInfo()
        {
        }

        public ErrorInfo(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            Id = Guid.NewGuid().ToString();
            Exception = exception;
            Exception baseException = Exception.GetBaseException();

            ErrorType = baseException.GetType().FullName;

#if !SILVERLIGHT
            Source = baseException.Source;
#endif
            Detail = exception.GetExceptionString();

            // Sometimes the message is empty string
            Message = string.IsNullOrWhiteSpace(baseException.Message) ? baseException.GetType().ToString() : baseException.Message;
            Date = DateTime.Now;
        }

        #region Public methods

        public void AddDetail(string detailName, string key, string value)
        {
            AddDetail(detailName, new Dictionary<string, string>());
            var detail = this.ErrorDetails.Single(x => string.Equals(x.Name, detailName, StringComparison.OrdinalIgnoreCase));
            detail.Items.Add(new KeyValuePair<string, string>(key, value));
        }

        public void AddDetail(string detailName, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            if (ErrorDetails.Any(x => string.Equals(x.Name, detailName, StringComparison.OrdinalIgnoreCase)) == false)
            {
                var detail = new DetailInfo(detailName, new List<KeyValuePair<string, string>>(pairs));
                _errorDetails.Add(detail);
            }
        }

        public string BuildMessage()
        {
            var sb = new StringBuilder();
            sb.Append(this.Detail);

            foreach (var detail in this.ErrorDetails)
            {
                if (sb.Length > 0)
                {
                    sb.Append(Constants.NEW_LINE);
                    sb.Append(Constants.NEW_LINE);
                }

                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0} {1}", Constants.HEADER_PREFIX, detail.Name));
                sb.AppendLine();

                foreach (var pair in detail.Items)
                {
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", pair.Key, pair.Value));
                }
            }
            return sb.ToString();
        }

        public void EnsureAppenders()
        {
            if (Appenders == null)
            {
                return;
            }
            var appenders = from x in Appenders
                            let a = Activator.CreateInstance(x) as BaseAppender
                            where a != null
                            orderby a.Order
                            select a;

            foreach (var appender in appenders)
            {
                try
                {
                    appender.Append(this);
                }
                catch (Exception ex)
                {
                    // If appender fails then add error details and attach the exeption instead.
                    this.AddDetail(appender.Name, "Message", "Appender failed to load exception details.");
                    this.AddDetail(appender.Name, "Error", ex.ToString());
                }
            }
        }

        public override string ToString()
        {
            return this.Message;
        }

        #endregion

        #region Public properties

        public string Id { get; internal set; }

        public string Host { get; set; }

        public string ErrorType { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string Detail { get; set; }

        public string ApplicationName { get; set; }

        public string User { get; set; }

        public int StatusCode { get; set; }

        public DateTime Date { get; set; }

        public Exception Exception { get; private set; }

        public IEnumerable<DetailInfo> ErrorDetails
        {
            get
            {
                if (_errorDetails == null)
                {
                    _errorDetails = new List<DetailInfo>();
                }
                return _errorDetails;
            }
        }

        public IEnumerable<Type> Appenders { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        #endregion
    }
}