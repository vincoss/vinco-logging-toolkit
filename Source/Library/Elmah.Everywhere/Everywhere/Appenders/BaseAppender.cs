using System;


namespace Elmah.Everywhere.Appenders
{
    public abstract class BaseAppender
    {
        protected BaseAppender()
        {
        }

        public abstract void Append(ErrorInfo error);

        public abstract string Name { get; }

        public virtual int Order
        {
            get { return 0; }
        }
    }
}
