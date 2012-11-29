using System.Collections.Generic;


namespace Elmah.Everywhere.Appenders
{
    public class PropertiesAppender : BaseAppender
    {
        public override void Append(ErrorInfo errorInfo)
        {
            if (errorInfo.Properties != null && errorInfo.Properties.Count > 0)
            {
                var pairs = new Dictionary<string, string>();
                foreach (var key in errorInfo.Properties.Keys)
                {
                    pairs.Add(key, errorInfo.Properties[key].ToString());
                }
                errorInfo.AddDetail(this.Name, pairs);
            }
        }

        public override int Order
        {
            get { return 0; }
        }

        public override string Name
        {
            get { return "Properties Appender"; }
        }
    }
}