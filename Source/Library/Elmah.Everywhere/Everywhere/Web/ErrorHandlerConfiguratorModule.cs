using System;
using System.Web;
using System.Configuration;
using Elmah.Everywhere.Diagnostics;


namespace Elmah.Everywhere.Web
{
    public class ErrorHandlerConfiguratorModule : IHttpModule
    {
        private static bool _initialized;
        private readonly object _lockObject = new object();

        public void Init(HttpApplication context)
        {
            if (!_initialized)
            {
                lock (_lockObject)
                {
                    if (!_initialized)
                    {
                        var writter = new HttpExceptionWritter
                        {
                            RequestUri = new Uri(GetValueFromConfig("ErrorHandlerRequestUri", true), UriKind.Absolute)
                        };

                        var defaults = new ExceptionDefaults
                        {
                            Token = GetValueFromConfig("Token", false),
                            ApplicationName = GetValueFromConfig("ApplicationName", false),
                            Host = GetValueFromConfig("ErrorHandlerHost", false)
                        };
                        ExceptionHandler.WithParameters(defaults, writter);
                        _initialized = true;
                    }
                }
            }
        }

        private static string GetValueFromConfig(string propertyName, bool throwIfMissing)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            propertyName = propertyName.Trim(new char[] { ' ' });
            if (propertyName.Length == 0)
            {
                throw new ArgumentNullException("propertyName");
            }
            string value = ConfigurationManager.AppSettings[propertyName];
            if (string.IsNullOrWhiteSpace(value) && throwIfMissing)
            {
                throw new ArgumentNullException(string.Format("Missing_Configuration_Property_Value : {0}", new object[] { propertyName }));
            }
            return value;
        }
        
        public void Dispose()
        {
        }
    }
}