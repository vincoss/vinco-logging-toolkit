using System;
using System.Reflection;
using System.Text;


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

            foreach (dynamic assembly in assemblies)
            {
                var assemblyName = new AssemblyName(assembly.FullName);

                sb.AppendLine(string.Format("File:                  {0}", assembly.Location));
                sb.AppendLine(string.Format("FullName:              {0}", assembly.FullName));
                sb.AppendLine(string.Format("ImageRuntimeVersion:   {0}", assembly.ImageRuntimeVersion));
                sb.AppendLine(string.Format("Version:               {0}", assemblyName.Version));
                sb.AppendLine(string.Format("IsDynamic:             {0}", assembly.IsDynamic));
                sb.AppendLine();
            }
            errorInfo.AddDetail(this.Name, "", sb.ToString());
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
