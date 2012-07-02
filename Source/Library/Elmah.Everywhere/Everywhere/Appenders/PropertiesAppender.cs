using System.Collections.Generic;


namespace Elmah.Everywhere.Appenders
{
    public class PropertiesAppender : BaseAppender
    {
        public override void Append(ErrorInfo error)
        {
            if (error.Properties != null && error.Properties.Count > 0)
            {
                var pairs = new Dictionary<string, string>();
                foreach (var key in error.Properties.Keys)
                {
                    pairs.Add(key, error.Properties[key].ToString());
                }
                error.AddDetail(this.Name, pairs);
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