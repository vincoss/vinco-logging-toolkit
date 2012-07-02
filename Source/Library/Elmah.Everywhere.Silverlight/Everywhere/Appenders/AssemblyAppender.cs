using System;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;
using System.Collections.Generic;


namespace Elmah.Everywhere.Appenders
{
    public class AssemblyAppender : BaseAppender
    {
        public override void Append(ErrorInfo error)
        {
            var pairs = new List<KeyValuePair<string, string>>();
            foreach (AssemblyPart ap in Deployment.Current.Parts)
            {
                StreamResourceInfo sri = Application.GetResourceStream(new Uri(ap.Source, UriKind.Relative));
                if (sri != null)
                {
                    Assembly assembly = new AssemblyPart().Load(sri.Stream);

                    pairs.Add(Message("Assembly", assembly.FullName));
                    pairs.Add(Message("ImageRuntimeVersion", assembly.ImageRuntimeVersion));

                    try
                    {
                        if (assembly.Location.Length != 0)
                        {
                            pairs.Add(Message("Location", assembly.Location));
                        }
                    }
                    catch
                    {
                    }
                }
            }
            error.AddDetail(this.Name, pairs);
        }

        private static KeyValuePair<string, string> Message(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }

        public override int Order
        {
            get { return 2; }
        }

        public override string Name
        {
            get { return "Assembly Apender"; }
        }
    }
}
