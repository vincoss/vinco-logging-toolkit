using System;


namespace Elmah.Everywhere
{
    public class ErrorInfo
    {
        public string Token { get; set; }

        public string ApplicationName { get; set; }

        public string HostName { get; set; }

        public string Type { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string Error { get; set; }

        public DateTime Date { get; set; }
    }
}