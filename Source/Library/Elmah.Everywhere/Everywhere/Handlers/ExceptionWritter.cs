using System;
using System.Collections.Generic;


namespace Elmah.Everywhere.Handlers
{
    public abstract class ExceptionWritter
    {
        public abstract void Write(Exception exception, ExceptionParameters parameters, IDictionary<string, object> propeties);
    }
}
