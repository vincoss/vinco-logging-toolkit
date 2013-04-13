using System;


namespace Elmah.Everywhere
{
    public class WritterEventArgs : EventArgs
    {
        public WritterEventArgs(ErrorInfo error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }
            this.Error = error;
        }

        public ErrorInfo Error { get; private set; }
    }
}

