using System;


namespace Elmah.Everywhere
{
    public abstract class ExceptionWritterBase
    {
        public event EventHandler Completed;

        public abstract void Write(string token, ErrorInfo error);

        protected virtual void OnCompleted(EventArgs e)
        {
            if (Completed != null)
            {
                Completed(this, e);
            }
        }

        public Exception Exception { get; protected set; }
    }
}
