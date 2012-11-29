using System;


namespace Elmah.Everywhere
{
    public abstract class ExceptionWritterBase
    {
        public event EventHandler<WritterEventArgs> Completed;

        public abstract void Write(string token, ErrorInfo errorInfo);

        protected void OnCompleted(WritterEventArgs e)
        {
            if (Completed != null)
            {
                Completed(this, e);
            }
        }

        public Exception Exception { get; protected set; }
    }
}
