using System;
using System.Collections.Generic;
using Elmah.Everywhere.Appenders;
using System.Diagnostics;
#if !SILVERLIGHT
using Elmah.Everywhere.Configuration;
#endif
using System.Configuration;
using System.IO;


namespace Elmah.Everywhere.Diagnostics
{
    // NOTE: Report error should be async

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

        // Can't use with Silverlight because throws method access exception.

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

        public static void WithParameters(ExceptionDefaults parameters, ExceptionWritterBase writter,
                                          IEnumerable<Type> appenders = null)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if (writter == null)
            {
                throw new ArgumentNullException("writter");
            }
            _appenders = appenders;
            if (_appenders == null)
            {
                _appenders = AllAppenders();
            }
            _parameters = parameters;
            _writter = writter;
        }

#if !SILVERLIGHT
        public static void WithParameters(ExceptionWritterBase writter, IEnumerable<Type> appenders = null)
        {
            var configuration =
                (EverywhereConfigurationSection)
                ConfigurationManager.GetSection(EverywhereConfigurationSection.SECTION_KEY);
            if (configuration == null)
            {
                throw new InvalidOperationException(string.Format("Could not find [{0}] configuration section.",
                                                                  EverywhereConfigurationSection.SECTION_KEY));
            }
            var parameters = new ExceptionDefaults
                                 {
                                     ApplicationName = configuration.ApplicationName,
                                     Host = configuration.Host,
                                     Token = configuration.Token
                                 };
            var httpWriter = writter as HttpExceptionWritter;
            if (httpWriter != null)
            {
                httpWriter.RequestUri = new Uri(configuration.RemoteLogUri, UriKind.Absolute);
            }
            WithParameters(parameters, writter);
        }
#endif

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
                _writter.Completed += Writter_Completed;


                string xml = Utils.Utility.SerializeXml(error);
                Write(xml);
                _writter.Write(_parameters.Token, error);
            }
            return error;
        }

        private static void Writter_Completed(object sender, EventArgs e)
        {
            var writter = sender as ExceptionWritterBase;
            if (writter != null && writter.Exception != null)
            {
#if !SILVERLIGHT
                Write(writter.Exception.ToString());
               //Trace.WriteLine(writter.Exception);
#endif
            }
        }

        public static ErrorInfo Report(Exception exception)
        {
            return Report(exception, null);
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
                           typeof (AssemblyAppender),
#endif
                       };
        }

        #region Private methods

        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Report((Exception) args.ExceptionObject, null);
        }

        #endregion

        public static bool IsEnabled { get; set; }

        // TODO: remove this
        private static void Write(string text)
        {
            var configuration =
               (EverywhereConfigurationSection)
               ConfigurationManager.GetSection(EverywhereConfigurationSection.SECTION_KEY);
            if (configuration == null)
            {
                throw new InvalidOperationException(string.Format("Could not find [{0}] configuration section.",
                                                                  EverywhereConfigurationSection.SECTION_KEY));
            }


            
            var writer = new StreamWriter(new FileStream(configuration.FileLogPath, FileMode.Append, FileAccess.Write, FileShare.Read), System.Text.Encoding.UTF8);
            writer.Write(text);
            
        }
    }
}