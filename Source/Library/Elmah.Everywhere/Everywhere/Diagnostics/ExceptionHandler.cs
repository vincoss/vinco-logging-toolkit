using System;
using System.Collections.Generic;


namespace Elmah.Everywhere.Diagnostics
{
    public sealed class ExceptionHandler
    {
        private static ExceptionWritterBase _writter;
        private static ExceptionDefaults _parameters;

        static ExceptionHandler()
        {
            IsEnabled = true;
        }

        #region Private methods

        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Report((Exception) args.ExceptionObject, null);
        }

        #endregion

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

        public static void WithParameters(ExceptionDefaults parameters, ExceptionWritterBase writter)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if (writter == null)
            {
                throw new ArgumentNullException("writter");
            }
            _parameters = parameters;
            _writter = writter;
        }

        public static void Report(Exception exception, IDictionary<string, object> propeties)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (IsEnabled)
            {
                var error = new ErrorInfo(exception, _parameters, propeties);
                error.EnsureErrorDetails();
                _writter.Write(error);
            }
        }

        public static void Report(ErrorInfo error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }
            if (IsEnabled)
            {
                error.EnsureErrorDetails();
                _writter.Write(error);
            }
        }

        public static bool IsEnabled { get; set; }
    }
}