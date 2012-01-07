using System;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Diagnostics;
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
            Report((Exception)args.ExceptionObject, null);
        }

        private static string GetDumpReport()
        {
            const string str = "\r\n";
            var builder = new StringBuilder();

            builder.Append("Date:                            ");
            builder.Append(DateTime.Now);
            builder.Append(str);

            builder.Append("Culture:                         ");
            builder.Append(CultureInfo.CurrentCulture.Name);
            builder.Append(str);

#if !SILVERLIGHT

            builder.Append("User:                            ");
            builder.Append(Environment.UserName);
            builder.Append(str);

            builder.Append("MachineName:                     ");
            builder.Append(Environment.MachineName);
            builder.Append(str);

            builder.Append("App up time:                     ");
            builder.Append((DateTime.Now - Process.GetCurrentProcess().StartTime));
            builder.Append(str);

#endif
            builder.Append("Version:                         ");
            builder.Append(new AssemblyName(typeof(ExceptionHandler).Assembly.FullName).Version);
            builder.Append(str);

            builder.Append("Operating System Version:        ");
            builder.Append(Environment.OSVersion);
            builder.Append(str);

            builder.Append("Common Language Runtime Version: ");
            builder.Append(Environment.Version);
            builder.Append(str);

#if SILVERLIGHT
            // TODO: Silverlight info here
#else
            // Get asembly list
            builder.AppendLine();
            builder.AppendLine("AppDomain Assemblies:");
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                builder.Append(assembly.GetName().FullName);
                builder.AppendLine();
            }
            builder.AppendLine();

            // Get file info
            foreach (Assembly assembly in assemblies)
            {
                bool flag = false;
                try
                {
                    if (assembly.Location.Length != 0)
                    {
                        builder.Append(FileVersionInfo.GetVersionInfo(assembly.Location).ToString());
                        builder.AppendLine();
                        flag = true;
                    }
                }
                catch
                {
                }
                if (!flag)
                {
                    builder.Append(assembly.ToString());
                }
                builder.AppendLine();
            }
#endif
            return builder.ToString();
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
                if (propeties == null)
                {
                    propeties = new Dictionary<string, object>();
                }
                string report = GetDumpReport();
                _writter.Report(exception, _parameters, propeties, report);
            }
        }

        public static bool IsEnabled { get; set; }
    }
}