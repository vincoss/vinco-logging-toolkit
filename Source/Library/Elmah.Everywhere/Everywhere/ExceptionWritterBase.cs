using System;


namespace Elmah.Everywhere
{
    public abstract class ExceptionWritterBase
    {
        public abstract void Write(ErrorInfo error);
    }
}
