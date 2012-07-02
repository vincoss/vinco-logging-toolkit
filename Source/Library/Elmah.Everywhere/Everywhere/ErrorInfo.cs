using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;

using Elmah.Everywhere.Utils;
using Elmah.Everywhere.Appenders;


namespace Elmah.Everywhere
{
    [DebuggerDisplay("Message : {Message}")]
    public class ErrorInfo
    {
        private string _id;
        private List<DetailInfo> _details;

        public ErrorInfo()
        {
        }

        public ErrorInfo(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            _id = Guid.NewGuid().ToString();
            Exception = exception;
            Exception baseException = Exception.GetBaseException();

            Type = baseException.GetType().FullName;

#if !SILVERLIGHT
            Source = baseException.Source;
#endif
            Detail = exception.GetExceptionString();
            Message = baseException.Message;
            Date = DateTime.Now;
        }

        #region Public methods

        public void AddDetail(string detailName, string key, string value)
        {
            AddDetail(detailName, new Dictionary<string, string>());
            var detail = this.Details.Single(x => string.Equals(x.Name, detailName, StringComparison.OrdinalIgnoreCase));
            detail.Items.Add(new KeyValuePair<string, string>(key, value));
        }

        public void AddDetail(string detailName, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            if (Details.Any(x => string.Equals(x.Name, detailName, StringComparison.OrdinalIgnoreCase)) == false)
            {
                var detail = new DetailInfo(detailName, new List<KeyValuePair<string, string>>(pairs));
                _details.Add(detail);
            }
        }

        public string BuildMessage()
        {
            var sb = new StringBuilder();
            sb.Append(this.Detail);

            foreach (var detail in this.Details)
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

        public override string ToString()
        {
            return this.Message;
        }

        #endregion

        #region Private methods

        private void EnsureAppenders()
        {
            if (Appenders == null)
            {
                return;
            }
            var appenders = (from x in Appenders
                             let a = Activator.CreateInstance(x) as BaseAppender
                             where a != null
                             orderby a.Order
                             select a).ToList();

            foreach (var appender in appenders)
            {
                try
                {
                    appender.Append(this);
                }
                catch (Exception ex)
                {
#if !SILVERLIGHT
                    Trace.WriteLine(ex); // TODO:
#endif
                }
            }
        }

        #endregion

        #region Public properties

        public string Id
        {
            get
            {
                EnsureAppenders();
                return _id;
            }
            internal set { _id = value; }
        }

        public string Host { get; set; }

        public string Type { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string Detail { get; set; }

        public string ApplicationName { get; set; }

        public string User { get; set; }

        public int StatusCode { get; set; }

        public DateTime Date { get; set; }

        public Exception Exception { get; private set; }

        public IEnumerable<DetailInfo> Details
        {
            get { return _details ?? (_details = new List<DetailInfo>()); }
        }

        public IEnumerable<Type> Appenders { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        #endregion

        #region Nested types

        [DebuggerDisplay("Name : {Name}")]
        public class DetailInfo
        {
            public DetailInfo(string name, IList<KeyValuePair<string, string>> pairs)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentNullException("name");
                }
                if (pairs == null)
                {
                    throw new ArgumentNullException("pairs");
                }
                Name = name;
                Items = pairs;
            }

            public string Name { get; private set; }
            public IList<KeyValuePair<string, string>> Items { get; private set; }
        }

        #endregion
    }
}