using System;
using Elmah.Everywhere.Appenders;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Resources;
using System.Reflection;


namespace Elmah.Everywhere.Appenders
{
    public class AssemblyAppender : BaseAppender
    {
        public override void Append(ErrorInfo error)
        {
           
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
