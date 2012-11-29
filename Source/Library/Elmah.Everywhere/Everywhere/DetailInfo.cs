using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Elmah.Everywhere
{
    [DebuggerDisplay("Name : {Name}")]
    public class DetailInfo
    {
        public DetailInfo(string name, IList<KeyValuePair<string, string>> pairs)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            if (pairs == null)
            {
                throw new ArgumentNullException("pairs");
            }
            Name = name;
            Items = pairs;
        }

        public string Name { get; private set; }
        public IList<KeyValuePair<string, string>> Items { get; private set; }
    }
}
