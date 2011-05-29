using System;
using System.Web;
using Vinco.ElmahHandler.Handlers;
using Vinco.ElmahHandler.Diagnostics;
using System.Configuration;


namespace Vinco.ElmahHandler.Web
{
    public class ErrorHandlerConfiguratorModule : IHttpModule
    {
        private static bool _initialized;
        private static object _lockObject = new object();

        public void Init(HttpApplication context)
        {
            if (!_initialized)
            {
                lock (_lockObject)
                {
                    if (!_initialized)
                    {
                        HttpHandler handler = new HttpHandler
                        {
                            RequestUri = new Uri(GetValueFromConfig("ErrorHandlerRequestUri", true), UriKind.Absolute)
                        };
                        ExceptionHandler.SetWritter(new HttpExceptionWritter(handler));
                        ExceptionHandler.SetParameters(new ExceptionParameters
                        {
                            Token = null,
                            ApplicationName = GetValueFromConfig("ApplicationName", false),
                            Host = GetValueFromConfig("ErrorHandlerHost", false)
                        });
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
            if ((value == null || value.Length == 0) && throwIfMissing)
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