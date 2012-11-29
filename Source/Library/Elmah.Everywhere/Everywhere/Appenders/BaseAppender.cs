using System;


namespace Elmah.Everywhere.Appenders
{
    public abstract class BaseAppender
    {
        protected BaseAppender()
        {
        }

        public abstract void Append(ErrorInfo errorInfo);

        public abstract string Name { get; }

        public virtual int Order
        {
            get { return 0; }
        }
    }
}
