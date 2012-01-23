using System;
using System.Text;


namespace Elmah.Everywhere
{
    public static class ExceptionExtension
    {
        public static string GetExceptionString(this Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            if (exception != null)
            {
                string className;
                string message = exception.Message;
                if (string.IsNullOrWhiteSpace(message))
                {
                    className = exception.GetType().Name;
                }
                else
                {
                    className = exception.GetType() + ": " + message;
                }
                sb.Append(className);
                if (exception.InnerException != null)
                {
                    sb.Append(" ---> " + exception.InnerException.GetExceptionString() + Environment.NewLine + "   ");
                    sb.Append("--- End of inner exception stack trace ---");
                }
                string stackTrace = exception.StackTrace;
                if (stackTrace != null)
                {
                    sb.AppendLine();
                    sb.Append(stackTrace);
                }
            }
            return sb.ToString();
        }
    }
}
