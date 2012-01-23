using System;
using System.Collections.Generic;
using System.Text;


namespace Elmah.Everywhere
{
    public class ErrorInfo
    {
        private readonly Guid _id;
        private readonly Exception _exception;
        private readonly StringBuilder _sb;
        private IDictionary<string, object> _properties;

        public ErrorInfo()
        {
            _id = Guid.NewGuid();
            _sb = new StringBuilder();
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
            Exception baseException = exception.GetBaseException();

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
            Date = DateTime.Now;
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