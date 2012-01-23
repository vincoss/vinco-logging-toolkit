using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using Elmah.Everywhere.Diagnostics;
using Elmah.Properties;


namespace Elmah.Everywhere
{
    public static class DumpHelper
    {
        private const string NEW_LINE = "\r\n";

#if !SILVERLIGHT
        private static readonly MemoryDetail MemoryDetail = new MemoryDetail();
#endif

        public static void WithException(Exception exception, StringBuilder sb)
        {
            sb.AppendLine(exception.GetExceptionString());
            sb.Append(NEW_LINE);
            sb.Append(NEW_LINE);
        }

        public static void WithDetail(StringBuilder sb)
        {
            sb.AppendLine(Strings.Dump_Report);

            sb.Append("Date:                            ");
            sb.Append(DateTime.Now);
            sb.Append(NEW_LINE);

            sb.Append("Culture:                         ");
            sb.Append(CultureInfo.CurrentCulture.Name);
            sb.Append(NEW_LINE);

#if !SILVERLIGHT

            sb.Append("User:                            ");
            sb.Append(Environment.UserName);
            sb.Append(NEW_LINE);

            sb.Append("MachineName:                     ");
            sb.Append(Environment.MachineName);
            sb.Append(NEW_LINE);

            sb.Append("App up time:                     ");
            sb.Append((DateTime.Now - Process.GetCurrentProcess().StartTime));
            sb.Append(NEW_LINE);

#endif
            sb.Append("Version:                         ");
            sb.Append(new AssemblyName(typeof (ExceptionHandler).Assembly.FullName).Version);
            sb.Append(NEW_LINE);

            sb.Append("Operating System Version:        ");
            sb.Append(Environment.OSVersion);
            sb.Append(NEW_LINE);

            sb.Append("Common Language Runtime Version: ");
            sb.Append(Environment.Version);
            sb.Append(NEW_LINE);
            sb.Append(NEW_LINE);
            sb.Append(NEW_LINE);
        }

        public static void WithMemory(StringBuilder sb)
        {
#if !SILVERLIGHT

            sb.AppendLine("*** Memory Report:");

            MemoryDetail.Append(sb);
            sb.Append(NEW_LINE);
            sb.Append(NEW_LINE);
#endif
        }

        // TODO: Add silverlight assembly and WP7
        public static void WithAssembly(StringBuilder sb)
        {
            sb.AppendLine("*** AppDomain Assemblies Report:");

            #if !SILVERLIGHT
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                sb.AppendLine(assembly.GetName().FullName);
            }
            sb.AppendLine();

            // Get file info
            foreach (Assembly assembly in assemblies)
            {
                bool flag = false;
                try
                {
                    if (assembly.Location.Length != 0)
                    {
                        sb.Append(FileVersionInfo.GetVersionInfo(assembly.Location).ToString());
                        sb.AppendLine();
                        flag = true;
                    }
                }
                catch
                {
                }
                if (!flag)
                {
                    sb.Append(assembly.ToString());
                }
                sb.AppendLine();
            }
            sb.Append(NEW_LINE);
            sb.Append(NEW_LINE);
#endif
        }

        public static void WithProperties(IDictionary<string, object> properties, StringBuilder sb)
        {
            sb.AppendLine("*** Properties Report:");

            if (properties != null && properties.Count > 0)
            {
                foreach (var key in properties.Keys)
                {
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", key, properties[key]));
                }
            }
            sb.Append(NEW_LINE);
            sb.Append(NEW_LINE);
        }

        public static void WithExceptionData(IDictionary exceptionData, StringBuilder sb)
        {
            sb.AppendLine("*** Exception Data Report:");

            if (exceptionData != null && exceptionData.Count > 0)
            {
                foreach (var key in exceptionData.Keys)
                {
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", key, exceptionData[key]));
                }
            }
            sb.Append(NEW_LINE);
            sb.Append(NEW_LINE);
        }
    }
}
