using System;
using System.Collections.Generic;
using Elmah.Everywhere.Appenders;

#if !SILVERLIGHT
using Elmah.Everywhere.Configuration;
#endif


namespace Elmah.Everywhere.Diagnostics
{
    public sealed class ExceptionHandler
    {
        private static ExceptionWritterBase _writter;
        private static ExceptionDefaults _parameters;
        private static IEnumerable<Type> _appenders;

        static ExceptionHandler()
        {
            IsEnabled = true;
        }

        #if !SILVERLIGHT

        public static void ConfigureFromConfigurationFile(ExceptionWritterBase writter, IEnumerable<Type> appenders = null)
        {
            var configuration = (EverywhereConfigurationSection)System.Configuration.ConfigurationManager.GetSection(EverywhereConfigurationSection.SECTION_KEY);
            if (configuration == null)
            {
                throw new InvalidOperationException(string.Format("Could not find [{0}] configuration section.", EverywhereConfigurationSection.SECTION_KEY));
            }
            Configure(writter, configuration, appenders);
        }

        public static void Configure(ExceptionWritterBase writter, EverywhereConfigurationSection section, IEnumerable<Type> appenders = null)
        {
            if (section == null)
            {
                throw new ArgumentNullException("section");
            }
            var parameters = new ExceptionDefaults
            {
                ApplicationName = section.ApplicationName,
                Host = section.Host,
                Token = section.Token,
                RemoteLogUri = section.RemoteLogUri
            };
            Configure(writter, parameters, appenders);
        }
#endif

        public static void Configure(ExceptionWritterBase writter, ExceptionDefaults parameters, IEnumerable<Type> appenders = null)
        {
            if (writter == null)
            {
                throw new ArgumentNullException("writter");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            _appenders = appenders;
            if (_appenders == null)
            {
                _appenders = AllAppenders();
            }
            var httpWriter = writter as HttpExceptionWritterBase;
            if (httpWriter != null && httpWriter.RequestUri == null)
            {
                httpWriter.RequestUri = new Uri(parameters.RemoteLogUri, UriKind.Absolute);
            }
            _writter = writter;
            _parameters = parameters;
            _writter.Completed += Writter_Completed;
        }

        public static ErrorInfo Report(Exception exception)
        {
            return Report(exception, null);
        }

        public static ErrorInfo Report(Exception exception, IDictionary<string, object> propeties)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            ErrorInfo error = null;
            if (IsEnabled)
            {
                error = new ErrorInfo(exception)
                {
                    ApplicationName = _parameters.ApplicationName,
                    Host = _parameters.Host,
                    Appenders = _appenders,
                    Properties = propeties
                };
                error.EnsureAppenders();
                _writter.Write(_parameters.Token, error);
            }
            return error;
        }

        public static IEnumerable<Type> AllAppenders()
        {
            return new List<Type>
                       {
                           typeof (PropertiesAppender),
                           typeof (DetailAppender),
                           #if !SILVERLIGHT
                           typeof (MemoryAppender),
                           typeof (HttpAppender),
                           #endif
                           typeof (AssemblyAppender),
                       };
        }

#if !SILVERLIGHT

        public static void Attach(AppDomain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            domain.UnhandledException += AppDomain_UnhandledException;
        }

        public static void Detach(AppDomain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            domain.UnhandledException -= AppDomain_UnhandledException;
        }

#endif

        #region Private methods

        private static void Writter_Completed(object sender, WritterEventArgs e)
        {
            var writter = sender as ExceptionWritterBase;
            if (writter == null)
            {
                return;
            }
#if !SILVERLIGHT
            System.Diagnostics.Trace.WriteLine(e.Error.BuildMessage());
            if (writter.Exception != null)
            {
                System.Diagnostics.Trace.WriteLine(writter.Exception.ToString());
            }
#endif
        }

        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Report((Exception) args.ExceptionObject, null);
        }

        #endregion

        public static bool IsEnabled { get; set; }
    }
}