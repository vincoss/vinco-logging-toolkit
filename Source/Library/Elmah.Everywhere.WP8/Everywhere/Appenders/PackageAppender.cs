using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel;


namespace Elmah.Everywhere.Appenders
{
    public class PackageAppender : BaseAppender
    {
        public override async void Append(ErrorInfo errorInfo)
        {
            var sb = new StringBuilder();
            var location = Package.Current.InstalledLocation;

            sb.AppendLine(string.Format("InstallFolderName:     {0}", location.Name));
            sb.AppendLine(string.Format("DateCreated:           {0}", location.DateCreated));
            sb.AppendLine(string.Format("InstallPackagePath:    {0}", location.Path));
            sb.AppendLine();

            var folder = Package.Current.InstalledLocation;
            foreach (var file in await folder.GetFilesAsync())
            {
                sb.AppendLine(string.Format("Name:                  {0}", file.Name));
                sb.AppendLine(string.Format("DateCreated:           {0}", file.DateCreated));
                sb.AppendLine(string.Format("Path:                  {0}", file.Path));
                sb.AppendLine();
            }

            errorInfo.AddDetail(this.Name, "", sb.ToString());
        }

        public override int Order
        {
            get { return 3; }
        }

        public override string Name
        {
            get { return "Package Appender"; }
        }
    }
}