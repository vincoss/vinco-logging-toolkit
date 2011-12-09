using System;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
using Elmah.Everywhere.Handlers;
using Elmah.Properties;


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
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();

            builder.AppendFormat(CultureInfo.InvariantCulture, "Date: {0}", DateTime.Now.ToString());
            builder.AppendLine();

#if !SILVERLIGHT

            builder.AppendFormat(CultureInfo.InvariantCulture, "User: {0}", Environment.UserName);
            builder.AppendLine();

            builder.AppendFormat(CultureInfo.InvariantCulture, "MachineName: {0}", Environment.MachineName);
            builder.AppendLine();

#endif

            builder.AppendFormat(CultureInfo.InvariantCulture, "Version: {0}", new AssemblyName(typeof (ExceptionHandler).Assembly.FullName).Version);
            builder.AppendLine();

            builder.AppendFormat(CultureInfo.InvariantCulture, "Operating System Version: {0}", Environment.OSVersion);
            builder.AppendLine();

            builder.AppendFormat(CultureInfo.InvariantCulture, "Common Language Runtime Version: {0}", Environment.Version);
            builder.AppendLine();

#if SILVERLIGHT
            // TODO: Silverlight info here
#else
            // Get asembly list
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
                exception.Data.Add(Strings.Data_Dump, GetDumpReport());
                _writter.Write(exception, _parameters, propeties);
            }
        }

        public static bool IsEnabled { get; set; }
    }
}