using System;
using System.Text;
using System.Reflection;
using System.Diagnostics;


namespace Elmah.Everywhere.Appenders
{
    public class AssemblyAppender : BaseAppender
    {
        public override void Append(ErrorInfo errorInfo)
        {
            var sb = new StringBuilder();
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
            errorInfo.AddDetail(this.Name, "Assemblies", sb.ToString());
        }

        public override int Order
        {
            get { return 4; }
        }

        public override string Name
        {
            get { return "AppDomain Assembly Appender"; }
        }
    }
}
